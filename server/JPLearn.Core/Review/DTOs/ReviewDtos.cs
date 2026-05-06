namespace JPLearn.Core.Review.DTOs;

public class ReviewCardDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Reading { get; set; } = string.Empty;
    public string WordType { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string? ExampleSentence { get; set; }
    public string? ExampleMeaning { get; set; }
    public int Level { get; set; }
    public string Status { get; set; } = "new";
    public DateTime NextReviewAt { get; set; }
    public int IntervalDays { get; set; }
    public int Repetitions { get; set; }
    public int LapseCount { get; set; }
    public int LearningStepIndex { get; set; }
}

public class ReviewAnswerDto
{
    public Guid ItemId { get; set; }
    public int Quality { get; set; } // 1=Quên, 3=Khó, 5=Nhớ
    public string Mode { get; set; } = string.Empty; // flashcard | multichoice | typing
    public string SessionType { get; set; } = ReviewSessionTypes.Due;
}

public class SaveSessionDto
{
    public Guid ListId { get; set; }
    public string Mode { get; set; } = string.Empty;
    public string SessionType { get; set; } = ReviewSessionTypes.Due;
    public int TotalCards { get; set; }
    public int CorrectCount { get; set; }
    public int WrongCount { get; set; }
    public int DurationSeconds { get; set; }
}

public class ReviewAnswerResultDto
{
    public Guid ItemId { get; set; }
    public int OldLevel { get; set; }
    public int NewLevel { get; set; }
    public string OldStatus { get; set; } = ReviewStates.New;
    public string NewStatus { get; set; } = ReviewStates.New;
    public DateTime NextReviewAt { get; set; }
    public int IntervalDays { get; set; }
    public int Repetitions { get; set; }
    public int LapseCount { get; set; }
    public int LearningStepIndex { get; set; }
    public bool RequeueInSession { get; set; }
    public int? RequeueAfterSeconds { get; set; }
}

public class ResetListProgressDto
{
    public string ResetType { get; set; } = ReviewResetTypes.All;
    public bool HardReset { get; set; }
    public List<Guid>? ItemIds { get; set; }
}

public class ResetListProgressResultDto
{
    public bool Success { get; set; }
    public int AffectedCount { get; set; }
}

public class CardsResponseDto
{
    public List<ReviewCardDto> Cards { get; set; } = [];
}

public class DueCardsResponse
{
    public int DueCount { get; set; }
    public List<ReviewCardDto> Cards { get; set; } = new();
}
