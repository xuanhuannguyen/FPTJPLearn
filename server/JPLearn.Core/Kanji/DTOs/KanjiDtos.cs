namespace JPLearn.Core.Kanji.DTOs;

public class KanjiLevelDto
{
    public string Level { get; set; } = string.Empty;
    public int LessonCount { get; set; }
    public int KanjiCount { get; set; }
    public int VocabularyCount { get; set; }
    public int LearnedCount { get; set; }
    public int PracticedCount { get; set; }
}

public class KanjiLessonDto
{
    public Guid Id { get; set; }
    public string Level { get; set; } = string.Empty;
    public int LessonNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = KanjiAccessTiers.Free;
    public string? PackageCode { get; set; }
    public bool IsLocked { get; set; }
    public int KanjiCount { get; set; }
    public int VocabularyCount { get; set; }
    public int LearnedCount { get; set; }
    public int PracticedCount { get; set; }
}

public class KanjiLessonDetailDto
{
    public KanjiLessonDto Lesson { get; set; } = new();
    public List<KanjiItemDto> KanjiItems { get; set; } = [];
    public List<KanjiVocabularyDto> VocabularyItems { get; set; } = [];
}

public class KanjiItemDto
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Character { get; set; } = string.Empty;
    public string HanViet { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public int StrokeCount { get; set; }
    public string? KunReading { get; set; }
    public string? OnReading { get; set; }
    public string? Mnemonic { get; set; }
    public string AccessTier { get; set; } = KanjiAccessTiers.Free;
    public string? PackageCode { get; set; }
    public bool IsLocked { get; set; }
    public bool IsLearned { get; set; }
    public int WritingPracticeCount { get; set; }
    public int FlashcardPracticeCount { get; set; }
    public int OrderIndex { get; set; }
    public string? StrokeSvg { get; set; }
    public string? StrokeDataJson { get; set; }
    public string? ComponentMapJson { get; set; }
}

public class KanjiDetailDto : KanjiItemDto
{
    public List<KanjiVocabularyDto> VocabularyItems { get; set; } = [];
}

public class KanjiVocabularyDto
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public Guid? KanjiItemId { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Word { get; set; } = string.Empty;
    public string Reading { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string? ExampleJapanese { get; set; }
    public string? ExampleReading { get; set; }
    public string? ExampleMeaning { get; set; }
    public int OrderIndex { get; set; }
}

public class KanjiProgressDto
{
    public Guid Id { get; set; }
    public Guid KanjiItemId { get; set; }
    public bool IsLearned { get; set; }
    public DateTime? LastViewedAt { get; set; }
    public int WritingPracticeCount { get; set; }
    public int FlashcardPracticeCount { get; set; }
}
