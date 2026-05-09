using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Memory.Entities;

public class UserMemoryKanjiItem : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? SourceKanjiItemId { get; set; }
    public string Character { get; set; } = string.Empty;
    public string HanViet { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string? KunReading { get; set; }
    public string? OnReading { get; set; }
    public string? Mnemonic { get; set; }
    public string? KanjiLevel { get; set; }
    public int StrokeCount { get; set; }
    public int Level { get; set; }
    public string Status { get; set; } = MemoryStates.New;
    public int Repetitions { get; set; }
    public double EaseFactor { get; set; } = 2.5;
    public int IntervalMinutes { get; set; }
    public int IntervalDays { get; set; }
    public DateTime NextReviewAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public int LapseCount { get; set; }
    public int LearningStepIndex { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
