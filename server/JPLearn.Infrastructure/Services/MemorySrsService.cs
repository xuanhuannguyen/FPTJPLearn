using JPLearn.Core.Memory;

namespace JPLearn.Infrastructure.Services;

public class MemorySrsService : IMemorySrsService
{
    public MemorySrsResult Calculate(
        int quality,
        int level,
        string? currentStatus,
        int repetitions,
        double easeFactor,
        int intervalMinutes,
        int intervalDays,
        int lapseCount,
        DateTime nowUtc)
    {
        var normalizedQuality = NormalizeQuality(quality);
        var oldLevel = Math.Clamp(level, MemoryLevels.Min, MemoryLevels.Max);
        var normalizedStatus = NormalizeStatus(currentStatus);
        var updatedEase = CalculateEaseFactor(easeFactor, normalizedQuality);

        return normalizedQuality switch
        {
            1 => HandleAgain(oldLevel, repetitions, updatedEase, lapseCount, nowUtc),
            3 => HandleHard(oldLevel, repetitions, updatedEase, lapseCount, nowUtc),
            4 => HandleGood(oldLevel, repetitions, updatedEase, intervalDays, lapseCount, nowUtc),
            _ => HandleEasy(oldLevel, normalizedStatus, repetitions, updatedEase, intervalDays, lapseCount, nowUtc)
        };
    }

    private static MemorySrsResult HandleAgain(
        int oldLevel,
        int repetitions,
        double easeFactor,
        int lapseCount,
        DateTime nowUtc)
    {
        var nextLevel = Math.Max(1, oldLevel - 2);
        var status = oldLevel >= MemoryLevels.Review ? MemoryStates.Relearning : MemoryStates.Learning;
        const int delayMinutes = 1;

        return new MemorySrsResult(
            Level: nextLevel,
            Status: status,
            Repetitions: Math.Max(0, repetitions),
            EaseFactor: easeFactor,
            IntervalMinutes: delayMinutes,
            IntervalDays: 0,
            NextReviewAt: nowUtc.AddMinutes(delayMinutes),
            LapseCount: lapseCount + 1,
            LearningStepIndex: 0,
            RequeueInSession: true,
            RequeueAfterSeconds: delayMinutes * 60,
            Message: "Hẹn ôn lại sau 1 phút");
    }

    private static MemorySrsResult HandleHard(
        int oldLevel,
        int repetitions,
        double easeFactor,
        int lapseCount,
        DateTime nowUtc)
    {
        var nextLevel = Math.Max(1, oldLevel);
        const int delayMinutes = 6;

        return new MemorySrsResult(
            Level: nextLevel,
            Status: nextLevel <= 2 ? MemoryStates.Learning : MemoryStates.Review,
            Repetitions: Math.Max(0, repetitions + 1),
            EaseFactor: easeFactor,
            IntervalMinutes: delayMinutes,
            IntervalDays: 0,
            NextReviewAt: nowUtc.AddMinutes(delayMinutes),
            LapseCount: lapseCount,
            LearningStepIndex: 1,
            RequeueInSession: false,
            RequeueAfterSeconds: null,
            Message: "Hẹn ôn lại sau 6 phút");
    }

    private static MemorySrsResult HandleGood(
        int oldLevel,
        int repetitions,
        double easeFactor,
        int intervalDays,
        int lapseCount,
        DateTime nowUtc)
    {
        var nextLevel = oldLevel == 0 ? 2 : Math.Min(MemoryLevels.Max, oldLevel + 1);
        var nextIntervalDays = nextLevel <= 2
            ? 1
            : Math.Max(1, (int)Math.Round(Math.Max(1, intervalDays) * Math.Max(1.4, easeFactor)));

        return new MemorySrsResult(
            Level: nextLevel,
            Status: DetermineStatus(nextLevel),
            Repetitions: Math.Max(1, repetitions + 1),
            EaseFactor: easeFactor,
            IntervalMinutes: 0,
            IntervalDays: nextIntervalDays,
            NextReviewAt: nowUtc.AddDays(nextIntervalDays),
            LapseCount: lapseCount,
            LearningStepIndex: 0,
            RequeueInSession: false,
            RequeueAfterSeconds: null,
            Message: nextIntervalDays == 1 ? "Hẹn ôn lại sau 1 ngày" : $"Hẹn ôn lại sau {nextIntervalDays} ngày");
    }

    private static MemorySrsResult HandleEasy(
        int oldLevel,
        string currentStatus,
        int repetitions,
        double easeFactor,
        int intervalDays,
        int lapseCount,
        DateTime nowUtc)
    {
        var nextLevel = oldLevel == 0 ? 3 : Math.Min(MemoryLevels.Max, oldLevel + 2);
        var nextIntervalDays = nextLevel <= 3 && currentStatus is MemoryStates.New or MemoryStates.Learning
            ? 4
            : Math.Max(4, (int)Math.Round(Math.Max(1, intervalDays) * Math.Max(1.8, easeFactor * 1.3)));

        return new MemorySrsResult(
            Level: nextLevel,
            Status: DetermineStatus(nextLevel),
            Repetitions: Math.Max(1, repetitions + 1),
            EaseFactor: easeFactor,
            IntervalMinutes: 0,
            IntervalDays: nextIntervalDays,
            NextReviewAt: nowUtc.AddDays(nextIntervalDays),
            LapseCount: lapseCount,
            LearningStepIndex: 0,
            RequeueInSession: false,
            RequeueAfterSeconds: null,
            Message: nextIntervalDays == 4 ? "Hẹn ôn lại sau 4 ngày" : $"Hẹn ôn lại sau {nextIntervalDays} ngày");
    }

    private static string DetermineStatus(int level)
    {
        return level switch
        {
            <= 0 => MemoryStates.New,
            <= 2 => MemoryStates.Learning,
            >= MemoryLevels.Mastered => MemoryStates.Mastered,
            _ => MemoryStates.Review
        };
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
            2 or 3 => 3,
            4 => 4,
            _ => 5
        };
    }

    private static string NormalizeStatus(string? status)
    {
        return status?.Trim().ToLowerInvariant() switch
        {
            MemoryStates.Learning => MemoryStates.Learning,
            MemoryStates.Review => MemoryStates.Review,
            MemoryStates.Mastered => MemoryStates.Mastered,
            MemoryStates.Relearning => MemoryStates.Relearning,
            _ => MemoryStates.New
        };
    }
}
