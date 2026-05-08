using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Memory.Entities;

public class MemoryReviewSession : BaseEntity
{
    public Guid UserId { get; set; }
    public string ItemType { get; set; } = MemoryItemTypes.Grammar;
    public string Scope { get; set; } = MemoryScopes.Due;
    public int TotalCards { get; set; }
    public int AgainCount { get; set; }
    public int HardCount { get; set; }
    public int GoodCount { get; set; }
    public int EasyCount { get; set; }
    public int DurationSeconds { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
