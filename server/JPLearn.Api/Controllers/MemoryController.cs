using JPLearn.Core.Memory;
using JPLearn.Core.Memory.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/memory")]
public class MemoryController : ControllerBase
{
    private readonly IMemoryService _memoryService;
    private readonly IMemoryGrammarService _grammarService;
    private static readonly Guid DevUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public MemoryController(IMemoryService memoryService, IMemoryGrammarService grammarService)
    {
        _memoryService = memoryService;
        _grammarService = grammarService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _memoryService.GetSummaryAsync(DevUserId);
        return Ok(result);
    }

    [HttpGet("grammar/summary")]
    public async Task<IActionResult> GetGrammarSummary()
    {
        var result = await _memoryService.GetGrammarSummaryAsync(DevUserId);
        return Ok(result);
    }

    [HttpPost("grammar/from-pattern/{patternId}")]
    public async Task<IActionResult> AddGrammarFromPattern(Guid patternId)
    {
        var result = await _grammarService.AddFromPatternAsync(DevUserId, patternId);
        if (result == null)
        {
            return NotFound(new { message = "Grammar pattern not found" });
        }

        return Ok(result);
    }

    [HttpGet("grammar/from-pattern/{patternId}/status")]
    public async Task<IActionResult> GetGrammarPatternMemoryStatus(Guid patternId)
    {
        var result = await _grammarService.GetPatternStatusAsync(DevUserId, patternId);
        return Ok(result);
    }

    [HttpGet("grammar/cards")]
    public async Task<IActionResult> GetGrammarCards([FromQuery] string scope = MemoryScopes.Due)
    {
        var result = await _grammarService.GetCardsAsync(DevUserId, scope);
        return Ok(result);
    }

    [HttpPost("grammar/answer")]
    public async Task<IActionResult> SubmitGrammarAnswer([FromBody] SubmitMemoryAnswerDto dto)
    {
        var result = await _grammarService.SubmitAnswerAsync(DevUserId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Memory grammar item not found" });
        }

        return Ok(result);
    }

    [HttpDelete("grammar/{memoryItemId}")]
    public async Task<IActionResult> RemoveGrammarItem(Guid memoryItemId)
    {
        var removed = await _grammarService.RemoveAsync(DevUserId, memoryItemId);
        if (!removed)
        {
            return NotFound(new { message = "Memory grammar item not found" });
        }

        return Ok(new { success = true });
    }

    [HttpPost("grammar/reset")]
    public async Task<IActionResult> ResetGrammar([FromBody] ResetMemoryItemsDto dto)
    {
        var count = await _grammarService.ResetAsync(DevUserId, dto);
        return Ok(new { success = true, count });
    }
}
