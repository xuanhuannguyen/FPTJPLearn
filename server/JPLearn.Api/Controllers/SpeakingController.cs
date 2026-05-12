using JPLearn.Core.Speaking;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

[Route("api/speaking")]
public class SpeakingController : ApiControllerBase
{
    private readonly ISpeakingService _speakingService;

    public SpeakingController(ISpeakingService speakingService)
    {
        _speakingService = speakingService;
    }

    [HttpGet("courses")]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _speakingService.GetCoursesAsync(CurrentUserId);
        return Ok(new { courses });
    }

    [HttpGet("{courseCode}/lessons")]
    public async Task<IActionResult> GetLessonsByCourse(string courseCode)
    {
        var lessons = await _speakingService.GetLessonsByCourseAsync(CurrentUserId, courseCode);
        if (lessons == null)
        {
            return BadRequest(new { message = "Invalid speaking course" });
        }

        return Ok(new { lessons });
    }

    [HttpGet("lessons/{lessonId}")]
    public async Task<IActionResult> GetLesson(Guid lessonId)
    {
        var lesson = await _speakingService.GetLessonDetailAsync(CurrentUserId, lessonId);
        if (lesson == null)
        {
            return NotFound(new { message = "Speaking lesson not found" });
        }

        return Ok(lesson);
    }
}
