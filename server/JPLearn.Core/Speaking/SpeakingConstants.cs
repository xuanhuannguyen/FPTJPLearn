namespace JPLearn.Core.Speaking;

public static class SpeakingCourseCodes
{
    public const string JPD113 = "jpd113";
    public const string JPD123 = "jpd123";

    public static readonly string[] All = [JPD113, JPD123];

    public static bool IsValid(string? value)
    {
        return !string.IsNullOrWhiteSpace(value)
            && All.Contains(value.Trim().ToLowerInvariant());
    }

    public static string Normalize(string value)
    {
        return value.Trim().ToLowerInvariant();
    }
}

public static class SpeakingAccessTiers
{
    public const string Free = "free";
    public const string Premium = "premium";
}
