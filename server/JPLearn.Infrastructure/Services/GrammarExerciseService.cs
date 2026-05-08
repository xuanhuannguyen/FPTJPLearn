using System.Text.Json;
using JPLearn.Core.Grammar;
using JPLearn.Core.Grammar.DTOs;
using JPLearn.Core.Grammar.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class GrammarExerciseService : IGrammarExerciseService
{
    private readonly AppDbContext _db;

    public GrammarExerciseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<GrammarExerciseDto>?> GetExercisesByPatternAsync(Guid patternId)
    {
        var patternExists = await _db.GrammarPatterns.AnyAsync(pattern => pattern.Id == patternId);
        if (!patternExists)
        {
            return null;
        }

        var exercises = await _db.GrammarExercises
            .Where(exercise => exercise.PatternId == patternId)
            .OrderBy(exercise => exercise.OrderIndex)
            .ThenBy(exercise => exercise.ExerciseType)
            .ToListAsync();

        return exercises.Select(GrammarService.MapExercise).ToList();
    }

    public async Task<GrammarExerciseCheckResultDto?> CheckAnswerAsync(Guid userId, Guid exerciseId, CheckGrammarExerciseDto dto)
    {
        var exercise = await _db.GrammarExercises.FirstOrDefaultAsync(item => item.Id == exerciseId);
        if (exercise == null)
        {
            return null;
        }

        var isCorrect = IsCorrect(exercise, dto);
        var feedback = isCorrect ? "Correct." : "Incorrect. Review the sample answer and try again.";
        var attempt = new GrammarExerciseAttempt
        {
            UserId = userId,
            GrammarExerciseId = exercise.Id,
            AnswerText = dto.AnswerText,
            SelectedOptionOrderJson = SerializeOrNull(dto.SelectedOptionOrder),
            IsCorrect = isCorrect,
            Score = isCorrect ? 10 : 0,
            Feedback = feedback,
            CheckedBy = GrammarAttemptCheckers.System
        };

        _db.GrammarExerciseAttempts.Add(attempt);
        await _db.SaveChangesAsync();

        return new GrammarExerciseCheckResultDto
        {
            ExerciseId = exercise.Id,
            IsCorrect = isCorrect,
            Score = attempt.Score ?? 0,
            Feedback = feedback,
            ExpectedAnswer = exercise.ExpectedAnswer,
            CorrectOrderJson = exercise.CorrectOrderJson,
            StarAnswer = exercise.StarAnswer,
            AttemptId = attempt.Id
        };
    }

    public async Task<GrammarExerciseAnswerDto?> RevealAnswerAsync(Guid exerciseId)
    {
        var exercise = await _db.GrammarExercises.FirstOrDefaultAsync(item => item.Id == exerciseId);
        return exercise == null ? null : GrammarService.MapExerciseAnswer(exercise);
    }

    public async Task<AiGrammarEvaluationResultDto?> EvaluateWithAiAsync(Guid userId, Guid exerciseId, AiEvaluateGrammarExerciseDto dto)
    {
        var exercise = await _db.GrammarExercises.FirstOrDefaultAsync(item => item.Id == exerciseId);
        if (exercise == null)
        {
            return null;
        }

        var hasApiKey = !string.IsNullOrWhiteSpace(dto.ApiKey);
        var isCorrect = IsCorrect(exercise, dto);
        var score = hasApiKey
            ? isCorrect ? 9 : 5
            : 0;
        var feedback = hasApiKey
            ? "AI provider integration is not enabled yet. This placeholder used the sample answer for basic feedback."
            : "API key is required for AI evaluation.";

        var attempt = new GrammarExerciseAttempt
        {
            UserId = userId,
            GrammarExerciseId = exercise.Id,
            AnswerText = dto.AnswerText,
            SelectedOptionOrderJson = SerializeOrNull(dto.SelectedOptionOrder),
            IsCorrect = isCorrect,
            Score = score,
            Feedback = feedback,
            CheckedBy = GrammarAttemptCheckers.Ai
        };

        _db.GrammarExerciseAttempts.Add(attempt);
        await _db.SaveChangesAsync();

        return new AiGrammarEvaluationResultDto
        {
            ExerciseId = exercise.Id,
            Score = score,
            IsAcceptable = isCorrect,
            CorrectedAnswer = exercise.ExpectedAnswer,
            Feedback = feedback,
            GrammarNotes = hasApiKey
                ? ["Provider call is deferred. Basic exact-answer comparison was used."]
                : ["Add an API key to enable AI evaluation."],
            AttemptId = attempt.Id
        };
    }

    private static bool IsCorrect(GrammarExercise exercise, CheckGrammarExerciseDto dto)
    {
        if (exercise.ExerciseType == GrammarExerciseTypes.Arrange)
        {
            return IsArrangeCorrect(exercise, dto.SelectedOptionOrder);
        }

        var answer = NormalizeAnswer(dto.AnswerText);
        if (string.IsNullOrWhiteSpace(answer))
        {
            return false;
        }

        var acceptedAnswers = GetAcceptedAnswers(exercise);
        return acceptedAnswers.Any(expected => NormalizeAnswer(expected) == answer);
    }

    private static bool IsArrangeCorrect(GrammarExercise exercise, List<string>? selectedOptionOrder)
    {
        if (selectedOptionOrder == null || selectedOptionOrder.Count == 0)
        {
            return false;
        }

        var expectedOrder = DeserializeStringList(exercise.CorrectOrderJson);
        if (expectedOrder.Count == 0 || expectedOrder.Count != selectedOptionOrder.Count)
        {
            return false;
        }

        return expectedOrder
            .Select(NormalizeAnswer)
            .SequenceEqual(selectedOptionOrder.Select(NormalizeAnswer));
    }

    private static List<string> GetAcceptedAnswers(GrammarExercise exercise)
    {
        var answers = DeserializeStringList(exercise.AcceptableAnswersJson);
        if (!string.IsNullOrWhiteSpace(exercise.ExpectedAnswer))
        {
            answers.Add(exercise.ExpectedAnswer);
        }

        return answers
            .Where(answer => !string.IsNullOrWhiteSpace(answer))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
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

    private static string? SerializeOrNull(List<string>? values)
    {
        return values == null ? null : JsonSerializer.Serialize(values);
    }

    private static string NormalizeAnswer(string? value)
    {
        return string.Join(" ", (value ?? string.Empty)
            .Trim()
            .ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}
