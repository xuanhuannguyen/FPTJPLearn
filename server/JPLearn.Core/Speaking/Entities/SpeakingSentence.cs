using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Speaking.Entities;

public class SpeakingSentence : BaseEntity
{
    public Guid LessonId { get; set; }
    public SpeakingLesson Lesson { get; set; } = null!;

    public int SentenceNumber { get; set; }
    public string PlainText { get; set; } = string.Empty;
    public string Romaji { get; set; } = string.Empty;
    public string ContentHtml { get; set; } = string.Empty;
    public string MeaningVi { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;
}
