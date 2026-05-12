using JPLearn.Core.Orders;
using JPLearn.Core.Orders.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ApiControllerBase
{
    private readonly AppDbContext _db;
    private readonly IEnumerable<IPaymentProvider> _providers;

    public OrderController(AppDbContext db, IEnumerable<IPaymentProvider> providers)
    {
        _db = db;
        _providers = providers;
    }

    /// <summary>Tạo đơn hàng mới + lấy link thanh toán (xoay vòng provider)</summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var price = PackageCodes.GetPrice(request.PackageCode);
        if (price == 0) return BadRequest(new { error = "Gói không hợp lệ" });

        var order = new Order
        {
            UserId = CurrentUserId,
            PackageCode = request.PackageCode,
            Amount = price,
            Description = $"JPLearn {request.PackageCode.ToUpper()}"
        };

        // Chọn provider xoay vòng
        var provider = await SelectProviderAsync();
        order.Provider = provider.ProviderName;

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        // Tạo link thanh toán
        var result = await provider.CreatePaymentLinkAsync(order);
        if (!result.Success)
        {
            // Provider chính lỗi → thử provider còn lại
            var fallback = _providers.FirstOrDefault(p => p.ProviderName != provider.ProviderName);
            if (fallback != null)
            {
                result = await fallback.CreatePaymentLinkAsync(order);
                order.Provider = fallback.ProviderName;
                await _db.SaveChangesAsync();
            }

            if (!result.Success)
                return StatusCode(500, new { error = "Không thể tạo link thanh toán", detail = result.Error });
        }

        order.ExternalId = result.ExternalId;
        order.PaymentUrl = result.PaymentUrl;
        await _db.SaveChangesAsync();

        return Ok(new
        {
            orderId = order.Id,
            paymentUrl = result.PaymentUrl,
            provider = order.Provider,
            amount = order.Amount,
            packageCode = order.PackageCode
        });
    }

    /// <summary>Lấy danh sách đơn hàng của user</summary>
    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders()
    {
        var orders = await _db.Orders
            .Where(o => o.UserId == CurrentUserId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new
            {
                o.Id,
                o.PackageCode,
                o.Amount,
                o.Status,
                o.Provider,
                o.PaidAt,
                o.CreatedAt
            })
            .ToListAsync();

        return Ok(orders);
    }

    /// <summary>Lấy thông tin subscription hiện tại</summary>
    [HttpGet("subscriptions")]
    public async Task<IActionResult> GetMySubscriptions()
    {
        var subs = await _db.Subscriptions
            .Where(s => s.UserId == CurrentUserId)
            .Select(s => new
            {
                s.CourseCode,
                s.ExpiresAt,
                isActive = s.ExpiresAt > DateTime.UtcNow
            })
            .ToListAsync();

        return Ok(subs);
    }

    /// <summary>Bảng giá</summary>
    [HttpGet("packages")]
    public IActionResult GetPackages()
    {
        return Ok(new object[]
        {
            new { code = PackageCodes.JPD113, name = "JPD113", price = 80000, originalPrice = (int?)null, duration = "6 tháng", discount = (string?)null },
            new { code = PackageCodes.JPD123, name = "JPD123", price = 100000, originalPrice = (int?)null, duration = "6 tháng", discount = (string?)null },
            new { code = PackageCodes.Combo, name = "Combo JPD113 + JPD123", price = 150000, originalPrice = (int?)180000, duration = "6 tháng", discount = (string?)"Giảm 17%" }
        });
    }

    /// <summary>Xoay vòng provider dựa trên số đơn trong ngày</summary>
    private async Task<IPaymentProvider> SelectProviderAsync()
    {
        var todayOrderCount = await _db.Orders
            .CountAsync(o => o.CreatedAt.Date == DateTime.UtcNow.Date);

        var providerList = _providers.ToList();
        if (providerList.Count == 0)
            throw new InvalidOperationException("No payment providers registered");

        // Xoay vòng: đơn chẵn → SePay, đơn lẻ → PayOS
        return providerList[todayOrderCount % providerList.Count];
    }
}

public record CreateOrderRequest(string PackageCode);
