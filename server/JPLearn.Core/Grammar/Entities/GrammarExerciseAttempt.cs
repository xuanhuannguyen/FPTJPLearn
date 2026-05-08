using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Grammar.Entities;

public class GrammarExerciseAttempt : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid GrammarExerciseId { get; set; }
    public string? AnswerText { get; set; }
    public string? SelectedOptionOrderJson { get; set; }
    public bool IsCorrect { get; set; }
    public int? Score { get; set; }
    public string? Feedback { get; set; }
    public string CheckedBy { get; set; } = GrammarAttemptCheckers.System;

    public GrammarExercise GrammarExercise { get; set; } = null!;
}
