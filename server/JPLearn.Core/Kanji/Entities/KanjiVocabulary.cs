using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Kanji.Entities;

public class KanjiVocabulary : BaseEntity
{
    public Guid LessonId { get; set; }
    public KanjiLesson Lesson { get; set; } = null!;

    public Guid? KanjiItemId { get; set; }
    public KanjiItem? KanjiItem { get; set; }

    public string Level { get; set; } = string.Empty;
    public string Word { get; set; } = string.Empty;
    public string Reading { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string? ExampleJapanese { get; set; }
    public string? ExampleReading { get; set; }
    public string? ExampleMeaning { get; set; }
    public int OrderIndex { get; set; }
}
