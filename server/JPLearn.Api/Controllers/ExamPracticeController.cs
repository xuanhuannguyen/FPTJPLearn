using JPLearn.Core.ExamPractice;
using JPLearn.Core.ExamPractice.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

[Route("api/exam")]
public class ExamPracticeController : ApiControllerBase
{
    private readonly IExamPracticeService _examService;

    public ExamPracticeController(IExamPracticeService examService)
    {
        _examService = examService;
    }

    [HttpGet("courses")]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _examService.GetCoursesAsync(CurrentUserId);
        return Ok(new { courses });
    }

    [HttpGet("topics")]
    public async Task<IActionResult> GetTopics([FromQuery] string? courseCode = null, [FromQuery] string? level = null)
    {
        var topics = await _examService.GetTopicsAsync(CurrentUserId, courseCode, level);
        return Ok(new { topics });
    }

    [HttpGet("questions")]
    public async Task<IActionResult> GetQuestions([FromQuery] string? courseCode = null, [FromQuery] string? topic = null, [FromQuery] string? level = null)
    {
        var questions = await _examService.GetQuestionsAsync(CurrentUserId, courseCode, topic, level);
        return Ok(new { questions });
    }

    [HttpGet("questions/{questionId}")]
    public async Task<IActionResult> GetQuestion(Guid questionId)
    {
        var question = await _examService.GetQuestionAsync(CurrentUserId, questionId);
        if (question == null)
        {
            return NotFound(new { message = "Exam question not found" });
        }

        return Ok(question);
    }

    [HttpPost("questions/{questionId}/answer")]
    public async Task<IActionResult> AnswerQuestion(Guid questionId, [FromBody] ExamAnswerQuestionDto dto)
    {
        var result = await _examService.AnswerQuestionAsync(CurrentUserId, questionId, dto);
        if (result == null)
        {
            return NotFound(new { message = "Exam question or option not found" });
        }

        return Ok(result);
    }

    [HttpPost("attempts/start")]
    public async Task<IActionResult> StartAttempt([FromBody] StartExamAttemptDto dto)
    {
        var attempt = await _examService.StartAttemptAsync(CurrentUserId, dto);
        if (attempt == null)
        {
            return BadRequest(new { message = "No exam questions are available for this filter" });
        }

        return Ok(attempt);
    }

    [HttpGet("attempts/{attemptId}")]
    public async Task<IActionResult> GetAttempt(Guid attemptId)
    {
        var attempt = await _examService.GetAttemptAsync(CurrentUserId, attemptId);
        if (attempt == null)
        {
            return NotFound(new { message = "Exam attempt not found" });
        }

        return Ok(attempt);
    }

    [HttpPost("attempts/{attemptId}/answers")]
    public async Task<IActionResult> SaveAttemptAnswer(Guid attemptId, [FromBody] SaveExamAttemptAnswerDto dto)
    {
        var answer = await _examService.SaveAttemptAnswerAsync(CurrentUserId, attemptId, dto);
        if (answer == null)
        {
            return BadRequest(new { message = "Cannot save answer for this exam attempt" });
        }

        return Ok(answer);
    }

    [HttpPost("attempts/{attemptId}/submit")]
    public async Task<IActionResult> SubmitAttempt(Guid attemptId)
    {
        var attempt = await _examService.SubmitAttemptAsync(CurrentUserId, attemptId);
        if (attempt == null)
        {
            return NotFound(new { message = "Exam attempt not found" });
        }

        return Ok(attempt);
    }

    [HttpGet("attempts/{attemptId}/review")]
    public async Task<IActionResult> GetAttemptReview(Guid attemptId)
    {
        var review = await _examService.GetAttemptReviewAsync(CurrentUserId, attemptId);
        if (review == null)
        {
            return NotFound(new { message = "Exam attempt not found" });
        }

        return Ok(review);
    }
}
