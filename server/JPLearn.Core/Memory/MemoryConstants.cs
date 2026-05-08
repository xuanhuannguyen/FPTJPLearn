namespace JPLearn.Core.Memory;

public static class MemoryItemTypes
{
    public const string Grammar = "grammar";
    public const string Kanji = "kanji";
    public const string Vocabulary = "vocabulary";
}

public static class MemoryStates
{
    public const string New = "new";
    public const string Learning = "learning";
    public const string Review = "review";
    public const string Mastered = "mastered";
    public const string Relearning = "relearning";
}

public static class MemoryLevels
{
    public const int Min = 0;
    public const int Max = 5;
    public const int Review = 3;
    public const int Mastered = 5;
}

public static class MemoryScopes
{
    public const string Due = "due";
    public const string All = "all";
    public const string New = "new";
    public const string Learning = "learning";
    public const string ShortTerm = "short_term";
    public const string LongTerm = "long_term";
}
