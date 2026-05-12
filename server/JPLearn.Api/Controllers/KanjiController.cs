using JPLearn.Core.Kanji;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

[Route("api/kanji")]
public class KanjiController : ApiControllerBase
{
    private readonly IKanjiService _kanjiService;

    public KanjiController(IKanjiService kanjiService)
    {
        _kanjiService = kanjiService;
    }

    [HttpGet("levels")]
    public async Task<IActionResult> GetLevels()
    {
        var levels = await _kanjiService.GetLevelsAsync(CurrentUserId);
        return Ok(levels);
    }

    [HttpGet("{level}/lessons")]
    public async Task<IActionResult> GetLessonsByLevel(string level)
    {
        var lessons = await _kanjiService.GetLessonsByLevelAsync(CurrentUserId, level);
        if (lessons == null)
        {
            return BadRequest(new { message = "Invalid kanji level" });
        }

        return Ok(new { lessons });
    }

    [HttpGet("lessons/{lessonId}")]
    public async Task<IActionResult> GetLesson(Guid lessonId)
    {
        var lesson = await _kanjiService.GetLessonDetailAsync(CurrentUserId, lessonId);
        if (lesson == null)
        {
            return NotFound(new { message = "Kanji lesson not found" });
        }

        return Ok(lesson);
    }

    [HttpGet("items/{kanjiItemId}")]
    public async Task<IActionResult> GetKanjiItem(Guid kanjiItemId)
    {
        var item = await _kanjiService.GetKanjiDetailAsync(CurrentUserId, kanjiItemId);
        if (item == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(item);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query = "")
    {
        var items = await _kanjiService.SearchAsync(CurrentUserId, query);
        return Ok(new { items });
    }

    [HttpPost("items/{kanjiItemId}/view")]
    public async Task<IActionResult> RecordView(Guid kanjiItemId)
    {
        var result = await _kanjiService.RecordViewAsync(CurrentUserId, kanjiItemId);
        if (result == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(result);
    }

    [HttpPost("items/{kanjiItemId}/memory")]
    public async Task<IActionResult> AddToMemory(Guid kanjiItemId)
    {
        var result = await _kanjiService.AddToMemoryAsync(CurrentUserId, kanjiItemId);
        if (result == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(result);
    }

    [HttpPost("items/{kanjiItemId}/writing-practice")]
    public async Task<IActionResult> RecordWritingPractice(Guid kanjiItemId)
    {
        var result = await _kanjiService.RecordWritingPracticeAsync(CurrentUserId, kanjiItemId);
        if (result == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(result);
    }

    [HttpPost("items/{kanjiItemId}/flashcard-practice")]
    public async Task<IActionResult> RecordFlashcardPractice(Guid kanjiItemId)
    {
        var result = await _kanjiService.RecordFlashcardPracticeAsync(CurrentUserId, kanjiItemId);
        if (result == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(result);
    }
}
