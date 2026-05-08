using JPLearn.Core.Memory;
using JPLearn.Core.Memory.DTOs;
using JPLearn.Core.Memory.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class MemoryGrammarService : IMemoryGrammarService
{
    private readonly AppDbContext _db;
    private readonly IMemorySrsService _srsService;

    public MemoryGrammarService(AppDbContext db, IMemorySrsService srsService)
    {
        _db = db;
        _srsService = srsService;
    }

    public async Task<AddGrammarToMemoryResultDto?> AddFromPatternAsync(Guid userId, Guid patternId)
    {
        var pattern = await _db.GrammarPatterns
            .Include(item => item.Examples)
            .FirstOrDefaultAsync(item => item.Id == patternId);

        if (pattern == null)
        {
            return null;
        }

        var existing = await _db.UserMemoryGrammarItems
            .FirstOrDefaultAsync(item => item.UserId == userId && item.SourceGrammarPatternId == patternId);

        if (existing != null)
        {
            var alreadyActive = existing.IsActive;
            existing.IsActive = true;
            existing.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return new AddGrammarToMemoryResultDto
            {
                MemoryItemId = existing.Id,
                SourceGrammarPatternId = patternId,
                AlreadyExists = alreadyActive,
                IsActive = existing.IsActive
            };
        }

        var firstExample = pattern.Examples
            .OrderBy(example => example.OrderIndex)
            .FirstOrDefault();

        var item = new UserMemoryGrammarItem
        {
            UserId = userId,
            SourceGrammarPatternId = pattern.Id,
            Pattern = pattern.Pattern,
            Title = pattern.Title,
            Meaning = pattern.Meaning,
            Structure = pattern.Structure,
            UsageScope = pattern.UsageScope,
            Formation = pattern.Formation,
            ExampleJapanese = firstExample?.Japanese,
            ExampleReading = firstExample?.Reading,
            ExampleMeaning = firstExample?.Meaning,
            Notes = pattern.Notes,
            TagsJson = pattern.TagsJson,
            Level = 0,
            Status = MemoryStates.New,
            EaseFactor = 2.5,
            IntervalMinutes = 0,
            IntervalDays = 0,
            NextReviewAt = DateTime.UtcNow,
            AddedAt = DateTime.UtcNow,
            IsActive = true
        };

        _db.UserMemoryGrammarItems.Add(item);
        await _db.SaveChangesAsync();

        return new AddGrammarToMemoryResultDto
        {
            MemoryItemId = item.Id,
            SourceGrammarPatternId = patternId,
            AlreadyExists = false,
            IsActive = true
        };
    }

    public async Task<MemoryGrammarStatusDto> GetPatternStatusAsync(Guid userId, Guid patternId)
    {
        var item = await _db.UserMemoryGrammarItems
            .Where(memoryItem => memoryItem.UserId == userId && memoryItem.SourceGrammarPatternId == patternId)
            .Select(memoryItem => new MemoryGrammarStatusDto
            {
                IsInMemory = memoryItem.IsActive,
                MemoryItemId = memoryItem.Id,
                IsActive = memoryItem.IsActive
            })
            .FirstOrDefaultAsync();

        return item ?? new MemoryGrammarStatusDto();
    }

    public async Task<MemoryCardsResponseDto> GetCardsAsync(Guid userId, string scope)
    {
        var normalizedScope = NormalizeScope(scope);
        var now = DateTime.UtcNow;
        var query = _db.UserMemoryGrammarItems
            .Where(item => item.UserId == userId && item.IsActive);

        query = normalizedScope switch
        {
            MemoryScopes.Due => query.Where(item => item.NextReviewAt <= now),
            MemoryScopes.New => query.Where(item => item.Level == 0),
            MemoryScopes.Learning => query.Where(item => item.Level == 1 || item.Level == 2),
            MemoryScopes.ShortTerm => query.Where(item => (item.Level == 3 || item.Level == 4) && item.IntervalDays < 21),
            MemoryScopes.LongTerm => query.Where(item => item.Level >= MemoryLevels.Mastered || item.IntervalDays >= 21),
            _ => query
        };

        var items = await query
            .OrderBy(item => item.NextReviewAt)
            .ThenBy(item => item.AddedAt)
            .ToListAsync();

        return new MemoryCardsResponseDto
        {
            Count = items.Count,
            Cards = items.Select(MapCard).ToList()
        };
    }

    public async Task<MemoryAnswerResultDto?> SubmitAnswerAsync(Guid userId, SubmitMemoryAnswerDto dto)
    {
        var item = await _db.UserMemoryGrammarItems
            .FirstOrDefaultAsync(memoryItem => memoryItem.Id == dto.MemoryItemId
                && memoryItem.UserId == userId
                && memoryItem.IsActive);

        if (item == null)
        {
            return null;
        }

        var oldLevel = item.Level;
        var oldStatus = item.Status;
        var result = _srsService.Calculate(
            dto.Quality,
            item.Level,
            item.Status,
            item.Repetitions,
            item.EaseFactor,
            item.IntervalMinutes,
            item.IntervalDays,
            item.LapseCount,
            DateTime.UtcNow);

        item.Level = result.Level;
        item.Status = result.Status;
        item.Repetitions = result.Repetitions;
        item.EaseFactor = result.EaseFactor;
        item.IntervalMinutes = result.IntervalMinutes;
        item.IntervalDays = result.IntervalDays;
        item.NextReviewAt = result.NextReviewAt;
        item.LastReviewedAt = DateTime.UtcNow;
        item.LapseCount = result.LapseCount;
        item.LearningStepIndex = result.LearningStepIndex;
        item.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return new MemoryAnswerResultDto
        {
            MemoryItemId = item.Id,
            ItemType = MemoryItemTypes.Grammar,
            OldLevel = oldLevel,
            Level = result.Level,
            OldStatus = oldStatus,
            Status = result.Status,
            IntervalMinutes = result.IntervalMinutes,
            IntervalDays = result.IntervalDays,
            NextReviewAt = result.NextReviewAt,
            Repetitions = result.Repetitions,
            LapseCount = result.LapseCount,
            RequeueInSession = result.RequeueInSession,
            RequeueAfterSeconds = result.RequeueAfterSeconds,
            Message = result.Message
        };
    }

    public async Task<bool> RemoveAsync(Guid userId, Guid memoryItemId)
    {
        var item = await _db.UserMemoryGrammarItems
            .FirstOrDefaultAsync(memoryItem => memoryItem.Id == memoryItemId && memoryItem.UserId == userId);

        if (item == null)
        {
            return false;
        }

        item.IsActive = false;
        item.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<int> ResetAsync(Guid userId, ResetMemoryItemsDto dto)
    {
        var query = _db.UserMemoryGrammarItems
            .Where(item => item.UserId == userId && item.IsActive);

        if (NormalizeScope(dto.Scope) != MemoryScopes.All && dto.MemoryItemIds.Count > 0)
        {
            query = query.Where(item => dto.MemoryItemIds.Contains(item.Id));
        }

        var items = await query.ToListAsync();
        foreach (var item in items)
        {
            ResetItem(item);
        }

        await _db.SaveChangesAsync();
        return items.Count;
    }

    private static void ResetItem(UserMemoryGrammarItem item)
    {
        item.Level = 0;
        item.Status = MemoryStates.New;
        item.Repetitions = 0;
        item.EaseFactor = 2.5;
        item.IntervalMinutes = 0;
        item.IntervalDays = 0;
        item.NextReviewAt = DateTime.UtcNow;
        item.LastReviewedAt = null;
        item.LapseCount = 0;
        item.LearningStepIndex = 0;
        item.IsActive = true;
        item.UpdatedAt = DateTime.UtcNow;
    }

    private static MemoryCardDto MapCard(UserMemoryGrammarItem item)
    {
        return new MemoryCardDto
        {
            Id = item.Id,
            ItemType = MemoryItemTypes.Grammar,
            FrontPrimary = item.Pattern,
            FrontSecondary = item.Structure,
            FrontMeta = item.Meaning,
            BackPrimary = item.Meaning,
            BackSecondary = item.Formation ?? item.Structure,
            Example = item.ExampleJapanese,
            ExampleReading = item.ExampleReading,
            ExampleMeaning = item.ExampleMeaning,
            Notes = item.Notes,
            Level = item.Level,
            Status = item.Status,
            NextReviewAt = item.NextReviewAt
        };
    }

    private static string NormalizeScope(string? scope)
    {
        return scope?.Trim().ToLowerInvariant() switch
        {
            MemoryScopes.All => MemoryScopes.All,
            MemoryScopes.New => MemoryScopes.New,
            MemoryScopes.Learning => MemoryScopes.Learning,
            MemoryScopes.ShortTerm => MemoryScopes.ShortTerm,
            MemoryScopes.LongTerm => MemoryScopes.LongTerm,
            _ => MemoryScopes.Due
        };
    }
}
