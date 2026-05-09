using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Kanji.Entities;

public class KanjiItem : BaseEntity
{
    public Guid LessonId { get; set; }
    public KanjiLesson Lesson { get; set; } = null!;

    public string Level { get; set; } = string.Empty;
    public string Character { get; set; } = string.Empty;
    public string HanViet { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public int StrokeCount { get; set; }
    public string? KunReading { get; set; }
    public string? OnReading { get; set; }
    public string? Mnemonic { get; set; }
    public string? StrokeSvg { get; set; }
    public string? StrokeDataJson { get; set; }
    public string? ComponentMapJson { get; set; }
    public string? AccessTierOverride { get; set; }
    public string? PackageCodeOverride { get; set; }
    public int OrderIndex { get; set; }

    public ICollection<KanjiVocabulary> VocabularyItems { get; set; } = new List<KanjiVocabulary>();
    public ICollection<UserKanjiProgress> ProgressRecords { get; set; } = new List<UserKanjiProgress>();
}
