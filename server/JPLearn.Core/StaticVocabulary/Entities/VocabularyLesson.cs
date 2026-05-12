using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.StaticVocabulary.Entities;

public class VocabularyLesson : BaseEntity
{
    public Guid CourseId { get; set; }
    public VocabularyCourse Course { get; set; } = null!;

    public string CourseCode { get; set; } = string.Empty;
    public int LessonNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = VocabularyAccessTiers.Free;
    public string? PackageCode { get; set; }
    public int OrderIndex { get; set; }

    public ICollection<StaticVocabularyItem> Items { get; set; } = new List<StaticVocabularyItem>();
}
