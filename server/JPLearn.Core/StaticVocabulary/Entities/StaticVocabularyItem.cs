using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.StaticVocabulary.Entities;

public class StaticVocabularyItem : BaseEntity
{
    public Guid LessonId { get; set; }
    public VocabularyLesson Lesson { get; set; } = null!;

    public string CourseCode { get; set; } = string.Empty;
    public string Word { get; set; } = string.Empty;
    public string Reading { get; set; } = string.Empty;
    public string WordType { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string? ExampleJapanese { get; set; }
    public string? ExampleReading { get; set; }
    public string? ExampleMeaning { get; set; }
    public string? Notes { get; set; }
    public string? AccessTierOverride { get; set; }
    public string? PackageCodeOverride { get; set; }
    public int OrderIndex { get; set; }

    public ICollection<UserVocabularyProgress> ProgressRecords { get; set; } = new List<UserVocabularyProgress>();
}
