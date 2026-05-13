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
            // Lesson 1 is free for all. JPD123 Lesson 4 is also free.
            var isFree = lesson.LessonNumber <= 1 || (lesson.PackageCode == "kanji_jpd123" && lesson.LessonNumber == 4);
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
            // Lesson 1 is free for all. 
            // In JPD123, Lesson 4 is split into IDs 1, 2, 3 (labeled 4-1, 4-2, 4-3). All should be free.
            var isFree = lesson.LessonNumber <= 1 || (lesson.CourseCode == "jpd123" && lesson.LessonNumber <= 3);
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
            // Lesson 1 is free for all. JPD123 Lesson 4 is also free.
            var isFree = lesson.LessonNumber <= 1 || (lesson.CourseCode == "jpd123" && lesson.LessonNumber == 4);
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
            // Tất cả khóa speaking đều là premium ở dashboard
            if (course.AccessTier != "premium")
            {
                course.AccessTier = "premium";
                updated++;
            }
        }

        var speakingLessons = await _db.SpeakingLessons.ToListAsync();
        foreach (var lesson in speakingLessons)
        {
            // Lesson 1 của JPD113 vẫn là free để dùng thử nếu user click vào được hoặc admin mở
            var isFree = lesson.CourseCode == "jpd113" && lesson.OrderIndex <= 1;
            var newTier = isFree ? "free" : "premium";
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
            // Tất cả khóa luyện thi đều là premium
            if (course.AccessTier != "premium")
            {
                course.AccessTier = "premium";
                updated++;
            }
        }

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = $"Đã cập nhật {updated} bản ghi.",
            details = new
            {
                kanjiCount = kanjiLessons.Count,
                vocabCount = vocabLessons.Count,
                grammarCount = grammarLessons.Count,
                speakingCount = speakingLessons.Count,
                examCount = examCourses.Count
            }
        });
    }
    /// <summary>
    /// Chạy toàn bộ quá trình Seed dữ liệu (Kanji, Vocab, Grammar, Speaking, Exam).
    /// CẢNH BÁO: Lệnh này sẽ xóa dữ liệu cũ và nạp lại từ đầu!
    /// Gọi: POST /api/admin/seed/full
    /// </summary>
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

            // Sau khi seed xong, tự động chạy enable-premium luôn
            await EnablePremium();

            return Ok(new { message = "Đã Seed toàn bộ dữ liệu thành công!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stack = ex.StackTrace });
        }
    }
}
