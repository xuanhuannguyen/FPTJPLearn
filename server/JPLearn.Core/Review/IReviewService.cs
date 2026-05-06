using JPLearn.Core.Review.DTOs;

namespace JPLearn.Core.Review;

public interface IReviewService
{
    Task<DueCardsResponse> GetDueCardsAsync(Guid userId, Guid listId);
    Task<CardsResponseDto> GetLearnedCardsAsync(Guid userId, Guid listId, string scope);
    Task<CardsResponseDto> GetCardsByLevelAsync(Guid userId, Guid listId, int minLevel, int maxLevel);
    Task<CardsResponseDto> GetAllCardsAsync(Guid userId, Guid listId);
    Task<ReviewAnswerResultDto?> SubmitAnswerAsync(Guid userId, ReviewAnswerDto dto);
    Task<Guid> SaveSessionAsync(Guid userId, SaveSessionDto dto);
    Task<ResetListProgressResultDto> ResetListProgressAsync(Guid userId, Guid listId, ResetListProgressDto dto);
}
