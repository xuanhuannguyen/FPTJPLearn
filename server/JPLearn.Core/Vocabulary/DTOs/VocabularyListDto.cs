namespace JPLearn.Core.Vocabulary.DTOs;

public class VocabularyListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int WordCount { get; set; }
    public int MasteredCount { get; set; }
    public int DueCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class VocabularyListDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int WordCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<VocabularyItemDto> Items { get; set; } = new();
}

public class VocabularyItemDto
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Reading { get; set; } = string.Empty;
    public string WordType { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string? ExampleSentence { get; set; }
    public string? ExampleMeaning { get; set; }
    public int OrderIndex { get; set; }
    public int Level { get; set; }
    public string Status { get; set; } = "new";
}
