using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Users.Entities;

public class UserSubscription : BaseEntity
{
    public Guid UserId { get; set; }
    public string CourseCode { get; set; } = string.Empty; // jpd113, jpd123
    public DateTime ExpiresAt { get; set; }
    public bool IsActive => ExpiresAt > DateTime.UtcNow;

    // Navigation
    public AppUser? User { get; set; }
}
