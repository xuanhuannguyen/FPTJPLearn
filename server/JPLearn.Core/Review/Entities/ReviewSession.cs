using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Review.Entities;

public class ReviewSession : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ListId { get; set; }
    public string Mode { get; set; } = string.Empty; // flashcard | multichoice | typing
    public int TotalCards { get; set; }
    public int CorrectCount { get; set; }
    public int WrongCount { get; set; }
    public int DurationSeconds { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
