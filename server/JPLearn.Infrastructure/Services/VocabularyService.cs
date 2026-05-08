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
        var list = new VocabularyList
        {
            UserId = userId,
            Name = dto.Name,
            Description = dto.Description,
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
            .Where(l => l.UserId == userId)
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
            .Where(l => l.Id == listId && l.UserId == userId)
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
            .FirstOrDefaultAsync(l => l.Id == listId && l.UserId == userId);

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
            .FirstOrDefaultAsync(l => l.Id == listId && l.UserId == userId);

        if (list == null) return false;

        _db.VocabularyLists.Remove(list); // Cascade deletes items + progress
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
}
