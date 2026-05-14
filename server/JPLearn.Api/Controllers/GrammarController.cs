using JPLearn.Core.Grammar;
using JPLearn.Core.Grammar.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

[Route("api/grammar")]
public class GrammarController : ApiControllerBase
{
    private readonly IGrammarService _grammarService;
    private readonly IGrammarReviewService _reviewService;
    private readonly IGrammarExerciseService _exerciseService;

    public GrammarController(
        IGrammarService grammarService,
        IGrammarReviewService reviewService,
        IGrammarExerciseService exerciseService)
    {
        _grammarService = grammarService;
        _reviewService = reviewService;
        _exerciseService = exerciseService;
    }

    [HttpGet("levels")]
    public async Task<IActionResult> GetLevels()
    {
        var levels = await _grammarService.GetLevelsAsync(CurrentUserId);
        return Ok(levels);
    }

    [HttpGet("{level}/lessons")]
    public async Task<IActionResult> GetLessonsByLevel(string level, [FromQuery] string? course = null)
    {
        var lessons = await _grammarService.GetLessonsByLevelAsync(CurrentUserId, level, course);
        if (lessons == null)
        {
            return BadRequest(new { message = "Invalid grammar level" });
        }

        return Ok(new { lessons });
    }

    [HttpGet("lessons/{lessonId}")]
    public async Task<IActionResult> GetLesson(Guid lessonId)
    {
        var lesson = await _grammarService.GetLessonAsync(CurrentUserId, lessonId);
        if (lesson == null)
        {
            return NotFound(new { message = "Grammar lesson not found" });
        }

        var patterns = await _grammarService.GetLessonPatternsAsync(CurrentUserId, lessonId) ?? [];
        return Ok(new { lesson, patterns });
    }

    [HttpGet("patterns/{patternId}")]
    public async Task<IActionResult> GetPattern(Guid patternId)
    {
        var pattern = await _grammarService.GetPatternDetailAsync(CurrentUserId, patternId);
        if (pattern == null)
        {
            return NotFound(new { message = "Grammar pattern not found" });
        }

        return Ok(pattern);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query = "")
    {
        var patterns = await _grammarService.SearchPatternsAsync(CurrentUserId, query);
        return Ok(new { patterns });
    }

    [HttpPost("patterns/{patternId}/study")]
    public async Task<IActionResult> AddToStudy(Guid patternId)
    {
        var result = await _reviewService.AddToStudyAsync(CurrentUserId, patternId);
        if (result == null)
        {
            return NotFound(new { message = "Grammar pattern not found" });
        }

        return Ok(result);
    }

    [HttpDelete("patterns/{patternId}/study")]
    public async Task<IActionResult> RemoveFromStudy(Guid patternId)
    {
        var removed = await _reviewService.RemoveFromStudyAsync(CurrentUserId, patternId);
        if (!removed)
        {
            return NotFound(new { message = "Grammar study progress not found" });
        }

        return Ok(new { success = true });
    }

    [HttpGet("review/due")]
    public async Task<IActionResult> GetDueCards()
    {
        var result = await _reviewService.GetDueCardsAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpPost("review/answer")]
    public async Task<IActionResult> SubmitAnswer([FromBody] SubmitGrammarAnswerDto dto)
    {
        var result = await _reviewService.SubmitAnswerAsync(CurrentUserId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Grammar study progress not found" });
        }

        return Ok(result);
    }

    [HttpGet("progress")]
    public async Task<IActionResult> GetProgressSummary()
    {
        var result = await _reviewService.GetProgressSummaryAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpGet("patterns/{patternId}/exercises")]
    public async Task<IActionResult> GetPatternExercises(Guid patternId)
    {
        var exercises = await _exerciseService.GetExercisesByPatternAsync(CurrentUserId, patternId);
        if (exercises == null)
        {
            return NotFound(new { message = "Grammar pattern not found" });
        }

        return Ok(new { exercises });
    }

    [HttpPost("exercises/{exerciseId}/check")]
    public async Task<IActionResult> CheckExercise(Guid exerciseId, [FromBody] CheckGrammarExerciseDto dto)
    {
        var result = await _exerciseService.CheckAnswerAsync(CurrentUserId, exerciseId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Grammar exercise not found" });
        }

        return Ok(result);
    }

    [HttpGet("exercises/{exerciseId}/answer")]
    public async Task<IActionResult> RevealExerciseAnswer(Guid exerciseId)
    {
        var result = await _exerciseService.RevealAnswerAsync(CurrentUserId, exerciseId);
        if (result == null)
        {
            return NotFound(new { message = "Grammar exercise not found" });
        }

        return Ok(result);
    }

    [HttpPost("exercises/{exerciseId}/ai-evaluate")]
    public async Task<IActionResult> EvaluateExerciseWithAi(Guid exerciseId, [FromBody] AiEvaluateGrammarExerciseDto dto)
    {
        var result = await _exerciseService.EvaluateWithAiAsync(CurrentUserId, exerciseId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Grammar exercise not found" });
        }

        return Ok(result);
    }
}
