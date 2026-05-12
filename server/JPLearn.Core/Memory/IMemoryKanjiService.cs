using JPLearn.Core.Memory.DTOs;

namespace JPLearn.Core.Memory;

public interface IMemoryKanjiService
{
    Task<AddKanjiToMemoryResultDto?> AddFromItemAsync(Guid userId, Guid kanjiItemId);
    Task<MemoryGrammarStatusDto> GetItemStatusAsync(Guid userId, Guid kanjiItemId);
    Task<MemoryCardsResponseDto> GetCardsAsync(Guid userId, string scope);
    Task<MemoryAnswerResultDto?> SubmitAnswerAsync(Guid userId, SubmitMemoryAnswerDto dto);
    Task<bool> RemoveAsync(Guid userId, Guid memoryItemId);
    Task<int> ResetAsync(Guid userId, ResetMemoryItemsDto dto);
}
