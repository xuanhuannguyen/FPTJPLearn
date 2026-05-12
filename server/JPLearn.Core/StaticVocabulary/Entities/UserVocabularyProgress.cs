using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.StaticVocabulary.Entities;

public class UserVocabularyProgress : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid VocabularyItemId { get; set; }
    public StaticVocabularyItem VocabularyItem { get; set; } = null!;

    public bool IsLearned { get; set; }
    public DateTime? LastViewedAt { get; set; }
    public int FlashcardPracticeCount { get; set; }
    public int MultipleChoicePracticeCount { get; set; }
    public int TypingPracticeCount { get; set; }
}
