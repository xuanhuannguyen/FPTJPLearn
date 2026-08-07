using System.Text.Json;
using JPLearn.Core.Kanji.Entities;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Data.Seed;

internal static class KanjiJpd133SeedData
{
    private static readonly DateTime SeededAt = new(2026, 8, 7, 0, 0, 0, DateTimeKind.Utc);
    private static readonly Guid CourseId = Guid.Parse("77777777-1133-0000-0000-000000000001");

    public static async Task SeedAsync(AppDbContext db)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Data", "Imports", "kanji", "jpd133.json");
        if (!File.Exists(path)) return;

        var import = JsonSerializer.Deserialize<ImportFile>(await File.ReadAllTextAsync(path), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
        if (import?.Lessons == null) return;

        foreach (var seed in import.Lessons.OrderBy(item => item.LessonNumber))
        {
            var lessonId = LessonId(seed.LessonNumber);
            db.KanjiLessons.Add(new KanjiLesson
            {
                Id = lessonId,
                Level = seed.Level,
                LessonNumber = seed.LessonNumber,
                Title = seed.LessonTitle,
                Description = $"{seed.LessonTitle} - JPD133 Kanji N5.",
                AccessTier = seed.AccessTier,
                PackageCode = "kanji_jpd133",
                OrderIndex = seed.LessonNumber,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt,
            });

            var kanjiIds = new Dictionary<string, Guid>();
            for (var index = 0; index < seed.KanjiItems.Length; index++)
            {
                var item = seed.KanjiItems[index];
                var itemId = KanjiId(seed.LessonNumber, index + 1);
                kanjiIds[item.Character] = itemId;
                db.KanjiItems.Add(new KanjiItem
                {
                    Id = itemId,
                    LessonId = lessonId,
                    Level = seed.Level,
                    Character = item.Character,
                    HanViet = item.HanViet,
                    Meaning = item.Meaning,
                    StrokeCount = item.StrokeCount,
                    KunReading = item.KunReading,
                    OnReading = item.OnReading,
                    Mnemonic = item.Mnemonic,
                    StrokeSvg = item.StrokeSvg,
                    StrokeDataJson = item.StrokeDataJson,
                    ComponentMapJson = item.ComponentMapJson,
                    OrderIndex = item.OrderIndex,
                    CreatedAt = SeededAt,
                    UpdatedAt = SeededAt,
                });
            }

            for (var index = 0; index < seed.Vocabulary.Length; index++)
            {
                var item = seed.Vocabulary[index];
                var kanjiItemId = item.KanjiCharacters.Select(character => kanjiIds.GetValueOrDefault(character)).FirstOrDefault(id => id != Guid.Empty);
                db.KanjiVocabularyItems.Add(new KanjiVocabulary
                {
                    Id = VocabularyId(seed.LessonNumber, index + 1),
                    LessonId = lessonId,
                    KanjiItemId = kanjiItemId == Guid.Empty ? null : kanjiItemId,
                    Level = seed.Level,
                    Word = item.Word,
                    Reading = item.Reading,
                    Meaning = item.Meaning,
                    ExampleJapanese = item.ExampleJapanese,
                    ExampleReading = item.ExampleReading,
                    ExampleMeaning = item.ExampleMeaning,
                    OrderIndex = item.OrderIndex,
                    CreatedAt = SeededAt,
                    UpdatedAt = SeededAt,
                });
            }
        }
    }

    private static Guid LessonId(int lessonNumber) => Guid.Parse($"77777777-1133-0000-0000-{lessonNumber + 100:000000000000}");
    private static Guid KanjiId(int lessonNumber, int order) => Guid.Parse($"77777777-1133-0000-0000-{lessonNumber * 1000 + order:000000000000}");
    private static Guid VocabularyId(int lessonNumber, int order) => Guid.Parse($"77777777-1133-0000-0000-{lessonNumber * 1000 + 500 + order:000000000000}");

    private sealed class ImportFile
    {
        public ImportLesson[] Lessons { get; set; } = [];
    }

    private sealed class ImportLesson
    {
        public string Level { get; set; } = "N5";
        public int LessonNumber { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public string AccessTier { get; set; } = "premium";
        public ImportKanji[] KanjiItems { get; set; } = [];
        public ImportVocabulary[] Vocabulary { get; set; } = [];
    }

    private sealed class ImportKanji
    {
        public string Character { get; set; } = string.Empty;
        public string HanViet { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public int StrokeCount { get; set; }
        public string? KunReading { get; set; }
        public string? OnReading { get; set; }
        public string? Mnemonic { get; set; }
        public string? StrokeSvg { get; set; }
        public string? StrokeDataJson { get; set; }
        public string? ComponentMapJson { get; set; }
        public int OrderIndex { get; set; }
    }

    private sealed class ImportVocabulary
    {
        public string Word { get; set; } = string.Empty;
        public string Reading { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public string? ExampleJapanese { get; set; }
        public string? ExampleReading { get; set; }
        public string? ExampleMeaning { get; set; }
        public string[] KanjiCharacters { get; set; } = [];
        public int OrderIndex { get; set; }
    }
}
