using System.Text.Json;
using JPLearn.Core.ExamPractice;
using JPLearn.Core.ExamPractice.Entities;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Data.Seed;

public static class ExamPracticeSeedData
{
    private static readonly DateTime SeededAt = DateTime.UtcNow;

    public static async Task SeedAsync(AppDbContext db)
    {
        // Force re-seed to ensure reading passages are updated
        Console.WriteLine("--> Force re-seeding exam data...");

        var imports = await LoadImportFilesAsync();
        if (imports.Count == 0) return;

        Console.WriteLine($"--> Seeding Exam Data: {imports.Count} files found...");

        // Disable change tracking for performance
        db.ChangeTracker.AutoDetectChangesEnabled = false;

        try 
        {
            // 1. Clear existing data (if any remained)
            await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE exam_attempt_answers CASCADE;");
            await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE exam_attempts CASCADE;");
            await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE exam_blueprint_rules CASCADE;");
            await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE exam_blueprints CASCADE;");
            await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE exam_question_options CASCADE;");
            await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE exam_questions CASCADE;");
            await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE exam_passages CASCADE;");
            await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE exam_topics CASCADE;");
            await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE exam_courses CASCADE;");

            foreach (var importFile in imports)
            {
                Console.WriteLine($"--> Processing course: {importFile.CourseCode}");
                
                // 1. Course
                var title = importFile.CourseCode == ExamCourseCodes.JPD113 ? "JPD113 - Tiếng Nhật Sơ cấp 1" : "JPD123 - Tiếng Nhật Sơ cấp 2";
                var desc = $"Ngân hàng câu hỏi luyện thi cho khóa học {importFile.CourseCode}";
                var course = new ExamCourse
                {
                    Id = Guid.NewGuid(),
                    Code = importFile.CourseCode,
                    Title = title,
                    Description = desc,
                    OrderIndex = importFile.CourseCode == ExamCourseCodes.JPD113 ? 1 : 2,
                    AccessTier = "premium", // Tất cả là premium mặc định
                    PackageCode = importFile.CourseCode, // jpd113 hoặc jpd123
                    IsActive = true,
                    CreatedAt = SeededAt,
                    UpdatedAt = SeededAt
                };
                db.ExamCourses.Add(course);

                // 2. Topics
                var allTopics = importFile.Topics.Select(t => new ExamTopic
                {
                    Id = Guid.NewGuid(),
                    CourseCode = importFile.CourseCode,
                    Code = t.Code,
                    Label = t.Label,
                    OrderIndex = t.OrderIndex,
                    IsActive = true,
                    CreatedAt = SeededAt,
                    UpdatedAt = SeededAt
                }).ToList();

                var questionTopicCodes = importFile.Questions.Select(q => NormalizeTopic(q.Topic)).Distinct();
                foreach (var qTopic in questionTopicCodes)
                {
                    if (!allTopics.Any(t => t.Code == qTopic))
                    {
                        allTopics.Add(new ExamTopic
                        {
                            Id = Guid.NewGuid(),
                            CourseCode = importFile.CourseCode,
                            Code = qTopic,
                            Label = qTopic.Replace("_", " ").ToUpper(),
                            OrderIndex = 99,
                            IsActive = true,
                            CreatedAt = SeededAt,
                            UpdatedAt = SeededAt
                        });
                    }
                }
                db.ExamTopics.AddRange(allTopics);

                // 3. Passages
                var passages = importFile.Passages.Select(seed => new ExamPassage
                {
                    Id = PassageId(importFile.CourseCode, seed.Id),
                    CourseCode = importFile.CourseCode,
                    Title = seed.Title,
                    Content = seed.Content,
                    Level = seed.Level ?? importFile.Level,
                    Topic = NormalizeTopic(seed.Topic),
                    OrderIndex = seed.OrderIndex ?? seed.Id,
                    IsActive = true,
                    CreatedAt = SeededAt,
                    UpdatedAt = SeededAt
                }).ToList();
                db.ExamPassages.AddRange(passages);

                // 4. Questions & Options
                var questions = new List<ExamQuestion>();
                var options = new List<ExamQuestionOption>();

                foreach (var seed in importFile.Questions)
                {
                    var qId = QuestionId(importFile.CourseCode, seed.Id);
                    var question = new ExamQuestion
                    {
                        Id = qId,
                        CourseCode = importFile.CourseCode,
                        PassageId = seed.PassageId.HasValue ? PassageId(importFile.CourseCode, seed.PassageId.Value) : null,
                        QuestionType = NormalizeQuestionType(seed.Type),
                        Topic = NormalizeTopic(seed.Topic),
                        Level = seed.Level ?? importFile.Level,
                        QuestionText = seed.QuestionText,
                        Explanation = seed.Explanation,
                        OrderIndex = seed.OrderIndex ?? seed.Id,
                        IsActive = true,
                        CreatedAt = SeededAt,
                        UpdatedAt = SeededAt
                    };
                    questions.Add(question);

                    foreach (var opt in seed.Options)
                    {
                        options.Add(new ExamQuestionOption
                        {
                            Id = Guid.NewGuid(),
                            QuestionId = qId,
                            Label = opt.Label,
                            Text = opt.Text,
                            IsCorrect = opt.IsCorrect,
                            OrderIndex = opt.Label[0] - 'A',
                            CreatedAt = SeededAt,
                            UpdatedAt = SeededAt
                        });
                    }
                }
                db.ExamQuestions.AddRange(questions);
                db.ExamQuestionOptions.AddRange(options);

                // 5. Blueprints
                var bId = Guid.NewGuid();
                db.ExamBlueprints.Add(new ExamBlueprint
                {
                    Id = bId,
                    CourseCode = importFile.CourseCode,
                    Title = $"Đề thi thử {importFile.CourseCode}",
                    TimeLimitMinutes = 60,
                    IsActive = true,
                    CreatedAt = SeededAt,
                    UpdatedAt = SeededAt
                });

                var topicCodes = importFile.Questions.Select(x => NormalizeTopic(x.Topic)).Distinct();
                foreach (var tc in topicCodes)
                {
                    db.ExamBlueprintRules.Add(new ExamBlueprintRule
                    {
                        Id = Guid.NewGuid(),
                        BlueprintId = bId,
                        Topic = tc,
                        QuestionCount = 5,
                        CreatedAt = SeededAt,
                        UpdatedAt = SeededAt
                    });
                }

                Console.WriteLine($"--> Saving data for {importFile.CourseCode}...");
                await db.SaveChangesAsync();
                db.ChangeTracker.Clear(); // Clear to free memory
            }

            Console.WriteLine("--> Exam seeding completed successfully.");
        }
        finally
        {
            db.ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }

    private static Guid PassageId(string courseCode, int originalId)
    {
        uint hash = (uint)courseCode.GetHashCode();
        // Last block: 12 chars. PPPPXXXXXXXX where PPPP is course prefix and XXXXXXXX is ID
        return Guid.Parse($"00000000-0000-0000-0000-{(hash & 0xFFFF):X4}{(uint)originalId:X8}");
    }

    private static Guid QuestionId(string courseCode, int originalId)
    {
        uint hash = (uint)courseCode.GetHashCode();
        // Last block: 12 chars. QQQQXXXXXXXX where QQQQ is course prefix (offset by 1) and XXXXXXXX is ID
        return Guid.Parse($"00000000-0000-0000-0000-{(hash & 0xFFFF | 0xF000):X4}{(uint)originalId:X8}");
    }

    private static string NormalizeTopic(string topic) => topic.ToLower().Trim().Replace(" ", "_");

    private static string NormalizeQuestionType(string type) => type switch
    {
        "reading" => ExamQuestionTypes.Passage,
        _ => ExamQuestionTypes.Standalone
    };

    private static async Task<List<ExamImportFile>> LoadImportFilesAsync()
    {
        var files = new List<ExamImportFile>();
        var importDir = Path.Combine(AppContext.BaseDirectory, "Data", "Imports", "exam");
        if (!Directory.Exists(importDir)) importDir = Path.Combine("..", "JPLearn.Infrastructure", "Data", "Imports", "exam");

        if (!Directory.Exists(importDir)) return files;

        foreach (var file in Directory.GetFiles(importDir, "*.questions.json"))
        {
            var content = await File.ReadAllTextAsync(file);
            var import = JsonSerializer.Deserialize<ExamImportFile>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (import != null) files.Add(import);
        }
        return files;
    }
}

public class ExamImportFile
{
    public string CourseCode { get; set; } = string.Empty;
    public string Level { get; set; } = "N5";
    public List<ExamTopicSeed> Topics { get; set; } = new();
    public List<ExamPassageSeed> Passages { get; set; } = new();
    public List<ExamQuestionSeed> Questions { get; set; } = new();
}

public class ExamTopicSeed
{
    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class ExamPassageSeed
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Level { get; set; }
    public string Topic { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
}

public class ExamQuestionSeed
{
    public int Id { get; set; }
    public int? PassageId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string? Level { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public ExamOptionSeed[] Options { get; set; } = Array.Empty<ExamOptionSeed>();
    public int? OrderIndex { get; set; }
}

public class ExamOptionSeed
{
    public string Label { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
