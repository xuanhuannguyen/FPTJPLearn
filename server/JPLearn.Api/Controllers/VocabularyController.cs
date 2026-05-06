using Microsoft.AspNetCore.Mvc;
using JPLearn.Core.Vocabulary;
using JPLearn.Core.Vocabulary.DTOs;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/vocabulary")]
public class VocabularyController : ControllerBase
{
    private readonly IVocabularyService _service;

    // Hardcoded userId cho development (chưa có Auth)
    // Sau khi implement Auth → lấy từ JWT claims
    private static readonly Guid DevUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public VocabularyController(IVocabularyService service)
    {
        _service = service;
    }

    /// <summary>
    /// Import vocabulary list from JSON
    /// </summary>
    [HttpPost("lists/import")]
    public async Task<IActionResult> Import([FromBody] ImportVocabularyDto dto)
    {
        var listId = await _service.ImportAsync(DevUserId, dto);
        return Ok(new { listId, wordCount = dto.Words.Count });
    }

    /// <summary>
    /// Get all vocabulary lists for current user
    /// </summary>
    [HttpGet("lists")]
    public async Task<IActionResult> GetAll()
    {
        var lists = await _service.GetListsAsync(DevUserId);
        return Ok(lists);
    }

    /// <summary>
    /// Get vocabulary list detail with items
    /// </summary>
    [HttpGet("lists/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var list = await _service.GetByIdAsync(DevUserId, id);
        if (list == null) return NotFound();
        return Ok(list);
    }

    /// <summary>
    /// Update vocabulary list name/description
    /// </summary>
    [HttpPut("lists/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateListDto dto)
    {
        var result = await _service.UpdateAsync(DevUserId, id, dto.Name, dto.Description);
        if (!result) return NotFound();
        return Ok(new { success = true });
    }

    /// <summary>
    /// Delete vocabulary list (cascade deletes items + progress)
    /// </summary>
    [HttpDelete("lists/{id}")]
    public async Task<IActionResult> DeleteList(Guid id)
    {
        var result = await _service.DeleteListAsync(DevUserId, id);
        if (!result) return NotFound();
        return Ok(new { success = true });
    }

    /// <summary>
    /// Delete a single vocabulary item
    /// </summary>
    [HttpDelete("items/{id}")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        var result = await _service.DeleteItemAsync(DevUserId, id);
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
            var itemId = await _service.AddItemAsync(DevUserId, listId, dto);
            return Ok(new { itemId });
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound(new { message = "List not found" });
        }
    }
}

public class UpdateListDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
