using JPLearn.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JPLearn.Core.Users.Entities;
using JPLearn.Core.Settings;
using Microsoft.AspNetCore.RateLimiting;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/admin")]
[EnableRateLimiting("admin-strict")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AdminController> _logger;
    private readonly IAccessSettingsService _accessSettings;

    public AdminController(
        AppDbContext db,
        IConfiguration configuration,
        ILogger<AdminController> logger,
        IAccessSettingsService accessSettings)
    {
        _db = db;
        _configuration = configuration;
        _logger = logger;
        _accessSettings = accessSettings;
    }

    private bool IsAdmin()
    {
        var adminKey = Request.Headers["X-Admin-Key"].ToString().Trim();
        var secret = _configuration["AdminSettings:SecretKey"]?.Trim();
        
        if (string.IsNullOrEmpty(adminKey)) return false;
        
        return adminKey == secret;
    }

    [HttpGet("verify")]
    public IActionResult VerifyAdmin()
    {
        var adminKey = Request.Headers["X-Admin-Key"].ToString().Trim();
        var secret = _configuration["AdminSettings:SecretKey"]?.Trim();

        _logger.LogInformation("Admin verify attempt from IP: {IP}", 
            HttpContext.Connection.RemoteIpAddress);

        if (string.IsNullOrEmpty(adminKey)) return Unauthorized("Mã mật định rỗng");
        
        if (adminKey != secret)
        {
            _logger.LogWarning("Admin verify FAILED from IP: {IP}", 
                HttpContext.Connection.RemoteIpAddress);
            return Unauthorized("Mã bí mật không chính xác");
        }

        return Ok(new { message = "Authorized" });
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        if (!IsAdmin()) return Unauthorized();
        
        var users = await _db.Users
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.DisplayName,
                u.AvatarUrl,
                u.CreatedAt,
                u.ActiveDeviceToken,
                u.LastLoginAt,
                Subscriptions = _db.Subscriptions
                    .Where(s => s.UserId == u.Id)
                    .Select(s => new { s.CourseCode, s.ExpiresAt, IsActive = s.ExpiresAt > DateTime.UtcNow })
                    .ToList()
            })
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
            
        return Ok(users);
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        if (!IsAdmin()) return Unauthorized();
        var orders = await _db.Orders
            .Join(_db.Users, o => o.UserId, u => u.Id, (o, u) => new {
                o.Id,
                o.PackageCode,
                o.Amount,
                o.Provider,
                o.Status,
                o.CreatedAt,
                UserDisplayName = u.DisplayName,
                UserEmail = u.Email
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
        return Ok(orders);
    }

    [HttpPost("users/{userId}/subscriptions")]
    public async Task<IActionResult> UpdateSubscription(Guid userId, [FromBody] UpdateSubRequest request)
    {
        if (!IsAdmin()) return Unauthorized();

        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound();

        var courseCode = request.CourseCode.Trim().ToLower();
        var sub = await _db.Subscriptions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.CourseCode == courseCode);

        // Nếu ngày hết hạn ở quá khứ -> Thực hiện XÓA (Hủy quyền)
        if (request.ExpiresAt <= DateTime.UtcNow)
        {
            if (sub != null)
            {
                _db.Subscriptions.Remove(sub);
                await _db.SaveChangesAsync();
            }
            return Ok(new { message = "Subscription removed" });
        }

        // Nếu ngày hết hạn ở tương lai -> Thêm hoặc Cập nhật
        if (sub == null)
        {
            sub = new UserSubscription
            {
                UserId = userId,
                CourseCode = courseCode,
                ExpiresAt = request.ExpiresAt
            };
            _db.Subscriptions.Add(sub);
        }
        else
        {
            sub.ExpiresAt = request.ExpiresAt;
        }

        await _db.SaveChangesAsync();
        return Ok(new { message = "Subscription updated" });
    }

    [HttpPost("orders/clear-pending")]
    public async Task<IActionResult> ClearPendingOrders()
    {
        if (!IsAdmin()) return Unauthorized();
        var pendingOrders = await _db.Orders.Where(o => o.Status == "pending").ToListAsync();
        _db.Orders.RemoveRange(pendingOrders);
        await _db.SaveChangesAsync();
        return Ok(new { message = $"Deleted {pendingOrders.Count} pending orders" });
    }

    [HttpGet("access-settings")]
    public async Task<IActionResult> GetAccessSettings()
    {
        if (!IsAdmin()) return Unauthorized();

        var freeExperienceEnabled = await _accessSettings.IsFreeExperienceEnabledAsync();
        return Ok(new
        {
            licensingEnabled = !freeExperienceEnabled,
            freeExperienceEnabled
        });
    }

    [HttpPut("access-settings")]
    public async Task<IActionResult> UpdateAccessSettings([FromBody] UpdateAccessSettingsRequest request)
    {
        if (!IsAdmin()) return Unauthorized();

        var freeExperienceEnabled = !request.LicensingEnabled;
        await _accessSettings.SetFreeExperienceEnabledAsync(freeExperienceEnabled);

        return Ok(new
        {
            licensingEnabled = request.LicensingEnabled,
            freeExperienceEnabled
        });
    }

    [HttpPost("users/{userId}/reset-device")]
    public async Task<IActionResult> ResetDevice(Guid userId)
    {
        if (!IsAdmin()) return Unauthorized();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return NotFound();

        user.ActiveDeviceToken = null;
        await _db.SaveChangesAsync();
        return Ok(new { message = "Device reset successfully" });
    }
}

public class UpdateSubRequest
{
    public string CourseCode { get; set; } = "";
    public DateTime ExpiresAt { get; set; }
}

public class UpdateAccessSettingsRequest
{
    public bool LicensingEnabled { get; set; }
}
