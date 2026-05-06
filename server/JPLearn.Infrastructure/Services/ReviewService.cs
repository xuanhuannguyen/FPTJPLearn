using JPLearn.Core.Common.Services;
using JPLearn.Core.Review;
using JPLearn.Core.Review.DTOs;
using JPLearn.Core.Review.Entities;
using JPLearn.Core.Vocabulary.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _db;

    public ReviewService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DueCardsResponse> GetDueCardsAsync(Guid userId, Guid listId)
    {
        var cards = await _db.UserWordProgress
            .Include(progress => progress.VocabularyItem)
            .ThenInclude(item => item.List)
            .Where(progress => progress.UserId == userId
                && progress.VocabularyItem.ListId == listId
                && progress.VocabularyItem.List.UserId == userId
                && progress.NextReviewAt <= DateTime.UtcNow)
            .OrderBy(progress => progress.NextReviewAt)
            .ThenBy(progress => progress.VocabularyItem.OrderIndex)
            .ToListAsync();

        return new DueCardsResponse
        {
            DueCount = cards.Count,
            Cards = cards.Select(MapCard).ToList()
        };
    }

    public async Task<CardsResponseDto> GetLearnedCardsAsync(Guid userId, Guid listId, string scope)
    {
        var normalizedScope = NormalizeScope(scope);

        var query = _db.UserWordProgress
            .Include(progress => progress.VocabularyItem)
            .ThenInclude(item => item.List)
            .Where(progress => progress.UserId == userId
                && progress.VocabularyItem.ListId == listId
                && progress.VocabularyItem.List.UserId == userId);

        query = normalizedScope switch
        {
            ReviewScopes.Mastered => query.Where(progress => progress.Level >= 5),
            ReviewScopes.Reviewed => query.Where(progress => progress.Level >= 3),
            _ => query.Where(progress => progress.Level >= 1)
        };

        var cards = await query
            .OrderBy(progress => progress.NextReviewAt)
            .ThenBy(progress => progress.VocabularyItem.OrderIndex)
            .ToListAsync();

        return new CardsResponseDto { Cards = cards.Select(MapCard).ToList() };
    }

    public async Task<CardsResponseDto> GetCardsByLevelAsync(Guid userId, Guid listId, int minLevel, int maxLevel)
    {
        var fromLevel = Math.Clamp(minLevel, 0, 5);
        var toLevel = Math.Clamp(maxLevel, fromLevel, 5);

        var cards = await _db.UserWordProgress
            .Include(progress => progress.VocabularyItem)
            .ThenInclude(item => item.List)
            .Where(progress => progress.UserId == userId
                && progress.VocabularyItem.ListId == listId
                && progress.VocabularyItem.List.UserId == userId
                && progress.Level >= fromLevel
                && progress.Level <= toLevel)
            .OrderBy(progress => progress.Level)
            .ThenBy(progress => progress.VocabularyItem.OrderIndex)
            .ToListAsync();

        return new CardsResponseDto { Cards = cards.Select(MapCard).ToList() };
    }

    public async Task<CardsResponseDto> GetAllCardsAsync(Guid userId, Guid listId)
    {
        var cards = await _db.UserWordProgress
            .Include(progress => progress.VocabularyItem)
            .ThenInclude(item => item.List)
            .Where(progress => progress.UserId == userId
                && progress.VocabularyItem.ListId == listId
                && progress.VocabularyItem.List.UserId == userId)
            .OrderBy(progress => progress.VocabularyItem.OrderIndex)
            .ToListAsync();

        return new CardsResponseDto { Cards = cards.Select(MapCard).ToList() };
    }

    public async Task<ReviewAnswerResultDto?> SubmitAnswerAsync(Guid userId, ReviewAnswerDto dto)
    {
        var progress = await _db.UserWordProgress
            .Include(itemProgress => itemProgress.VocabularyItem)
            .ThenInclude(item => item.List)
            .FirstOrDefaultAsync(progress => progress.UserId == userId
                && progress.VocabularyItemId == dto.ItemId
                && progress.VocabularyItem.List.UserId == userId);

        if (progress == null)
        {
            return null;
        }

        var result = SrsAlgorithm.Calculate(
            dto.Quality,
            progress.Level,
            progress.Status,
            progress.Repetitions,
            progress.EaseFactor,
            progress.IntervalDays,
            progress.LapseCount,
            DateTime.UtcNow);

        var oldLevel = progress.Level;
        var oldStatus = progress.Status;

        progress.Level = result.Level;
        progress.Repetitions = result.Repetitions;
        progress.EaseFactor = result.EaseFactor;
        progress.IntervalDays = result.IntervalDays;
        progress.LearningStepIndex = result.LearningStepIndex;
        progress.LapseCount = result.LapseCount;
        progress.NextReviewAt = result.NextReviewAt;
        progress.LastReviewedAt = DateTime.UtcNow;
        progress.Status = result.Status;
        progress.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return new ReviewAnswerResultDto
        {
            ItemId = progress.VocabularyItemId,
            OldLevel = oldLevel,
            NewLevel = result.Level,
            OldStatus = oldStatus,
            NewStatus = result.Status,
            NextReviewAt = result.NextReviewAt,
            IntervalDays = result.IntervalDays,
            Repetitions = result.Repetitions,
            LapseCount = result.LapseCount,
            LearningStepIndex = result.LearningStepIndex,
            RequeueInSession = result.RequeueInSession,
            RequeueAfterSeconds = result.RequeueAfterSeconds
        };
    }

    public async Task<Guid> SaveSessionAsync(Guid userId, SaveSessionDto dto)
    {
        var listExists = await _db.VocabularyLists
            .AnyAsync(list => list.Id == dto.ListId && list.UserId == userId);

        if (!listExists)
        {
            throw new UnauthorizedAccessException("List not found");
        }

        var session = new ReviewSession
        {
            UserId = userId,
            ListId = dto.ListId,
            Mode = dto.Mode,
            TotalCards = dto.TotalCards,
            CorrectCount = dto.CorrectCount,
            WrongCount = dto.WrongCount,
            DurationSeconds = dto.DurationSeconds,
            StartedAt = DateTime.UtcNow.AddSeconds(-dto.DurationSeconds),
            CompletedAt = DateTime.UtcNow
        };

        _db.ReviewSessions.Add(session);
        await _db.SaveChangesAsync();
        return session.Id;
    }

    public async Task<ResetListProgressResultDto> ResetListProgressAsync(Guid userId, Guid listId, ResetListProgressDto dto)
    {
        var normalizedResetType = NormalizeResetType(dto.ResetType);

        var query = _db.UserWordProgress
            .Include(progress => progress.VocabularyItem)
            .ThenInclude(item => item.List)
            .Where(progress => progress.UserId == userId
                && progress.VocabularyItem.ListId == listId
                && progress.VocabularyItem.List.UserId == userId);

        if (normalizedResetType == ReviewResetTypes.Mastered)
        {
            query = query.Where(progress => progress.Level >= 5);
        }
        else if (normalizedResetType == ReviewResetTypes.Selected)
        {
            var itemIds = dto.ItemIds?
                .Where(itemId => itemId != Guid.Empty)
                .Distinct()
                .ToList() ?? [];

            if (itemIds.Count == 0)
            {
                return new ResetListProgressResultDto
                {
                    Success = false,
                    AffectedCount = 0
                };
            }

            query = query.Where(progress => itemIds.Contains(progress.VocabularyItemId));
        }

        var progressRecords = await query.ToListAsync();

        if (progressRecords.Count == 0)
        {
            return new ResetListProgressResultDto
            {
                Success = true,
                AffectedCount = 0
            };
        }

        if (dto.HardReset)
        {
            var itemIds = progressRecords
                .Select(progress => progress.VocabularyItemId)
                .Distinct()
                .ToList();

            _db.UserWordProgress.RemoveRange(progressRecords);
            _db.UserWordProgress.AddRange(itemIds.Select(itemId => CreateFreshProgress(userId, itemId)));
        }
        else
        {
            foreach (var progress in progressRecords)
            {
                ResetProgress(progress);
            }
        }

        await _db.SaveChangesAsync();

        return new ResetListProgressResultDto
        {
            Success = true,
            AffectedCount = progressRecords.Count
        };
    }

    private static ReviewCardDto MapCard(UserWordProgress progress)
    {
        return new ReviewCardDto
        {
            Id = progress.Id,
            ItemId = progress.VocabularyItemId,
            Word = progress.VocabularyItem.Word,
            Reading = progress.VocabularyItem.Reading,
            WordType = progress.VocabularyItem.WordType,
            Meaning = progress.VocabularyItem.Meaning,
            ExampleSentence = progress.VocabularyItem.ExampleSentence,
            ExampleMeaning = progress.VocabularyItem.ExampleMeaning,
            Level = progress.Level,
            Status = progress.Status,
            NextReviewAt = progress.NextReviewAt,
            IntervalDays = progress.IntervalDays,
            Repetitions = progress.Repetitions,
            LapseCount = progress.LapseCount,
            LearningStepIndex = progress.LearningStepIndex
        };
    }

    private static void ResetProgress(UserWordProgress progress)
    {
        progress.Level = 0;
        progress.Repetitions = 0;
        progress.EaseFactor = 2.5;
        progress.IntervalDays = 0;
        progress.LearningStepIndex = 0;
        progress.LapseCount = 0;
        progress.NextReviewAt = DateTime.UtcNow;
        progress.LastReviewedAt = null;
        progress.Status = ReviewStates.New;
        progress.UpdatedAt = DateTime.UtcNow;
    }

    private static UserWordProgress CreateFreshProgress(Guid userId, Guid itemId)
    {
        return new UserWordProgress
        {
            UserId = userId,
            VocabularyItemId = itemId,
            Level = 0,
            Status = ReviewStates.New,
            EaseFactor = 2.5,
            NextReviewAt = DateTime.UtcNow
        };
    }

    private static string NormalizeScope(string? scope)
    {
        return scope?.Trim().ToLowerInvariant() switch
        {
            ReviewScopes.Mastered => ReviewScopes.Mastered,
            ReviewScopes.Reviewed => ReviewScopes.Reviewed,
            _ => ReviewScopes.All
        };
    }

    private static string NormalizeResetType(string? resetType)
    {
        return resetType?.Trim().ToLowerInvariant() switch
        {
            ReviewResetTypes.Mastered => ReviewResetTypes.Mastered,
            ReviewResetTypes.Selected => ReviewResetTypes.Selected,
            _ => ReviewResetTypes.All
        };
    }
}
