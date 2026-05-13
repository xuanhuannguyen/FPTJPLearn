using JPLearn.Core.Vocabulary.DTOs;

namespace JPLearn.Core.Vocabulary;

public interface IVocabularyService
{
    Task<Guid> ImportAsync(Guid userId, ImportVocabularyDto dto);
    Task<List<VocabularyListDto>> GetListsAsync(Guid userId);
    Task<VocabularyListDetailDto?> GetByIdAsync(Guid userId, Guid listId);
    Task<bool> UpdateAsync(Guid userId, Guid listId, string name, string? description);
    Task<bool> DeleteListAsync(Guid userId, Guid listId);
    Task<bool> DeleteItemAsync(Guid userId, Guid itemId);
    Task<Guid> AddItemAsync(Guid userId, Guid listId, VocabularyWordDto dto);
    Task<List<VocabularySearchItemDto>> GetSearchIndexAsync(Guid userId);
    Task<VocabularyQuotaDto> GetQuotaAsync(Guid userId);
}
