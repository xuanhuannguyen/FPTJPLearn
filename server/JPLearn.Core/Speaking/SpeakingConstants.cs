namespace JPLearn.Core.Speaking;

public static class SpeakingCourseCodes
{
    public const string JPD113 = "jpd113";
    public const string JPD123 = "jpd123";
    public const string JPD133 = "jpd133";

    public static readonly string[] All = [JPD113, JPD123, JPD133];

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

public static class SpeakingLessonTypes
{
    public const string Reading = "reading";
    public const string Qa = "qa";

    public static readonly string[] All = [Reading, Qa];
}
