using JPLearn.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Api.Controllers;

/// <summary>API hỗ trợ quản trị — chạy một lần để cập nhật AccessTier.</summary>
[ApiController]
[Route("api/admin/seed")]
public class SeedController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    public SeedController(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    private bool IsAdmin()
    {
        var adminKey = Request.Headers["X-Admin-Key"].ToString();
        return adminKey == _configuration["AdminSettings:SecretKey"];
    }

    /// <summary>
    /// Bật Premium cho tất cả bài học từ bài 2 trở đi.
    /// Bài 1 (LessonNumber == 1 hoặc OrderIndex == 1) = Free.
    /// Gọi: POST /api/admin/seed/enable-premium
    /// </summary>
    [HttpPost("enable-premium")]
    public async Task<IActionResult> EnablePremium()
    {
        if (!IsAdmin()) return Unauthorized();

        int updated = 0;

        // === KANJI ===
        var kanjiLessons = await _db.KanjiLessons.ToListAsync();
        foreach (var lesson in kanjiLessons)
        {
            var newTier = lesson.LessonNumber <= 1 ? "free" : "premium";
            if (lesson.AccessTier != newTier)
            {
                lesson.AccessTier = newTier;
                updated++;
            }
        }

        // === VOCABULARY ===
        var vocabLessons = await _db.StaticVocabularyLessons.ToListAsync();
        foreach (var lesson in vocabLessons)
        {
            var newTier = lesson.LessonNumber <= 1 ? "free" : "premium";
            if (lesson.AccessTier != newTier)
            {
                lesson.AccessTier = newTier;
                updated++;
            }
        }

        // === GRAMMAR ===
        var grammarLessons = await _db.GrammarLessons.ToListAsync();
        foreach (var lesson in grammarLessons)
        {
            var newTier = lesson.LessonNumber <= 1 ? "free" : "premium";
            if (lesson.AccessTier != newTier)
            {
                lesson.AccessTier = newTier;
                updated++;
            }
        }

        // === SPEAKING ===
        var speakingLessons = await _db.SpeakingLessons.ToListAsync();
        foreach (var lesson in speakingLessons)
        {
            var newTier = lesson.OrderIndex <= 1 ? "free" : "premium";
            if (lesson.AccessTier != newTier)
            {
                lesson.AccessTier = newTier;
                updated++;
            }
        }

        // === EXAM ===
        var examCourses = await _db.ExamCourses.ToListAsync();
        foreach (var course in examCourses)
        {
            // Exam luôn premium
            if (course.AccessTier != "premium")
            {
                course.AccessTier = "premium";
                updated++;
            }
        }

        await _db.SaveChangesAsync();

        return Ok(new { message = $"Đã cập nhật {updated} bài học thành Premium.", updated });
    }
}
