using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using JPLearn.Core.Orders.Entities;
using JPLearn.Core.Users.Entities;
using Microsoft.AspNetCore.RateLimiting;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/payments")]
[EnableRateLimiting("webhook")]
public class PaymentWebhookController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PaymentWebhookController> _logger;

    public PaymentWebhookController(AppDbContext db, IConfiguration configuration, ILogger<PaymentWebhookController> logger)
    {
        _db = db;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("payos")]
    public async Task<IActionResult> PayOSWebhook([FromBody] JsonElement payload)
    {
        try
        {
            _logger.LogInformation("PayOS Webhook received: {Payload}", payload.GetRawText());
            
            if (!payload.TryGetProperty("data", out var data)) return Ok();
            
            // 1. Xác thực chữ ký (Manual Verification)
            var checksumKey = _configuration["PaymentSettings:PayOS:ChecksumKey"] ?? "";
            var signature = payload.GetProperty("signature").GetString();
            
            if (!VerifyPayOSSignature(data, signature, checksumKey))
            {
                _logger.LogWarning("PayOS Webhook: Invalid signature detected!");
                return Ok(); // Trả về Ok để tránh PayOS retry liên tục, nhưng log cảnh báo.
            }

            if (payload.GetProperty("code").GetString() != "00") return Ok(); 

            var orderCode = data.GetProperty("orderCode").GetInt64();

            // 2. Tìm đơn hàng (Sử dụng Idempotency: Chỉ xử lý đơn Pending)
            var orders = await _db.Orders
                .Where(o => o.Status == OrderStatuses.Pending && o.Provider == "PayOS")
                .ToListAsync();
            
            var order = orders.FirstOrDefault(o => Math.Abs(o.Id.GetHashCode()) == orderCode);

            if (order != null)
            {
                order.Status = OrderStatuses.Paid;
                order.PaidAt = DateTime.UtcNow;
                if (data.TryGetProperty("paymentLinkId", out var linkId))
                    order.ExternalId = linkId.GetString() ?? "";
                
                await ProcessActivation(order);
                await _db.SaveChangesAsync();
                _logger.LogInformation("PayOS Order {OrderId} activated successfully", order.Id);
            }

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing PayOS webhook");
            return Ok();
        }
    }

    private bool VerifyPayOSSignature(JsonElement data, string? receivedSignature, string checksumKey)
    {
        if (string.IsNullOrEmpty(receivedSignature)) return false;

        // PayOS sorted keys: amount, cancelUrl, description, orderCode, returnUrl (như trong PayOSProvider)
        // Lưu ý: Tùy vào webhook payload mà các trường có thể khác. 
        // Tuy nhiên PayOS Webhook Data thường gồm: orderCode, amount, description, v.v.
        // Cách an toàn nhất là sort toàn bộ keys trừ signature.
        
        var dict = new SortedDictionary<string, string>();
        foreach (var prop in data.EnumerateObject())
        {
            dict.Add(prop.Name, prop.Value.ToString());
        }

        var signData = string.Join("&", dict.Select(kv => $"{kv.Key}={kv.Value}"));
        
        using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(checksumKey));
        var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(signData));
        var computedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();

        return computedSignature == receivedSignature;
    }

    [HttpPost("sepay")]
    public async Task<IActionResult> SePayWebhook([FromBody] JsonElement rawPayload)
    {
        _logger.LogInformation("SEPAY WEBHOOK RECEIVED from IP: {IP} | Payload: {Payload}", 
            HttpContext.Connection.RemoteIpAddress, rawPayload.GetRawText());

        // 1. Kiểm tra Token bảo mật (Hỗ trợ cả Bearer và Apikey prefix)
        var expectedToken = _configuration["PaymentSettings:SePay:WebhookToken"];
        var authHeader = Request.Headers["Authorization"].ToString();
        var receivedToken = authHeader.Replace("Bearer ", "").Replace("Apikey ", "").Trim();

        if (string.IsNullOrEmpty(receivedToken) || receivedToken != expectedToken)
        {
            _logger.LogWarning("SePay Webhook: Unauthorized token.");
            return Unauthorized("Token không hợp lệ");
        }

        // 2. Trích xuất dữ liệu linh hoạt (Hỗ trợ cả snake_case và camelCase)
        string content = "";
        double amount = 0;

        if (rawPayload.TryGetProperty("content", out var contentProp)) content = contentProp.GetString() ?? "";
        
        if (rawPayload.TryGetProperty("transfer_amount", out var amountProp1)) amount = amountProp1.GetDouble();
        else if (rawPayload.TryGetProperty("transferAmount", out var amountProp2)) amount = amountProp2.GetDouble();
        else if (rawPayload.TryGetProperty("amount_in", out var amountProp3)) amount = amountProp3.GetDouble();

        _logger.LogInformation("Extracted Data -> Content: {Content}, Amount: {Amount}", content, amount);

        // 3. Tìm đơn hàng (Case-insensitive)
        var contentUpper = content.ToUpper();
        var pendingOrders = await _db.Orders
            .Where(o => o.Status == OrderStatuses.Pending && o.Provider == "SePay")
            .ToListAsync();

        var order = pendingOrders.FirstOrDefault(o => 
            !string.IsNullOrEmpty(o.ExternalId) && contentUpper.Contains(o.ExternalId.ToUpper()));

        // Nếu không tìm thấy qua ExternalId hoàn chỉnh, thử tìm qua mã ngắn (JP XXXX -> lấy XXXX)
        if (order == null)
        {
            order = pendingOrders.FirstOrDefault(o => 
                !string.IsNullOrEmpty(o.ExternalId) && 
                contentUpper.Contains(o.ExternalId.Replace("JP ", "").Trim().ToUpper()));
        }

        if (order == null)
        {
            _logger.LogWarning("SePay Webhook: No matching pending order found for content: {Content}", content);
            return Ok(new { message = "Không tìm thấy đơn hàng khớp" });
        }

        // 4. Kiểm tra số tiền
        if (amount < (double)order.Amount)
        {
            _logger.LogWarning("SePay Webhook: Insufficient amount. Order: {OrderId}, Req: {Req}, Rec: {Rec}", 
                order.Id, order.Amount, amount);
            return Ok(new { message = "Số tiền chuyển khoản không đủ" });
        }

        // 5. Cập nhật + mở khóa
        order.Status = OrderStatuses.Paid;
        order.PaidAt = DateTime.UtcNow;
        await ProcessActivation(order);
        await _db.SaveChangesAsync();

        _logger.LogInformation("SePay Order {OrderId} activated successfully!", order.Id);
        return Ok(new { success = true });
    }

    private async Task ProcessActivation(Order order)
    {
        // Thời hạn mặc định 6 tháng nếu không cấu hình
        int months = 6;
        if (int.TryParse(_configuration["PaymentSettings:SubscriptionMonths"], out var configMonths))
        {
            months = configMonths;
        }

        var expiresAt = DateTime.UtcNow.AddMonths(months);
        
        if (order.PackageCode == PackageCodes.Combo)
        {
            await AddOrUpdateSubscription(order.UserId, PackageCodes.JPD113, expiresAt);
            await AddOrUpdateSubscription(order.UserId, PackageCodes.JPD123, expiresAt);
        }
        else
        {
            await AddOrUpdateSubscription(order.UserId, order.PackageCode, expiresAt);
        }
    }

    private async Task AddOrUpdateSubscription(Guid userId, string courseCode, DateTime expiresAt)
    {
        var existing = await _db.Subscriptions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.CourseCode == courseCode);

        if (existing != null)
        {
            existing.ExpiresAt = expiresAt;
        }
        else
        {
            _db.Subscriptions.Add(new UserSubscription
            {
                UserId = userId,
                CourseCode = courseCode,
                ExpiresAt = expiresAt
            });
        }
    }
}

public class SePayWebhookPayload
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("transfer_amount")]
    public double TransferAmount { get; set; }

    [JsonPropertyName("reference_code")]
    public string? ReferenceCode { get; set; }
    
    [JsonPropertyName("gateway")]
    public string? Gateway { get; set; }
}
