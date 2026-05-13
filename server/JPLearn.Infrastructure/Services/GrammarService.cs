using JPLearn.Core.Grammar;
using JPLearn.Core.Grammar.DTOs;
using JPLearn.Core.Grammar.Entities;
using JPLearn.Core.Payments;
using JPLearn.Core.Review;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JPLearn.Infrastructure.Services;

public class GrammarService : IGrammarService
{
    private readonly AppDbContext _db;
    private readonly IPaymentAccessService _paymentAccess;

    public GrammarService(AppDbContext db, IPaymentAccessService paymentAccess)
    {
        _db = db;
        _paymentAccess = paymentAccess;
    }

    public async Task<List<GrammarLevelDto>> GetLevelsAsync(Guid userId)
    {
        var lessons = await _db.GrammarLessons
            .Include(lesson => lesson.Patterns)
            .ThenInclude(pattern => pattern.ProgressRecords.Where(progress => progress.UserId == userId))
            .ToListAsync();

        return lessons
            .GroupBy(l => l.CourseCode)
            .Select(group =>
            {
                var levelLessons = group.ToList();
                var firstLesson = levelLessons.First();
                var patterns = levelLessons
                    .SelectMany(lesson => lesson.Patterns)
                    .ToList();
                var progress = patterns
                    .SelectMany(pattern => pattern.ProgressRecords)
                    .Where(item => item.IsActive)
                    .ToList();

                return new GrammarLevelDto
                {
                    Level = firstLesson.Level,
                    CourseCode = group.Key,
                    LessonCount = levelLessons.Count,
                    PatternCount = patterns.Count,
                    FreeCount = patterns.Count(pattern => ResolveAccessTier(pattern) == GrammarAccessTiers.Free),
                    PremiumCount = patterns.Count(pattern => ResolveAccessTier(pattern) == GrammarAccessTiers.Premium),
                    InStudyCount = progress.Count,
                    MasteredCount = progress.Count(item => item.Level >= ReviewLevels.Mastered),
                    DueCount = progress.Count(item => item.NextReviewAt <= DateTime.UtcNow)
                };
            })
            .OrderBy(dto => dto.Level)
            .ThenBy(dto => dto.CourseCode)
            .ToList();
    }

    public async Task<List<GrammarLessonDto>?> GetLessonsByLevelAsync(Guid userId, string level, string? courseCode = null)
    {
        if (!GrammarLevels.IsValid(level))
        {
            return null;
        }

        var normalizedLevel = GrammarLevels.Normalize(level);
        var query = _db.GrammarLessons
            .Include(lesson => lesson.Patterns)
            .ThenInclude(pattern => pattern.ProgressRecords.Where(progress => progress.UserId == userId))
            .Where(lesson => lesson.Level == normalizedLevel);

        if (!string.IsNullOrWhiteSpace(courseCode))
        {
            var normalizedCourse = courseCode.Trim().ToLowerInvariant();
            query = query.Where(lesson => lesson.CourseCode.ToLower() == normalizedCourse);
        }

        var lessons = await query
            .OrderBy(lesson => lesson.OrderIndex)
            .ThenBy(lesson => lesson.LessonNumber)
            .ToListAsync();

        return lessons.Select(lesson => MapLesson(userId, lesson)).ToList();
    }

    public async Task<GrammarLessonDto?> GetLessonAsync(Guid userId, Guid lessonId)
    {
        var lesson = await _db.GrammarLessons
            .Include(item => item.Patterns)
            .ThenInclude(pattern => pattern.ProgressRecords.Where(progress => progress.UserId == userId))
            .FirstOrDefaultAsync(item => item.Id == lessonId);

        return lesson == null ? null : MapLesson(userId, lesson);
    }

    public async Task<List<GrammarPatternDto>?> GetLessonPatternsAsync(Guid userId, Guid lessonId)
    {
        var exists = await _db.GrammarLessons.AnyAsync(lesson => lesson.Id == lessonId);
        if (!exists)
        {
            return null;
        }

        var patterns = await _db.GrammarPatterns
            .Include(pattern => pattern.Lesson)
            .Include(pattern => pattern.ProgressRecords.Where(progress => progress.UserId == userId))
            .Where(pattern => pattern.LessonId == lessonId)
            .OrderBy(pattern => pattern.OrderIndex)
            .ToListAsync();

        return patterns.Select(pattern => MapPattern(pattern, userId)).ToList();
    }

