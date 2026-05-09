using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Kanji.Entities;

public class UserKanjiProgress : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid KanjiItemId { get; set; }
    public KanjiItem KanjiItem { get; set; } = null!;

    public bool IsLearned { get; set; }
    public DateTime? LastViewedAt { get; set; }
    public int WritingPracticeCount { get; set; }
    public int FlashcardPracticeCount { get; set; }
}
