using JPLearn.Core.Memory;
using JPLearn.Core.Memory.DTOs;
using JPLearn.Core.Memory.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class MemoryService : IMemoryService
{
    private readonly AppDbContext _db;

    public MemoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<MemorySummaryDto> GetSummaryAsync(Guid userId)
    {
        return new MemorySummaryDto
        {
            Grammar = await GetGrammarSummaryAsync(userId),
            Kanji = await GetKanjiSummaryAsync(userId),
<<<<<<< HEAD
            Vocabulary = await GetVocabularySummaryAsync(userId)
=======
            Vocabulary = new MemoryTypeSummaryDto()
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        };
    }

    public async Task<MemoryTypeSummaryDto> GetGrammarSummaryAsync(Guid userId)
    {
        var items = await _db.UserMemoryGrammarItems
            .Where(item => item.UserId == userId && item.IsActive)
            .ToListAsync();

        return BuildGrammarSummary(items);
    }

    private async Task<MemoryTypeSummaryDto> GetKanjiSummaryAsync(Guid userId)
    {
        var items = await _db.UserMemoryKanjiItems
            .Where(item => item.UserId == userId && item.IsActive)
            .ToListAsync();

        return BuildKanjiSummary(items);
    }

<<<<<<< HEAD
    private async Task<MemoryTypeSummaryDto> GetVocabularySummaryAsync(Guid userId)
    {
        var items = await _db.UserMemoryVocabularyItems
            .Where(item => item.UserId == userId && item.IsActive)
            .ToListAsync();

        return BuildVocabularySummary(items);
    }

=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
    private static MemoryTypeSummaryDto BuildGrammarSummary(List<UserMemoryGrammarItem> items)
    {
        var now = DateTime.UtcNow;
        return new MemoryTypeSummaryDto
        {
            Due = items.Count(item => item.NextReviewAt <= now),
            New = items.Count(item => item.Level == 0),
            Learning = items.Count(item => item.Level is 1 or 2),
            ShortTerm = items.Count(item => item.Level is 3 or 4 && item.IntervalDays < 21),
            LongTerm = items.Count(item => item.Level >= MemoryLevels.Mastered || item.IntervalDays >= 21),
            TotalStudied = items.Count(item => item.Repetitions > 0),
            NextReviewAt = items.Count == 0 ? null : items.Min(item => item.NextReviewAt)
        };
    }

    private static MemoryTypeSummaryDto BuildKanjiSummary(List<UserMemoryKanjiItem> items)
<<<<<<< HEAD
    {
        var now = DateTime.UtcNow;
        return new MemoryTypeSummaryDto
        {
            Due = items.Count(item => item.NextReviewAt <= now),
            New = items.Count(item => item.Level == 0),
            Learning = items.Count(item => item.Level is 1 or 2),
            ShortTerm = items.Count(item => item.Level is 3 or 4 && item.IntervalDays < 21),
            LongTerm = items.Count(item => item.Level >= MemoryLevels.Mastered || item.IntervalDays >= 21),
            TotalStudied = items.Count(item => item.Repetitions > 0),
            NextReviewAt = items.Count == 0 ? null : items.Min(item => item.NextReviewAt)
        };
    }

    private static MemoryTypeSummaryDto BuildVocabularySummary(List<UserMemoryVocabularyItem> items)
=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
    {
        var now = DateTime.UtcNow;
        return new MemoryTypeSummaryDto
        {
            Due = items.Count(item => item.NextReviewAt <= now),
            New = items.Count(item => item.Level == 0),
            Learning = items.Count(item => item.Level is 1 or 2),
            ShortTerm = items.Count(item => item.Level is 3 or 4 && item.IntervalDays < 21),
            LongTerm = items.Count(item => item.Level >= MemoryLevels.Mastered || item.IntervalDays >= 21),
            TotalStudied = items.Count(item => item.Repetitions > 0),
            NextReviewAt = items.Count == 0 ? null : items.Min(item => item.NextReviewAt)
        };
    }
}
