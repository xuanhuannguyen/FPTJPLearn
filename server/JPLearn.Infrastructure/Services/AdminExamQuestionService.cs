using JPLearn.Core.Admin.Exam;
using JPLearn.Core.ExamPractice;
using JPLearn.Core.ExamPractice.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class AdminExamQuestionService : IAdminExamQuestionService
{
    private readonly AppDbContext _db;

    public AdminExamQuestionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<AdminExamQuestionDto>> GetQuestionsAsync(string? courseCode = null, string? topic = null, bool includeInactive = false)
    {
        var normalizedCourse = NormalizeOptional(courseCode);
        var normalizedTopic = NormalizeOptional(topic);
        var query = _db.ExamQuestions
            .Include(question => question.Passage)
            .Include(question => question.Options.OrderBy(option => option.OrderIndex))
            .AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(question => question.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(normalizedCourse))
        {
            query = query.Where(question => question.CourseCode == normalizedCourse);
        }

        if (!string.IsNullOrWhiteSpace(normalizedTopic))
        {
            query = query.Where(question => question.Topic == normalizedTopic);
        }

        var questions = await query
            .OrderBy(question => question.CourseCode)
            .ThenBy(question => question.Topic)
            .ThenBy(question => question.OrderIndex)
            .ToListAsync();

        return questions.Select(MapQuestion).ToList();
    }

    public async Task<AdminExamQuestionDto?> GetQuestionAsync(Guid questionId)
    {
        var question = await LoadQuestionAsync(questionId);
        return question == null ? null : MapQuestion(question);
    }

    public AdminExamImportFileInput GetImportTemplate(string type)
    {
        return type.Trim().Equals("reading", StringComparison.OrdinalIgnoreCase)
            ? BuildReadingTemplate()
            : BuildStandardTemplate();
    }

    public async Task<AdminExamImportResult> ImportQuestionsAsync(AdminExamImportFileInput input)
    {
        await ValidateImportAsync(input);

        var now = DateTime.UtcNow;
        var passageIdsByLocalId = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        var importedPassages = 0;
        var createdQuestions = 0;
        var updatedQuestions = 0;

        foreach (var passageInput in input.Passages)
        {
            var existingPassage = !string.IsNullOrWhiteSpace(passageInput.LocalId)
                ? await _db.ExamPassages.FirstOrDefaultAsync(passage =>
                    passage.CourseCode == NormalizeCourseCode(input.CourseCode)
                    && passage.Title == passageInput.Title.Trim())
                : null;

            if (existingPassage == null)
            {
                existingPassage = new ExamPassage
                {
                    Id = Guid.NewGuid(),
                    CourseCode = NormalizeCourseCode(input.CourseCode),
                    CreatedAt = now
                };
                _db.ExamPassages.Add(existingPassage);
                importedPassages++;
            }

            existingPassage.Title = passageInput.Title.Trim();
            existingPassage.Content = passageInput.Content.Trim();
            existingPassage.Level = NormalizeLevel(passageInput.Level ?? input.Level);
            existingPassage.Topic = NormalizeTopic(passageInput.Topic ?? ExamQuestionTopics.Reading);
            existingPassage.OrderIndex = passageInput.OrderIndex ?? await NextPassageOrderIndexAsync(input.CourseCode);
            existingPassage.IsActive = true;
            existingPassage.UpdatedAt = now;

            if (!string.IsNullOrWhiteSpace(passageInput.LocalId))
            {
                passageIdsByLocalId[passageInput.LocalId.Trim()] = existingPassage.Id;
            }
        }

        foreach (var questionInput in input.Questions)
        {
            var payload = BuildImportQuestionPayload(input, questionInput, passageIdsByLocalId);
            var existingQuestion = !string.IsNullOrWhiteSpace(questionInput.LocalId)
                ? await _db.ExamQuestions
                    .Include(question => question.Options)
                    .FirstOrDefaultAsync(question =>
                        question.CourseCode == NormalizeCourseCode(input.CourseCode)
                        && question.QuestionText == questionInput.QuestionText.Trim())
                : null;

            if (existingQuestion == null)
            {
                var question = await BuildQuestionAsync(payload, now);
                _db.ExamQuestions.Add(question);
                createdQuestions++;
                continue;
            }

            await ApplyQuestionInputAsync(existingQuestion, payload, now);
            updatedQuestions++;
        }

        await _db.SaveChangesAsync();
        return new AdminExamImportResult(importedPassages, createdQuestions, updatedQuestions);
    }

    public async Task<AdminExamQuestionDto> CreateQuestionAsync(AdminExamQuestionInput input)
    {
        await ValidateInputAsync(input);

        var now = DateTime.UtcNow;
        var question = await BuildQuestionAsync(input, now);
        _db.ExamQuestions.Add(question);
        await _db.SaveChangesAsync();

        return MapQuestion(await LoadQuestionAsync(question.Id) ?? question);
    }

    public async Task<AdminExamQuestionDto?> UpdateQuestionAsync(Guid questionId, AdminExamQuestionInput input)
    {
        var question = await LoadQuestionAsync(questionId);
        if (question == null)
        {
            return null;
        }

        await ValidateInputAsync(input);
        await ApplyQuestionInputAsync(question, input, DateTime.UtcNow);
        await _db.SaveChangesAsync();

        return MapQuestion(await LoadQuestionAsync(question.Id) ?? question);
    }

    public async Task<bool> DeleteQuestionAsync(Guid questionId)
    {
        var question = await _db.ExamQuestions.FirstOrDefaultAsync(item => item.Id == questionId);
        if (question == null)
        {
            return false;
        }

        question.IsActive = false;
        question.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private async Task<ExamQuestion> BuildQuestionAsync(AdminExamQuestionInput input, DateTime now)
    {
        var question = new ExamQuestion
        {
            Id = Guid.NewGuid(),
            CourseCode = NormalizeCourseCode(input.CourseCode),
            QuestionType = NormalizeQuestionType(input.QuestionType),
            Topic = NormalizeTopic(input.Topic),
            Level = NormalizeLevel(input.Level),
            QuestionText = input.QuestionText.Trim(),
            Explanation = input.Explanation.Trim(),
            OrderIndex = input.OrderIndex ?? await NextOrderIndexAsync(input.CourseCode, input.Topic),
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        question.PassageId = await ResolvePassageIdAsync(input, now);
        ApplyOptions(question, input.Options, now);
        return question;
    }

    private async Task ApplyQuestionInputAsync(ExamQuestion question, AdminExamQuestionInput input, DateTime now)
    {
        question.CourseCode = NormalizeCourseCode(input.CourseCode);
        question.QuestionType = NormalizeQuestionType(input.QuestionType);
        question.Topic = NormalizeTopic(input.Topic);
        question.Level = NormalizeLevel(input.Level);
        question.QuestionText = input.QuestionText.Trim();
        question.Explanation = input.Explanation.Trim();
        question.OrderIndex = input.OrderIndex ?? question.OrderIndex;
        question.IsActive = input.IsActive ?? question.IsActive;
        question.PassageId = await ResolvePassageIdAsync(input, now);
        question.UpdatedAt = now;
        ApplyOptions(question, input.Options, now);
    }

    private async Task ValidateInputAsync(AdminExamQuestionInput input)
    {
        var courseCode = NormalizeCourseCode(input.CourseCode);
        var questionType = NormalizeQuestionType(input.QuestionType);
        var topic = NormalizeTopic(input.Topic);

        Require(ExamCourseCodes.All.Contains(courseCode), "Invalid courseCode.");
        Require(questionType is ExamQuestionTypes.Standalone or ExamQuestionTypes.Passage, "Invalid questionType.");
        Require(ExamQuestionTopics.All.Contains(topic), "Invalid topic.");
        Require(!string.IsNullOrWhiteSpace(input.QuestionText), "Question text is required.");
        Require(!string.IsNullOrWhiteSpace(input.Explanation), "Explanation is required.");
        Require(input.Options.Count >= 2, "At least two options are required.");
        Require(input.Options.Count(option => option.IsCorrect) == 1, "Exactly one option must be correct.");
        Require(input.Options.All(option => !string.IsNullOrWhiteSpace(option.Text)), "Option text is required.");

        var labels = input.Options
            .Select((option, index) => NormalizeLabel(option.Label, index))
            .ToList();
        Require(labels.Distinct().Count() == labels.Count, "Option labels must be unique.");
        Require(await _db.ExamCourses.AnyAsync(course => course.Code == courseCode && course.IsActive), "Course does not exist.");

        if (questionType != ExamQuestionTypes.Passage)
        {
            return;
        }

        if (input.PassageId.HasValue)
        {
            var passageExists = await _db.ExamPassages.AnyAsync(passage =>
                passage.Id == input.PassageId.Value && passage.CourseCode == courseCode);
            Require(passageExists, "Passage does not exist for this course.");
            return;
        }

        Require(input.Passage != null
            && !string.IsNullOrWhiteSpace(input.Passage.Title)
            && !string.IsNullOrWhiteSpace(input.Passage.Content), "Passage title and content are required for passage questions.");
    }

    private async Task ValidateImportAsync(AdminExamImportFileInput input)
    {
        Require(ExamCourseCodes.All.Contains(NormalizeCourseCode(input.CourseCode)), "Invalid courseCode.");
        Require(await _db.ExamCourses.AnyAsync(course => course.Code == NormalizeCourseCode(input.CourseCode) && course.IsActive), "Course does not exist.");
        Require(input.Questions.Count > 0, "Import file must include at least one question.");

        var passageLocalIds = input.Passages
            .Where(passage => !string.IsNullOrWhiteSpace(passage.LocalId))
            .Select(passage => passage.LocalId!.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var passage in input.Passages)
        {
            Require(!string.IsNullOrWhiteSpace(passage.Title) && !string.IsNullOrWhiteSpace(passage.Content), "Every passage needs title and content.");
        }

        foreach (var question in input.Questions)
        {
            var payload = new AdminExamQuestionInput
            {
                CourseCode = input.CourseCode,
                QuestionType = question.QuestionType,
                Topic = question.Topic,
                Level = string.IsNullOrWhiteSpace(question.Level) ? input.Level : question.Level,
                QuestionText = question.QuestionText,
                Explanation = question.Explanation,
                Options = question.Options.Select(option => new AdminExamQuestionOptionInput
                {
                    Label = option.Label,
                    Text = option.Text,
                    IsCorrect = option.IsCorrect
                }).ToList()
            };

            if (NormalizeQuestionType(question.QuestionType) == ExamQuestionTypes.Passage)
            {
                if (!string.IsNullOrWhiteSpace(question.PassageLocalId))
                {
                    Require(passageLocalIds.Contains(question.PassageLocalId.Trim()),
                        $"Question {question.LocalId ?? question.QuestionText} references unknown passageLocalId.");
                    payload.Passage = new AdminExamPassageInput
                    {
                        Title = "linked passage",
                        Content = "linked passage",
                        Level = payload.Level,
                        Topic = ExamQuestionTopics.Reading
                    };
                }
                else
                {
                    Require(question.Passage != null,
                        $"Passage question {question.LocalId ?? question.QuestionText} needs passageLocalId or passage.");
                    payload.Passage = question.Passage;
                }
            }

            await ValidateInputAsync(payload);
        }
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
        {
            throw new ArgumentException(message);
        }
    }

    private static AdminExamQuestionInput BuildImportQuestionPayload(
        AdminExamImportFileInput file,
        AdminExamImportQuestionInput question,
        IReadOnlyDictionary<string, Guid> passageIdsByLocalId)
    {
        var questionType = NormalizeQuestionType(question.QuestionType);
        var payload = new AdminExamQuestionInput
        {
            CourseCode = file.CourseCode,
            QuestionType = questionType,
            Topic = question.Topic,
            Level = string.IsNullOrWhiteSpace(question.Level) ? file.Level : question.Level,
            QuestionText = question.QuestionText,
            Explanation = question.Explanation,
            OrderIndex = question.OrderIndex,
            IsActive = true,
            Options = question.Options.Select(option => new AdminExamQuestionOptionInput
            {
                Label = option.Label,
                Text = option.Text,
                IsCorrect = option.IsCorrect
            }).ToList()
        };

        if (questionType != ExamQuestionTypes.Passage)
        {
            return payload;
        }

        if (!string.IsNullOrWhiteSpace(question.PassageLocalId)
            && passageIdsByLocalId.TryGetValue(question.PassageLocalId.Trim(), out var passageId))
        {
            payload.PassageId = passageId;
        }
        else if (question.Passage != null)
        {
            payload.Passage = question.Passage;
        }

        return payload;
    }

    private async Task<Guid?> ResolvePassageIdAsync(AdminExamQuestionInput input, DateTime now)
    {
        if (NormalizeQuestionType(input.QuestionType) != ExamQuestionTypes.Passage)
        {
            return null;
        }

        if (input.PassageId.HasValue && input.Passage == null)
        {
            return input.PassageId.Value;
        }

        if (input.PassageId.HasValue)
        {
            var existingPassage = await _db.ExamPassages.FirstAsync(passage => passage.Id == input.PassageId.Value);
            existingPassage.Title = input.Passage!.Title.Trim();
            existingPassage.Content = input.Passage.Content.Trim();
            existingPassage.Level = NormalizeLevel(input.Passage.Level ?? input.Level);
            existingPassage.Topic = NormalizeTopic(input.Passage.Topic ?? ExamQuestionTopics.Reading);
            existingPassage.CourseCode = NormalizeCourseCode(input.CourseCode);
            existingPassage.OrderIndex = input.Passage.OrderIndex ?? existingPassage.OrderIndex;
            existingPassage.IsActive = true;
            existingPassage.UpdatedAt = now;
            return existingPassage.Id;
        }

        var passage = new ExamPassage
        {
            Id = Guid.NewGuid(),
            CourseCode = NormalizeCourseCode(input.CourseCode),
            Title = input.Passage!.Title.Trim(),
            Content = input.Passage.Content.Trim(),
            Level = NormalizeLevel(input.Passage.Level ?? input.Level),
            Topic = NormalizeTopic(input.Passage.Topic ?? ExamQuestionTopics.Reading),
            OrderIndex = input.Passage.OrderIndex ?? await NextPassageOrderIndexAsync(input.CourseCode),
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.ExamPassages.Add(passage);
        return passage.Id;
    }

    private void ApplyOptions(ExamQuestion question, IReadOnlyList<AdminExamQuestionOptionInput> optionInputs, DateTime now)
    {
        for (var index = 0; index < optionInputs.Count; index++)
        {
            var optionInput = optionInputs[index];
            var label = NormalizeLabel(optionInput.Label, index);
            var existing = optionInput.Id.HasValue
                ? question.Options.FirstOrDefault(option => option.Id == optionInput.Id.Value)
                : question.Options.FirstOrDefault(option => option.Label == label);

            if (existing == null)
            {
                question.Options.Add(new ExamQuestionOption
                {
                    Id = Guid.NewGuid(),
                    QuestionId = question.Id,
                    Label = label,
                    Text = optionInput.Text.Trim(),
                    IsCorrect = optionInput.IsCorrect,
                    OrderIndex = index + 1,
                    CreatedAt = now,
                    UpdatedAt = now
                });
                continue;
            }

            existing.Label = label;
            existing.Text = optionInput.Text.Trim();
            existing.IsCorrect = optionInput.IsCorrect;
            existing.OrderIndex = index + 1;
            existing.UpdatedAt = now;
        }
    }

    private async Task<ExamQuestion?> LoadQuestionAsync(Guid questionId)
    {
        return await _db.ExamQuestions
            .Include(question => question.Passage)
            .Include(question => question.Options.OrderBy(option => option.OrderIndex))
            .FirstOrDefaultAsync(question => question.Id == questionId);
    }

    private async Task<int> NextOrderIndexAsync(string courseCode, string topic)
    {
        var normalizedCourse = NormalizeCourseCode(courseCode);
        var normalizedTopic = NormalizeTopic(topic);
        var currentMax = await _db.ExamQuestions
            .Where(question => question.CourseCode == normalizedCourse && question.Topic == normalizedTopic)
            .Select(question => (int?)question.OrderIndex)
            .MaxAsync();

        return (currentMax ?? 0) + 1;
    }

    private async Task<int> NextPassageOrderIndexAsync(string courseCode)
    {
        var normalizedCourse = NormalizeCourseCode(courseCode);
        var currentMax = await _db.ExamPassages
            .Where(passage => passage.CourseCode == normalizedCourse)
            .Select(passage => (int?)passage.OrderIndex)
            .MaxAsync();

        return (currentMax ?? 0) + 1;
    }

    private static AdminExamQuestionDto MapQuestion(ExamQuestion question)
    {
        return new AdminExamQuestionDto(
            question.Id,
            question.CourseCode,
            question.QuestionType,
            question.Topic,
            question.Level ?? "N5",
            question.QuestionText,
            question.Explanation,
            question.PassageId,
            question.Passage == null ? null : new AdminExamPassageDto(
                question.Passage.Id,
                question.Passage.CourseCode,
                question.Passage.Title,
                question.Passage.Content,
                question.Passage.Level ?? "N5",
                question.Passage.Topic,
                question.Passage.OrderIndex,
                question.Passage.IsActive),
            question.Options
                .OrderBy(option => option.OrderIndex)
                .Select(option => new AdminExamQuestionOptionDto(
                    option.Id,
                    option.Label,
                    option.Text,
                    option.IsCorrect,
                    option.OrderIndex))
                .ToList(),
            question.OrderIndex,
            question.IsActive);
    }

    private static AdminExamImportFileInput BuildStandardTemplate()
    {
        return new AdminExamImportFileInput
        {
            CourseCode = ExamCourseCodes.JPD113,
            Level = "N5",
            Passages = [],
            Questions =
            [
                new()
                {
                    LocalId = "q1",
                    QuestionType = ExamQuestionTypes.Standalone,
                    Topic = ExamQuestionTopics.Vocabulary,
                    Level = "N5",
                    QuestionText = "学生 nghĩa là gì?",
                    Explanation = "学生 đọc là がくせい, nghĩa là học sinh/sinh viên.",
                    OrderIndex = 1,
                    Options =
                    [
                        new() { Label = "A", Text = "học sinh; sinh viên", IsCorrect = true },
                        new() { Label = "B", Text = "giáo viên", IsCorrect = false },
                        new() { Label = "C", Text = "công ty", IsCorrect = false },
                        new() { Label = "D", Text = "bệnh viện", IsCorrect = false }
                    ]
                },
                new()
                {
                    LocalId = "q2",
                    QuestionType = ExamQuestionTypes.Standalone,
                    Topic = ExamQuestionTopics.Grammar,
                    Level = "N5",
                    QuestionText = "Chọn trợ từ đúng: 学校___行きます。",
                    Explanation = "Địa điểm đích đến dùng trợ từ に hoặc へ.",
                    OrderIndex = 2,
                    Options =
                    [
                        new() { Label = "A", Text = "に", IsCorrect = true },
                        new() { Label = "B", Text = "を", IsCorrect = false },
                        new() { Label = "C", Text = "で", IsCorrect = false },
                        new() { Label = "D", Text = "と", IsCorrect = false }
                    ]
                }
            ]
        };
    }

    private static AdminExamImportFileInput BuildReadingTemplate()
    {
        return new AdminExamImportFileInput
        {
            CourseCode = ExamCourseCodes.JPD113,
            Level = "N5",
            Passages =
            [
                new()
                {
                    LocalId = "reading-1",
                    Title = "Một ngày của Tanaka",
                    Content = "田中さんは毎朝六時に起きます。七時に朝ごはんを食べます。それから電車で学校へ行きます。",
                    Level = "N5",
                    Topic = ExamQuestionTopics.Reading,
                    OrderIndex = 1
                }
            ],
            Questions =
            [
                new()
                {
                    LocalId = "reading-1-q1",
                    QuestionType = ExamQuestionTypes.Passage,
                    Topic = ExamQuestionTopics.Reading,
                    Level = "N5",
                    PassageLocalId = "reading-1",
                    QuestionText = "田中さんは何時に起きますか。",
                    Explanation = "Đoạn văn nói 毎朝六時に起きます。",
                    OrderIndex = 1,
                    Options =
                    [
                        new() { Label = "A", Text = "六時", IsCorrect = true },
                        new() { Label = "B", Text = "七時", IsCorrect = false },
                        new() { Label = "C", Text = "八時", IsCorrect = false },
                        new() { Label = "D", Text = "九時", IsCorrect = false }
                    ]
                },
                new()
                {
                    LocalId = "reading-1-q2",
                    QuestionType = ExamQuestionTypes.Passage,
                    Topic = ExamQuestionTopics.Reading,
                    Level = "N5",
                    PassageLocalId = "reading-1",
                    QuestionText = "田中さんは何で学校へ行きますか。",
                    Explanation = "Đoạn văn nói 電車で学校へ行きます。",
                    OrderIndex = 2,
                    Options =
                    [
                        new() { Label = "A", Text = "バス", IsCorrect = false },
                        new() { Label = "B", Text = "車", IsCorrect = false },
                        new() { Label = "C", Text = "電車", IsCorrect = true },
                        new() { Label = "D", Text = "自転車", IsCorrect = false }
                    ]
                }
            ]
        };
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();
    }

    private static string NormalizeCourseCode(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? ExamCourseCodes.JPD113 : value.Trim().ToLowerInvariant();
    }

    private static string NormalizeQuestionType(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? ExamQuestionTypes.Standalone : value.Trim().ToLowerInvariant();
    }

    private static string NormalizeTopic(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? ExamQuestionTopics.Vocabulary : value.Trim().ToLowerInvariant();
    }

    private static string NormalizeLevel(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? "N5" : value.Trim().ToUpperInvariant();
    }

    private static string NormalizeLabel(string? value, int index)
    {
        return string.IsNullOrWhiteSpace(value)
            ? ((char)('A' + index)).ToString()
            : value.Trim().ToUpperInvariant();
    }
}
