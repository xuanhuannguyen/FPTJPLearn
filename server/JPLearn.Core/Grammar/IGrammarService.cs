using JPLearn.Core.Grammar.DTOs;

namespace JPLearn.Core.Grammar;

public interface IGrammarService
{
    Task<List<GrammarLevelDto>> GetLevelsAsync(Guid userId);
    Task<List<GrammarLessonDto>?> GetLessonsByLevelAsync(Guid userId, string level);
    Task<GrammarLessonDto?> GetLessonAsync(Guid userId, Guid lessonId);
    Task<List<GrammarPatternDto>?> GetLessonPatternsAsync(Guid userId, Guid lessonId);
    Task<GrammarPatternDetailDto?> GetPatternDetailAsync(Guid userId, Guid patternId);
    Task<List<GrammarPatternDto>> SearchPatternsAsync(Guid userId, string query);
}
