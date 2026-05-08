using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Grammar.Entities;

public class GrammarExercise : BaseEntity
{
    public Guid PatternId { get; set; }
    public string ExerciseType { get; set; } = GrammarExerciseTypes.VietnameseToJapanese;
    public string Prompt { get; set; } = string.Empty;
    public string? PromptReading { get; set; }
    public string ExpectedAnswer { get; set; } = string.Empty;
    public string? AcceptableAnswersJson { get; set; }
    public string? Hint { get; set; }
    public string? Explanation { get; set; }
    public string? TemplateText { get; set; }
    public string? OptionsJson { get; set; }
    public string? CorrectOrderJson { get; set; }
    public int? StarPosition { get; set; }
    public string? StarAnswer { get; set; }
    public int OrderIndex { get; set; }

    public GrammarPattern Pattern { get; set; } = null!;
    public ICollection<GrammarExerciseAttempt> Attempts { get; set; } = new List<GrammarExerciseAttempt>();
}
