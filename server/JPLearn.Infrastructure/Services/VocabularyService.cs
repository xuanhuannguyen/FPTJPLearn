using Microsoft.EntityFrameworkCore;
using JPLearn.Core.Vocabulary;
using JPLearn.Core.Vocabulary.DTOs;
using JPLearn.Core.Vocabulary.Entities;
using JPLearn.Core.Review;
using JPLearn.Infrastructure.Data;

namespace JPLearn.Infrastructure.Services;

public class VocabularyService : IVocabularyService
{
    private readonly AppDbContext _db;

    public VocabularyService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> ImportAsync(Guid userId, ImportVocabularyDto dto)
    {
        // 1. Kiểm tra quyền Premium (bất kỳ khóa nào còn hạn)
        var isPremium = await _db.Subscriptions.AnyAsync(s => 
            s.UserId == userId && s.ExpiresAt > DateTime.UtcNow);

        if (isPremium)
        {
            // Premium: 10 lần / ngày
            var today = DateTime.UtcNow.Date;
            var countToday = await _db.VocabularyLists.CountAsync(l => 
                l.UserId == userId && l.CreatedAt >= today);

            if (countToday >= 10)
            {
                throw new InvalidOperationException("Bạn đã đạt giới hạn 10 lượt thêm từ vựng/ngày của gói Premium.");
            }
        }
        else
        {
            // Free: Tối đa 2 lần tổng cộng
            var totalCount = await _db.VocabularyLists.CountAsync(l => l.UserId == userId);
            if (totalCount >= 2)
            {
                throw new InvalidOperationException("Tài khoản miễn phí chỉ được thêm tối đa 2 danh sách từ vựng. Vui lòng nâng cấp Premium để thêm 10 danh sách mỗi ngày!");
            }
        }

        if (dto.Words.Count > 50)
        {
            throw new InvalidOperationException("Mỗi danh sách chỉ được chứa tối đa 50 từ.");
        }

        var list = new VocabularyList
        {
            UserId = userId,
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            WordCount = dto.Words.Count
        };

        var items = dto.Words.Select((w, index) => new VocabularyItem
        {
            ListId = list.Id,
            Word = w.Word,
            Reading = w.Reading,
            WordType = w.Type,
            Meaning = w.Meaning,
            ExampleSentence = w.Example,
            ExampleMeaning = w.ExampleMeaning,
            OrderIndex = index
        }).ToList();

        var progressRecords = items.Select(item => new UserWordProgress
        {
            UserId = userId,
            VocabularyItemId = item.Id,
            Level = 0,
            Status = ReviewStates.New,
            NextReviewAt = DateTime.UtcNow
        }).ToList();

        _db.VocabularyLists.Add(list);
        _db.VocabularyItems.AddRange(items);
        _db.UserWordProgress.AddRange(progressRecords);
        await _db.SaveChangesAsync();

        return list.Id;
    }

