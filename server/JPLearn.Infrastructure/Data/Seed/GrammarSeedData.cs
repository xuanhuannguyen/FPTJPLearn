using System.Text.Json;
using JPLearn.Core.Grammar;
using JPLearn.Core.Grammar.Entities;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Data.Seed;

public static class GrammarSeedData
{
    private static readonly DateTime SeededAt = new(2026, 5, 12, 0, 0, 0, DateTimeKind.Utc);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public static async Task SeedAsync(AppDbContext db)
    {
        Console.WriteLine("--> Seeding Grammar data...");

        var imports = LoadImportFiles();
        if (imports.Count == 0)
        {
            Console.WriteLine("--> No grammar import files found, skipping.");
            return;
        }

        // Clear existing grammar data
        await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE grammar_exercise_attempts CASCADE;");
        await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE grammar_exercises CASCADE;");
        await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE grammar_examples CASCADE;");
        await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE user_grammar_progress CASCADE;");
        await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE grammar_patterns CASCADE;");
        await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE grammar_lessons CASCADE;");

        db.ChangeTracker.AutoDetectChangesEnabled = false;
        try
        {
            var lessonOrderBase = 0;
            foreach (var import in imports)
            {
                var level = GrammarLevels.Normalize(import.Level ?? "N5");
                foreach (var lessonSeed in import.Lessons)
                {
                    lessonOrderBase++;
                    var lessonId = LessonId(import.CourseCode, lessonSeed.LessonNumber);
                    var lesson = new GrammarLesson
                    {
                        Id = lessonId,
                        Level = level,
                        LessonNumber = lessonSeed.LessonNumber,
                        Title = lessonSeed.Title,
                        Description = lessonSeed.Description,
                        AccessTier = NormalizeAccessTier(lessonSeed.AccessTier),
                        PackageCode = lessonSeed.PackageCode,
                        CourseCode = import.CourseCode,
                        OrderIndex = lessonSeed.OrderIndex ?? lessonOrderBase,
                        CreatedAt = SeededAt,
                        UpdatedAt = SeededAt
                    };
                    db.GrammarLessons.Add(lesson);

                    for (var pi = 0; pi < lessonSeed.Patterns.Length; pi++)
                    {
                        var patternSeed = lessonSeed.Patterns[pi];
                        var patternId = PatternId(import.CourseCode, lessonSeed.LessonNumber, pi);
                        var pattern = new GrammarPattern
                        {
                            Id = patternId,
                            LessonId = lessonId,
                            Level = level,
                            Pattern = patternSeed.Pattern,
                            Title = patternSeed.Title,
                            Meaning = patternSeed.Meaning,
                            Structure = patternSeed.Structure,
                            UsageScope = patternSeed.UsageScope,
                            Formation = patternSeed.Formation,
                            Notes = patternSeed.Notes,
                            TagsJson = null,
                            AccessTierOverride = null,
                            PackageCodeOverride = null,
                            OrderIndex = patternSeed.OrderIndex ?? pi + 1,
                            CreatedAt = SeededAt,
                            UpdatedAt = SeededAt
                        };
                        db.GrammarPatterns.Add(pattern);

                        // Examples
                        for (var ei = 0; ei < patternSeed.Examples.Length; ei++)
                        {
                            var ex = patternSeed.Examples[ei];
                            db.GrammarExamples.Add(new GrammarExample
                            {
                                Id = ExampleId(import.CourseCode, lessonSeed.LessonNumber, pi, ei),
                                PatternId = patternId,
                                Japanese = ex.Japanese,
                                Reading = ex.Reading,
                                Meaning = ex.Meaning,
                                Note = ex.Note,
                                OrderIndex = ex.OrderIndex ?? ei + 1,
                                CreatedAt = SeededAt,
                                UpdatedAt = SeededAt
                            });
                        }

                        // Exercises: auto-generate from examples
                        var exerciseOrder = 0;
                        var modeCounts = new Dictionary<string, int> { { "ja", 0 }, { "vi", 0 }, { "arr", 0 } };

                        for (var ei = 0; ei < patternSeed.Examples.Length; ei++)
                        {
                            var ex = patternSeed.Examples[ei];

                            // ja_to_vi
                            if (!string.IsNullOrWhiteSpace(ex.Meaning))
                            {
                                AddExercise(db, import.CourseCode, lessonSeed.LessonNumber, pi, "ja", ei, patternId, GrammarExerciseTypes.JapaneseToVietnamese, ex.Japanese, ex.Meaning, ex.Reading, patternSeed.Pattern, patternSeed.Notes, ++exerciseOrder);
                                modeCounts["ja"]++;
                            }

                            // vi_to_ja
                            if (!string.IsNullOrWhiteSpace(ex.Japanese))
                            {
                                AddExercise(db, import.CourseCode, lessonSeed.LessonNumber, pi, "vi", ei, patternId, GrammarExerciseTypes.VietnameseToJapanese, ex.Meaning, ex.Japanese, null, patternSeed.Formation, patternSeed.Notes, ++exerciseOrder, ex.Reading);
                                modeCounts["vi"]++;
                            }

                            // arrange (auto-gen)
                            if (!string.IsNullOrWhiteSpace(ex.Japanese))
                            {
                                var options = SmartSplitWithReading(ex.Japanese, ex.Reading);
                                if (options.Count >= 2)
                                {
                                    AddArrangeExercise(db, import.CourseCode, lessonSeed.LessonNumber, pi, "arr", ei, patternId, ex.Meaning, options, ++exerciseOrder);
                                    modeCounts["arr"]++;
                                }
                            }
                        }

                        // Exercises from seed if provided
                        if (patternSeed.Exercises != null)
                        {
                            for (var xi = 0; xi < patternSeed.Exercises.Length; xi++)
                            {
                                var xSeed = patternSeed.Exercises[xi];
                                exerciseOrder++;
                                db.GrammarExercises.Add(new GrammarExercise
                                {
                                    Id = ExerciseId(import.CourseCode, lessonSeed.LessonNumber, pi, "custom", xi),
                                    PatternId = patternId,
                                    ExerciseType = NormalizeExerciseType(xSeed.ExerciseType),
                                    Prompt = xSeed.Prompt,
                                    PromptReading = xSeed.PromptReading,
                                    ExpectedAnswer = xSeed.ExpectedAnswer ?? string.Empty,
                                    AcceptableAnswersJson = xSeed.AcceptableAnswers != null
                                        ? JsonSerializer.Serialize(xSeed.AcceptableAnswers)
                                        : null,
                                    Hint = xSeed.Hint,
                                    Explanation = xSeed.Explanation,
                                    TemplateText = xSeed.TemplateText,
                                    OptionsJson = xSeed.Options != null
                                        ? JsonSerializer.Serialize(xSeed.Options)
                                        : null,
                                    CorrectOrderJson = xSeed.CorrectOrder != null
                                        ? JsonSerializer.Serialize(xSeed.CorrectOrder)
                                        : null,
                                    StarPosition = xSeed.StarPosition,
                                    StarAnswer = xSeed.StarAnswer,
                                    OrderIndex = xSeed.OrderIndex ?? exerciseOrder,
                                    CreatedAt = SeededAt,
                                    UpdatedAt = SeededAt
                                });
                            }
                        }
                    }
                }

                await db.SaveChangesAsync();
                db.ChangeTracker.Clear();
                Console.WriteLine($"--> Grammar seeded: {import.CourseCode} ({import.Lessons.Length} lessons)");
            }
        }
        finally
        {
            db.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        Console.WriteLine("--> Grammar seeding completed.");
    }

    private static IReadOnlyList<GrammarImportFile> LoadImportFiles()
    {
        var dir = ResolveDirectory();
        if (!Directory.Exists(dir)) return [];

        return Directory.EnumerateFiles(dir, "grammar_*.json")
            .OrderBy(p => p)
            .Select(path =>
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<GrammarImportFile>(json, JsonOptions)
                    ?? throw new InvalidOperationException($"Invalid grammar import file: {path}");
            })
            .ToList();
    }

