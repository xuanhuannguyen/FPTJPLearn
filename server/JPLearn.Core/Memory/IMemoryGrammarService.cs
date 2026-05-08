using JPLearn.Core.Memory.DTOs;

namespace JPLearn.Core.Memory;

public interface IMemoryGrammarService
{
    Task<AddGrammarToMemoryResultDto?> AddFromPatternAsync(Guid userId, Guid patternId);
    Task<MemoryGrammarStatusDto> GetPatternStatusAsync(Guid userId, Guid patternId);
    Task<MemoryCardsResponseDto> GetCardsAsync(Guid userId, string scope);
    Task<MemoryAnswerResultDto?> SubmitAnswerAsync(Guid userId, SubmitMemoryAnswerDto dto);
    Task<bool> RemoveAsync(Guid userId, Guid memoryItemId);
    Task<int> ResetAsync(Guid userId, ResetMemoryItemsDto dto);
}
