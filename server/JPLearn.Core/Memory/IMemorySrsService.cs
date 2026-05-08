namespace JPLearn.Core.Memory;

public interface IMemorySrsService
{
    MemorySrsResult Calculate(
        int quality,
        int level,
        string? currentStatus,
        int repetitions,
        double easeFactor,
        int intervalMinutes,
        int intervalDays,
        int lapseCount,
        DateTime nowUtc);
}

public record MemorySrsResult(
    int Level,
    string Status,
    int Repetitions,
    double EaseFactor,
    int IntervalMinutes,
    int IntervalDays,
    DateTime NextReviewAt,
    int LapseCount,
    int LearningStepIndex,
    bool RequeueInSession,
    int? RequeueAfterSeconds,
    string Message
);
