using JPLearn.Core.Settings;
using JPLearn.Core.Settings.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JPLearn.Infrastructure.Services;

public class AccessSettingsService : IAccessSettingsService
{
    public const string AccessPolicyModeKey = "Access:PolicyMode";
    public const string FreeExperienceEnabledKey = "Payments:FreeExperienceEnabled";

    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    public AccessSettingsService(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public string GetAccessPolicyMode()
    {
        var setting = _db.AppSettings
            .AsNoTracking()
            .FirstOrDefault(s => s.Key == AccessPolicyModeKey);

        if (setting != null)
        {
            return AccessPolicyModes.Normalize(setting.Value);
        }

        return GetConfiguredPolicyModeDefault();
    }

    public async Task<string> GetAccessPolicyModeAsync(CancellationToken cancellationToken = default)
    {
        var setting = await _db.AppSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Key == AccessPolicyModeKey, cancellationToken);

        if (setting != null)
        {
            return AccessPolicyModes.Normalize(setting.Value);
        }

        return GetConfiguredPolicyModeDefault();
    }

    public async Task SetAccessPolicyModeAsync(string mode, CancellationToken cancellationToken = default)
    {
        var normalizedMode = AccessPolicyModes.Normalize(mode);
        var setting = await _db.AppSettings
            .FirstOrDefaultAsync(s => s.Key == AccessPolicyModeKey, cancellationToken);

        if (setting == null)
        {
            setting = new AppSetting
            {
                Key = AccessPolicyModeKey,
                Description = "Access policy mode: half or full licensing."
            };
            _db.AppSettings.Add(setting);
        }

        setting.Value = normalizedMode;
        setting.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
    }

    public bool IsHalfLicensingEnabled()
    {
        return GetAccessPolicyMode() == AccessPolicyModes.Half;
    }

    public bool IsFreeExperienceEnabled()
    {
        return IsHalfLicensingEnabled();
    }

    public async Task<bool> IsFreeExperienceEnabledAsync(CancellationToken cancellationToken = default)
    {
        return await GetAccessPolicyModeAsync(cancellationToken) == AccessPolicyModes.Half;
    }

    public async Task SetFreeExperienceEnabledAsync(bool isEnabled, CancellationToken cancellationToken = default)
    {
        await SetAccessPolicyModeAsync(isEnabled ? AccessPolicyModes.Half : AccessPolicyModes.Full, cancellationToken);
    }

    private string GetConfiguredPolicyModeDefault()
    {
        var configuredMode = _configuration[AccessPolicyModeKey];
        if (!string.IsNullOrWhiteSpace(configuredMode))
        {
            return AccessPolicyModes.Normalize(configuredMode);
        }

        return bool.TryParse(_configuration[FreeExperienceEnabledKey], out var freeExperienceEnabled) && !freeExperienceEnabled
            ? AccessPolicyModes.Full
            : AccessPolicyModes.Half;
    }
}
