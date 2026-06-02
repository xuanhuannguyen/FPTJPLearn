using JPLearn.Core.Speaking;
using JPLearn.Core.Speaking.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;

namespace JPLearn.Infrastructure.Data.Seed;

public static partial class SpeakingSeedData
{
    private static readonly DateTime SeededAt = new(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc);

    private static readonly Guid Jpd113CourseId = Guid.Parse("88888888-1113-0000-0000-000000000001");
    private static readonly Guid Jpd123CourseId = Guid.Parse("88888888-1123-0000-0000-000000000001");

    public static async Task SeedAsync(AppDbContext db)
    {
        await EnsureCoursesAsync(db);
        await EnsureLessonsAsync(db);
        await EnsureSentencesAsync(db);
        await db.SaveChangesAsync();
    }

    private static async Task EnsureCoursesAsync(AppDbContext db)
    {
        var existingCourses = await db.SpeakingCourses.ToListAsync();
        var existingSet = existingCourses.Select(course => course.Code).ToHashSet();

        var courses = new[]
        {
            new SpeakingCourse
            {
                Id = Jpd113CourseId,
                Code = SpeakingCourseCodes.JPD113,
                Title = "JPD113",
                Description = "Bài đọc luyện nói nền tảng cho JPD113.",
                AccessTier = SpeakingAccessTiers.Premium,
                PackageCode = "speaking_jpd113",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new SpeakingCourse
            {
                Id = Jpd123CourseId,
                Code = SpeakingCourseCodes.JPD123,
                Title = "JPD123",
                Description = "Bài đọc luyện nói mở rộng cho JPD123.",
                AccessTier = SpeakingAccessTiers.Premium,
                PackageCode = "speaking_jpd123",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            }
        };

        foreach (var course in courses)
        {
            var existing = existingCourses.FirstOrDefault(item => item.Code == course.Code);
            if (existing == null)
            {
                db.SpeakingCourses.Add(course);
                continue;
            }

            existing.Title = course.Title;
            existing.Description = course.Description;
            existing.AccessTier = course.AccessTier;
            existing.PackageCode = course.PackageCode;
            existing.OrderIndex = course.OrderIndex;
            existing.IsActive = true;
            existing.UpdatedAt = SeededAt;
        }
    }

    private static async Task EnsureLessonsAsync(AppDbContext db)
    {
        var imports = LoadImportFiles();
        var lessons = imports
            .SelectMany(importFile => importFile.Lessons.Select(seed =>
                Lesson(
                    LessonId(importFile.CourseCode, seed.Id),
                    CourseId(importFile.CourseCode),
                    importFile.CourseCode,
                    seed.Id,
                    seed.Title,
                    $"{seed.Topic} - {seed.Subtitle}",
                    importFile.AccessTier,
                    importFile.PackageCode,
                    seed.Id,
                    seed.LessonType ?? SpeakingLessonTypes.Reading)))
            .ToList();
        var desiredKeys = lessons
            .Select(lesson => lesson.CourseCode + ":" + lesson.LessonNumber)
            .ToHashSet();

        var existingLessons = await db.SpeakingLessons.ToListAsync();
        foreach (var existing in existingLessons.Where(lesson => SpeakingCourseCodes.All.Contains(lesson.CourseCode)
            && !desiredKeys.Contains(lesson.CourseCode + ":" + lesson.LessonNumber)))
        {
            existing.IsActive = false;
            existing.UpdatedAt = SeededAt;
        }

        foreach (var lesson in lessons)
        {
            var existing = existingLessons.FirstOrDefault(item =>
                item.CourseCode == lesson.CourseCode && item.LessonNumber == lesson.LessonNumber);

            if (existing == null)
            {
                db.SpeakingLessons.Add(lesson);
                continue;
            }

            existing.Title = lesson.Title;
            existing.Description = lesson.Description;
            existing.AccessTier = lesson.AccessTier;
            existing.PackageCode = lesson.PackageCode;
            existing.LessonType = lesson.LessonType;
            existing.OrderIndex = lesson.OrderIndex;
            existing.IsActive = true;
            existing.UpdatedAt = SeededAt;
        }
    }

    private static async Task EnsureSentencesAsync(AppDbContext db)
    {
        var sentences = BuildSentences();
        var desiredKeys = sentences
            .Select(sentence => sentence.LessonId + ":" + sentence.SentenceNumber)
            .ToHashSet();
        var existingSentences = await db.SpeakingSentences.ToListAsync();
        foreach (var existing in existingSentences.Where(sentence => !desiredKeys.Contains(sentence.LessonId + ":" + sentence.SentenceNumber)))
        {
            existing.IsActive = false;
            existing.UpdatedAt = SeededAt;
        }

        foreach (var sentence in sentences)
        {
            var existing = existingSentences.FirstOrDefault(item =>
                item.LessonId == sentence.LessonId && item.SentenceNumber == sentence.SentenceNumber);

            if (existing == null)
            {
                db.SpeakingSentences.Add(sentence);
                continue;
            }

            existing.PlainText = sentence.PlainText;
            existing.Romaji = sentence.Romaji;
            existing.ContentHtml = sentence.ContentHtml;
            existing.MeaningVi = sentence.MeaningVi;
            existing.OrderIndex = sentence.OrderIndex;
            existing.IsActive = true;
            existing.UpdatedAt = SeededAt;
        }
    }

    private static List<SpeakingSentence> BuildSentences()
    {
        var sentences = new List<SpeakingSentence>();

        foreach (var importFile in LoadImportFiles())
        {
            foreach (var lesson in importFile.Lessons)
            {
                for (var index = 0; index < lesson.Sentences.Length; index++)
                {
                    var sentence = lesson.Sentences[index];
                    var sentenceNumber = index + 1;
                    sentences.Add(Sentence(
                        SentenceId(importFile.CourseCode, lesson.Id, sentenceNumber).ToString(),
                        LessonId(importFile.CourseCode, lesson.Id),
                        sentenceNumber,
                        ToPlainText(sentence.Jp),
                        sentence.Romaji,
                        ToRubyHtml(sentence.Jp),
                        sentence.Vi,
                        sentenceNumber));
                }
            }
        }

        return sentences;
    }

    private static SpeakingLesson Lesson(
        Guid id,
        Guid courseId,
        string courseCode,
        int lessonNumber,
        string title,
        string description,
        string accessTier,
        string? packageCode,
        int orderIndex,
        string lessonType)
    {
        return new SpeakingLesson
        {
            Id = id,
            CourseId = courseId,
            CourseCode = courseCode,
            LessonNumber = lessonNumber,
            Title = title,
            Description = description,
            AccessTier = accessTier,
            PackageCode = packageCode,
            LessonType = lessonType,
            OrderIndex = orderIndex,
            CreatedAt = SeededAt,
            UpdatedAt = SeededAt
        };
    }

    private static SpeakingSentence Sentence(
        string id,
        Guid lessonId,
        int sentenceNumber,
        string plainText,
        string romaji,
        string contentHtml,
        string meaningVi,
        int orderIndex)
    {
        return new SpeakingSentence
        {
            Id = Guid.Parse(id),
            LessonId = lessonId,
            SentenceNumber = sentenceNumber,
            PlainText = plainText,
            Romaji = romaji,
            ContentHtml = contentHtml,
            MeaningVi = meaningVi,
            OrderIndex = orderIndex,
            CreatedAt = SeededAt,
            UpdatedAt = SeededAt
        };
    }

    private static Guid CourseId(string courseCode)
    {
        return courseCode == SpeakingCourseCodes.JPD113 ? Jpd113CourseId : Jpd123CourseId;
    }

    private static Guid LessonId(string courseCode, int lessonNumber)
    {
        var courseSegment = courseCode == SpeakingCourseCodes.JPD113 ? "1113" : "1123";
        return Guid.Parse($"88888888-{courseSegment}-0000-0000-{lessonNumber + 100:000000000000}");
    }

    private static Guid SentenceId(string courseCode, int lessonNumber, int sentenceNumber)
    {
        var courseSegment = courseCode == SpeakingCourseCodes.JPD113 ? "1113" : "1123";
        return Guid.Parse($"88888888-{courseSegment}-0000-0000-{lessonNumber * 1000 + sentenceNumber:000000000000}");
    }

    private static string ToPlainText(string japanese)
    {
        return RubyPattern().Replace(japanese, match => match.Groups["text"].Value).Replace(" ", "");
    }

    private static string ToRubyHtml(string japanese)
    {
        var result = RubyPattern().Replace(japanese, match =>
        {
            var text = WebUtility.HtmlEncode(match.Groups["text"].Value);
            var reading = WebUtility.HtmlEncode(match.Groups["reading"].Value);
            return $"<ruby>{text}<rt>{reading}</rt></ruby>";
        });

        return WebUtility.HtmlEncode(result)
            .Replace("&lt;ruby&gt;", "<ruby>")
            .Replace("&lt;/ruby&gt;", "</ruby>")
            .Replace("&lt;rt&gt;", "<rt>")
            .Replace("&lt;/rt&gt;", "</rt>");
    }

    private static IReadOnlyList<SpeakingImportFile> LoadImportFiles()
    {
        return ImportDataFileLoader.LoadAll<SpeakingImportFile>("speaking")
            .Select(file => file with
            {
                CourseCode = SpeakingCourseCodes.Normalize(file.CourseCode),
                AccessTier = string.IsNullOrWhiteSpace(file.AccessTier)
                    ? SpeakingAccessTiers.Free
                    : file.AccessTier.Trim().ToLowerInvariant()
            })
            .Where(file => SpeakingCourseCodes.IsValid(file.CourseCode))
            .ToList();
    }

    private sealed record SpeakingImportFile(
        string CourseCode,
        string AccessTier,
        string? PackageCode,
        SpeakingLessonSeed[] Lessons);

    private sealed record SpeakingLessonSeed(
        int Id,
        string Topic,
        string Title,
        string Subtitle,
        string Summary,
        string? LessonType,
        SpeakingSentenceSeed[] Sentences);

    private sealed record SpeakingSentenceSeed(string Jp, string Vi, string Romaji);

    [GeneratedRegex(@"\[\[(?<text>[^|\]]+)\|(?<reading>[^\]]+)\]\]")]
    private static partial Regex RubyPattern();
}
