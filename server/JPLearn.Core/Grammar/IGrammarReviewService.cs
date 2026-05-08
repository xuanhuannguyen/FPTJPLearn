using JPLearn.Core.Grammar.DTOs;

namespace JPLearn.Core.Grammar;

public interface IGrammarReviewService
{
    Task<AddGrammarStudyResultDto?> AddToStudyAsync(Guid userId, Guid patternId);
    Task<bool> RemoveFromStudyAsync(Guid userId, Guid patternId);
    Task<GrammarDueCardsResponse> GetDueCardsAsync(Guid userId);
    Task<GrammarAnswerResultDto?> SubmitAnswerAsync(Guid userId, SubmitGrammarAnswerDto dto);
    Task<GrammarProgressSummaryDto> GetProgressSummaryAsync(Guid userId);
}
