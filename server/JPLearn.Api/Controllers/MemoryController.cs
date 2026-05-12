using JPLearn.Core.Memory;
using JPLearn.Core.Memory.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

[Route("api/memory")]
public class MemoryController : ApiControllerBase
{
    private readonly IMemoryService _memoryService;
    private readonly IMemoryGrammarService _grammarService;
    private readonly IMemoryKanjiService _kanjiService;
<<<<<<< HEAD
    private readonly IMemoryVocabularyService _vocabularyService;

    public MemoryController(
        IMemoryService memoryService,
        IMemoryGrammarService grammarService,
        IMemoryKanjiService kanjiService,
        IMemoryVocabularyService vocabularyService)
=======
    private static readonly Guid DevUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public MemoryController(IMemoryService memoryService, IMemoryGrammarService grammarService, IMemoryKanjiService kanjiService)
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
    {
        _memoryService = memoryService;
        _grammarService = grammarService;
        _kanjiService = kanjiService;
<<<<<<< HEAD
        _vocabularyService = vocabularyService;
=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _memoryService.GetSummaryAsync(CurrentUserId);
        return Ok(result);
    }

    // ─── Grammar ───────────────────────────────────────

    [HttpGet("grammar/summary")]
    public async Task<IActionResult> GetGrammarSummary()
    {
        var result = await _memoryService.GetGrammarSummaryAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpPost("grammar/from-pattern/{patternId}")]
    public async Task<IActionResult> AddGrammarFromPattern(Guid patternId)
    {
        var result = await _grammarService.AddFromPatternAsync(CurrentUserId, patternId);
        if (result == null)
        {
            return NotFound(new { message = "Grammar pattern not found" });
        }

        return Ok(result);
    }

    [HttpGet("grammar/from-pattern/{patternId}/status")]
    public async Task<IActionResult> GetGrammarPatternMemoryStatus(Guid patternId)
    {
        var result = await _grammarService.GetPatternStatusAsync(CurrentUserId, patternId);
        return Ok(result);
    }

    [HttpGet("grammar/cards")]
    public async Task<IActionResult> GetGrammarCards([FromQuery] string scope = MemoryScopes.Due)
    {
        var result = await _grammarService.GetCardsAsync(CurrentUserId, scope);
        return Ok(result);
    }

    [HttpPost("grammar/answer")]
    public async Task<IActionResult> SubmitGrammarAnswer([FromBody] SubmitMemoryAnswerDto dto)
    {
        var result = await _grammarService.SubmitAnswerAsync(CurrentUserId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Memory grammar item not found" });
        }

        return Ok(result);
    }

    [HttpDelete("grammar/{memoryItemId}")]
    public async Task<IActionResult> RemoveGrammarItem(Guid memoryItemId)
    {
        var removed = await _grammarService.RemoveAsync(CurrentUserId, memoryItemId);
        if (!removed)
        {
            return NotFound(new { message = "Memory grammar item not found" });
        }

        return Ok(new { success = true });
    }

    [HttpPost("grammar/reset")]
    public async Task<IActionResult> ResetGrammar([FromBody] ResetMemoryItemsDto dto)
    {
        var count = await _grammarService.ResetAsync(CurrentUserId, dto);
        return Ok(new { success = true, count });
    }

    // ─── Kanji ─────────────────────────────────────────

