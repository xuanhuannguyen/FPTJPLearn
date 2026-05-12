namespace JPLearn.Core.Memory.DTOs;

public class MemoryTypeSummaryDto
{
    public int Due { get; set; }
    public int New { get; set; }
    public int Learning { get; set; }
    public int ShortTerm { get; set; }
    public int LongTerm { get; set; }
    public int TotalStudied { get; set; }
    public DateTime? NextReviewAt { get; set; }
}

public class MemorySummaryDto
{
    public MemoryTypeSummaryDto Kanji { get; set; } = new();
    public MemoryTypeSummaryDto Vocabulary { get; set; } = new();
    public MemoryTypeSummaryDto Grammar { get; set; } = new();
}

public class MemoryCardDto
{
    public Guid Id { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public Guid? SourceGrammarPatternId { get; set; }
<<<<<<< HEAD
    public Guid? SourceVocabularyItemId { get; set; }
=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
    public string FrontPrimary { get; set; } = string.Empty;
    public string? FrontSecondary { get; set; }
    public string? FrontMeta { get; set; }
    public string BackPrimary { get; set; } = string.Empty;
    public string? BackSecondary { get; set; }
    public string? Example { get; set; }
    public string? ExampleReading { get; set; }
    public string? ExampleMeaning { get; set; }
    public string? Notes { get; set; }
    public int Level { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime NextReviewAt { get; set; }
}

public class MemoryCardsResponseDto
{
    public int Count { get; set; }
    public List<MemoryCardDto> Cards { get; set; } = [];
}

public class AddGrammarToMemoryResultDto
{
    public Guid MemoryItemId { get; set; }
    public Guid SourceGrammarPatternId { get; set; }
    public bool AlreadyExists { get; set; }
    public bool IsActive { get; set; }
}

public class AddKanjiToMemoryResultDto
{
    public Guid MemoryItemId { get; set; }
    public Guid SourceKanjiItemId { get; set; }
    public bool AlreadyExists { get; set; }
    public bool IsActive { get; set; }
}

<<<<<<< HEAD
public class AddVocabularyToMemoryResultDto
{
    public Guid MemoryItemId { get; set; }
    public Guid SourceVocabularyItemId { get; set; }
    public bool AlreadyExists { get; set; }
    public bool IsActive { get; set; }
}

=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
public class MemoryGrammarStatusDto
{
    public bool IsInMemory { get; set; }
    public Guid? MemoryItemId { get; set; }
    public bool IsActive { get; set; }
}

public class MemoryVocabularyStatusDto
{
    public bool IsInMemory { get; set; }
    public Guid? MemoryItemId { get; set; }
    public bool IsActive { get; set; }
}

public class SubmitMemoryAnswerDto
{
    public Guid MemoryItemId { get; set; }
    public int Quality { get; set; }
    public Guid? SessionId { get; set; }
}

public class MemoryAnswerResultDto
{
    public Guid MemoryItemId { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public int OldLevel { get; set; }
    public int Level { get; set; }
    public string OldStatus { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int IntervalMinutes { get; set; }
    public int IntervalDays { get; set; }
    public DateTime NextReviewAt { get; set; }
    public int Repetitions { get; set; }
    public int LapseCount { get; set; }
    public bool RequeueInSession { get; set; }
    public int? RequeueAfterSeconds { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ResetMemoryItemsDto
{
    public string Scope { get; set; } = MemoryScopes.All;
    public List<Guid> MemoryItemIds { get; set; } = [];
}
