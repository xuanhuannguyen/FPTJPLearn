using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Orders.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public long OrderCode { get; set; } // Numeric code for providers like PayOS
    public string PackageCode { get; set; } = string.Empty; // jpd113, jpd123, combo
    public decimal Amount { get; set; }
    public string Status { get; set; } = OrderStatuses.Pending;
    
    // Payment Tracking
    public string Provider { get; set; } = string.Empty; // SePay or PayOS
    public string ExternalId { get; set; } = string.Empty; // Transaction ID from provider
    public string PaymentUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; // Nội dung chuyển khoản (để khớp lệnh)

    public DateTime? PaidAt { get; set; }
}

public static class OrderStatuses
{
    public const string Pending = "pending";
    public const string Paid = "paid";
    public const string Cancelled = "cancelled";
    public const string Expired = "expired";
}

public static class PackageCodes
{
    public const string JPD113 = "jpd113";
    public const string JPD123 = "jpd123";
    public const string Combo = "combo";

    public static decimal GetPrice(string code) => code switch
    {
        JPD113 => 30000,
        JPD123 => 30000,
        Combo => 50000,
        _ => 0
    };
}
