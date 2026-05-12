using JPLearn.Core.Kanji.DTOs;

namespace JPLearn.Core.Kanji;

public interface IKanjiService
{
    Task<List<KanjiLevelDto>> GetLevelsAsync(Guid userId);
    Task<List<KanjiLessonDto>?> GetLessonsByLevelAsync(Guid userId, string level);
    Task<KanjiLessonDetailDto?> GetLessonDetailAsync(Guid userId, Guid lessonId);
    Task<KanjiDetailDto?> GetKanjiDetailAsync(Guid userId, Guid kanjiItemId);
    Task<List<KanjiItemDto>> SearchAsync(Guid userId, string query);
    Task<KanjiProgressDto?> AddToMemoryAsync(Guid userId, Guid kanjiItemId);
    Task<KanjiProgressDto?> RecordViewAsync(Guid userId, Guid kanjiItemId);
    Task<KanjiProgressDto?> RecordWritingPracticeAsync(Guid userId, Guid kanjiItemId);
    Task<KanjiProgressDto?> RecordFlashcardPracticeAsync(Guid userId, Guid kanjiItemId);
}
