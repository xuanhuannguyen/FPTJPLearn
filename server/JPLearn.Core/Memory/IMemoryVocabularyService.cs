using JPLearn.Core.Memory.DTOs;

namespace JPLearn.Core.Memory;

public interface IMemoryVocabularyService
{
    Task<AddVocabularyToMemoryResultDto?> AddFromItemAsync(Guid userId, Guid vocabularyItemId);
    Task<MemoryVocabularyStatusDto> GetItemStatusAsync(Guid userId, Guid vocabularyItemId);
    Task<MemoryCardsResponseDto> GetCardsAsync(Guid userId, string scope);
    Task<MemoryAnswerResultDto?> SubmitAnswerAsync(Guid userId, SubmitMemoryAnswerDto dto);
    Task<bool> RemoveAsync(Guid userId, Guid memoryItemId);
    Task<int> ResetAsync(Guid userId, ResetMemoryItemsDto dto);
}
