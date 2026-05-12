using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using JPLearn.Core.Orders.Entities;
using JPLearn.Core.Users.Entities;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/payment/webhook")]
public class PaymentWebhookController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    public PaymentWebhookController(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    [HttpPost("payos")]
    public async Task<IActionResult> PayOSWebhook([FromBody] JsonElement payload)
    {
        try
        {
            // PayOS gửi webhook với data chứa orderCode và code
            if (!payload.TryGetProperty("data", out var data)) return Ok();
            
            var code = data.GetProperty("code").GetString();
            if (code != "00") return Ok(); // Chỉ xử lý khi thanh toán thành công

            var orderCode = data.GetProperty("orderCode").GetInt64();

            // Tìm đơn hàng khớp hashCode
            var orders = await _db.Orders
                .Where(o => o.Status == OrderStatuses.Pending && o.Provider == "PayOS")
                .ToListAsync();
            
            var order = orders.FirstOrDefault(o => Math.Abs(o.Id.GetHashCode()) == orderCode);

            if (order != null)
            {
                order.Status = OrderStatuses.Paid;
                order.PaidAt = DateTime.UtcNow;
                await ProcessActivation(order);
                await _db.SaveChangesAsync();
            }

            return Ok(new { success = true });
        }
        catch (Exception)
        {
            return Ok();
        }
    }

    [HttpPost("sepay")]
    public async Task<IActionResult> SePayWebhook([FromBody] SePayWebhookPayload payload)
    {
        // 1. Kiểm tra Token bảo mật
        var expectedToken = _configuration["PaymentSettings:SePay:WebhookToken"];
        var receivedToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (receivedToken != expectedToken)
            return Unauthorized("Token không hợp lệ");

        // 2. Tìm đơn hàng dựa trên nội dung chuyển khoản
        var content = payload.Content?.ToUpper() ?? "";
        var order = await _db.Orders
            .FirstOrDefaultAsync(o => o.Status == OrderStatuses.Pending && content.Contains(o.ExternalId));

        if (order == null)
            return Ok(new { message = "Không tìm thấy đơn hàng khớp" });

        // 3. Kiểm tra số tiền
        if (payload.TransferAmount < (double)order.Amount)
            return Ok(new { message = "Số tiền chuyển khoản không đủ" });

        // 4. Cập nhật + mở khóa
        order.Status = OrderStatuses.Paid;
        order.PaidAt = DateTime.UtcNow;
        await ProcessActivation(order);
        await _db.SaveChangesAsync();

        return Ok(new { success = true });
    }

    private async Task ProcessActivation(Order order)
    {
        var expiresAt = DateTime.UtcNow.AddMonths(6);
        
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
    public string? Content { get; set; }
    public double TransferAmount { get; set; }
    public string? ReferenceCode { get; set; }
}
