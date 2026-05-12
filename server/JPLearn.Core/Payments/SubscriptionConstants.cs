namespace JPLearn.Core.Payments;

public static class SubscriptionStatuses
{
    public const string Active = "active";
    public const string Expired = "expired";
    public const string Cancelled = "cancelled";
}

public static class SubscriptionPlans
{
    public const string Free = "free";
    public const string Premium = "premium";

    public static readonly string[] All = [Free, Premium];
}
