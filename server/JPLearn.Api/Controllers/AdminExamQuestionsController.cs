using JPLearn.Core.Admin.Exam;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

[Route("api/admin/exam/questions")]
public class AdminExamQuestionsController : ApiControllerBase
{
    private readonly IAdminExamQuestionService _service;
    private readonly IConfiguration _configuration;

    public AdminExamQuestionsController(IAdminExamQuestionService service, IConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetQuestions(
        [FromQuery] string? courseCode = null,
        [FromQuery] string? topic = null,
        [FromQuery] bool includeInactive = false)
    {
        var adminCheck = EnsureAdminAccess();
        if (adminCheck != null) return adminCheck;

        var questions = await _service.GetQuestionsAsync(courseCode, topic, includeInactive);
        return Ok(new { questions });
    }

    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetQuestion(Guid questionId)
    {
        var adminCheck = EnsureAdminAccess();
        if (adminCheck != null) return adminCheck;

        var question = await _service.GetQuestionAsync(questionId);
        return question == null
            ? NotFound(new { message = "Exam question not found" })
            : Ok(question);
    }

    [HttpGet("import-template")]
    public IActionResult GetImportTemplate([FromQuery] string type = "standard")
    {
        var adminCheck = EnsureAdminAccess();
        if (adminCheck != null) return adminCheck;

        return Ok(_service.GetImportTemplate(type));
    }

    [HttpPost("import-json")]
    public async Task<IActionResult> ImportQuestions([FromBody] AdminExamImportFileInput input)
    {
        var adminCheck = EnsureAdminAccess();
        if (adminCheck != null) return adminCheck;

        return await HandleBadRequestAsync(async () => Ok(await _service.ImportQuestionsAsync(input)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuestion([FromBody] AdminExamQuestionInput input)
    {
        var adminCheck = EnsureAdminAccess();
        if (adminCheck != null) return adminCheck;

        return await HandleBadRequestAsync(async () =>
        {
            var question = await _service.CreateQuestionAsync(input);
            return CreatedAtAction(nameof(GetQuestion), new { questionId = question.Id }, question);
        });
    }

    [HttpPut("{questionId:guid}")]
    public async Task<IActionResult> UpdateQuestion(Guid questionId, [FromBody] AdminExamQuestionInput input)
    {
        var adminCheck = EnsureAdminAccess();
        if (adminCheck != null) return adminCheck;

        return await HandleBadRequestAsync(async () =>
        {
            var question = await _service.UpdateQuestionAsync(questionId, input);
            return question == null
                ? NotFound(new { message = "Exam question not found" })
                : Ok(question);
        });
    }

    [HttpDelete("{questionId:guid}")]
    public async Task<IActionResult> DeleteQuestion(Guid questionId)
    {
        var adminCheck = EnsureAdminAccess();
        if (adminCheck != null) return adminCheck;

        var deleted = await _service.DeleteQuestionAsync(questionId);
        return deleted
            ? NoContent()
            : NotFound(new { message = "Exam question not found" });
    }

    private async Task<IActionResult> HandleBadRequestAsync(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private IActionResult? EnsureAdminAccess()
    {
        var configuredKey = _configuration.GetValue<string>("Admin:ApiKey");
        if (string.IsNullOrWhiteSpace(configuredKey))
        {
            return null;
        }

        var providedKey = Request.Headers.TryGetValue("X-Admin-Key", out var headerValues)
            ? headerValues.FirstOrDefault()
            : null;

        return string.Equals(configuredKey, providedKey, StringComparison.Ordinal)
            ? null
            : Unauthorized(new { message = "Admin access is required." });
    }
}
