using JPLearn.Core.StaticVocabulary;
using JPLearn.Core.StaticVocabulary.Entities;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Data.Seed;

public static class VocabularySeedData
{
    private static readonly DateTime SeededAt = new(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc);

    private static readonly Guid Jpd113CourseId = Guid.Parse("66666666-1113-0000-0000-000000000001");
    private static readonly Guid Jpd123CourseId = Guid.Parse("66666666-1123-0000-0000-000000000001");

    public static async Task SeedAsync(AppDbContext db)
    {
        var imports = LoadImportFiles();
        ValidateImportFiles(imports);

        // Clear existing static vocabulary data as requested
        var existingItems = await db.StaticVocabularyItems.ToListAsync();
        db.StaticVocabularyItems.RemoveRange(existingItems);
        
        var existingLessons = await db.StaticVocabularyLessons.ToListAsync();
        db.StaticVocabularyLessons.RemoveRange(existingLessons);
        
        var existingCourses = await db.VocabularyCourses.ToListAsync();
        db.VocabularyCourses.RemoveRange(existingCourses);
        
        await db.SaveChangesAsync();

        await EnsureCoursesAsync(db, imports);
        await EnsureLessonsAsync(db, imports);
        await EnsureItemsAsync(db, imports);
        await db.SaveChangesAsync();
    }

    private static async Task EnsureCoursesAsync(AppDbContext db, IReadOnlyList<VocabularyImportFile> imports)
    {
        var existingCourses = await db.VocabularyCourses.ToListAsync();

        foreach (var importFile in imports)
        {
            var course = new VocabularyCourse
            {
                Id = CourseId(importFile.CourseCode),
                Code = importFile.CourseCode,
                Title = string.IsNullOrWhiteSpace(importFile.Title) ? importFile.CourseCode.ToUpperInvariant() : importFile.Title,
                Description = importFile.Description,
                OrderIndex = CourseOrder(importFile.CourseCode),
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            };

            var existing = existingCourses.FirstOrDefault(item => item.Code == course.Code);
            if (existing == null)
            {
                db.VocabularyCourses.Add(course);
                continue;
            }

            existing.Title = course.Title;
            existing.Description = course.Description;
            existing.OrderIndex = course.OrderIndex;
            existing.UpdatedAt = SeededAt;
        }
    }

    private static async Task EnsureLessonsAsync(AppDbContext db, IReadOnlyList<VocabularyImportFile> imports)
    {
        var existingLessons = await db.StaticVocabularyLessons.ToListAsync();

        foreach (var importFile in imports)
        {
            foreach (var seed in importFile.Lessons)
            {
                var lesson = new VocabularyLesson
                {
                    Id = LessonId(importFile.CourseCode, seed.Id),
                    CourseId = CourseId(importFile.CourseCode),
                    CourseCode = importFile.CourseCode,
                    LessonNumber = seed.Id,
                    Title = seed.Title,
                    Description = seed.Description,
                    AccessTier = NormalizeAccessTier(seed.AccessTier),
                    PackageCode = seed.PackageCode,
                    OrderIndex = seed.OrderIndex ?? seed.Id,
                    CreatedAt = SeededAt,
                    UpdatedAt = SeededAt
                };

                var existing = existingLessons.FirstOrDefault(item =>
                    item.CourseCode == lesson.CourseCode && item.LessonNumber == lesson.LessonNumber);

                if (existing == null)
                {
                    db.StaticVocabularyLessons.Add(lesson);
                    continue;
                }

                existing.CourseId = lesson.CourseId;
                existing.Title = lesson.Title;
                existing.Description = lesson.Description;
                existing.AccessTier = lesson.AccessTier;
                existing.PackageCode = lesson.PackageCode;
                existing.OrderIndex = lesson.OrderIndex;
                existing.UpdatedAt = SeededAt;
            }
        }
    }

    private static async Task EnsureItemsAsync(AppDbContext db, IReadOnlyList<VocabularyImportFile> imports)
    {
        var existingItems = await db.StaticVocabularyItems.ToListAsync();

        foreach (var importFile in imports)
        {
            foreach (var lesson in importFile.Lessons)
            {
                var lessonId = LessonId(importFile.CourseCode, lesson.Id);
                for (var index = 0; index < lesson.Items.Length; index++)
                {
                    var seed = lesson.Items[index];
                    var item = new StaticVocabularyItem
                    {
                        Id = ItemId(importFile.CourseCode, lesson.Id, seed, index),
                        LessonId = lessonId,
                        CourseCode = importFile.CourseCode,
                        Word = seed.Word,
                        Reading = seed.Reading,
                        WordType = seed.WordType,
                        Meaning = seed.Meaning,
                        ExampleJapanese = seed.ExampleJapanese,
                        ExampleReading = seed.ExampleReading,
                        ExampleMeaning = seed.ExampleMeaning,
                        Notes = seed.Notes,
                        AccessTierOverride = seed.AccessTierOverride,
                        PackageCodeOverride = seed.PackageCodeOverride,
                        OrderIndex = seed.OrderIndex ?? index + 1,
                        CreatedAt = SeededAt,
                        UpdatedAt = SeededAt
                    };

                    var existing = existingItems.FirstOrDefault(existingItem => existingItem.Id == item.Id)
                        ?? existingItems.FirstOrDefault(existingItem =>
                            existingItem.LessonId == item.LessonId && existingItem.Word == item.Word);

                    if (existing == null)
                    {
                        db.StaticVocabularyItems.Add(item);
                        continue;
                    }

                    existing.LessonId = item.LessonId;
                    existing.CourseCode = item.CourseCode;
                    existing.Word = item.Word;
                    existing.Reading = item.Reading;
                    existing.WordType = item.WordType;
                    existing.Meaning = item.Meaning;
                    existing.ExampleJapanese = item.ExampleJapanese;
                    existing.ExampleReading = item.ExampleReading;
                    existing.ExampleMeaning = item.ExampleMeaning;
                    existing.Notes = item.Notes;
                    existing.AccessTierOverride = item.AccessTierOverride;
                    existing.PackageCodeOverride = item.PackageCodeOverride;
                    existing.OrderIndex = item.OrderIndex;
                    existing.UpdatedAt = SeededAt;
                }
            }
        }
    }

