using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Vocabulary.Entities;

public class VocabularyItem : BaseEntity
{
    public Guid ListId { get; set; }
    public string Word { get; set; } = string.Empty;        // 行きます
    public string Reading { get; set; } = string.Empty;      // いきます
    public string WordType { get; set; } = string.Empty;     // Động từ nhóm 1
    public string Meaning { get; set; } = string.Empty;      // Đi
    public string? ExampleSentence { get; set; }             // 学校に行きます。
    public string? ExampleMeaning { get; set; }              // Tôi đi đến trường.
    public int OrderIndex { get; set; }

    // Navigation
    public VocabularyList List { get; set; } = null!;
    public ICollection<UserWordProgress> ProgressRecords { get; set; } = new List<UserWordProgress>();
}
