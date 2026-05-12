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

    public async Task<PaymentLinkResult> CreatePaymentLinkAsync(Order order)
    {
        try
        {
            long orderCode = Math.Abs(order.Id.GetHashCode());
            var domain = _configuration["AppSettings:BaseUrl"] ?? "http://localhost:5175";
            var checksumKey = _configuration["PaymentSettings:PayOS:ChecksumKey"] ?? "";

            var body = new
            {
                orderCode,
                amount = (int)order.Amount,
                description = $"Thanh toan {order.PackageCode.ToUpper()}",
                cancelUrl = $"{domain}/payment/cancel",
                returnUrl = $"{domain}/payment/success",
                signature = ComputeSignature(orderCode, (int)order.Amount, $"Thanh toan {order.PackageCode.ToUpper()}", checksumKey)
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
            return new PaymentLinkResult(false, "", "", errMsg);
        }
        catch (Exception ex)
        {
            return new PaymentLinkResult(false, "", "", ex.Message);
        }
    }

    private static string ComputeSignature(long orderCode, int amount, string description, string checksumKey)
    {
        var data = $"amount={amount}&cancelUrl=&description={description}&orderCode={orderCode}&returnUrl=";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(checksumKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