    private static string ResolveDirectory()
    {
        var copied = Path.Combine(AppContext.BaseDirectory, "Data", "Imports");
        if (Directory.Exists(copied)) return copied;

        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null)
        {
            var src = Path.Combine(current.FullName, "JPLearn.Infrastructure", "Data", "Imports");
            if (Directory.Exists(src)) return src;
            current = current.Parent;
        }

        return copied;
    }

    private static void AddExercise(AppDbContext db, string courseCode, int lessonNumber, int pi, string typeKey, int index, Guid patternId, string type, string prompt, string expected, string? reading, string? hint, string? explanation, int order, string? acceptable = null)
    {
        db.GrammarExercises.Add(new GrammarExercise
        {
            Id = ExerciseId(courseCode, lessonNumber, pi, typeKey, index),
            PatternId = patternId,
            ExerciseType = type,
            Prompt = prompt,
            PromptReading = reading,
            ExpectedAnswer = expected,
            AcceptableAnswersJson = acceptable != null ? JsonSerializer.Serialize(new[] { acceptable }) : null,
            Hint = hint,
            Explanation = explanation,
            OrderIndex = order,
            CreatedAt = SeededAt,
            UpdatedAt = SeededAt
        });
    }

    private static void AddArrangeExercise(AppDbContext db, string courseCode, int lessonNumber, int pi, string typeKey, int index, Guid patternId, string prompt, List<string> options, int order)
    {
        db.GrammarExercises.Add(new GrammarExercise
        {
            Id = ExerciseId(courseCode, lessonNumber, pi, typeKey, index),
            PatternId = patternId,
            ExerciseType = GrammarExerciseTypes.Arrange,
            Prompt = prompt,
            ExpectedAnswer = string.Join("", options),
            OptionsJson = JsonSerializer.Serialize(options.OrderBy(_ => Guid.NewGuid()).ToList()),
            CorrectOrderJson = JsonSerializer.Serialize(options),
            OrderIndex = order,
            CreatedAt = SeededAt,
            UpdatedAt = SeededAt
        });
    }

    private static List<string> SmartSplitWithReading(string text, string? reading)
    {
        var clean = text.TrimEnd('。', '！', '？', '.');
        var cleanReading = reading?.TrimEnd('。', '！', '？', '.');
        
        var particles = new[] { "は", "の", "に", "へ", "を", "が", "と", "も", "から", "まで", "です", "でした", "じゃありません" };
        var parts = new List<string>();
        
        var current = clean;
        var currentReading = cleanReading ?? "";

        while (!string.IsNullOrEmpty(current))
        {
            var bestMatchIndex = -1;
            var bestParticle = "";

            foreach (var p in particles)
            {
                var idx = current.IndexOf(p);
                if (idx != -1 && (bestMatchIndex == -1 || idx < bestMatchIndex))
                {
                    bestMatchIndex = idx;
                    bestParticle = p;
                }
            }

            if (bestMatchIndex != -1)
            {
                if (bestMatchIndex > 0)
                {
                    var japPart = current.Substring(0, bestMatchIndex);
                    
                    // Try to find corresponding part in reading
                    if (!string.IsNullOrEmpty(currentReading) && currentReading.Contains(bestParticle))
                    {
                        var readIdx = currentReading.IndexOf(bestParticle);
                        if (readIdx > 0)
                        {
                            var readPart = currentReading.Substring(0, readIdx);
                            parts.Add(FormatWithFurigana(japPart, readPart));
                            currentReading = currentReading.Substring(readIdx + bestParticle.Length);
                        }
                        else
                        {
                             parts.Add(japPart);
                             currentReading = currentReading.Substring(bestParticle.Length);
                        }
                    }
                    else
                    {
                        parts.Add(japPart);
                    }
                }
                
                parts.Add(bestParticle);
                current = current.Substring(bestMatchIndex + bestParticle.Length);
            }
            else
            {
                parts.Add(FormatWithFurigana(current, currentReading));
                current = "";
            }
        }
        
        return parts.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
    }

    private static string FormatWithFurigana(string japanese, string reading)
    {
        if (string.IsNullOrWhiteSpace(japanese)) return "";
        if (string.IsNullOrWhiteSpace(reading) || japanese == reading) return japanese;
        
        // Simple check: if japanese contains any Kanji
        if (japanese.Any(c => c >= 0x4E00 && c <= 0x9FAF))
        {
            return $"{japanese} ({reading})";
        }
        return japanese;
    }

    private static string NormalizeAccessTier(string? tier) =>
        string.IsNullOrWhiteSpace(tier) ? GrammarAccessTiers.Free : tier.Trim().ToLowerInvariant();

    private static string NormalizeExerciseType(string? type) => type?.ToLowerInvariant() switch
    {
        "vi_to_ja" => GrammarExerciseTypes.VietnameseToJapanese,
        "ja_to_vi" => GrammarExerciseTypes.JapaneseToVietnamese,
        "arrange" => GrammarExerciseTypes.Arrange,
        _ => GrammarExerciseTypes.VietnameseToJapanese
    };

    private static Guid LessonId(string courseCode, int lessonNumber)
    {
        var seg = CourseSegment(courseCode);
        return Guid.Parse($"77777777-{seg}-0000-0000-{lessonNumber + 100:000000000000}");
    }

    private static Guid PatternId(string courseCode, int lessonNumber, int patternIndex)
    {
        var seg = CourseSegment(courseCode);
        return Guid.Parse($"77777777-{seg}-0001-0000-{lessonNumber * 1000 + patternIndex + 1:000000000000}");
    }

    private static Guid ExampleId(string courseCode, int lessonNumber, int patternIndex, int exampleIndex)
    {
        var seg = CourseSegment(courseCode);
        var n = lessonNumber * 100000 + patternIndex * 1000 + exampleIndex + 1;
        return Guid.Parse($"77777777-{seg}-0002-0000-{n:000000000000}");
    }

    private static Guid ExerciseId(string courseCode, int lessonNumber, int patternIndex, string typeKey, int index)
    {
        var seg = CourseSegment(courseCode);
        var typeOffset = typeKey switch { 
            "ja" => 10000, "ja_p" => 15000, 
            "vi" => 20000, "vi_p" => 25000, 
            "arr" => 30000, "arr_p" => 35000, 
            _ => 40000 
        };
        var n = lessonNumber * 100000 + patternIndex * 1000 + typeOffset + index + 1;
        return Guid.Parse($"77777777-{seg}-0003-0000-{n:000000000000}");
    }

    private static string CourseSegment(string courseCode) =>
        courseCode.ToLowerInvariant().Contains("113") ? "1113" : "1123";

    // Import models
    private sealed record GrammarImportFile(
        string CourseCode,
        string? Level,
        GrammarLessonSeed[] Lessons);

    private sealed record GrammarLessonSeed(
        int LessonNumber,
        string Title,
        string? Description,
        string? AccessTier,
        string? PackageCode,
        int? OrderIndex,
        GrammarPatternSeed[] Patterns);

    private sealed record GrammarPatternSeed(
        string Pattern,
        string Title,
        string Meaning,
        string Structure,
        string? UsageScope,
        string? Formation,
        string? Notes,
        int? OrderIndex,
        GrammarExampleSeed[] Examples,
        GrammarExerciseSeed[]? Exercises);

    private sealed record GrammarExampleSeed(
        string Japanese,
        string? Reading,
        string Meaning,
        string? Note,
        int? OrderIndex);

    private sealed record GrammarExerciseSeed(
        string? ExerciseType,
        string Prompt,
        string? PromptReading,
        string? ExpectedAnswer,
        string[]? AcceptableAnswers,
        string? Hint,
        string? Explanation,
        string? TemplateText,
        string[]? Options,
        string[]? CorrectOrder,
        int? StarPosition,
        string? StarAnswer,
        int? OrderIndex);
}
