using Microsoft.AspNetCore.Mvc;
using JPLearn.Core.Review;
using JPLearn.Core.Review.DTOs;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/review")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _service;
    private static readonly Guid DevUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public ReviewController(IReviewService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get cards due for review (SRS)
    /// </summary>
    [HttpGet("{listId}/due")]
    public async Task<IActionResult> GetDueCards(Guid listId)
    {
        var result = await _service.GetDueCardsAsync(DevUserId, listId);
        return Ok(result);
    }

    /// <summary>
    /// Get learned cards in a list for re-checking without waiting until due
    /// </summary>
    [HttpGet("{listId}/learned")]
    public async Task<IActionResult> GetLearnedCards(Guid listId, [FromQuery] string scope = ReviewScopes.All)
    {
        var result = await _service.GetLearnedCardsAsync(DevUserId, listId, scope);
        return Ok(result);
    }

    /// <summary>
    /// Get cards in a list filtered by level range
    /// </summary>
    [HttpGet("{listId}/levels")]
    public async Task<IActionResult> GetCardsByLevel(Guid listId, [FromQuery] int minLevel = 0, [FromQuery] int maxLevel = 5)
    {
        var result = await _service.GetCardsByLevelAsync(DevUserId, listId, minLevel, maxLevel);
        return Ok(result);
    }

    /// <summary>
    /// Get all cards in a list (for multichoice/typing quiz)
    /// </summary>
    [HttpGet("{listId}/all")]
    public async Task<IActionResult> GetAllCards(Guid listId)
    {
        var cards = await _service.GetAllCardsAsync(DevUserId, listId);
        return Ok(cards);
    }

    /// <summary>
    /// Submit answer for a single card — updates SRS state
    /// </summary>
    [HttpPost("answer")]
    public async Task<IActionResult> SubmitAnswer([FromBody] ReviewAnswerDto dto)
    {
        var result = await _service.SubmitAnswerAsync(DevUserId, dto);
        if (result == null) return NotFound(new { message = "Card progress not found" });
        return Ok(result);
    }

    /// <summary>
    /// Save review session results
    /// </summary>
    [HttpPost("session")]
    public async Task<IActionResult> SaveSession([FromBody] SaveSessionDto dto)
    {
        var sessionId = await _service.SaveSessionAsync(DevUserId, dto);
        return Ok(new { sessionId });
    }

    /// <summary>
    /// Reset progress for a list or a subset of cards within that list
    /// </summary>
    [HttpPost("{listId}/reset")]
    public async Task<IActionResult> ResetProgress(Guid listId, [FromBody] ResetListProgressDto dto)
    {
        var result = await _service.ResetListProgressAsync(DevUserId, listId, dto);
        return Ok(result);
    }
}
