using JPLearn.Core.StaticVocabulary;
using JPLearn.Core.StaticVocabulary.DTOs;
using JPLearn.Core.StaticVocabulary.Entities;
using JPLearn.Core.Payments;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class StaticVocabularyService : IStaticVocabularyService
{
    private readonly AppDbContext _db;
    private readonly IPaymentAccessService _paymentAccess;

    public StaticVocabularyService(AppDbContext db, IPaymentAccessService paymentAccess)
    {
        _db = db;
        _paymentAccess = paymentAccess;
    }

    public async Task<List<VocabularyCourseDto>> GetCoursesAsync(Guid userId)
    {
        var courses = await _db.VocabularyCourses
            .Include(course => course.Lessons)
            .ThenInclude(lesson => lesson.Items)
            .ThenInclude(item => item.ProgressRecords.Where(progress => progress.UserId == userId))
            .OrderBy(course => course.OrderIndex)
            .ToListAsync();

        return courses.Select(course =>
        {
            var items = course.Lessons.SelectMany(lesson => lesson.Items).ToList();
            var progress = items.SelectMany(item => item.ProgressRecords).ToList();

            return new VocabularyCourseDto
            {
                Id = course.Id,
                Code = course.Code,
                Title = course.Title,
                Description = course.Description,
                LessonCount = course.Lessons.Count,
                WordCount = items.Count,
                LearnedCount = progress.Count(item => item.IsLearned),
                PracticedCount = progress.Count(IsPracticed)
            };
        }).ToList();
    }

    public async Task<List<StaticVocabularyLessonDto>?> GetLessonsByCourseAsync(Guid userId, string courseCode)
    {
        if (!VocabularyCourseCodes.IsValid(courseCode))
        {
            return null;
        }

        var normalizedCourse = VocabularyCourseCodes.Normalize(courseCode);
        var lessons = await _db.StaticVocabularyLessons
            .Include(lesson => lesson.Items)
            .ThenInclude(item => item.ProgressRecords.Where(progress => progress.UserId == userId))
            .Where(lesson => lesson.CourseCode == normalizedCourse)
            .OrderBy(lesson => lesson.OrderIndex)
            .ThenBy(lesson => lesson.LessonNumber)
            .ToListAsync();

        return lessons.Select(lesson => MapLesson(userId, lesson)).ToList();
    }

    public async Task<StaticVocabularyLessonDetailDto?> GetLessonDetailAsync(Guid userId, Guid lessonId)
    {
        var lesson = await _db.StaticVocabularyLessons
            .Include(item => item.Items.OrderBy(vocabulary => vocabulary.OrderIndex))
            .ThenInclude(item => item.ProgressRecords.Where(progress => progress.UserId == userId))
            .FirstOrDefaultAsync(item => item.Id == lessonId);

        if (lesson == null)
        {
            return null;
        }

        if (_paymentAccess.IsContentLocked(userId, lesson.AccessTier, lesson.PackageCode))
        {
            return null;
        }

        return new StaticVocabularyLessonDetailDto
        {
            Lesson = MapLesson(userId, lesson),
            Items = lesson.Items
                .OrderBy(item => item.OrderIndex)
                .Select(item => MapItem(userId, item))
                .ToList()
        };
    }

    public async Task<StaticVocabularyItemDto?> GetItemDetailAsync(Guid userId, Guid itemId)
    {
        var item = await _db.StaticVocabularyItems
            .Include(vocabulary => vocabulary.Lesson)
            .Include(vocabulary => vocabulary.ProgressRecords.Where(progress => progress.UserId == userId))
            .FirstOrDefaultAsync(vocabulary => vocabulary.Id == itemId);

        if (item == null)
        {
            return null;
        }

        return _paymentAccess.IsContentLocked(userId, ResolveAccessTier(item), ResolvePackageCode(item))
            ? null
            : MapItem(userId, item);
    }

    public async Task<List<StaticVocabularyItemDto>> SearchAsync(Guid userId, string query, string? courseCode)
    {
        var normalizedQuery = query.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalizedQuery))
        {
            return [];
        }

        var itemsQuery = _db.StaticVocabularyItems
            .Include(item => item.Lesson)
            .Include(item => item.ProgressRecords.Where(progress => progress.UserId == userId))
            .Where(item =>
                item.Word.ToLower().Contains(normalizedQuery)
                || item.Reading.ToLower().Contains(normalizedQuery)
                || item.Meaning.ToLower().Contains(normalizedQuery)
                || item.WordType.ToLower().Contains(normalizedQuery));

        if (!string.IsNullOrWhiteSpace(courseCode) && VocabularyCourseCodes.IsValid(courseCode))
        {
            var normalizedCourse = VocabularyCourseCodes.Normalize(courseCode);
            itemsQuery = itemsQuery.Where(item => item.CourseCode == normalizedCourse);
        }

        var items = await itemsQuery
            .OrderBy(item => item.CourseCode)
            .ThenBy(item => item.Lesson.OrderIndex)
            .ThenBy(item => item.OrderIndex)
            .Take(50)
            .ToListAsync();

        return items
            .Where(item => !_paymentAccess.IsContentLocked(userId, ResolveAccessTier(item), ResolvePackageCode(item)))
            .Select(item => MapItem(userId, item))
            .ToList();
    }

    public async Task<List<VocabularyPracticeCardDto>?> GetLessonPracticeCardsAsync(Guid userId, Guid lessonId, string mode)
    {
        var normalizedMode = VocabularyPracticeModes.Normalize(mode);
        var lesson = await _db.StaticVocabularyLessons.FirstOrDefaultAsync(item => item.Id == lessonId);
        if (lesson == null)
        {
            return null;
        }

        if (_paymentAccess.IsContentLocked(userId, lesson.AccessTier, lesson.PackageCode))
        {
            return null;
        }

        var lessonItems = await _db.StaticVocabularyItems
            .Where(item => item.LessonId == lessonId)
            .OrderBy(item => item.OrderIndex)
            .ToListAsync();

        var distractors = await _db.StaticVocabularyItems
            .Where(item => item.LessonId != lessonId)
            .OrderBy(item => item.CourseCode)
            .ThenBy(item => item.OrderIndex)
            .Take(80)
            .Select(item => item.Meaning)
            .ToListAsync();

        return lessonItems.Select(item => MapPracticeCard(item, normalizedMode, distractors)).ToList();
    }

    public Task<StaticVocabularyProgressDto?> RecordViewAsync(Guid userId, Guid itemId)
    {
        return UpdateProgressAsync(userId, itemId, progress =>
        {
            progress.LastViewedAt = DateTime.UtcNow;
        });
    }

    public Task<StaticVocabularyProgressDto?> RecordFlashcardPracticeAsync(Guid userId, Guid itemId)
    {
        return UpdateProgressAsync(userId, itemId, progress =>
        {
            progress.IsLearned = true;
            progress.LastViewedAt = DateTime.UtcNow;
            progress.FlashcardPracticeCount += 1;
        });
    }

    public Task<StaticVocabularyProgressDto?> RecordMultipleChoicePracticeAsync(Guid userId, Guid itemId)
    {
        return UpdateProgressAsync(userId, itemId, progress =>
        {
            progress.IsLearned = true;
            progress.LastViewedAt = DateTime.UtcNow;
            progress.MultipleChoicePracticeCount += 1;
        });
    }

    public Task<StaticVocabularyProgressDto?> RecordTypingPracticeAsync(Guid userId, Guid itemId)
    {
        return UpdateProgressAsync(userId, itemId, progress =>
        {
            progress.IsLearned = true;
            progress.LastViewedAt = DateTime.UtcNow;
            progress.TypingPracticeCount += 1;
        });
    }

    private async Task<StaticVocabularyProgressDto?> UpdateProgressAsync(Guid userId, Guid itemId, Action<UserVocabularyProgress> update)
    {
        var item = await _db.StaticVocabularyItems
            .Include(vocabulary => vocabulary.Lesson)
            .FirstOrDefaultAsync(vocabulary => vocabulary.Id == itemId);
        if (item == null)
        {
            return null;
        }

        if (_paymentAccess.IsContentLocked(userId, ResolveAccessTier(item), ResolvePackageCode(item)))
        {
            return null;
        }

        var progress = await _db.UserVocabularyProgress
            .FirstOrDefaultAsync(item => item.UserId == userId && item.VocabularyItemId == itemId);

        if (progress == null)
        {
            progress = new UserVocabularyProgress
            {
                UserId = userId,
                VocabularyItemId = itemId
            };
            _db.UserVocabularyProgress.Add(progress);
        }

        update(progress);
        progress.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return MapProgress(progress);
    }

    private StaticVocabularyLessonDto MapLesson(Guid userId, VocabularyLesson lesson)
    {
        var progress = lesson.Items.SelectMany(item => item.ProgressRecords).ToList();

        return new StaticVocabularyLessonDto
        {
            Id = lesson.Id,
            CourseCode = lesson.CourseCode,
            LessonNumber = lesson.LessonNumber,
            Title = lesson.Title,
            Description = lesson.Description,
            AccessTier = lesson.AccessTier,
            PackageCode = lesson.PackageCode,
            IsLocked = _paymentAccess.IsContentLocked(userId, lesson.AccessTier, lesson.PackageCode),
            WordCount = lesson.Items.Count,
            LearnedCount = progress.Count(item => item.IsLearned),
            PracticedCount = progress.Count(IsPracticed)
        };
    }

    private StaticVocabularyItemDto MapItem(Guid userId, StaticVocabularyItem item)
    {
        var progress = item.ProgressRecords.FirstOrDefault();
        var accessTier = ResolveAccessTier(item);
        var packageCode = ResolvePackageCode(item);

        return new StaticVocabularyItemDto
        {
            Id = item.Id,
            LessonId = item.LessonId,
            CourseCode = item.CourseCode,
            Word = item.Word,
            Reading = item.Reading,
            WordType = item.WordType,
            Meaning = item.Meaning,
            ExampleJapanese = item.ExampleJapanese,
            ExampleReading = item.ExampleReading,
            ExampleMeaning = item.ExampleMeaning,
            Notes = item.Notes,
            AccessTier = accessTier,
            PackageCode = packageCode,
            IsLocked = _paymentAccess.IsContentLocked(userId, accessTier, packageCode),
            IsLearned = progress?.IsLearned == true,
            FlashcardPracticeCount = progress?.FlashcardPracticeCount ?? 0,
            MultipleChoicePracticeCount = progress?.MultipleChoicePracticeCount ?? 0,
            TypingPracticeCount = progress?.TypingPracticeCount ?? 0,
            OrderIndex = item.OrderIndex
        };
    }

    private static VocabularyPracticeCardDto MapPracticeCard(
        StaticVocabularyItem item,
        string mode,
        List<string> distractors)
    {
        var options = mode == VocabularyPracticeModes.MultipleChoice
            ? BuildOptions(item.Meaning, distractors)
            : [];

        return new VocabularyPracticeCardDto
        {
            ItemId = item.Id,
            Mode = mode,
            Prompt = mode == VocabularyPracticeModes.Typing ? item.Meaning : item.Word,
            PromptReading = mode == VocabularyPracticeModes.Typing ? null : item.Reading,
            CorrectAnswer = mode == VocabularyPracticeModes.Typing ? item.Word : item.Meaning,
            Options = options,
            Word = item.Word,
            Reading = item.Reading,
            Meaning = item.Meaning,
            ExampleJapanese = item.ExampleJapanese,
            ExampleMeaning = item.ExampleMeaning
        };
    }

    private static List<string> BuildOptions(string correctAnswer, List<string> distractors)
    {
        var options = distractors
            .Where(option => !string.Equals(option, correctAnswer, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(3)
            .Append(correctAnswer)
            .OrderBy(_ => Guid.NewGuid())
            .ToList();

        return options;
    }

    private static StaticVocabularyProgressDto MapProgress(UserVocabularyProgress progress)
    {
        return new StaticVocabularyProgressDto
        {
            Id = progress.Id,
            VocabularyItemId = progress.VocabularyItemId,
            IsLearned = progress.IsLearned,
            LastViewedAt = progress.LastViewedAt,
            FlashcardPracticeCount = progress.FlashcardPracticeCount,
            MultipleChoicePracticeCount = progress.MultipleChoicePracticeCount,
            TypingPracticeCount = progress.TypingPracticeCount
        };
    }

    private static bool IsPracticed(UserVocabularyProgress progress)
    {
        return progress.FlashcardPracticeCount > 0
            || progress.MultipleChoicePracticeCount > 0
            || progress.TypingPracticeCount > 0;
    }

    private static string ResolveAccessTier(StaticVocabularyItem item)
    {
        return string.IsNullOrWhiteSpace(item.AccessTierOverride)
            ? item.Lesson?.AccessTier ?? VocabularyAccessTiers.Free
            : item.AccessTierOverride;
    }

    private static string? ResolvePackageCode(StaticVocabularyItem item)
    {
        return string.IsNullOrWhiteSpace(item.PackageCodeOverride)
            ? item.Lesson?.PackageCode
            : item.PackageCodeOverride;
    }
}
