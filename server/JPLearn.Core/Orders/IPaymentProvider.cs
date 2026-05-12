using JPLearn.Core.Orders.Entities;

namespace JPLearn.Core.Orders;

public interface IPaymentProvider
{
    string ProviderName { get; }
    Task<PaymentLinkResult> CreatePaymentLinkAsync(Order order);
}

public record PaymentLinkResult(bool Success, string PaymentUrl, string ExternalId, string Error = "");
