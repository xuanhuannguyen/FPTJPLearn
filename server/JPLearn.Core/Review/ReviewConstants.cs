namespace JPLearn.Core.Review;

public static class ReviewStates
{
    public const string New = "new";
    public const string Learning = "learning";
    public const string Review = "review";
    public const string Mastered = "mastered";
    public const string Relearning = "relearning";
}

public static class ReviewLevels
{
    public const int Min = 0;
    public const int Max = 3;
    public const int Review = 2;
    public const int Mastered = 3;
}

public static class ReviewSessionTypes
{
    public const string Due = "due";
    public const string CheckLearned = "check_learned";
    public const string Relearn = "relearn";
}

public static class ReviewScopes
{
    public const string Mastered = "mastered";
    public const string Reviewed = "reviewed";
    public const string All = "all";
}

public static class ReviewResetTypes
{
    public const string All = "all";
    public const string Mastered = "mastered";
    public const string Selected = "selected";
}
