using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Grammar.Entities;

public class UserGrammarProgress : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid GrammarPatternId { get; set; }
    public int Level { get; set; }
    public string Status { get; set; } = "new";
    public int Repetitions { get; set; }
    public double EaseFactor { get; set; } = 2.5;
    public int IntervalDays { get; set; }
    public DateTime NextReviewAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public int LapseCount { get; set; }
    public int LearningStepIndex { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public GrammarPattern GrammarPattern { get; set; } = null!;
}
