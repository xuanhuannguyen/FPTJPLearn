using JPLearn.Core.Memory;
using JPLearn.Core.Memory.DTOs;
using JPLearn.Core.Memory.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class MemoryKanjiService : IMemoryKanjiService
{
    private readonly AppDbContext _db;
    private readonly IMemorySrsService _srsService;

    public MemoryKanjiService(AppDbContext db, IMemorySrsService srsService)
    {
        _db = db;
        _srsService = srsService;
    }

    public async Task<AddKanjiToMemoryResultDto?> AddFromItemAsync(Guid userId, Guid kanjiItemId)
    {
        var kanjiItem = await _db.KanjiItems
            .FirstOrDefaultAsync(item => item.Id == kanjiItemId);

        if (kanjiItem == null)
        {
            return null;
        }

        var existing = await _db.UserMemoryKanjiItems
            .FirstOrDefaultAsync(item => item.UserId == userId && item.SourceKanjiItemId == kanjiItemId);

        if (existing != null)
        {
            var alreadyActive = existing.IsActive;
            existing.IsActive = true;
            existing.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return new AddKanjiToMemoryResultDto
            {
                MemoryItemId = existing.Id,
                SourceKanjiItemId = kanjiItemId,
                AlreadyExists = alreadyActive,
                IsActive = existing.IsActive
            };
        }

        var item = new UserMemoryKanjiItem
        {
            UserId = userId,
            SourceKanjiItemId = kanjiItem.Id,
            Character = kanjiItem.Character,
            HanViet = kanjiItem.HanViet,
            Meaning = kanjiItem.Meaning,
            KunReading = kanjiItem.KunReading,
            OnReading = kanjiItem.OnReading,
            Mnemonic = kanjiItem.Mnemonic,
            KanjiLevel = kanjiItem.Level,
            StrokeCount = kanjiItem.StrokeCount,
            Level = 0,
            Status = MemoryStates.New,
            EaseFactor = 2.5,
            IntervalMinutes = 0,
            IntervalDays = 0,
            NextReviewAt = DateTime.UtcNow,
            AddedAt = DateTime.UtcNow,
            IsActive = true
        };

        _db.UserMemoryKanjiItems.Add(item);
        await _db.SaveChangesAsync();

        return new AddKanjiToMemoryResultDto
        {
            MemoryItemId = item.Id,
            SourceKanjiItemId = kanjiItemId,
            AlreadyExists = false,
            IsActive = true
        };
    }

    public async Task<MemoryGrammarStatusDto> GetItemStatusAsync(Guid userId, Guid kanjiItemId)
    {
        var item = await _db.UserMemoryKanjiItems
            .Where(memoryItem => memoryItem.UserId == userId && memoryItem.SourceKanjiItemId == kanjiItemId)
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
        var query = _db.UserMemoryKanjiItems
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
        var item = await _db.UserMemoryKanjiItems
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
            ItemType = MemoryItemTypes.Kanji,
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
        var item = await _db.UserMemoryKanjiItems
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
        var query = _db.UserMemoryKanjiItems
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

    private static void ResetItem(UserMemoryKanjiItem item)
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

    private static MemoryCardDto MapCard(UserMemoryKanjiItem item)
    {
        // Encode Kun/On/Mnemonic into backSecondary as JSON for frontend parsing
        var backSecondary = System.Text.Json.JsonSerializer.Serialize(new
        {
            kunReading = item.KunReading,
            onReading = item.OnReading,
            mnemonic = item.Mnemonic
        });

        return new MemoryCardDto
        {
            Id = item.Id,
            ItemType = MemoryItemTypes.Kanji,
            FrontPrimary = item.Character,
            FrontSecondary = item.HanViet,
            FrontMeta = item.KanjiLevel,
            BackPrimary = item.Meaning,
            BackSecondary = backSecondary,
            Notes = item.Mnemonic,
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
