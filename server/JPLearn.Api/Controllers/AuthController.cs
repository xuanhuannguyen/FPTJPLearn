using JPLearn.Api.Infrastructure.Auth;
using JPLearn.Infrastructure.Data;
using JPLearn.Core.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ApiControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("sync")]
    public async Task<IActionResult> SyncUser([FromBody] SyncUserRequest request)
    {
        try
        {
            var userId = CurrentUserId;
            if (userId == Guid.Empty) return Unauthorized(new { error = "INVALID_USER_ID" });

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null)
            {
                user = new AppUser
                {
                    Id = userId,
                    FirebaseUid = HttpContext.Items["FirebaseUid"]?.ToString() ?? "",
                    Email = HttpContext.Items["FirebaseEmail"]?.ToString() ?? "",
                    DisplayName = HttpContext.Items["FirebaseName"]?.ToString() ?? request.DisplayName,
                    AvatarUrl = request.AvatarUrl,
                    ActiveDeviceToken = request.DeviceToken,
                    LastLoginAt = DateTime.UtcNow
                };
                _db.Users.Add(user);
            }
            else
            {
                user.ActiveDeviceToken = request.DeviceToken;
                user.LastLoginAt = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(request.DisplayName)) user.DisplayName = request.DisplayName;
                if (!string.IsNullOrEmpty(request.AvatarUrl)) user.AvatarUrl = request.AvatarUrl;
            }

            await _db.SaveChangesAsync();
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var userId = CurrentUserId;
        if (userId == Guid.Empty) return Unauthorized(new { error = "INVALID_USER_ID" });

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return NotFound(new { error = "USER_NOT_FOUND" });

        if (!string.IsNullOrWhiteSpace(request.DeviceToken) &&
            user.ActiveDeviceToken == request.DeviceToken)
        {
            user.ActiveDeviceToken = null;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        return Ok(new { message = "Logged out" });
    }

}

public class SyncUserRequest
{
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string DeviceToken { get; set; } = string.Empty;
}

public class LogoutRequest
{
    public string DeviceToken { get; set; } = string.Empty;
}
