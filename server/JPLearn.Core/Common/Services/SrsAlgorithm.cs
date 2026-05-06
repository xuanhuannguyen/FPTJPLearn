using JPLearn.Core.Review;

namespace JPLearn.Core.Common.Services;

public static class SrsAlgorithm
{
    private static readonly TimeSpan LearningStepOne = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan LearningStepTwo = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan RelearningStep = TimeSpan.FromMinutes(10);

    public record SrsResult(
        int Level,
        int Repetitions,
        double EaseFactor,
        int IntervalDays,
        int LearningStepIndex,
        int LapseCount,
        DateTime NextReviewAt,
        string Status,
        bool RequeueInSession,
        int? RequeueAfterSeconds
    );

    public static SrsResult Calculate(
        int quality,
        int level,
        string? currentStatus,
        int repetitions,
        double easeFactor,
        int intervalDays,
        int lapseCount,
        DateTime nowUtc)
    {
        var normalizedQuality = NormalizeQuality(quality);
        var currentLevel = Math.Clamp(level, 0, 5);
        var normalizedStatus = NormalizeStatus(currentStatus);
        var updatedEaseFactor = CalculateEaseFactor(easeFactor, normalizedQuality);

        return normalizedQuality switch
        {
            5 => HandleRemembered(currentLevel, normalizedStatus, repetitions, updatedEaseFactor, intervalDays, lapseCount, nowUtc),
            3 => HandleHard(currentLevel, normalizedStatus, repetitions, updatedEaseFactor, intervalDays, lapseCount, nowUtc),
            _ => HandleForgotten(currentLevel, repetitions, updatedEaseFactor, intervalDays, lapseCount, nowUtc)
        };
    }

    private static SrsResult HandleRemembered(
        int level,
        string currentStatus,
        int repetitions,
        double easeFactor,
        int intervalDays,
        int lapseCount,
        DateTime nowUtc)
    {
        var newLevel = Math.Min(5, level + 1);
        var newStatus = DetermineStatus(newLevel, currentStatus == ReviewStates.Relearning);
        var newRepetitions = newLevel >= 3 ? Math.Max(1, repetitions + 1) : repetitions;

        if (newLevel <= 1)
        {
            return BuildStepResult(newLevel, newStatus, newRepetitions, easeFactor, 0, 0, lapseCount, nowUtc, LearningStepOne);
        }

        if (newLevel == 2)
        {
            return BuildStepResult(newLevel, newStatus, newRepetitions, easeFactor, 0, 1, lapseCount, nowUtc, LearningStepTwo);
        }

        var newInterval = newLevel switch
        {
            3 => 1,
            4 => Math.Max(3, intervalDays > 0 ? Math.Max(intervalDays + 1, (int)Math.Ceiling(intervalDays * 1.5)) : 3),
            5 when level < 5 => Math.Max(7, intervalDays > 0 ? Math.Max(intervalDays + 1, (int)Math.Round(intervalDays * 2.0)) : 7),
            _ => Math.Max(intervalDays + 1, (int)Math.Round(Math.Max(1, intervalDays) * Math.Max(1.4, easeFactor)))
        };

        return new SrsResult(
            Level: newLevel,
            Repetitions: newRepetitions,
            EaseFactor: easeFactor,
            IntervalDays: newInterval,
            LearningStepIndex: 0,
            LapseCount: lapseCount,
            NextReviewAt: nowUtc.AddDays(newInterval),
            Status: newStatus,
            RequeueInSession: false,
            RequeueAfterSeconds: null);
    }

