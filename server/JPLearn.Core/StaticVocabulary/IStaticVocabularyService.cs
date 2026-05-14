using JPLearn.Core.StaticVocabulary.DTOs;

namespace JPLearn.Core.StaticVocabulary;

public interface IStaticVocabularyService
{
    Task<List<VocabularyCourseDto>> GetCoursesAsync(Guid userId);
    Task<List<StaticVocabularyLessonDto>?> GetLessonsByCourseAsync(Guid userId, string courseCode);
    Task<StaticVocabularyLessonDetailDto?> GetLessonDetailAsync(Guid userId, Guid lessonId);
    Task<StaticVocabularyItemDto?> GetItemDetailAsync(Guid userId, Guid itemId);
    Task<List<StaticVocabularyItemDto>> SearchAsync(Guid userId, string query, string? courseCode);
    Task<List<VocabularyPracticeCardDto>?> GetLessonPracticeCardsAsync(Guid userId, Guid lessonId, string mode);
    Task<StaticVocabularyProgressDto?> RecordViewAsync(Guid userId, Guid itemId);
    Task<StaticVocabularyProgressDto?> RecordFlashcardPracticeAsync(Guid userId, Guid itemId);
    Task<StaticVocabularyProgressDto?> RecordMultipleChoicePracticeAsync(Guid userId, Guid itemId);
    Task<StaticVocabularyProgressDto?> RecordTypingPracticeAsync(Guid userId, Guid itemId);
}
