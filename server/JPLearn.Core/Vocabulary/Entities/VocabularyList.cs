using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Vocabulary.Entities;

public class VocabularyList : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int WordCount { get; set; }

    // Navigation
    public ICollection<VocabularyItem> Items { get; set; } = new List<VocabularyItem>();
}
