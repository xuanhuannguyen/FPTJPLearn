using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Memory.Entities;

public class UserMemoryGrammarItem : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? SourceGrammarPatternId { get; set; }
    public string? SourceVersion { get; set; }
    public string Pattern { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string Structure { get; set; } = string.Empty;
    public string? UsageScope { get; set; }
    public string? Formation { get; set; }
    public string? ExampleJapanese { get; set; }
    public string? ExampleReading { get; set; }
    public string? ExampleMeaning { get; set; }
    public string? Notes { get; set; }
    public string? TagsJson { get; set; }
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
