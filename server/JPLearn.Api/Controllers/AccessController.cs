using JPLearn.Core.Settings;
using JPLearn.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/access")]
public class AccessController : ApiControllerBase
{
    private readonly AppDbContext _db;
    private readonly IAccessSettingsService _accessSettings;

    public AccessController(AppDbContext db, IAccessSettingsService accessSettings)
    {
        _db = db;
        _accessSettings = accessSettings;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyAccess()
    {
        var accessPolicyMode = _accessSettings.GetAccessPolicyMode();
        var isHalfLicensing = accessPolicyMode == AccessPolicyModes.Half;
        var userId = CurrentUserId;
        var now = DateTime.UtcNow;

        List<AccessSubscriptionDto> subscriptions = userId == Guid.Empty
            ? []
            : await _db.Subscriptions
                .Where(s => s.UserId == userId)
                .Select(s => new AccessSubscriptionDto(
                    s.CourseCode,
                    s.ExpiresAt,
                    s.ExpiresAt > now))
                .ToListAsync();

        var activeCourseCodes = subscriptions
            .Where(s => s.IsActive)
            .Select(s => s.CourseCode.Trim().ToLowerInvariant())
            .Distinct()
            .OrderBy(code => code)
            .ToList();

        return Ok(new AccessStatusDto(
            AccessPolicyMode: accessPolicyMode,
            LicensingEnabled: true,
            FreeExperienceEnabled: isHalfLicensing,
            ActiveCourseCodes: activeCourseCodes,
            Subscriptions: subscriptions));
    }
}

public sealed record AccessStatusDto(
    string AccessPolicyMode,
    bool LicensingEnabled,
    bool FreeExperienceEnabled,
    IReadOnlyList<string> ActiveCourseCodes,
    IReadOnlyList<AccessSubscriptionDto> Subscriptions);

public sealed record AccessSubscriptionDto(
    string CourseCode,
    DateTime ExpiresAt,
    bool IsActive);
