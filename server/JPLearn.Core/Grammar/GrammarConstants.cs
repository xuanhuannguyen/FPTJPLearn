namespace JPLearn.Core.Grammar;

public static class GrammarLevels
{
    public const string N5 = "N5";
    public const string N4 = "N4";
    public const string N3 = "N3";
    public const string N2 = "N2";
    public const string N1 = "N1";

    public static readonly string[] All = [N5, N4, N3, N2, N1];

    public static bool IsValid(string? level)
    {
        return All.Contains(level?.Trim().ToUpperInvariant());
    }

    public static string Normalize(string level)
    {
        return level.Trim().ToUpperInvariant();
    }
}

public static class GrammarAccessTiers
{
    public const string Free = "free";
    public const string Premium = "premium";
}

public static class GrammarExerciseTypes
{
    public const string VietnameseToJapanese = "vi_to_ja";
    public const string JapaneseToVietnamese = "ja_to_vi";
    public const string Arrange = "arrange";
}

public static class GrammarAttemptCheckers
{
    public const string System = "system";
    public const string Ai = "ai";
}