    [HttpPost("kanji/from-item/{kanjiItemId}")]
    public async Task<IActionResult> AddKanjiFromItem(Guid kanjiItemId)
    {
        var result = await _kanjiService.AddFromItemAsync(CurrentUserId, kanjiItemId);
        if (result == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(result);
    }

    [HttpGet("kanji/from-item/{kanjiItemId}/status")]
    public async Task<IActionResult> GetKanjiItemMemoryStatus(Guid kanjiItemId)
    {
        var result = await _kanjiService.GetItemStatusAsync(CurrentUserId, kanjiItemId);
        return Ok(result);
    }

    [HttpGet("kanji/cards")]
    public async Task<IActionResult> GetKanjiCards([FromQuery] string scope = MemoryScopes.Due)
    {
        var result = await _kanjiService.GetCardsAsync(CurrentUserId, scope);
        return Ok(result);
    }

    [HttpPost("kanji/answer")]
    public async Task<IActionResult> SubmitKanjiAnswer([FromBody] SubmitMemoryAnswerDto dto)
    {
        var result = await _kanjiService.SubmitAnswerAsync(CurrentUserId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Memory kanji item not found" });
        }

        return Ok(result);
    }

    [HttpDelete("kanji/{memoryItemId}")]
    public async Task<IActionResult> RemoveKanjiItem(Guid memoryItemId)
    {
        var removed = await _kanjiService.RemoveAsync(CurrentUserId, memoryItemId);
        if (!removed)
        {
            return NotFound(new { message = "Memory kanji item not found" });
        }

        return Ok(new { success = true });
    }

    [HttpPost("kanji/reset")]
    public async Task<IActionResult> ResetKanji([FromBody] ResetMemoryItemsDto dto)
    {
        var count = await _kanjiService.ResetAsync(CurrentUserId, dto);
        return Ok(new { success = true, count });
    }

    // ─── Vocabulary ───────────────────────────────────

    [HttpPost("vocabulary/from-item/{vocabularyItemId}")]
    public async Task<IActionResult> AddVocabularyFromItem(Guid vocabularyItemId)
    {
        var result = await _vocabularyService.AddFromItemAsync(CurrentUserId, vocabularyItemId);
        if (result == null)
        {
            return NotFound(new { message = "Vocabulary item not found" });
        }

        return Ok(result);
    }

    [HttpGet("vocabulary/from-item/{vocabularyItemId}/status")]
    public async Task<IActionResult> GetVocabularyItemMemoryStatus(Guid vocabularyItemId)
    {
        var result = await _vocabularyService.GetItemStatusAsync(CurrentUserId, vocabularyItemId);
        return Ok(result);
    }

    [HttpGet("vocabulary/cards")]
    public async Task<IActionResult> GetVocabularyCards([FromQuery] string scope = MemoryScopes.Due)
    {
        var result = await _vocabularyService.GetCardsAsync(CurrentUserId, scope);
        return Ok(result);
    }

    [HttpPost("vocabulary/answer")]
    public async Task<IActionResult> SubmitVocabularyAnswer([FromBody] SubmitMemoryAnswerDto dto)
    {
        var result = await _vocabularyService.SubmitAnswerAsync(CurrentUserId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Memory vocabulary item not found" });
        }

        return Ok(result);
    }

    [HttpDelete("vocabulary/{memoryItemId}")]
    public async Task<IActionResult> RemoveVocabularyItem(Guid memoryItemId)
    {
        var removed = await _vocabularyService.RemoveAsync(CurrentUserId, memoryItemId);
        if (!removed)
        {
            return NotFound(new { message = "Memory vocabulary item not found" });
        }

        return Ok(new { success = true });
    }

    [HttpPost("vocabulary/reset")]
    public async Task<IActionResult> ResetVocabulary([FromBody] ResetMemoryItemsDto dto)
    {
        var count = await _vocabularyService.ResetAsync(CurrentUserId, dto);
        return Ok(new { success = true, count });
    }

    // ─── Kanji ─────────────────────────────────────────

    [HttpPost("kanji/from-item/{kanjiItemId}")]
    public async Task<IActionResult> AddKanjiFromItem(Guid kanjiItemId)
    {
        var result = await _kanjiService.AddFromItemAsync(DevUserId, kanjiItemId);
        if (result == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(result);
    }

    [HttpGet("kanji/from-item/{kanjiItemId}/status")]
    public async Task<IActionResult> GetKanjiItemMemoryStatus(Guid kanjiItemId)
    {
        var result = await _kanjiService.GetItemStatusAsync(DevUserId, kanjiItemId);
        return Ok(result);
    }

    [HttpGet("kanji/cards")]
    public async Task<IActionResult> GetKanjiCards([FromQuery] string scope = MemoryScopes.Due)
    {
        var result = await _kanjiService.GetCardsAsync(DevUserId, scope);
        return Ok(result);
    }

    [HttpPost("kanji/answer")]
    public async Task<IActionResult> SubmitKanjiAnswer([FromBody] SubmitMemoryAnswerDto dto)
    {
        var result = await _kanjiService.SubmitAnswerAsync(DevUserId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Memory kanji item not found" });
        }

        return Ok(result);
    }

    [HttpDelete("kanji/{memoryItemId}")]
    public async Task<IActionResult> RemoveKanjiItem(Guid memoryItemId)
    {
        var removed = await _kanjiService.RemoveAsync(DevUserId, memoryItemId);
        if (!removed)
        {
            return NotFound(new { message = "Memory kanji item not found" });
        }

        return Ok(new { success = true });
    }

    [HttpPost("kanji/reset")]
    public async Task<IActionResult> ResetKanji([FromBody] ResetMemoryItemsDto dto)
    {
        var count = await _kanjiService.ResetAsync(DevUserId, dto);
        return Ok(new { success = true, count });
    }
}
