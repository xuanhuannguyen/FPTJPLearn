using JPLearn.Core.Review;

namespace JPLearn.Core.Grammar.DTOs;

public class GrammarLevelDto
{
    public string Level { get; set; } = string.Empty;
    public int LessonCount { get; set; }
    public int PatternCount { get; set; }
    public int FreeCount { get; set; }
    public int PremiumCount { get; set; }
    public int InStudyCount { get; set; }
    public int MasteredCount { get; set; }
    public int DueCount { get; set; }
}

public class GrammarLessonDto
{
    public Guid Id { get; set; }
    public string Level { get; set; } = string.Empty;
    public int LessonNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = GrammarAccessTiers.Free;
    public string? PackageCode { get; set; }
    public bool IsLocked { get; set; }
    public int PatternCount { get; set; }
    public int InStudyCount { get; set; }
    public int MasteredCount { get; set; }
    public int DueCount { get; set; }
}

public class GrammarPatternDto
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string Structure { get; set; } = string.Empty;
    public string AccessTier { get; set; } = GrammarAccessTiers.Free;
    public string? PackageCode { get; set; }
    public bool IsLocked { get; set; }
    public bool IsInStudy { get; set; }
    public GrammarProgressDto? Progress { get; set; }
}

public class GrammarPatternDetailDto : GrammarPatternDto
{
    public string? UsageScope { get; set; }
    public string? Formation { get; set; }
    public string? Notes { get; set; }
    public string? TagsJson { get; set; }
    public List<GrammarExampleDto> Examples { get; set; } = [];
    public List<GrammarExerciseDto> Exercises { get; set; } = [];
}

public class GrammarExampleDto
{
    public Guid Id { get; set; }
    public string Japanese { get; set; } = string.Empty;
    public string? Reading { get; set; }
    public string Meaning { get; set; } = string.Empty;
    public string? Note { get; set; }
    public int OrderIndex { get; set; }
}

public class GrammarExerciseDto
{
    public Guid Id { get; set; }
    public Guid PatternId { get; set; }
    public string ExerciseType { get; set; } = GrammarExerciseTypes.VietnameseToJapanese;
    public string Prompt { get; set; } = string.Empty;
    public string? PromptReading { get; set; }
    public string? Hint { get; set; }
    public string? Explanation { get; set; }
    public string? TemplateText { get; set; }
    public string? OptionsJson { get; set; }
    public List<string> Options { get; set; } = [];
    public int? StarPosition { get; set; }
    public int OrderIndex { get; set; }
}

public class GrammarExerciseAnswerDto : GrammarExerciseDto
{
    public string ExpectedAnswer { get; set; } = string.Empty;
    public string? AcceptableAnswersJson { get; set; }
    public List<string> AcceptableAnswers { get; set; } = [];
    public string? CorrectOrderJson { get; set; }
    public List<string> CorrectOrder { get; set; } = [];
    public string? StarAnswer { get; set; }
}

public class GrammarProgressDto
{
    public Guid Id { get; set; }
    public Guid PatternId { get; set; }
    public int Level { get; set; }
    public string Status { get; set; } = ReviewStates.New;
    public DateTime NextReviewAt { get; set; }
    public int IntervalDays { get; set; }
    public int Repetitions { get; set; }
    public int LapseCount { get; set; }
    public bool IsActive { get; set; }
}

public class AddGrammarStudyResultDto
{
    public bool Success { get; set; }
    public bool AlreadyExists { get; set; }
    public GrammarProgressDto? Progress { get; set; }
}

public class GrammarReviewCardDto
{
    public Guid ProgressId { get; set; }
    public Guid PatternId { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string Structure { get; set; } = string.Empty;
    public string? UsageScope { get; set; }
    public string? Formation { get; set; }
    public string? Notes { get; set; }
    public List<GrammarExampleDto> Examples { get; set; } = [];
    public int StudyLevel { get; set; }
    public string Status { get; set; } = ReviewStates.New;
    public DateTime NextReviewAt { get; set; }
    public int IntervalDays { get; set; }
    public int Repetitions { get; set; }
}

public class GrammarDueCardsResponse
{
    public int DueCount { get; set; }
    public List<GrammarReviewCardDto> Cards { get; set; } = [];
}

public class SubmitGrammarAnswerDto
{
    public Guid PatternId { get; set; }
    public int Quality { get; set; }
}

public class GrammarAnswerResultDto
{
    public Guid PatternId { get; set; }
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

public class GrammarProgressSummaryDto
{
    public int InStudyCount { get; set; }
    public int DueCount { get; set; }
    public int MasteredCount { get; set; }
    public int LearningCount { get; set; }
    public int ReviewCount { get; set; }
}

public class CheckGrammarExerciseDto
{
    public string? AnswerText { get; set; }
    public List<string>? SelectedOptionOrder { get; set; }
}

public class GrammarExerciseCheckResultDto
{
    public Guid ExerciseId { get; set; }
    public bool IsCorrect { get; set; }
    public int Score { get; set; }
    public string Feedback { get; set; } = string.Empty;
    public string ExpectedAnswer { get; set; } = string.Empty;
    public string? CorrectOrderJson { get; set; }
    public string? StarAnswer { get; set; }
    public Guid AttemptId { get; set; }
}

public class AiEvaluateGrammarExerciseDto : CheckGrammarExerciseDto
{
    public string Provider { get; set; } = "gemini";
    public string ApiKey { get; set; } = string.Empty;
}

public class AiGrammarEvaluationResultDto
{
    public Guid ExerciseId { get; set; }
    public int Score { get; set; }
    public bool IsAcceptable { get; set; }
    public string CorrectedAnswer { get; set; } = string.Empty;
    public string Feedback { get; set; } = string.Empty;
    public List<string> GrammarNotes { get; set; } = [];
    public Guid AttemptId { get; set; }
}
