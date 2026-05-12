namespace JPLearn.Core.StaticVocabulary.DTOs;

public class VocabularyCourseDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int LessonCount { get; set; }
    public int WordCount { get; set; }
    public int LearnedCount { get; set; }
    public int PracticedCount { get; set; }
}

public class StaticVocabularyLessonDto
{
    public Guid Id { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public int LessonNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = VocabularyAccessTiers.Free;
    public string? PackageCode { get; set; }
    public bool IsLocked { get; set; }
    public int WordCount { get; set; }
    public int LearnedCount { get; set; }
    public int PracticedCount { get; set; }
}

public class StaticVocabularyLessonDetailDto
{
    public StaticVocabularyLessonDto Lesson { get; set; } = new();
    public List<StaticVocabularyItemDto> Items { get; set; } = [];
}

public class StaticVocabularyItemDto
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string Word { get; set; } = string.Empty;
    public string Reading { get; set; } = string.Empty;
    public string WordType { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string? ExampleJapanese { get; set; }
    public string? ExampleReading { get; set; }
    public string? ExampleMeaning { get; set; }
    public string? Notes { get; set; }
    public string AccessTier { get; set; } = VocabularyAccessTiers.Free;
    public string? PackageCode { get; set; }
    public bool IsLocked { get; set; }
    public bool IsLearned { get; set; }
    public int FlashcardPracticeCount { get; set; }
    public int MultipleChoicePracticeCount { get; set; }
    public int TypingPracticeCount { get; set; }
    public int OrderIndex { get; set; }
}

public class StaticVocabularyProgressDto
{
    public Guid Id { get; set; }
    public Guid VocabularyItemId { get; set; }
    public bool IsLearned { get; set; }
    public DateTime? LastViewedAt { get; set; }
    public int FlashcardPracticeCount { get; set; }
    public int MultipleChoicePracticeCount { get; set; }
    public int TypingPracticeCount { get; set; }
}

public class VocabularyPracticeCardDto
{
    public Guid ItemId { get; set; }
    public string Mode { get; set; } = VocabularyPracticeModes.Flashcard;
    public string Prompt { get; set; } = string.Empty;
    public string? PromptReading { get; set; }
    public string CorrectAnswer { get; set; } = string.Empty;
    public List<string> Options { get; set; } = [];
    public string Word { get; set; } = string.Empty;
    public string Reading { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string? ExampleJapanese { get; set; }
    public string? ExampleMeaning { get; set; }
}