    private static SrsResult HandleHard(
        int level,
        string currentStatus,
        int repetitions,
        double easeFactor,
        int intervalDays,
        int lapseCount,
        DateTime nowUtc)
    {
        if (level == 0)
        {
            return BuildStepResult(1, ReviewStates.Learning, repetitions, easeFactor, 0, 0, lapseCount, nowUtc, LearningStepTwo);
        }

        if (level <= 2)
        {
            var learningStepIndex = level == 1 ? 0 : 1;
            return BuildStepResult(level, ReviewStates.Learning, repetitions, easeFactor, 0, learningStepIndex, lapseCount, nowUtc, LearningStepTwo);
        }

        var newStatus = level >= 5 ? ReviewStates.Mastered : ReviewStates.Review;
        var newInterval = intervalDays > 0
            ? Math.Max(1, (int)Math.Ceiling(intervalDays * 0.6))
            : level switch
            {
                3 => 1,
                4 => 3,
                _ => 7
            };

        return new SrsResult(
            Level: level,
            Repetitions: Math.Max(1, repetitions),
            EaseFactor: easeFactor,
            IntervalDays: newInterval,
            LearningStepIndex: 0,
            LapseCount: lapseCount,
            NextReviewAt: nowUtc.AddDays(newInterval),
            Status: newStatus,
            RequeueInSession: false,
            RequeueAfterSeconds: null);
    }

    private static SrsResult HandleForgotten(
        int level,
        int repetitions,
        double easeFactor,
        int intervalDays,
        int lapseCount,
        DateTime nowUtc)
    {
        if (level == 0)
        {
            return BuildStepResult(0, ReviewStates.New, 0, easeFactor, 0, 0, lapseCount, nowUtc, LearningStepOne);
        }

        if (level <= 2)
        {
            var newLevel = Math.Max(0, level - 1);
            var status = newLevel == 0 ? ReviewStates.New : ReviewStates.Learning;
            return BuildStepResult(newLevel, status, 0, easeFactor, 0, newLevel == 2 ? 1 : 0, lapseCount, nowUtc, LearningStepOne);
        }

        var demotedLevel = level switch
        {
            3 => 2,
            4 => 2,
            _ => 3
        };

        return new SrsResult(
            Level: demotedLevel,
            Repetitions: Math.Max(0, repetitions - 1),
            EaseFactor: easeFactor,
            IntervalDays: Math.Max(0, intervalDays / 2),
            LearningStepIndex: 0,
            LapseCount: lapseCount + 1,
            NextReviewAt: nowUtc.Add(RelearningStep),
            Status: ReviewStates.Relearning,
            RequeueInSession: true,
            RequeueAfterSeconds: (int)RelearningStep.TotalSeconds);
    }

    private static SrsResult BuildStepResult(
        int level,
        string status,
        int repetitions,
        double easeFactor,
        int intervalDays,
        int learningStepIndex,
        int lapseCount,
        DateTime nowUtc,
        TimeSpan delay)
    {
        return new SrsResult(
            Level: level,
            Repetitions: repetitions,
            EaseFactor: easeFactor,
            IntervalDays: intervalDays,
            LearningStepIndex: learningStepIndex,
            LapseCount: lapseCount,
            NextReviewAt: nowUtc.Add(delay),
            Status: status,
            RequeueInSession: true,
            RequeueAfterSeconds: (int)delay.TotalSeconds);
    }

    private static string DetermineStatus(int level, bool fromRelearning)
    {
        if (level <= 0)
        {
            return ReviewStates.New;
        }

        if (level <= 2)
        {
            return ReviewStates.Learning;
        }

        if (level == 5)
        {
            return ReviewStates.Mastered;
        }

        return fromRelearning ? ReviewStates.Review : ReviewStates.Review;
    }

    private static double CalculateEaseFactor(double currentEaseFactor, int quality)
    {
        var updatedEaseFactor = currentEaseFactor + (0.1 - (5 - quality) * (0.08 + (5 - quality) * 0.02));
        return Math.Max(1.3, updatedEaseFactor);
    }

    private static int NormalizeQuality(int quality)
    {
        return quality switch
        {
            <= 1 => 1,
            2 or 3 or 4 => 3,
            _ => 5
        };
    }

    private static string NormalizeStatus(string? status)
    {
        return status?.Trim().ToLowerInvariant() switch
        {
            ReviewStates.Learning => ReviewStates.Learning,
            ReviewStates.Review => ReviewStates.Review,
            ReviewStates.Mastered => ReviewStates.Mastered,
            ReviewStates.Relearning => ReviewStates.Relearning,
            _ => ReviewStates.New
        };
    }
}
