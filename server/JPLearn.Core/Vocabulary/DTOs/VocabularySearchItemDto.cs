namespace JPLearn.Core.Vocabulary.DTOs;

public class VocabularySearchItemDto
{
    public Guid Id { get; set; }
    public Guid ListId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Reading { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string WordType { get; set; } = string.Empty;
}
