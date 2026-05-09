using JPLearn.Core.Kanji;
using JPLearn.Core.Kanji.DTOs;
using JPLearn.Core.Kanji.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class KanjiService : IKanjiService
{
    private readonly AppDbContext _db;

    public KanjiService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<KanjiLevelDto>> GetLevelsAsync(Guid userId)
    {
        var lessons = await _db.KanjiLessons
            .Include(lesson => lesson.KanjiItems)
            .ThenInclude(item => item.ProgressRecords.Where(progress => progress.UserId == userId))
            .Include(lesson => lesson.VocabularyItems)
            .ToListAsync();

        return KanjiLevels.All
            .Select(level =>
            {
                var levelLessons = lessons
                    .Where(lesson => lesson.Level == level)
                    .ToList();
                var kanjiItems = levelLessons
                    .SelectMany(lesson => lesson.KanjiItems)
                    .ToList();
                var progress = kanjiItems
                    .SelectMany(item => item.ProgressRecords)
                    .ToList();

                return new KanjiLevelDto
                {
                    Level = level,
                    LessonCount = levelLessons.Count,
                    KanjiCount = kanjiItems.Count,
                    VocabularyCount = levelLessons.Sum(lesson => lesson.VocabularyItems.Count),
                    LearnedCount = progress.Count(item => item.IsLearned),
                    PracticedCount = progress.Count(item => item.WritingPracticeCount > 0 || item.FlashcardPracticeCount > 0)
                };
            })
            .ToList();
    }

    public async Task<List<KanjiLessonDto>?> GetLessonsByLevelAsync(Guid userId, string level)
    {
        if (!KanjiLevels.IsValid(level))
        {
            return null;
        }

        var normalizedLevel = KanjiLevels.Normalize(level);
        var lessons = await _db.KanjiLessons
            .Include(lesson => lesson.KanjiItems)
            .ThenInclude(item => item.ProgressRecords.Where(progress => progress.UserId == userId))
            .Include(lesson => lesson.VocabularyItems)
            .Where(lesson => lesson.Level == normalizedLevel)
            .OrderBy(lesson => lesson.OrderIndex)
            .ThenBy(lesson => lesson.LessonNumber)
            .ToListAsync();

        return lessons.Select(MapLesson).ToList();
    }

    public async Task<KanjiLessonDetailDto?> GetLessonDetailAsync(Guid userId, Guid lessonId)
    {
        var lesson = await _db.KanjiLessons
            .Include(item => item.KanjiItems.OrderBy(kanji => kanji.OrderIndex))
            .ThenInclude(kanji => kanji.ProgressRecords.Where(progress => progress.UserId == userId))
            .Include(item => item.VocabularyItems.OrderBy(vocabulary => vocabulary.OrderIndex))
            .FirstOrDefaultAsync(item => item.Id == lessonId);

        if (lesson == null)
        {
            return null;
        }

        return new KanjiLessonDetailDto
        {
            Lesson = MapLesson(lesson),
            KanjiItems = lesson.KanjiItems
                .OrderBy(item => item.OrderIndex)
                .Select(MapKanjiItem)
                .ToList(),
            VocabularyItems = lesson.VocabularyItems
                .OrderBy(item => item.OrderIndex)
                .Select(MapVocabulary)
                .ToList()
        };
    }

    public async Task<KanjiDetailDto?> GetKanjiDetailAsync(Guid userId, Guid kanjiItemId)
    {
        var item = await _db.KanjiItems
            .Include(kanji => kanji.Lesson)
            .Include(kanji => kanji.ProgressRecords.Where(progress => progress.UserId == userId))
            .Include(kanji => kanji.VocabularyItems.OrderBy(vocabulary => vocabulary.OrderIndex))
            .FirstOrDefaultAsync(kanji => kanji.Id == kanjiItemId);

        return item == null ? null : MapKanjiDetail(item);
    }

    public async Task<List<KanjiItemDto>> SearchAsync(Guid userId, string query)
    {
        var normalizedQuery = query.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalizedQuery))
        {
            return [];
        }

        var items = await _db.KanjiItems
            .Include(item => item.Lesson)
            .Include(item => item.ProgressRecords.Where(progress => progress.UserId == userId))
            .Where(item =>
                item.Character.ToLower().Contains(normalizedQuery)
                || item.HanViet.ToLower().Contains(normalizedQuery)
                || item.Meaning.ToLower().Contains(normalizedQuery)
                || (item.KunReading != null && item.KunReading.ToLower().Contains(normalizedQuery))
                || (item.OnReading != null && item.OnReading.ToLower().Contains(normalizedQuery)))
            .OrderBy(item => item.Level)
            .ThenBy(item => item.Lesson.OrderIndex)
            .ThenBy(item => item.OrderIndex)
            .Take(50)
            .ToListAsync();

        return items.Select(MapKanjiItem).ToList();
    }

    public Task<KanjiProgressDto?> RecordViewAsync(Guid userId, Guid kanjiItemId)
    {
        return UpdateProgressAsync(userId, kanjiItemId, progress =>
        {
            progress.LastViewedAt = DateTime.UtcNow;
        });
    }

    public Task<KanjiProgressDto?> AddToMemoryAsync(Guid userId, Guid kanjiItemId)
    {
        return UpdateProgressAsync(userId, kanjiItemId, progress =>
        {
            progress.IsLearned = true;
            progress.LastViewedAt = DateTime.UtcNow;
        });
    }

    public Task<KanjiProgressDto?> RecordWritingPracticeAsync(Guid userId, Guid kanjiItemId)
    {
        return UpdateProgressAsync(userId, kanjiItemId, progress =>
        {
            progress.LastViewedAt = DateTime.UtcNow;
            progress.WritingPracticeCount += 1;
        });
    }

    public Task<KanjiProgressDto?> RecordFlashcardPracticeAsync(Guid userId, Guid kanjiItemId)
    {
        return UpdateProgressAsync(userId, kanjiItemId, progress =>
        {
            progress.LastViewedAt = DateTime.UtcNow;
            progress.FlashcardPracticeCount += 1;
        });
    }

    private async Task<KanjiProgressDto?> UpdateProgressAsync(Guid userId, Guid kanjiItemId, Action<UserKanjiProgress> update)
    {
        var exists = await _db.KanjiItems.AnyAsync(item => item.Id == kanjiItemId);
        if (!exists)
        {
            return null;
        }

        var progress = await _db.UserKanjiProgress
            .FirstOrDefaultAsync(item => item.UserId == userId && item.KanjiItemId == kanjiItemId);

        if (progress == null)
        {
            progress = new UserKanjiProgress
            {
                UserId = userId,
                KanjiItemId = kanjiItemId
            };
            _db.UserKanjiProgress.Add(progress);
        }

        update(progress);
        progress.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return MapProgress(progress);
    }

    private static KanjiLessonDto MapLesson(KanjiLesson lesson)
    {
        var progress = lesson.KanjiItems
            .SelectMany(item => item.ProgressRecords)
            .ToList();

        return new KanjiLessonDto
        {
            Id = lesson.Id,
            Level = lesson.Level,
            LessonNumber = lesson.LessonNumber,
            Title = lesson.Title,
            Description = lesson.Description,
            AccessTier = lesson.AccessTier,
            PackageCode = lesson.PackageCode,
            IsLocked = false,
            KanjiCount = lesson.KanjiItems.Count,
            VocabularyCount = lesson.VocabularyItems.Count,
            LearnedCount = progress.Count(item => item.IsLearned),
            PracticedCount = progress.Count(item => item.WritingPracticeCount > 0 || item.FlashcardPracticeCount > 0)
        };
    }

    private static KanjiItemDto MapKanjiItem(KanjiItem item)
    {
        var progress = item.ProgressRecords.FirstOrDefault();

        return new KanjiItemDto
        {
            Id = item.Id,
            LessonId = item.LessonId,
            Level = item.Level,
            Character = item.Character,
            HanViet = item.HanViet,
            Meaning = item.Meaning,
            StrokeCount = item.StrokeCount,
            KunReading = item.KunReading,
            OnReading = item.OnReading,
            Mnemonic = item.Mnemonic,
            AccessTier = ResolveAccessTier(item),
            PackageCode = ResolvePackageCode(item),
            IsLocked = false,
            IsLearned = progress?.IsLearned == true,
            WritingPracticeCount = progress?.WritingPracticeCount ?? 0,
            FlashcardPracticeCount = progress?.FlashcardPracticeCount ?? 0,
            OrderIndex = item.OrderIndex
        };
    }

    private static KanjiDetailDto MapKanjiDetail(KanjiItem item)
    {
        var baseDto = MapKanjiItem(item);

        return new KanjiDetailDto
        {
            Id = baseDto.Id,
            LessonId = baseDto.LessonId,
            Level = baseDto.Level,
            Character = baseDto.Character,
            HanViet = baseDto.HanViet,
            Meaning = baseDto.Meaning,
            StrokeCount = baseDto.StrokeCount,
            KunReading = baseDto.KunReading,
            OnReading = baseDto.OnReading,
            Mnemonic = baseDto.Mnemonic,
            AccessTier = baseDto.AccessTier,
            PackageCode = baseDto.PackageCode,
            IsLocked = baseDto.IsLocked,
            IsLearned = baseDto.IsLearned,
            WritingPracticeCount = baseDto.WritingPracticeCount,
            FlashcardPracticeCount = baseDto.FlashcardPracticeCount,
            OrderIndex = baseDto.OrderIndex,
            StrokeSvg = item.StrokeSvg,
            StrokeDataJson = item.StrokeDataJson,
            ComponentMapJson = item.ComponentMapJson,
            VocabularyItems = item.VocabularyItems
                .OrderBy(vocabulary => vocabulary.OrderIndex)
                .Select(MapVocabulary)
                .ToList()
        };
    }

    private static KanjiVocabularyDto MapVocabulary(KanjiVocabulary item)
    {
        return new KanjiVocabularyDto
        {
            Id = item.Id,
            LessonId = item.LessonId,
            KanjiItemId = item.KanjiItemId,
            Level = item.Level,
            Word = item.Word,
            Reading = item.Reading,
            Meaning = item.Meaning,
            ExampleJapanese = item.ExampleJapanese,
            ExampleReading = item.ExampleReading,
            ExampleMeaning = item.ExampleMeaning,
            OrderIndex = item.OrderIndex
        };
    }

    private static KanjiProgressDto MapProgress(UserKanjiProgress progress)
    {
        return new KanjiProgressDto
        {
            Id = progress.Id,
            KanjiItemId = progress.KanjiItemId,
            IsLearned = progress.IsLearned,
            LastViewedAt = progress.LastViewedAt,
            WritingPracticeCount = progress.WritingPracticeCount,
            FlashcardPracticeCount = progress.FlashcardPracticeCount
        };
    }

    private static string ResolveAccessTier(KanjiItem item)
    {
        return string.IsNullOrWhiteSpace(item.AccessTierOverride)
            ? item.Lesson?.AccessTier ?? KanjiAccessTiers.Free
            : item.AccessTierOverride;
    }

    private static string? ResolvePackageCode(KanjiItem item)
    {
        return string.IsNullOrWhiteSpace(item.PackageCodeOverride)
            ? item.Lesson?.PackageCode
            : item.PackageCodeOverride;
    }
}
