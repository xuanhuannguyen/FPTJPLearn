using JPLearn.Core.Settings;
using JPLearn.Core.Settings.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JPLearn.Infrastructure.Services;

public class AccessSettingsService : IAccessSettingsService
{
    public const string FreeExperienceEnabledKey = "Payments:FreeExperienceEnabled";

    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    public AccessSettingsService(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public bool IsFreeExperienceEnabled()
    {
        var setting = _db.AppSettings
            .AsNoTracking()
            .FirstOrDefault(s => s.Key == FreeExperienceEnabledKey);

        if (setting != null && bool.TryParse(setting.Value, out var dbValue))
        {
            return dbValue;
        }

        return GetConfiguredDefault();
    }

    public async Task<bool> IsFreeExperienceEnabledAsync(CancellationToken cancellationToken = default)
    {
        var setting = await _db.AppSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Key == FreeExperienceEnabledKey, cancellationToken);

        if (setting != null && bool.TryParse(setting.Value, out var dbValue))
        {
            return dbValue;
        }

        return GetConfiguredDefault();
    }

    public async Task SetFreeExperienceEnabledAsync(bool isEnabled, CancellationToken cancellationToken = default)
    {
        var setting = await _db.AppSettings
            .FirstOrDefaultAsync(s => s.Key == FreeExperienceEnabledKey, cancellationToken);

        if (setting == null)
        {
            setting = new AppSetting
            {
                Key = FreeExperienceEnabledKey,
                Description = "When true, all paid content is opened for free experience mode."
            };
            _db.AppSettings.Add(setting);
        }

        setting.Value = isEnabled.ToString().ToLowerInvariant();
        setting.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
    }

    private bool GetConfiguredDefault()
    {
        return !bool.TryParse(_configuration[FreeExperienceEnabledKey], out var isEnabled) || isEnabled;
    }
}
