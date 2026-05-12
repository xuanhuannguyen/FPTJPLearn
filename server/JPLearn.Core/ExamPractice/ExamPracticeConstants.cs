namespace JPLearn.Core.ExamPractice;

public static class ExamCourseCodes
{
    public const string JPD113 = "jpd113";
    public const string JPD123 = "jpd123";

    public static readonly string[] All = [JPD113, JPD123];
}

public static class ExamQuestionTypes
{
    public const string Standalone = "standalone";
    public const string Passage = "passage";
}

public static class ExamQuestionTopics
{
    public const string Kanji = "kanji";
    public const string Grammar = "grammar";
    public const string Vocabulary = "vocabulary";
    public const string OddOneOut = "odd_one_out";
    public const string Reading = "reading";

    public static readonly string[] All =
    [
        Kanji,
        Grammar,
        Vocabulary,
        OddOneOut,
        Reading
    ];

    public static string GetLabel(string topic)
    {
        return topic switch
        {
            Kanji => "Kanji",
            Grammar => "Ngữ pháp",
            Vocabulary => "Từ vựng",
            OddOneOut => "Phân biệt từ khác loại",
            Reading => "Đọc hiểu",
            _ => topic
        };
    }

    public static int GetOrderIndex(string topic)
    {
        return topic switch
        {
            Kanji => 1,
            Grammar => 2,
            Vocabulary => 3,
            OddOneOut => 4,
            Reading => 5,
            _ => int.MaxValue
        };
    }
}

public static class ExamAttemptModes
{
    public const string Practice = "practice";
    public const string Exam = "exam";
}

public static class ExamAttemptStatuses
{
    public const string InProgress = "in_progress";
    public const string Submitted = "submitted";
    public const string Expired = "expired";
}

public static class ExamDefaults
{
    public const int AttemptQuestionCount = 30;
    public const int AttemptDurationMinutes = 30;
}
