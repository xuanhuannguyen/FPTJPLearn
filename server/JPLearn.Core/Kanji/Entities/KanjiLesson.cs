using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Kanji.Entities;

public class KanjiLesson : BaseEntity
{
    public string Level { get; set; } = string.Empty;
    public int LessonNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = KanjiAccessTiers.Free;
    public string? PackageCode { get; set; }
    public int OrderIndex { get; set; }

    public ICollection<KanjiItem> KanjiItems { get; set; } = new List<KanjiItem>();
    public ICollection<KanjiVocabulary> VocabularyItems { get; set; } = new List<KanjiVocabulary>();
}
