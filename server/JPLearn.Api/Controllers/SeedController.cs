using JPLearn.Infrastructure.Data;
using JPLearn.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;

namespace JPLearn.Api.Controllers;

/// <summary>API hỗ trợ quản trị — chạy một lần để cập nhật AccessTier.</summary>
[ApiController]
[Route("api/admin/seed")]
[EnableRateLimiting("admin-strict")]
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

    [HttpPost("enable-premium")]
    public async Task<IActionResult> EnablePremium()
    {
        if (!IsAdmin()) return Unauthorized();

        int updated = 0;

        // === KANJI ===
        var kanjiLessons = await _db.KanjiLessons.ToListAsync();
        foreach (var lesson in kanjiLessons)
        {
            // Lesson 1 (JPD113) and Lesson 4 (First lesson of JPD123) are free
            var isFree = lesson.LessonNumber == 1 || lesson.LessonNumber == 4;
            var newTier = isFree ? "free" : "premium";
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
            // First 3 sub-lessons (1-1, 1-2, 1-3 for JPD113 or 4-1, 4-2, 4-3 for JPD123) are free
            var isFree = lesson.LessonNumber <= 3;
            var newTier = isFree ? "free" : "premium";
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
            // First lesson of JPD113 (No. 1) and JPD123 (No. 4) are free
            var isFree = lesson.LessonNumber == 1 || lesson.LessonNumber == 4;
            var newTier = isFree ? "free" : "premium";
            if (lesson.AccessTier != newTier)
            {
                lesson.AccessTier = newTier;
                updated++;
            }
        }

        // === SPEAKING ===
        var speakingCourses = await _db.SpeakingCourses.ToListAsync();
        foreach (var course in speakingCourses)
        {
            if (course.AccessTier != "premium")
            {
                course.AccessTier = "premium";
                updated++;
            }
        }

        var speakingLessons = await _db.SpeakingLessons.ToListAsync();
        foreach (var lesson in speakingLessons)
        {
            // All speaking content is premium
            if (lesson.AccessTier != "premium")
            {
                lesson.AccessTier = "premium";
                updated++;
            }
        }

        // === EXAM ===
        var examCourses = await _db.ExamCourses.ToListAsync();
        foreach (var course in examCourses)
        {
            if (course.AccessTier != "premium")
            {
                course.AccessTier = "premium";
                updated++;
            }
        }

        await _db.SaveChangesAsync();

        return Ok(new { message = $"Đã cập nhật {updated} bản ghi." });
    }

    [HttpPost("full")]
    public async Task<IActionResult> FullSeed()
    {
        if (!IsAdmin()) return Unauthorized();

        try 
        {
            await KanjiSeedData.SeedAsync(_db);
            await VocabularySeedData.SeedAsync(_db);
            await GrammarSeedData.SeedAsync(_db);
            await SpeakingSeedData.SeedAsync(_db);
            await ExamPracticeSeedData.SeedAsync(_db);

            await EnablePremium();

            return Ok(new { message = "Đã Seed toàn bộ dữ liệu thành công!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("kanji")]
    public async Task<IActionResult> SeedKanji()
    {
        if (!IsAdmin()) return Unauthorized();
        try 
        {
            await KanjiSeedData.SeedAsync(_db);
            return Ok(new { message = "Đã Seed Kanji thành công!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                error = ex.Message, 
                inner = ex.InnerException?.Message,
                stack = ex.StackTrace 
            });
        }
    }

    [HttpPost("vocab")]
    public async Task<IActionResult> SeedVocab()
    {
        if (!IsAdmin()) return Unauthorized();
        try 
        {
            await VocabularySeedData.SeedAsync(_db);
            return Ok(new { message = "Đã Seed Vocabulary thành công!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                error = ex.Message, 
                inner = ex.InnerException?.Message,
                stack = ex.StackTrace 
            });
        }
    }

    [HttpPost("grammar")]
    public async Task<IActionResult> SeedGrammar()
    {
        if (!IsAdmin()) return Unauthorized();
        await GrammarSeedData.SeedAsync(_db);
        return Ok(new { message = "Đã Seed Grammar thành công!" });
    }

    [HttpPost("speaking")]
    public async Task<IActionResult> SeedSpeaking()
    {
        if (!IsAdmin()) return Unauthorized();
        await SpeakingSeedData.SeedAsync(_db);
        return Ok(new { message = "Đã Seed Speaking thành công!" });
    }

    [HttpPost("exam")]
    public async Task<IActionResult> SeedExam()
    {
        if (!IsAdmin()) return Unauthorized();
        await ExamPracticeSeedData.SeedAsync(_db);
        return Ok(new { message = "Đã Seed Exam thành công!" });
    }
}
