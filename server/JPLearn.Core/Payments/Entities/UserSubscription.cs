using JPLearn.Core.Common.Entities;
using JPLearn.Core.Users.Entities;

namespace JPLearn.Core.Payments.Entities;

public class UserSubscription : BaseEntity
{
    public Guid UserId { get; set; }
    public string PlanCode { get; set; } = string.Empty;
    public string Status { get; set; } = SubscriptionStatuses.Active;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? PaymentProvider { get; set; }
    public string? ExternalTransactionId { get; set; }

    // Navigation
    public AppUser? User { get; set; }
}
