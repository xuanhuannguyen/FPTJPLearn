using JPLearn.Core.Memory;
using JPLearn.Core.StaticVocabulary;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

[Route("api/vocabulary")]
public class StaticVocabularyController : ApiControllerBase
{
    private readonly IStaticVocabularyService _vocabularyService;
    private readonly IMemoryVocabularyService _memoryVocabularyService;

    public StaticVocabularyController(
        IStaticVocabularyService vocabularyService,
        IMemoryVocabularyService memoryVocabularyService)
    {
        _vocabularyService = vocabularyService;
        _memoryVocabularyService = memoryVocabularyService;
    }

    [HttpGet("courses")]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _vocabularyService.GetCoursesAsync(CurrentUserId);
        return Ok(new { courses });
    }

    [HttpGet("{courseCode}/lessons")]
    public async Task<IActionResult> GetLessonsByCourse(string courseCode)
    {
        var lessons = await _vocabularyService.GetLessonsByCourseAsync(CurrentUserId, courseCode);
        if (lessons == null)
        {
            return BadRequest(new { message = "Invalid vocabulary course" });
        }

        return Ok(new { lessons });
    }

    [HttpGet("lessons/{lessonId}")]
    public async Task<IActionResult> GetLesson(Guid lessonId)
    {
        var lesson = await _vocabularyService.GetLessonDetailAsync(CurrentUserId, lessonId);
        if (lesson == null)
        {
            return NotFound(new { message = "Vocabulary lesson not found" });
        }

        return Ok(lesson);
    }

    [HttpGet("lessons/{lessonId}/practice")]
    public async Task<IActionResult> GetPracticeCards(Guid lessonId, [FromQuery] string mode = VocabularyPracticeModes.Flashcard)
    {
        var cards = await _vocabularyService.GetLessonPracticeCardsAsync(CurrentUserId, lessonId, mode);
        if (cards == null)
        {
            return NotFound(new { message = "Vocabulary lesson not found" });
        }

        return Ok(new { mode = VocabularyPracticeModes.Normalize(mode), cards });
    }

    [HttpGet("items/{itemId}")]
    public async Task<IActionResult> GetItem(Guid itemId)
    {
        var item = await _vocabularyService.GetItemDetailAsync(CurrentUserId, itemId);
        if (item == null)
        {
            return NotFound(new { message = "Vocabulary item not found" });
        }

        return Ok(item);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query = "", [FromQuery] string? courseCode = null)
    {
        var items = await _vocabularyService.SearchAsync(CurrentUserId, query, courseCode);
        return Ok(new { items });
    }

    [HttpPost("items/{itemId}/view")]
    public async Task<IActionResult> RecordView(Guid itemId)
    {
        var result = await _vocabularyService.RecordViewAsync(CurrentUserId, itemId);
        if (result == null)
        {
            return NotFound(new { message = "Vocabulary item not found" });
        }

        return Ok(result);
    }

    [HttpPost("items/{itemId}/flashcard-practice")]
    public async Task<IActionResult> RecordFlashcardPractice(Guid itemId)
    {
        var result = await _vocabularyService.RecordFlashcardPracticeAsync(CurrentUserId, itemId);
        if (result == null)
        {
            return NotFound(new { message = "Vocabulary item not found" });
        }

        return Ok(result);
    }

    [HttpPost("items/{itemId}/multiple-choice-practice")]
    public async Task<IActionResult> RecordMultipleChoicePractice(Guid itemId)
    {
        var result = await _vocabularyService.RecordMultipleChoicePracticeAsync(CurrentUserId, itemId);
        if (result == null)
        {
            return NotFound(new { message = "Vocabulary item not found" });
        }

        return Ok(result);
    }

    [HttpPost("items/{itemId}/typing-practice")]
    public async Task<IActionResult> RecordTypingPractice(Guid itemId)
    {
        var result = await _vocabularyService.RecordTypingPracticeAsync(CurrentUserId, itemId);
        if (result == null)
        {
            return NotFound(new { message = "Vocabulary item not found" });
        }

        return Ok(result);
    }

    [HttpPost("items/{itemId}/memory")]
    public async Task<IActionResult> AddToMemory(Guid itemId)
    {
        var result = await _memoryVocabularyService.AddFromItemAsync(CurrentUserId, itemId);
        if (result == null)
        {
            return NotFound(new { message = "Vocabulary item not found" });
        }

        return Ok(result);
    }
}
