using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using JPLearn.Core.Orders;
using JPLearn.Core.Orders.Entities;
using Microsoft.Extensions.Configuration;

namespace JPLearn.Infrastructure.Services.Payments;

/// <summary>PayOS integration via HTTP API (no SDK dependency).</summary>
public class PayOSProvider : IPaymentProvider
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    public string ProviderName => "PayOS";

    public PayOSProvider(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api-merchant.payos.vn")
        };
        _httpClient.DefaultRequestHeaders.Add("x-client-id", _configuration["PaymentSettings:PayOS:ClientId"]);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _configuration["PaymentSettings:PayOS:ApiKey"]);
    }

    public async Task<PaymentLinkResult> CreatePaymentLinkAsync(Order order, string returnUrl, string cancelUrl)
    {
        try
        {
            long orderCode = Math.Abs(order.Id.GetHashCode());
            var checksumKey = _configuration["PaymentSettings:PayOS:ChecksumKey"] ?? "";

            var description = $"Thanh toan {order.PackageCode.ToUpper()}";

            var body = new
            {
                orderCode,
                amount = (int)order.Amount,
                description,
                cancelUrl,
                returnUrl,
                signature = ComputeSignature(orderCode, (int)order.Amount, description, cancelUrl, returnUrl, checksumKey)
            };

            var response = await _httpClient.PostAsJsonAsync("/v2/payment-requests", body);
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            if (json.GetProperty("code").GetString() == "00")
            {
                var data = json.GetProperty("data");
                return new PaymentLinkResult(
                    Success: true,
                    PaymentUrl: data.GetProperty("checkoutUrl").GetString() ?? "",
                    ExternalId: data.GetProperty("paymentLinkId").GetString() ?? ""
                );
            }

            var errMsg = json.GetProperty("desc").GetString() ?? "Unknown error";
            Console.WriteLine($"PayOS Error: {errMsg}");
            return new PaymentLinkResult(false, "", "", errMsg);
        }
        catch (Exception ex)
        {
            return new PaymentLinkResult(false, "", "", ex.Message);
        }
    }

    private static string ComputeSignature(long orderCode, int amount, string description, string cancelUrl, string returnUrl, string checksumKey)
    {
        // PayOS requires keys sorted alphabetically: amount, cancelUrl, description, orderCode, returnUrl
        var data = $"amount={amount}&cancelUrl={cancelUrl}&description={description}&orderCode={orderCode}&returnUrl={returnUrl}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(checksumKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
