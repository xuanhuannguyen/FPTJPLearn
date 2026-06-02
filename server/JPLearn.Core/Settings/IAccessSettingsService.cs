namespace JPLearn.Core.Settings;

public interface IAccessSettingsService
{
    bool IsFreeExperienceEnabled();
    Task<bool> IsFreeExperienceEnabledAsync(CancellationToken cancellationToken = default);
    Task SetFreeExperienceEnabledAsync(bool isEnabled, CancellationToken cancellationToken = default);
}
