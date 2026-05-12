using JPLearn.Core.ExamPractice;
using JPLearn.Core.ExamPractice.DTOs;
using JPLearn.Core.ExamPractice.Entities;
using JPLearn.Core.Payments;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class ExamPracticeService : IExamPracticeService
{
    private readonly AppDbContext _db;
    private readonly IPaymentAccessService _paymentAccess;

    public ExamPracticeService(AppDbContext db, IPaymentAccessService paymentAccess)
    {
        _db = db;
        _paymentAccess = paymentAccess;
    }

    public async Task<List<ExamCourseDto>> GetCoursesAsync(Guid userId)
    {
        var courses = await _db.ExamCourses
            .Where(course => course.IsActive)
            .OrderBy(course => course.OrderIndex)
            .ToListAsync();

        var questionCounts = await _db.ExamQuestions
            .Where(question => question.IsActive)
            .GroupBy(question => question.CourseCode)
            .Select(group => new { CourseCode = group.Key, Count = group.Count() })
            .ToListAsync();

        var passageCounts = await _db.ExamPassages
            .Where(passage => passage.IsActive)
            .GroupBy(passage => passage.CourseCode)
            .Select(group => new { CourseCode = group.Key, Count = group.Count() })
            .ToListAsync();

        return courses.Select(course => new ExamCourseDto
        {
            Id = course.Id,
            Code = course.Code,
            Title = course.Title,
            Description = course.Description,
            AccessTier = course.AccessTier,
            PackageCode = course.PackageCode,
            IsLocked = _paymentAccess.IsContentLocked(userId, course.AccessTier, course.PackageCode),
            QuestionCount = questionCounts.FirstOrDefault(item => item.CourseCode == course.Code)?.Count ?? 0,
            PassageCount = passageCounts.FirstOrDefault(item => item.CourseCode == course.Code)?.Count ?? 0
        }).ToList();
    }

    public async Task<List<ExamTopicDto>> GetTopicsAsync(Guid userId, string? courseCode = null, string? level = null)
    {
        var normalizedLevel = NormalizeLevel(level);
        var normalizedCourse = NormalizeOptional(courseCode);
        var query = _db.ExamQuestions.Where(question => question.IsActive);
        var accessibleCourseCodes = await GetAccessibleCourseCodesAsync(userId);
        var topicQuery = _db.ExamTopics.Where(topic => topic.IsActive);

        if (!string.IsNullOrWhiteSpace(normalizedCourse))
        {
            topicQuery = topicQuery.Where(topic => topic.CourseCode == normalizedCourse);

            if (accessibleCourseCodes.Contains(normalizedCourse))
            {
                query = query.Where(question => question.CourseCode == normalizedCourse);
            }
            else
            {
                query = query.Where(question => false);
            }
        }
        else
        {
            query = query.Where(question => accessibleCourseCodes.Contains(question.CourseCode));
            topicQuery = topicQuery.Where(topic => accessibleCourseCodes.Contains(topic.CourseCode));
        }

        if (!string.IsNullOrWhiteSpace(normalizedLevel))
        {
            query = query.Where(question => question.Level == normalizedLevel);
        }

        var topics = await topicQuery
            .OrderBy(topic => topic.OrderIndex)
            .ThenBy(topic => topic.Code)
            .ThenBy(topic => topic.CourseCode)
            .ToListAsync();

        var counts = await query
            .GroupBy(question => question.Topic)
            .Select(group => new { Topic = group.Key, Count = group.Count() })
            .ToListAsync();

        var topicRows = topics.Count == 0
            ? ExamQuestionTopics.All
                .Select(topic => new ExamTopicDto
                {
                    Topic = topic,
                    Label = GetTopicLabel(topic),
                    QuestionCount = counts.FirstOrDefault(item => item.Topic == topic)?.Count ?? 0
                })
                .ToList()
            : topics
                .GroupBy(topic => topic.Code)
                .Select(group => group.First())
                .OrderBy(topic => topic.OrderIndex)
                .Select(topic => new ExamTopicDto
                {
                    Topic = topic.Code,
                    Label = topic.Label,
                    QuestionCount = counts.FirstOrDefault(item => item.Topic == topic.Code)?.Count ?? 0
                })
                .ToList();

        return topicRows;
    }

    public async Task<List<ExamQuestionDto>> GetQuestionsAsync(Guid userId, string? courseCode = null, string? topic = null, string? level = null)
    {
        var query = BuildActiveQuestionQuery(topic, level);
        var accessibleCourseCodes = await GetAccessibleCourseCodesAsync(userId);

        var normalizedCourse = NormalizeOptional(courseCode);
        if (!string.IsNullOrWhiteSpace(normalizedCourse))
        {
            if (!accessibleCourseCodes.Contains(normalizedCourse))
            {
                return [];
            }

            query = query.Where(question => question.CourseCode == normalizedCourse);
        }
        else
        {
            query = query.Where(question => accessibleCourseCodes.Contains(question.CourseCode));
        }

        var questions = await query
            .OrderBy(question => question.Topic)
            .ThenBy(question => question.OrderIndex)
            .ToListAsync();

        return questions.Select(MapQuestion).ToList();
    }

    public async Task<ExamQuestionDetailDto?> GetQuestionAsync(Guid userId, Guid questionId)
    {
        var question = await _db.ExamQuestions
            .Include(item => item.Course)
            .Include(item => item.Passage)
            .Include(item => item.Options.OrderBy(option => option.OrderIndex))
            .FirstOrDefaultAsync(item => item.Id == questionId && item.IsActive);

        return question == null || IsCourseLocked(userId, question.Course)
            ? null
            : MapQuestionDetail(question);
    }

    public async Task<ExamAnswerResultDto?> AnswerQuestionAsync(Guid userId, Guid questionId, ExamAnswerQuestionDto dto)
    {
        var question = await _db.ExamQuestions
            .Include(item => item.Course)
            .Include(item => item.Options)
            .FirstOrDefaultAsync(item => item.Id == questionId && item.IsActive);

        if (question == null
            || IsCourseLocked(userId, question.Course)
            || question.Options.All(option => option.Id != dto.SelectedOptionId))
        {
            return null;
        }

        var correctOption = question.Options.First(option => option.IsCorrect);

        return new ExamAnswerResultDto
        {
            QuestionId = question.Id,
            SelectedOptionId = dto.SelectedOptionId,
            CorrectOptionId = correctOption.Id,
            IsCorrect = dto.SelectedOptionId == correctOption.Id,
            Explanation = question.Explanation
        };
    }

    public async Task<ExamAttemptDto?> StartAttemptAsync(Guid userId, StartExamAttemptDto dto)
    {
        var mode = dto.Mode?.ToLower() == ExamAttemptModes.Exam ? ExamAttemptModes.Exam : ExamAttemptModes.Practice;
        var courseCode = NormalizeOptional(dto.CourseCode) ?? ExamCourseCodes.JPD113;
        var course = await _db.ExamCourses
            .FirstOrDefaultAsync(item => item.Code == courseCode && item.IsActive);

        if (course == null || IsCourseLocked(userId, course))
        {
            return null;
        }

        List<ExamQuestion> selectedQuestions;

        if (mode == ExamAttemptModes.Exam)
        {
            // Exam Mode: Use Blueprint or Random Balanced Pick
            var questionCount = NormalizeQuestionCount(dto.QuestionCount);
            var durationMinutes = NormalizeDuration(dto.DurationMinutes);
            
            var questions = await BuildAttemptQuestionPool(dto)
                .Include(question => question.Passage)
                .Include(question => question.Options.OrderBy(option => option.OrderIndex))
                .ToListAsync();

            selectedQuestions = questions
                .OrderBy(_ => Random.Shared.Next())
                .Take(questionCount)
                .ToList();
        }
        else
        {
            // Practice Mode: Sequential by OrderIndex
            var query = BuildAttemptQuestionPool(dto);
            
            selectedQuestions = await query
                .Include(question => question.Passage)
                .Include(question => question.Options.OrderBy(option => option.OrderIndex))
                .OrderBy(question => question.Topic)
                .ThenBy(question => question.OrderIndex)
                .ToListAsync();
        }

        if (selectedQuestions.Count == 0)
        {
            return null;
        }

        var duration = mode == ExamAttemptModes.Exam ? NormalizeDuration(dto.DurationMinutes) : 9999;
        var now = DateTime.UtcNow;
        var attempt = new ExamAttempt
        {
            UserId = userId,
            CourseCode = courseCode,
            Mode = mode,
            StartedAt = now,
            ExpiresAt = now.AddMinutes(duration),
            DurationMinutes = duration,
            TotalQuestions = selectedQuestions.Count,
            Status = ExamAttemptStatuses.InProgress
        };

        var sequence = 1;
        foreach (var question in selectedQuestions)
        {
            attempt.Answers.Add(new ExamAttemptAnswer
            {
                QuestionId = question.Id,
                SequenceNumber = sequence++
            });
        }

        _db.ExamAttempts.Add(attempt);
        await _db.SaveChangesAsync();

        return await GetAttemptAsync(userId, attempt.Id);
    }

    public async Task<ExamAttemptDto?> GetAttemptAsync(Guid userId, Guid attemptId)
    {
        var attempt = await LoadAttemptAsync(userId, attemptId);
        if (attempt == null)
        {
            return null;
        }

        if (attempt.Status == ExamAttemptStatuses.InProgress && DateTime.UtcNow >= attempt.ExpiresAt)
        {
            await CompleteAttemptAsync(attempt, ExamAttemptStatuses.Expired);
        }

        return MapAttempt(attempt);
    }

    public async Task<ExamAttemptAnswerDto?> SaveAttemptAnswerAsync(Guid userId, Guid attemptId, SaveExamAttemptAnswerDto dto)
    {
        var attempt = await _db.ExamAttempts
            .Include(item => item.Answers)
            .ThenInclude(answer => answer.Question)
            .ThenInclude(question => question.Options)
            .FirstOrDefaultAsync(item => item.Id == attemptId && item.UserId == userId);

        if (attempt == null || attempt.Status != ExamAttemptStatuses.InProgress || DateTime.UtcNow >= attempt.ExpiresAt)
        {
            return null;
        }

        var answer = attempt.Answers.FirstOrDefault(item => item.QuestionId == dto.QuestionId);
        if (answer == null)
        {
            return null;
        }

        var selectedOption = answer.Question.Options.FirstOrDefault(option => option.Id == dto.SelectedOptionId);
        if (selectedOption == null)
        {
            return null;
        }

        answer.SelectedOptionId = selectedOption.Id;
        answer.IsCorrect = selectedOption.IsCorrect;
        answer.AnsweredAt = DateTime.UtcNow;
        answer.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapAttemptAnswer(answer, revealCorrect: false);
    }

    public async Task<ExamAttemptDto?> SubmitAttemptAsync(Guid userId, Guid attemptId)
    {
        var attempt = await LoadAttemptAsync(userId, attemptId);
        if (attempt == null)
        {
            return null;
        }

        if (attempt.Status == ExamAttemptStatuses.InProgress)
        {
            var status = DateTime.UtcNow >= attempt.ExpiresAt
                ? ExamAttemptStatuses.Expired
                : ExamAttemptStatuses.Submitted;
            await CompleteAttemptAsync(attempt, status);
        }

        return MapAttempt(attempt);
    }

    public async Task<ExamAttemptReviewDto?> GetAttemptReviewAsync(Guid userId, Guid attemptId)
    {
        var attempt = await LoadAttemptAsync(userId, attemptId);
        if (attempt == null)
        {
            return null;
        }

        if (attempt.Status == ExamAttemptStatuses.InProgress && DateTime.UtcNow >= attempt.ExpiresAt)
        {
            await CompleteAttemptAsync(attempt, ExamAttemptStatuses.Expired);
        }

        return MapAttemptReview(attempt);
    }

    private IQueryable<ExamQuestion> BuildActiveQuestionQuery(string? topic, string? level)
    {
        var normalizedTopic = NormalizeOptional(topic);
        var normalizedLevel = NormalizeLevel(level);
        var query = _db.ExamQuestions
            .Where(question => question.IsActive && question.Options.Any(option => option.IsCorrect));

        if (!string.IsNullOrWhiteSpace(normalizedTopic))
        {
            query = query.Where(question => question.Topic == normalizedTopic);
        }

        if (!string.IsNullOrWhiteSpace(normalizedLevel))
        {
            query = query.Where(question => question.Level == normalizedLevel);
        }

        return query;
    }

    private async Task<HashSet<string>> GetAccessibleCourseCodesAsync(Guid userId)
    {
        var courses = await _db.ExamCourses
            .Where(course => course.IsActive)
            .Select(course => new
            {
                course.Code,
                course.AccessTier,
                course.PackageCode
            })
            .ToListAsync();

        return courses
            .Where(course => _paymentAccess.HasContentAccess(userId, course.AccessTier, course.PackageCode))
            .Select(course => course.Code)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private bool IsCourseLocked(Guid userId, ExamCourse course)
    {
        return _paymentAccess.IsContentLocked(userId, course.AccessTier, course.PackageCode);
    }

    private IQueryable<ExamQuestion> BuildAttemptQuestionPool(StartExamAttemptDto dto)
    {
        var normalizedLevel = NormalizeLevel(dto.Level);
        var normalizedCourse = NormalizeOptional(dto.CourseCode);
        var topics = dto.Topics
            .Select(NormalizeOptional)
            .Where(topic => !string.IsNullOrWhiteSpace(topic))
            .ToHashSet();

        var query = _db.ExamQuestions
            .Where(question => question.IsActive && question.Options.Any(option => option.IsCorrect));

        if (!string.IsNullOrWhiteSpace(normalizedCourse))
        {
            query = query.Where(question => question.CourseCode == normalizedCourse);
        }

        if (!string.IsNullOrWhiteSpace(normalizedLevel))
        {
            query = query.Where(question => question.Level == normalizedLevel);
        }

        if (topics.Count > 0)
        {
            query = query.Where(question => topics.Contains(question.Topic));
        }

        return query;
    }

    private async Task<ExamAttempt?> LoadAttemptAsync(Guid userId, Guid attemptId)
    {
        return await _db.ExamAttempts
            .Include(item => item.Answers.OrderBy(answer => answer.SequenceNumber))
            .ThenInclude(answer => answer.Question)
            .ThenInclude(question => question.Passage)
            .Include(item => item.Answers.OrderBy(answer => answer.SequenceNumber))
            .ThenInclude(answer => answer.Question)
            .ThenInclude(question => question.Options.OrderBy(option => option.OrderIndex))
            .FirstOrDefaultAsync(item => item.Id == attemptId && item.UserId == userId);
    }

    private async Task CompleteAttemptAsync(ExamAttempt attempt, string status)
    {
        attempt.CorrectCount = attempt.Answers.Count(answer => answer.IsCorrect == true);
        attempt.TotalQuestions = attempt.Answers.Count;
        attempt.ScorePercent = attempt.TotalQuestions == 0
            ? 0
            : Math.Round(attempt.CorrectCount * 100.0 / attempt.TotalQuestions, 2);
        attempt.Status = status;
        attempt.SubmittedAt ??= DateTime.UtcNow;
        attempt.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    private static ExamQuestionDto MapQuestion(ExamQuestion question)
    {
        return new ExamQuestionDto
        {
            Id = question.Id,
            QuestionType = question.QuestionType,
            Topic = question.Topic,
            CourseCode = question.CourseCode,
            Level = question.Level,
            QuestionText = question.QuestionText,
            PassageId = question.PassageId
        };
    }

    private static ExamQuestionDetailDto MapQuestionDetail(ExamQuestion question)
    {
        return new ExamQuestionDetailDto
        {
            Id = question.Id,
            QuestionType = question.QuestionType,
            Topic = question.Topic,
            CourseCode = question.CourseCode,
            Level = question.Level,
            QuestionText = question.QuestionText,
            PassageId = question.PassageId,
            Passage = question.Passage == null ? null : MapPassage(question.Passage),
            Options = question.Options
                .OrderBy(option => option.OrderIndex)
                .Select(MapOption)
                .ToList()
        };
    }

    private static ExamAttemptDto MapAttempt(ExamAttempt attempt)
    {
        return new ExamAttemptDto
        {
            Id = attempt.Id,
            CourseCode = attempt.CourseCode,
            Mode = attempt.Mode,
            Status = attempt.Status,
            StartedAt = attempt.StartedAt,
            ExpiresAt = attempt.ExpiresAt,
            SubmittedAt = attempt.SubmittedAt,
            DurationMinutes = attempt.DurationMinutes,
            TotalQuestions = attempt.TotalQuestions,
            CorrectCount = attempt.CorrectCount,
            ScorePercent = attempt.ScorePercent,
            Questions = attempt.Answers
                .OrderBy(answer => answer.SequenceNumber)
                .Select(answer =>
                {
                    var question = answer.Question;
                    return new ExamAttemptQuestionDto
                    {
                        Id = question.Id,
                        QuestionType = question.QuestionType,
                        Topic = question.Topic,
                        CourseCode = question.CourseCode,
                        Level = question.Level,
                        QuestionText = question.QuestionText,
                        PassageId = question.PassageId,
                        Passage = question.Passage == null ? null : MapPassage(question.Passage),
                        Options = question.Options.OrderBy(option => option.OrderIndex).Select(MapOption).ToList(),
                        AttemptAnswerId = answer.Id,
                        SelectedOptionId = answer.SelectedOptionId,
                        IsCorrect = attempt.Status == ExamAttemptStatuses.InProgress ? null : answer.IsCorrect,
                        SequenceNumber = answer.SequenceNumber
                    };
                })
                .ToList()
        };
    }

    private static ExamAttemptReviewDto MapAttemptReview(ExamAttempt attempt)
    {
        return new ExamAttemptReviewDto
        {
            Id = attempt.Id,
            CourseCode = attempt.CourseCode,
            Mode = attempt.Mode,
            Status = attempt.Status,
            StartedAt = attempt.StartedAt,
            SubmittedAt = attempt.SubmittedAt,
            TotalQuestions = attempt.TotalQuestions,
            CorrectCount = attempt.CorrectCount,
            ScorePercent = attempt.ScorePercent,
            Questions = attempt.Answers
                .OrderBy(answer => answer.SequenceNumber)
                .Select(answer =>
                {
                    var question = answer.Question;
                    var correctOption = question.Options.First(option => option.IsCorrect);
                    return new ExamAttemptReviewQuestionDto
                    {
                        Id = question.Id,
                        QuestionType = question.QuestionType,
                        Topic = question.Topic,
                        CourseCode = question.CourseCode,
                        Level = question.Level,
                        QuestionText = question.QuestionText,
                        PassageId = question.PassageId,
                        Passage = question.Passage == null ? null : MapPassage(question.Passage),
                        Options = question.Options.OrderBy(option => option.OrderIndex).Select(MapReviewOption).ToList(),
                        SelectedOptionId = answer.SelectedOptionId,
                        CorrectOptionId = correctOption.Id,
                        IsCorrect = answer.IsCorrect == true,
                        Explanation = question.Explanation,
                        SequenceNumber = answer.SequenceNumber
                    };
                })
                .ToList()
        };
    }

    private static ExamAttemptAnswerDto MapAttemptAnswer(ExamAttemptAnswer answer, bool revealCorrect)
    {
        return new ExamAttemptAnswerDto
        {
            Id = answer.Id,
            QuestionId = answer.QuestionId,
            SelectedOptionId = answer.SelectedOptionId,
            IsCorrect = revealCorrect ? answer.IsCorrect : null,
            AnsweredAt = answer.AnsweredAt,
            SequenceNumber = answer.SequenceNumber
        };
    }

    private static ExamPassageDto MapPassage(ExamPassage passage)
    {
        return new ExamPassageDto
        {
            Id = passage.Id,
            Title = passage.Title,
            Content = passage.Content,
            Level = passage.Level,
            Topic = passage.Topic,
            CourseCode = passage.CourseCode
        };
    }

    private static ExamQuestionOptionDto MapOption(ExamQuestionOption option)
    {
        return new ExamQuestionOptionDto
        {
            Id = option.Id,
            Label = option.Label,
            Text = option.Text
        };
    }

    private static ExamQuestionReviewOptionDto MapReviewOption(ExamQuestionOption option)
    {
        return new ExamQuestionReviewOptionDto
        {
            Id = option.Id,
            Label = option.Label,
            Text = option.Text,
            IsCorrect = option.IsCorrect
        };
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();
    }

    private static string? NormalizeLevel(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();
    }

    private static int NormalizeQuestionCount(int value)
    {
        if (value <= 0)
        {
            return ExamDefaults.AttemptQuestionCount;
        }

        return Math.Min(value, ExamDefaults.AttemptQuestionCount);
    }

    private static int NormalizeDuration(int value)
    {
        if (value <= 0)
        {
            return ExamDefaults.AttemptDurationMinutes;
        }

        return Math.Min(value, ExamDefaults.AttemptDurationMinutes);
    }

    private static string GetTopicLabel(string topic)
    {
        return ExamQuestionTopics.GetLabel(topic);
    }
}
