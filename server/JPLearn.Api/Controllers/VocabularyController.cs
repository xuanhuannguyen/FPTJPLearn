using Microsoft.AspNetCore.Mvc;
using JPLearn.Core.Vocabulary;
using JPLearn.Core.Vocabulary.DTOs;

namespace JPLearn.Api.Controllers;

[Route("api/active-vocabulary")]
public class ActiveVocabularyController : ApiControllerBase
{
    private readonly IVocabularyService _service;

    public ActiveVocabularyController(IVocabularyService service)
    {
        _service = service;
    }

    /// <summary>
    /// Import vocabulary list from JSON
    /// </summary>
    [HttpPost("lists/import")]
    [RequestSizeLimit(2 * 1024 * 1024)]
    public async Task<IActionResult> Import([FromBody] ImportVocabularyDto dto)
    {
        try
        {
            var listId = await _service.ImportAsync(CurrentUserId, dto);
            return Ok(new { listId, wordCount = dto.Words.Count });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get all vocabulary lists for current user
    /// </summary>
    [HttpGet("lists")]
    public async Task<IActionResult> GetAll()
    {
        var lists = await _service.GetListsAsync(CurrentUserId);
        return Ok(lists);
    }

    /// <summary>
    /// Get vocabulary list detail with items
    /// </summary>
    [HttpGet("lists/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var list = await _service.GetByIdAsync(CurrentUserId, id);
        if (list == null) return NotFound();
        return Ok(list);
    }

    /// <summary>
    /// Update vocabulary list name/description
    /// </summary>
    [HttpPut("lists/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateListDto dto)
    {
        var result = await _service.UpdateAsync(CurrentUserId, id, dto.Name, dto.Description);
        if (!result) return NotFound();
        return Ok(new { success = true });
    }

    /// <summary>
    /// Delete vocabulary list (cascade deletes items + progress)
    /// </summary>
    [HttpDelete("lists/{id}")]
    public async Task<IActionResult> DeleteList(Guid id)
    {
        var result = await _service.DeleteListAsync(CurrentUserId, id);
        if (!result) return NotFound();
        return Ok(new { success = true });
    }

    /// <summary>
    /// Delete a single vocabulary item
    /// </summary>
    [HttpDelete("items/{id}")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        var result = await _service.DeleteItemAsync(CurrentUserId, id);
        if (!result) return NotFound();
        return Ok(new { success = true });
    }

    /// <summary>
    /// Add a single vocabulary item to a list
    /// </summary>
    [HttpPost("lists/{listId}/items")]
    public async Task<IActionResult> AddItem(Guid listId, [FromBody] VocabularyWordDto dto)
    {
        try
        {
            var itemId = await _service.AddItemAsync(CurrentUserId, listId, dto);
            return Ok(new { itemId });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "List not found" });
        }
    }

    /// <summary>
    /// Get all vocabulary items for fast client-side searching
    /// </summary>
    [HttpGet("items/search-index")]
    public async Task<IActionResult> GetSearchIndex()
    {
        var items = await _service.GetSearchIndexAsync(CurrentUserId);
        return Ok(items);
    }

    /// <summary>
    /// Get current usage quota for vocabulary imports
    /// </summary>
    [HttpGet("quota")]
    public async Task<IActionResult> GetQuota()
    {
        var quota = await _service.GetQuotaAsync(CurrentUserId);
        return Ok(quota);
    }
}

public class UpdateListDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
