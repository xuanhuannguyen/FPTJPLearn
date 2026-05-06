using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Vocabulary.Entities;

public class UserWordProgress : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid VocabularyItemId { get; set; }

    // Review scheduling fields
    public int Level { get; set; }
    public int Repetitions { get; set; }
    public double EaseFactor { get; set; } = 2.5;
    public int IntervalDays { get; set; }
    public DateTime NextReviewAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }
    public string Status { get; set; } = "new"; // new | learning | review | mastered | relearning
    public int LapseCount { get; set; }
    public int LearningStepIndex { get; set; }

    // Navigation
    public VocabularyItem VocabularyItem { get; set; } = null!;
}