    public async Task<GrammarPatternDetailDto?> GetPatternDetailAsync(Guid userId, Guid patternId)
    {
        var pattern = await _db.GrammarPatterns
            .Include(item => item.Lesson)
            .Include(item => item.Examples.OrderBy(example => example.OrderIndex))
            .Include(item => item.Exercises.OrderBy(exercise => exercise.OrderIndex))
            .Include(item => item.ProgressRecords.Where(progress => progress.UserId == userId))
            .FirstOrDefaultAsync(item => item.Id == patternId);

        return pattern == null ? null : MapPatternDetail(pattern, userId);
    }

    public async Task<List<GrammarPatternDto>> SearchPatternsAsync(Guid userId, string query)
    {
        var normalizedQuery = query.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalizedQuery))
        {
            return [];
        }

        var patterns = await _db.GrammarPatterns
            .Include(pattern => pattern.Lesson)
            .Include(pattern => pattern.ProgressRecords.Where(progress => progress.UserId == userId))
            .Where(pattern =>
                pattern.Pattern.ToLower().Contains(normalizedQuery)
                || pattern.Title.ToLower().Contains(normalizedQuery)
                || pattern.Meaning.ToLower().Contains(normalizedQuery)
                || pattern.Structure.ToLower().Contains(normalizedQuery)
                || (pattern.TagsJson != null && pattern.TagsJson.ToLower().Contains(normalizedQuery)))
            .OrderBy(pattern => pattern.Level)
            .ThenBy(pattern => pattern.Lesson.OrderIndex)
            .ThenBy(pattern => pattern.OrderIndex)
            .Take(50)
            .ToListAsync();

        return patterns.Select(pattern => MapPattern(pattern, userId)).ToList();
    }

    private GrammarLessonDto MapLesson(Guid userId, GrammarLesson lesson)
    {
        var progress = lesson.Patterns
            .SelectMany(pattern => pattern.ProgressRecords)
            .Where(item => item.IsActive)
            .ToList();

        return new GrammarLessonDto
        {
            Id = lesson.Id,
            Level = lesson.Level,
            LessonNumber = lesson.LessonNumber,
            Title = lesson.Title,
            Description = lesson.Description,
            AccessTier = lesson.AccessTier,
            PackageCode = lesson.PackageCode,
            CourseCode = lesson.CourseCode,
            IsLocked = _paymentAccess.IsContentLocked(userId, lesson.AccessTier, lesson.PackageCode ?? lesson.CourseCode),
            PatternCount = lesson.Patterns.Count,
            InStudyCount = progress.Count,
            MasteredCount = progress.Count(item => item.Level >= ReviewLevels.Mastered),
            DueCount = progress.Count(item => item.NextReviewAt <= DateTime.UtcNow)
        };
    }

    private GrammarPatternDto MapPattern(GrammarPattern pattern, Guid userId)
    {
        var progress = pattern.ProgressRecords.FirstOrDefault(item => item.UserId == userId && item.IsActive);
        var accessTier = ResolveAccessTier(pattern);
        var packageCode = ResolvePackageCode(pattern);

        return new GrammarPatternDto
        {
            Id = pattern.Id,
            LessonId = pattern.LessonId,
            Level = pattern.Level,
            CourseCode = pattern.Lesson.CourseCode,
            Pattern = pattern.Pattern,
            Title = pattern.Title,
            Meaning = pattern.Meaning,
            Structure = pattern.Structure,
            AccessTier = accessTier,
            PackageCode = packageCode,
            IsLocked = _paymentAccess.IsContentLocked(userId, accessTier, packageCode ?? pattern.Lesson.CourseCode),
            IsInStudy = progress?.IsActive == true,
            Progress = progress == null ? null : MapProgress(progress)
        };
    }

    private GrammarPatternDetailDto MapPatternDetail(GrammarPattern pattern, Guid userId)
    {
        var baseDto = MapPattern(pattern, userId);
        return new GrammarPatternDetailDto
        {
            Id = baseDto.Id,
            LessonId = baseDto.LessonId,
            Level = baseDto.Level,
            Pattern = baseDto.Pattern,
            Title = baseDto.Title,
            Meaning = baseDto.Meaning,
            Structure = baseDto.Structure,
            AccessTier = baseDto.AccessTier,
            PackageCode = baseDto.PackageCode,
            IsLocked = baseDto.IsLocked,
            IsInStudy = baseDto.IsInStudy,
            Progress = baseDto.Progress,
            UsageScope = pattern.UsageScope,
            Formation = pattern.Formation,
            Notes = pattern.Notes,
            TagsJson = pattern.TagsJson,
            Examples = pattern.Examples
                .OrderBy(example => example.OrderIndex)
                .Select(MapExample)
                .ToList(),
            Exercises = pattern.Exercises
                .OrderBy(exercise => exercise.OrderIndex)
                .Select(MapExercise)
                .ToList()
        };
    }

    internal static GrammarProgressDto MapProgress(UserGrammarProgress progress)
    {
        return new GrammarProgressDto
        {
            Id = progress.Id,
            PatternId = progress.GrammarPatternId,
            Level = Math.Clamp(progress.Level, ReviewLevels.Min, ReviewLevels.Max),
            Status = progress.Status,
            NextReviewAt = progress.NextReviewAt,
            IntervalDays = progress.IntervalDays,
            Repetitions = progress.Repetitions,
            LapseCount = progress.LapseCount,
            IsActive = progress.IsActive
        };
    }

    internal static GrammarExampleDto MapExample(GrammarExample example)
    {
        return new GrammarExampleDto
        {
            Id = example.Id,
            Japanese = example.Japanese,
            Reading = example.Reading,
            Meaning = example.Meaning,
            Note = example.Note,
            OrderIndex = example.OrderIndex
        };
    }

    internal static GrammarExerciseDto MapExercise(GrammarExercise exercise)
    {
        return new GrammarExerciseDto
        {
            Id = exercise.Id,
            PatternId = exercise.PatternId,
            ExerciseType = exercise.ExerciseType,
            Prompt = exercise.Prompt,
            PromptReading = exercise.PromptReading,
            Hint = exercise.Hint,
            Explanation = exercise.Explanation,
            TemplateText = exercise.TemplateText,
            OptionsJson = exercise.OptionsJson,
            Options = DeserializeStringList(exercise.OptionsJson),
            StarPosition = exercise.StarPosition,
            OrderIndex = exercise.OrderIndex
        };
    }

    internal static GrammarExerciseAnswerDto MapExerciseAnswer(GrammarExercise exercise)
    {
        var dto = MapExercise(exercise);
        return new GrammarExerciseAnswerDto
        {
            Id = dto.Id,
            PatternId = dto.PatternId,
            ExerciseType = dto.ExerciseType,
            Prompt = dto.Prompt,
            PromptReading = dto.PromptReading,
            Hint = dto.Hint,
            Explanation = dto.Explanation,
            TemplateText = dto.TemplateText,
            OptionsJson = dto.OptionsJson,
            StarPosition = dto.StarPosition,
            OrderIndex = dto.OrderIndex,
            ExpectedAnswer = exercise.ExpectedAnswer,
            AcceptableAnswersJson = exercise.AcceptableAnswersJson,
            AcceptableAnswers = DeserializeStringList(exercise.AcceptableAnswersJson),
            CorrectOrderJson = exercise.CorrectOrderJson,
            CorrectOrder = DeserializeStringList(exercise.CorrectOrderJson),
            StarAnswer = exercise.StarAnswer
        };
    }

    internal static string ResolveAccessTier(GrammarPattern pattern)
    {
        return string.IsNullOrWhiteSpace(pattern.AccessTierOverride)
            ? pattern.Lesson.AccessTier
            : pattern.AccessTierOverride;
    }

    internal static string? ResolvePackageCode(GrammarPattern pattern)
    {
        return string.IsNullOrWhiteSpace(pattern.PackageCodeOverride)
            ? pattern.Lesson.PackageCode
            : pattern.PackageCodeOverride;
    }

    private static List<string> DeserializeStringList(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }
}
