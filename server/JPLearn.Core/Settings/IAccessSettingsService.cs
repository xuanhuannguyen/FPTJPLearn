namespace JPLearn.Core.Settings;

public interface IAccessSettingsService
{
    string GetAccessPolicyMode();
    Task<string> GetAccessPolicyModeAsync(CancellationToken cancellationToken = default);
    Task SetAccessPolicyModeAsync(string mode, CancellationToken cancellationToken = default);
    bool IsHalfLicensingEnabled();
    bool IsFreeExperienceEnabled();
    Task<bool> IsFreeExperienceEnabledAsync(CancellationToken cancellationToken = default);
    Task SetFreeExperienceEnabledAsync(bool isEnabled, CancellationToken cancellationToken = default);
}

public static class AccessPolicyModes
{
    public const string Half = "half";
    public const string Full = "full";

    public static string Normalize(string? value)
    {
        return string.Equals(value?.Trim(), Full, StringComparison.OrdinalIgnoreCase)
            ? Full
            : Half;
    }
}
