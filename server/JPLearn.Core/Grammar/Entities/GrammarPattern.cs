using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Grammar.Entities;

public class GrammarPattern : BaseEntity
{
    public Guid LessonId { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string Structure { get; set; } = string.Empty;
    public string? UsageScope { get; set; }
    public string? Formation { get; set; }
    public string? Notes { get; set; }
    public string? TagsJson { get; set; }
    public string? AccessTierOverride { get; set; }
    public string? PackageCodeOverride { get; set; }
    public int OrderIndex { get; set; }

    public GrammarLesson Lesson { get; set; } = null!;
    public ICollection<GrammarExample> Examples { get; set; } = new List<GrammarExample>();
    public ICollection<GrammarExercise> Exercises { get; set; } = new List<GrammarExercise>();
    public ICollection<UserGrammarProgress> ProgressRecords { get; set; } = new List<UserGrammarProgress>();
}
