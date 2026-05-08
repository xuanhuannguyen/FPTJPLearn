using JPLearn.Core.Common.Services;
using JPLearn.Core.Grammar;
using JPLearn.Core.Grammar.DTOs;
using JPLearn.Core.Grammar.Entities;
using JPLearn.Core.Review;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class GrammarReviewService : IGrammarReviewService
{
    private readonly AppDbContext _db;

    public GrammarReviewService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AddGrammarStudyResultDto?> AddToStudyAsync(Guid userId, Guid patternId)
    {
        var patternExists = await _db.GrammarPatterns.AnyAsync(pattern => pattern.Id == patternId);
        if (!patternExists)
        {
            return null;
        }

        var progress = await _db.UserGrammarProgress
            .FirstOrDefaultAsync(item => item.UserId == userId && item.GrammarPatternId == patternId);

        if (progress != null)
        {
            var wasActive = progress.IsActive;
            progress.IsActive = true;
            progress.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return new AddGrammarStudyResultDto
            {
                Success = true,
                AlreadyExists = wasActive,
                Progress = GrammarService.MapProgress(progress)
            };
        }

        progress = new UserGrammarProgress
        {
            UserId = userId,
            GrammarPatternId = patternId,
            Level = 0,
            Status = ReviewStates.New,
            EaseFactor = 2.5,
            NextReviewAt = DateTime.UtcNow,
            AddedAt = DateTime.UtcNow,
            IsActive = true
        };

        _db.UserGrammarProgress.Add(progress);
        await _db.SaveChangesAsync();

        return new AddGrammarStudyResultDto
        {
            Success = true,
            AlreadyExists = false,
            Progress = GrammarService.MapProgress(progress)
        };
    }

    public async Task<bool> RemoveFromStudyAsync(Guid userId, Guid patternId)
    {
        var progress = await _db.UserGrammarProgress
            .FirstOrDefaultAsync(item => item.UserId == userId && item.GrammarPatternId == patternId);

        if (progress == null)
        {
            return false;
        }

        progress.IsActive = false;
        progress.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<GrammarDueCardsResponse> GetDueCardsAsync(Guid userId)
    {
        var cards = await _db.UserGrammarProgress
            .Include(progress => progress.GrammarPattern)
            .ThenInclude(pattern => pattern.Lesson)
            .Include(progress => progress.GrammarPattern)
            .ThenInclude(pattern => pattern.Examples)
            .Where(progress => progress.UserId == userId
                && progress.IsActive
                && progress.NextReviewAt <= DateTime.UtcNow)
            .OrderBy(progress => progress.NextReviewAt)
            .ThenBy(progress => progress.GrammarPattern.Level)
            .ThenBy(progress => progress.GrammarPattern.OrderIndex)
            .ToListAsync();

        return new GrammarDueCardsResponse
        {
            DueCount = cards.Count,
            Cards = cards.Select(MapReviewCard).ToList()
        };
    }

    public async Task<GrammarAnswerResultDto?> SubmitAnswerAsync(Guid userId, SubmitGrammarAnswerDto dto)
    {
        var progress = await _db.UserGrammarProgress
            .FirstOrDefaultAsync(item => item.UserId == userId
                && item.GrammarPatternId == dto.PatternId
                && item.IsActive);

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
        progress.Status = result.Status;
        progress.Repetitions = result.Repetitions;
        progress.EaseFactor = result.EaseFactor;
        progress.IntervalDays = result.IntervalDays;
        progress.NextReviewAt = result.NextReviewAt;
        progress.LastReviewedAt = DateTime.UtcNow;
        progress.LapseCount = result.LapseCount;
        progress.LearningStepIndex = result.LearningStepIndex;
        progress.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return new GrammarAnswerResultDto
        {
            PatternId = dto.PatternId,
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

    public async Task<GrammarProgressSummaryDto> GetProgressSummaryAsync(Guid userId)
    {
        var progress = await _db.UserGrammarProgress
            .Where(item => item.UserId == userId && item.IsActive)
            .ToListAsync();

        return new GrammarProgressSummaryDto
        {
            InStudyCount = progress.Count,
            DueCount = progress.Count(item => item.NextReviewAt <= DateTime.UtcNow),
            MasteredCount = progress.Count(item => item.Level >= ReviewLevels.Mastered),
            LearningCount = progress.Count(item => item.Status == ReviewStates.Learning || item.Status == ReviewStates.Relearning),
            ReviewCount = progress.Count(item => item.Status == ReviewStates.Review)
        };
    }

    private static GrammarReviewCardDto MapReviewCard(UserGrammarProgress progress)
    {
        var pattern = progress.GrammarPattern;
        return new GrammarReviewCardDto
        {
            ProgressId = progress.Id,
            PatternId = pattern.Id,
            Level = pattern.Level,
            Pattern = pattern.Pattern,
            Title = pattern.Title,
            Meaning = pattern.Meaning,
            Structure = pattern.Structure,
            UsageScope = pattern.UsageScope,
            Formation = pattern.Formation,
            Notes = pattern.Notes,
            Examples = pattern.Examples
                .OrderBy(example => example.OrderIndex)
                .Select(GrammarService.MapExample)
                .ToList(),
            StudyLevel = Math.Clamp(progress.Level, ReviewLevels.Min, ReviewLevels.Max),
            Status = progress.Status,
            NextReviewAt = progress.NextReviewAt,
            IntervalDays = progress.IntervalDays,
            Repetitions = progress.Repetitions
        };
    }
}
