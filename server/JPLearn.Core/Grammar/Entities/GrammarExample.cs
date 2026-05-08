using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Grammar.Entities;

public class GrammarExample : BaseEntity
{
    public Guid PatternId { get; set; }
    public string Japanese { get; set; } = string.Empty;
    public string? Reading { get; set; }
    public string Meaning { get; set; } = string.Empty;
    public string? Note { get; set; }
    public int OrderIndex { get; set; }

    public GrammarPattern Pattern { get; set; } = null!;
}
