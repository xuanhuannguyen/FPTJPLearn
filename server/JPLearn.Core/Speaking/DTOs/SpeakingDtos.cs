namespace JPLearn.Core.Speaking.DTOs;

public class SpeakingCourseDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = SpeakingAccessTiers.Free;
    public string? PackageCode { get; set; }
    public bool IsLocked { get; set; }
    public int LessonCount { get; set; }
    public int SentenceCount { get; set; }
}

public class SpeakingLessonDto
{
    public Guid Id { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public int LessonNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = SpeakingAccessTiers.Free;
    public string? PackageCode { get; set; }
    public bool IsLocked { get; set; }
    public int SentenceCount { get; set; }
}

public class SpeakingLessonDetailDto
{
    public SpeakingLessonDto Lesson { get; set; } = new();
    public List<SpeakingSentenceDto> Sentences { get; set; } = [];
}

public class SpeakingSentenceDto
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public int SentenceNumber { get; set; }
    public string PlainText { get; set; } = string.Empty;
    public string Romaji { get; set; } = string.Empty;
    public string ContentHtml { get; set; } = string.Empty;
    public string MeaningVi { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}
