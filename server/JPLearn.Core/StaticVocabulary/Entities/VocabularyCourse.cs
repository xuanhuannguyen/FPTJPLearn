using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.StaticVocabulary.Entities;

public class VocabularyCourse : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int OrderIndex { get; set; }

    public ICollection<VocabularyLesson> Lessons { get; set; } = new List<VocabularyLesson>();
}
