using JPLearn.Core.Grammar;
using JPLearn.Core.Grammar.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/grammar")]
public class GrammarController : ControllerBase
{
    private readonly IGrammarService _grammarService;
    private readonly IGrammarReviewService _reviewService;
    private readonly IGrammarExerciseService _exerciseService;
    private static readonly Guid DevUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

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
        var levels = await _grammarService.GetLevelsAsync(DevUserId);
        return Ok(levels);
    }

    [HttpGet("{level}/lessons")]
    public async Task<IActionResult> GetLessonsByLevel(string level)
    {
        var lessons = await _grammarService.GetLessonsByLevelAsync(DevUserId, level);
        if (lessons == null)
        {
            return BadRequest(new { message = "Invalid grammar level" });
        }

        return Ok(new { lessons });
    }

    [HttpGet("lessons/{lessonId}")]
    public async Task<IActionResult> GetLesson(Guid lessonId)
    {
        var lesson = await _grammarService.GetLessonAsync(DevUserId, lessonId);
        if (lesson == null)
        {
            return NotFound(new { message = "Grammar lesson not found" });
        }

        var patterns = await _grammarService.GetLessonPatternsAsync(DevUserId, lessonId) ?? [];
        return Ok(new { lesson, patterns });
    }

    [HttpGet("patterns/{patternId}")]
    public async Task<IActionResult> GetPattern(Guid patternId)
    {
        var pattern = await _grammarService.GetPatternDetailAsync(DevUserId, patternId);
        if (pattern == null)
        {
            return NotFound(new { message = "Grammar pattern not found" });
        }

        return Ok(pattern);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query = "")
    {
        var patterns = await _grammarService.SearchPatternsAsync(DevUserId, query);
        return Ok(new { patterns });
    }

    [HttpPost("patterns/{patternId}/study")]
    public async Task<IActionResult> AddToStudy(Guid patternId)
    {
        var result = await _reviewService.AddToStudyAsync(DevUserId, patternId);
        if (result == null)
        {
            return NotFound(new { message = "Grammar pattern not found" });
        }

        return Ok(result);
    }

    [HttpDelete("patterns/{patternId}/study")]
    public async Task<IActionResult> RemoveFromStudy(Guid patternId)
    {
        var removed = await _reviewService.RemoveFromStudyAsync(DevUserId, patternId);
        if (!removed)
        {
            return NotFound(new { message = "Grammar study progress not found" });
        }

        return Ok(new { success = true });
    }

    [HttpGet("review/due")]
    public async Task<IActionResult> GetDueCards()
    {
        var result = await _reviewService.GetDueCardsAsync(DevUserId);
        return Ok(result);
    }

    [HttpPost("review/answer")]
    public async Task<IActionResult> SubmitAnswer([FromBody] SubmitGrammarAnswerDto dto)
    {
        var result = await _reviewService.SubmitAnswerAsync(DevUserId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Grammar study progress not found" });
        }

        return Ok(result);
    }

    [HttpGet("progress")]
    public async Task<IActionResult> GetProgressSummary()
    {
        var result = await _reviewService.GetProgressSummaryAsync(DevUserId);
        return Ok(result);
    }

    [HttpGet("patterns/{patternId}/exercises")]
    public async Task<IActionResult> GetPatternExercises(Guid patternId)
    {
        var exercises = await _exerciseService.GetExercisesByPatternAsync(patternId);
        if (exercises == null)
        {
            return NotFound(new { message = "Grammar pattern not found" });
        }

        return Ok(new { exercises });
    }

    [HttpPost("exercises/{exerciseId}/check")]
    public async Task<IActionResult> CheckExercise(Guid exerciseId, [FromBody] CheckGrammarExerciseDto dto)
    {
        var result = await _exerciseService.CheckAnswerAsync(DevUserId, exerciseId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Grammar exercise not found" });
        }

        return Ok(result);
    }

    [HttpGet("exercises/{exerciseId}/answer")]
    public async Task<IActionResult> RevealExerciseAnswer(Guid exerciseId)
    {
        var result = await _exerciseService.RevealAnswerAsync(exerciseId);
        if (result == null)
        {
            return NotFound(new { message = "Grammar exercise not found" });
        }

        return Ok(result);
    }

    [HttpPost("exercises/{exerciseId}/ai-evaluate")]
    public async Task<IActionResult> EvaluateExerciseWithAi(Guid exerciseId, [FromBody] AiEvaluateGrammarExerciseDto dto)
    {
        var result = await _exerciseService.EvaluateWithAiAsync(DevUserId, exerciseId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Grammar exercise not found" });
        }

        return Ok(result);
    }
}
