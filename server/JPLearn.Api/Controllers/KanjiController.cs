using JPLearn.Core.Kanji;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

<<<<<<< HEAD
[Route("api/kanji")]
public class KanjiController : ApiControllerBase
{
    private readonly IKanjiService _kanjiService;
=======
[ApiController]
[Route("api/kanji")]
public class KanjiController : ControllerBase
{
    private readonly IKanjiService _kanjiService;
    private static readonly Guid DevUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf

    public KanjiController(IKanjiService kanjiService)
    {
        _kanjiService = kanjiService;
    }

    [HttpGet("levels")]
    public async Task<IActionResult> GetLevels()
    {
<<<<<<< HEAD
        var levels = await _kanjiService.GetLevelsAsync(CurrentUserId);
=======
        var levels = await _kanjiService.GetLevelsAsync(DevUserId);
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        return Ok(levels);
    }

    [HttpGet("{level}/lessons")]
    public async Task<IActionResult> GetLessonsByLevel(string level)
    {
<<<<<<< HEAD
        var lessons = await _kanjiService.GetLessonsByLevelAsync(CurrentUserId, level);
=======
        var lessons = await _kanjiService.GetLessonsByLevelAsync(DevUserId, level);
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        if (lessons == null)
        {
            return BadRequest(new { message = "Invalid kanji level" });
        }

        return Ok(new { lessons });
    }

    [HttpGet("lessons/{lessonId}")]
    public async Task<IActionResult> GetLesson(Guid lessonId)
    {
<<<<<<< HEAD
        var lesson = await _kanjiService.GetLessonDetailAsync(CurrentUserId, lessonId);
=======
        var lesson = await _kanjiService.GetLessonDetailAsync(DevUserId, lessonId);
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        if (lesson == null)
        {
            return NotFound(new { message = "Kanji lesson not found" });
        }

        return Ok(lesson);
    }

    [HttpGet("items/{kanjiItemId}")]
    public async Task<IActionResult> GetKanjiItem(Guid kanjiItemId)
    {
<<<<<<< HEAD
        var item = await _kanjiService.GetKanjiDetailAsync(CurrentUserId, kanjiItemId);
=======
        var item = await _kanjiService.GetKanjiDetailAsync(DevUserId, kanjiItemId);
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        if (item == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(item);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query = "")
    {
<<<<<<< HEAD
        var items = await _kanjiService.SearchAsync(CurrentUserId, query);
=======
        var items = await _kanjiService.SearchAsync(DevUserId, query);
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        return Ok(new { items });
    }

    [HttpPost("items/{kanjiItemId}/view")]
    public async Task<IActionResult> RecordView(Guid kanjiItemId)
    {
<<<<<<< HEAD
        var result = await _kanjiService.RecordViewAsync(CurrentUserId, kanjiItemId);
=======
        var result = await _kanjiService.RecordViewAsync(DevUserId, kanjiItemId);
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        if (result == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(result);
    }

    [HttpPost("items/{kanjiItemId}/memory")]
    public async Task<IActionResult> AddToMemory(Guid kanjiItemId)
    {
<<<<<<< HEAD
        var result = await _kanjiService.AddToMemoryAsync(CurrentUserId, kanjiItemId);
=======
        var result = await _kanjiService.AddToMemoryAsync(DevUserId, kanjiItemId);
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        if (result == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(result);
    }

    [HttpPost("items/{kanjiItemId}/writing-practice")]
    public async Task<IActionResult> RecordWritingPractice(Guid kanjiItemId)
    {
<<<<<<< HEAD
        var result = await _kanjiService.RecordWritingPracticeAsync(CurrentUserId, kanjiItemId);
=======
        var result = await _kanjiService.RecordWritingPracticeAsync(DevUserId, kanjiItemId);
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        if (result == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(result);
    }

    [HttpPost("items/{kanjiItemId}/flashcard-practice")]
    public async Task<IActionResult> RecordFlashcardPractice(Guid kanjiItemId)
    {
<<<<<<< HEAD
        var result = await _kanjiService.RecordFlashcardPracticeAsync(CurrentUserId, kanjiItemId);
=======
        var result = await _kanjiService.RecordFlashcardPracticeAsync(DevUserId, kanjiItemId);
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        if (result == null)
        {
            return NotFound(new { message = "Kanji item not found" });
        }

        return Ok(result);
    }
}