    public async Task<List<VocabularyListDto>> GetListsAsync(Guid userId)
    {
        return await _db.VocabularyLists
            .Where(l => l.UserId == userId && !l.IsDeleted)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new VocabularyListDto
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                WordCount = l.WordCount,
                CreatedAt = l.CreatedAt,
                MasteredCount = _db.UserWordProgress
                    .Count(p => p.UserId == userId
                        && p.VocabularyItem.ListId == l.Id
                        && p.Level >= ReviewLevels.Mastered),
                DueCount = _db.UserWordProgress
                    .Count(p => p.UserId == userId
                        && p.VocabularyItem.ListId == l.Id
                        && p.NextReviewAt <= DateTime.UtcNow)
            })
            .ToListAsync();
    }

    public async Task<VocabularyListDetailDto?> GetByIdAsync(Guid userId, Guid listId)
    {
        var list = await _db.VocabularyLists
            .Where(l => l.Id == listId && l.UserId == userId && !l.IsDeleted)
            .FirstOrDefaultAsync();

        if (list == null) return null;

        var items = await _db.VocabularyItems
            .Where(i => i.ListId == listId)
            .OrderBy(i => i.OrderIndex)
            .Select(i => new VocabularyItemDto
            {
                Id = i.Id,
                Word = i.Word,
                Reading = i.Reading,
                WordType = i.WordType,
                Meaning = i.Meaning,
                ExampleSentence = i.ExampleSentence,
                ExampleMeaning = i.ExampleMeaning,
                OrderIndex = i.OrderIndex,
                Level = _db.UserWordProgress
                    .Where(p => p.UserId == userId && p.VocabularyItemId == i.Id)
                    .Select(p => p.Level < ReviewLevels.Min ? ReviewLevels.Min : p.Level > ReviewLevels.Max ? ReviewLevels.Max : p.Level)
                    .FirstOrDefault(),
                Status = _db.UserWordProgress
                    .Where(p => p.UserId == userId && p.VocabularyItemId == i.Id)
                    .Select(p => p.Status)
                    .FirstOrDefault() ?? "new"
            })
            .ToListAsync();

        return new VocabularyListDetailDto
        {
            Id = list.Id,
            Name = list.Name,
            Description = list.Description,
            WordCount = list.WordCount,
            CreatedAt = list.CreatedAt,
            Items = items
        };
    }

    public async Task<bool> UpdateAsync(Guid userId, Guid listId, string name, string? description)
    {
        var list = await _db.VocabularyLists
            .FirstOrDefaultAsync(l => l.Id == listId && l.UserId == userId && !l.IsDeleted);

        if (list == null) return false;

        list.Name = name;
        list.Description = description;
        list.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteListAsync(Guid userId, Guid listId)
    {
        var list = await _db.VocabularyLists
            .FirstOrDefaultAsync(l => l.Id == listId && l.UserId == userId && !l.IsDeleted);

        if (list == null) return false;

        list.IsDeleted = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteItemAsync(Guid userId, Guid itemId)
    {
        var item = await _db.VocabularyItems
            .Include(i => i.List)
            .FirstOrDefaultAsync(i => i.Id == itemId && i.List.UserId == userId);

        if (item == null) return false;

        item.List.WordCount--;
        _db.VocabularyItems.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Guid> AddItemAsync(Guid userId, Guid listId, VocabularyWordDto dto)
    {
        var list = await _db.VocabularyLists.FirstOrDefaultAsync(l => l.Id == listId && l.UserId == userId);
        if (list == null) throw new UnauthorizedAccessException("List not found or unauthorized");

        // Get max order index
        var maxIndex = await _db.VocabularyItems
            .Where(i => i.ListId == listId)
            .Select(i => (int?)i.OrderIndex)
            .MaxAsync() ?? -1;

        var item = new VocabularyItem
        {
            ListId = listId,
            Word = dto.Word,
            Reading = dto.Reading,
            WordType = dto.Type,
            Meaning = dto.Meaning,
            ExampleSentence = dto.Example,
            ExampleMeaning = dto.ExampleMeaning,
            OrderIndex = maxIndex + 1
        };

        var progress = new UserWordProgress
        {
            UserId = userId,
            VocabularyItem = item, // Entity Framework will automatically link
            Level = 0,
            Status = ReviewStates.New,
            NextReviewAt = DateTime.UtcNow
        };

        list.WordCount++;
        _db.VocabularyItems.Add(item);
        _db.UserWordProgress.Add(progress);
        await _db.SaveChangesAsync();

        return item.Id;
    }

    public async Task<List<VocabularySearchItemDto>> GetSearchIndexAsync(Guid userId)
    {
        return await _db.VocabularyItems
            .Where(i => i.List.UserId == userId && !i.List.IsDeleted)
            .Select(i => new VocabularySearchItemDto
            {
                Id = i.Id,
                ListId = i.ListId,
                Word = i.Word,
                Reading = i.Reading,
                Meaning = i.Meaning,
                WordType = i.WordType
            })
            .ToListAsync();
    }

    public async Task<VocabularyQuotaDto> GetQuotaAsync(Guid userId)
    {
        var isPremium = await _db.Subscriptions.AnyAsync(s => 
            s.UserId == userId && s.ExpiresAt > DateTime.UtcNow);

        if (isPremium)
        {
            var today = DateTime.UtcNow.Date;
            var usedToday = await _db.VocabularyLists.CountAsync(l => 
                l.UserId == userId && l.CreatedAt >= today);
            
            return new VocabularyQuotaDto
            {
                IsPremium = true,
                UsedCount = usedToday,
                MaxCount = 10,
                RemainingCount = Math.Max(0, 10 - usedToday),
                Period = "daily"
            };
        }
        else
        {
            var totalUsed = await _db.VocabularyLists.CountAsync(l => l.UserId == userId);
            
            return new VocabularyQuotaDto
            {
                IsPremium = false,
                UsedCount = totalUsed,
                MaxCount = 2,
                RemainingCount = Math.Max(0, 2 - totalUsed),
                Period = "total"
            };
        }
    }
}
