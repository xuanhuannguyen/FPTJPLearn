using JPLearn.Core.Memory.DTOs;

namespace JPLearn.Core.Memory;

public interface IMemoryService
{
    Task<MemorySummaryDto> GetSummaryAsync(Guid userId);
    Task<MemoryTypeSummaryDto> GetGrammarSummaryAsync(Guid userId);
}
