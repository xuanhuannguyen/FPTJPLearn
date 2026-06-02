using JPLearn.Core.Speaking;
using JPLearn.Core.Speaking.DTOs;
using JPLearn.Core.Speaking.Entities;
using JPLearn.Core.Payments;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Services;

public class SpeakingService : ISpeakingService
{
    private readonly AppDbContext _db;
    private readonly IPaymentAccessService _paymentAccess;

    public SpeakingService(AppDbContext db, IPaymentAccessService paymentAccess)
    {
        _db = db;
        _paymentAccess = paymentAccess;
    }

    public async Task<List<SpeakingCourseDto>> GetCoursesAsync(Guid userId)
    {
        var courses = await _db.SpeakingCourses
            .Include(course => course.Lessons.Where(lesson => lesson.IsActive))
            .ThenInclude(lesson => lesson.Sentences.Where(sentence => sentence.IsActive))
            .Where(course => course.IsActive)
            .OrderBy(course => course.OrderIndex)
            .ToListAsync();

        return courses.Select(course => new SpeakingCourseDto
        {
            Id = course.Id,
            Code = course.Code,
            Title = course.Title,
            Description = course.Description,
            AccessTier = ResolveAccessTier(course.AccessTier, course.Code),
            PackageCode = ResolvePackageCode(course.PackageCode, course.Code),
            IsLocked = _paymentAccess.IsContentLocked(
                userId,
                ResolveAccessTier(course.AccessTier, course.Code),
                ResolvePackageCode(course.PackageCode, course.Code)),
            LessonCount = course.Lessons.Count,
            SentenceCount = course.Lessons.SelectMany(lesson => lesson.Sentences).Count()
        }).ToList();
    }

    public async Task<List<SpeakingLessonDto>?> GetLessonsByCourseAsync(Guid userId, string courseCode)
    {
        if (!SpeakingCourseCodes.IsValid(courseCode))
        {
            return null;
        }

        var normalizedCourse = SpeakingCourseCodes.Normalize(courseCode);
        var lessons = await _db.SpeakingLessons
            .Include(lesson => lesson.Sentences.Where(sentence => sentence.IsActive))
            .Where(lesson => lesson.CourseCode == normalizedCourse && lesson.IsActive)
            .OrderBy(lesson => lesson.OrderIndex)
            .ThenBy(lesson => lesson.LessonNumber)
            .ToListAsync();

        return lessons.Select(lesson => MapLesson(userId, lesson)).ToList();
    }

    public async Task<SpeakingLessonDetailDto?> GetLessonDetailAsync(Guid userId, Guid lessonId)
    {
        var lesson = await _db.SpeakingLessons
            .Include(item => item.Sentences.Where(sentence => sentence.IsActive))
            .FirstOrDefaultAsync(item => item.Id == lessonId && item.IsActive);

        if (lesson == null)
        {
            return null;
        }

        if (_paymentAccess.IsContentLocked(
            userId,
            ResolveAccessTier(lesson.AccessTier, lesson.CourseCode),
            ResolvePackageCode(lesson.PackageCode, lesson.CourseCode)))
        {
            return null;
        }

        return new SpeakingLessonDetailDto
        {
            Lesson = MapLesson(userId, lesson),
            Sentences = lesson.Sentences
                .OrderBy(sentence => sentence.OrderIndex)
                .ThenBy(sentence => sentence.SentenceNumber)
                .Select(MapSentence)
                .ToList()
        };
    }

    private SpeakingLessonDto MapLesson(Guid userId, SpeakingLesson lesson)
    {
        return new SpeakingLessonDto
        {
            Id = lesson.Id,
            CourseCode = lesson.CourseCode,
            LessonNumber = lesson.LessonNumber,
            Title = lesson.Title,
            Description = lesson.Description,
            AccessTier = ResolveAccessTier(lesson.AccessTier, lesson.CourseCode),
            PackageCode = ResolvePackageCode(lesson.PackageCode, lesson.CourseCode),
            IsLocked = _paymentAccess.IsContentLocked(
                userId,
                ResolveAccessTier(lesson.AccessTier, lesson.CourseCode),
                ResolvePackageCode(lesson.PackageCode, lesson.CourseCode)),
            SentenceCount = lesson.Sentences.Count(sentence => sentence.IsActive),
            LessonType = lesson.LessonType
        };
    }

    private static string ResolveAccessTier(string? accessTier, string courseCode)
    {
        return SpeakingCourseCodes.IsValid(courseCode)
            ? SpeakingAccessTiers.Premium
            : string.IsNullOrWhiteSpace(accessTier)
                ? SpeakingAccessTiers.Free
                : accessTier.Trim().ToLowerInvariant();
    }

    private static string? ResolvePackageCode(string? packageCode, string courseCode)
    {
        if (!string.IsNullOrWhiteSpace(packageCode))
        {
            return packageCode.Trim().ToLowerInvariant();
        }

        var normalizedCourse = SpeakingCourseCodes.Normalize(courseCode);
        return SpeakingCourseCodes.IsValid(normalizedCourse)
            ? $"speaking_{normalizedCourse}"
            : null;
    }

    private static SpeakingSentenceDto MapSentence(SpeakingSentence sentence)
    {
        return new SpeakingSentenceDto
        {
            Id = sentence.Id,
            LessonId = sentence.LessonId,
            SentenceNumber = sentence.SentenceNumber,
            PlainText = sentence.PlainText,
            Romaji = sentence.Romaji,
            ContentHtml = sentence.ContentHtml,
            MeaningVi = sentence.MeaningVi,
            OrderIndex = sentence.OrderIndex
        };
    }

}