    private static IReadOnlyList<VocabularyImportFile> LoadImportFiles()
    {
        return ImportDataFileLoader.LoadAll<VocabularyImportFile>("vocabulary")
            .Select(file => file with
            {
                CourseCode = VocabularyCourseCodes.Normalize(file.CourseCode)
            })
            .Where(file => VocabularyCourseCodes.IsValid(file.CourseCode))
            .OrderBy(file => CourseOrder(file.CourseCode))
            .ToList();
    }

    private static void ValidateImportFiles(IReadOnlyList<VocabularyImportFile> imports)
    {
        if (imports.Count == 0)
        {
            throw new InvalidOperationException("No vocabulary import files were found.");
        }

        var courseCodes = new HashSet<string>();
        foreach (var importFile in imports)
        {
            if (!courseCodes.Add(importFile.CourseCode))
            {
                throw new InvalidOperationException($"Duplicate vocabulary import course: {importFile.CourseCode}");
            }

            var lessonIds = new HashSet<int>();
            foreach (var lesson in importFile.Lessons)
            {
                if (!lessonIds.Add(lesson.Id))
                {
                    throw new InvalidOperationException($"Duplicate vocabulary lesson id {lesson.Id} in {importFile.CourseCode}.");
                }

                Require(lesson.Id > 0, $"Vocabulary lesson id must be positive in {importFile.CourseCode}.");
                Require(!string.IsNullOrWhiteSpace(lesson.Title), $"Vocabulary lesson {lesson.Id} is missing title.");
                Require(lesson.Items.Length > 0, $"Vocabulary lesson {lesson.Id} has no items.");

                var words = new HashSet<string>();
                foreach (var item in lesson.Items)
                {
                    Require(!string.IsNullOrWhiteSpace(item.Word), $"Vocabulary lesson {lesson.Id} has item without word.");
                    Require(!string.IsNullOrWhiteSpace(item.Reading), $"Vocabulary item {item.Word} is missing reading.");
                    Require(!string.IsNullOrWhiteSpace(item.Meaning), $"Vocabulary item {item.Word} is missing meaning.");
                    Require(words.Add(item.Word), $"Vocabulary lesson {lesson.Id} has duplicate word {item.Word}.");

                    if (!string.IsNullOrWhiteSpace(item.Id))
                    {
                        Require(Guid.TryParse(item.Id, out _), $"Vocabulary item {item.Word} has invalid id {item.Id}.");
                    }
                }
            }
        }
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static string NormalizeAccessTier(string? accessTier)
    {
        return string.IsNullOrWhiteSpace(accessTier) ? VocabularyAccessTiers.Free : accessTier.Trim().ToLowerInvariant();
    }

    private static int CourseOrder(string courseCode)
    {
        return courseCode == VocabularyCourseCodes.Jpd113 ? 1 : 2;
    }

    private static Guid CourseId(string courseCode)
    {
        return courseCode == VocabularyCourseCodes.Jpd113 ? Jpd113CourseId : Jpd123CourseId;
    }

    private static Guid LessonId(string courseCode, int lessonNumber)
    {
        var courseSegment = courseCode == VocabularyCourseCodes.Jpd113 ? "1113" : "1123";
        return Guid.Parse($"66666666-{courseSegment}-0000-0000-{lessonNumber + 100:000000000000}");
    }

    private static Guid ItemId(string courseCode, int lessonNumber, VocabularyItemSeed seed, int index)
    {
        if (!string.IsNullOrWhiteSpace(seed.Id))
        {
            return Guid.Parse(seed.Id);
        }

        var courseSegment = courseCode == VocabularyCourseCodes.Jpd113 ? "1113" : "1123";
        return Guid.Parse($"66666666-{courseSegment}-0000-0000-{lessonNumber * 1000 + index + 1:000000000000}");
    }

    private sealed record VocabularyImportFile(
        string CourseCode,
        string? Title,
        string? Description,
        VocabularyLessonSeed[] Lessons);

    private sealed record VocabularyLessonSeed(
        int Id,
        string Title,
        string? Description,
        string? AccessTier,
        string? PackageCode,
        int? OrderIndex,
        VocabularyItemSeed[] Items);

    private sealed record VocabularyItemSeed(
        string? Id,
        string Word,
        string Reading,
        string WordType,
        string Meaning,
        string? ExampleJapanese,
        string? ExampleReading,
        string? ExampleMeaning,
        string? Notes,
        string? AccessTierOverride,
        string? PackageCodeOverride,
        int? OrderIndex);
}
