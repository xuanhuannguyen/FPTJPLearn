using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Grammar.Entities;

public class GrammarLesson : BaseEntity
{
    public string Level { get; set; } = string.Empty;
    public int LessonNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = GrammarAccessTiers.Free;
    public string? PackageCode { get; set; }
    public int OrderIndex { get; set; }

    public ICollection<GrammarPattern> Patterns { get; set; } = new List<GrammarPattern>();
}
