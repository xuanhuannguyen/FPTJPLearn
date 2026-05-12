using JPLearn.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    public AdminController(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    private bool IsAdmin()
    {
        var adminKey = Request.Headers["X-Admin-Key"].ToString();
        return adminKey == _configuration["AdminSettings:SecretKey"];
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        if (!IsAdmin()) return Unauthorized();
        var users = await _db.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
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
