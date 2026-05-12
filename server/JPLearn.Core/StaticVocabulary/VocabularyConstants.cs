namespace JPLearn.Core.StaticVocabulary;

public static class VocabularyCourseCodes
{
    public const string Jpd113 = "jpd113";
    public const string Jpd123 = "jpd123";

    public static readonly string[] All = [Jpd113, Jpd123];

    public static bool IsValid(string? courseCode)
    {
        return All.Contains(courseCode?.Trim().ToLowerInvariant());
    }

    public static string Normalize(string courseCode)
    {
        return courseCode.Trim().ToLowerInvariant();
    }
}

public static class VocabularyAccessTiers
{
    public const string Free = "free";
    public const string Premium = "premium";
}

public static class VocabularyPracticeModes
{
    public const string Flashcard = "flashcard";
    public const string MultipleChoice = "multichoice";
    public const string Typing = "typing";

    public static string Normalize(string? mode)
    {
        return mode?.Trim().ToLowerInvariant() switch
        {
            MultipleChoice => MultipleChoice,
            Typing => Typing,
            _ => Flashcard
        };
    }
}
