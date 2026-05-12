using JPLearn.Core.Kanji.Entities;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Data.Seed;

public static class KanjiSeedData
{
    private static readonly DateTime SeededAt = new(2026, 5, 12, 0, 0, 0, DateTimeKind.Utc);

    public static async Task SeedAsync(AppDbContext db)
    {
        await ClearExistingDataAsync(db);
        await SeedLessonsAsync(db);
        await db.SaveChangesAsync();
    }

    private static async Task ClearExistingDataAsync(AppDbContext db)
    {
        // Remove progress first due to FKs
        db.UserKanjiProgress.RemoveRange(db.UserKanjiProgress);
        db.KanjiVocabularyItems.RemoveRange(db.KanjiVocabularyItems);
        db.KanjiItems.RemoveRange(db.KanjiItems);
        db.KanjiLessons.RemoveRange(db.KanjiLessons);
        await db.SaveChangesAsync();
    }

    private static async Task SeedLessonsAsync(AppDbContext db)
    {
        // Lesson 1: Giới thiệu bản thân và Trường học (JPD113)
        var lesson1Id = Guid.Parse("c051212d-48c6-5be0-a941-26505615cb23");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson1Id))
        {
            db.KanjiLessons.Add(new KanjiLesson
            {
                Id = lesson1Id,
                Level = "N5",
                LessonNumber = 1,
                Title = "Giới thiệu bản thân và Trường học",
                Description = "Giới thiệu bản thân và Trường học - JPD113 Kanji N5.",
                AccessTier = "free",
                PackageCode = "kanji_jpd113",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            });
        }

        var kanjiItems1 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("28bf8b0c-7514-5252-b44f-4cae34d664a9"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "私",
                HanViet = "TƯ",
                Meaning = "Tôi, riêng tư",
                StrokeCount = 7,
                KunReading = "わたし、わたくし",
                OnReading = "シ",
                Mnemonic = "Giữ bó lúa (禾) cho riêng mình (厶) tạo thành chữ tôi (私).",
                ComponentMapJson = @"[{""component"": ""禾"", ""name"": ""lúa"", ""meaning"": ""lúa"", ""position"": ""unknown""}, {""component"": ""厶"", ""name"": ""riêng tư"", ""meaning"": ""riêng tư"", ""position"": ""unknown""}]",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "日",
                HanViet = "NHẬT",
                Meaning = "Mặt trời, ngày",
                StrokeCount = 4,
                KunReading = "ひ、か",
                OnReading = "ニチ、ジツ",
                Mnemonic = "Hình vuông có tia sáng bên trong tượng trưng cho mặt trời (日).",
                ComponentMapJson = @"[]",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("5653afa6-d7a0-5c1f-b172-e8808f8fb825"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "本",
                HanViet = "BẢN",
                Meaning = "Gốc, sách, căn bản",
                StrokeCount = 5,
                KunReading = "もと",
                OnReading = "ホン",
                Mnemonic = "Nét ngang (一) chỉ vào gốc của cây (木) tạo thành chữ gốc, căn bản (本).",
                ComponentMapJson = @"[{""component"": ""木"", ""name"": ""cây"", ""meaning"": ""cây"", ""position"": ""unknown""}, {""component"": ""一"", ""name"": ""gốc cây"", ""meaning"": ""gốc cây"", ""position"": ""unknown""}]",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("beac45f0-e750-5dfd-8226-a870eb4cbd24"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "大",
                HanViet = "ĐẠI",
                Meaning = "To, lớn",
                StrokeCount = 3,
                KunReading = "おおきい",
                OnReading = "ダイ、タイ",
                Mnemonic = "Người dang rộng tay chân thể hiện sự to lớn (大).",
                ComponentMapJson = @"[]",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("8e5758ae-b26b-57b7-a0d9-bdd56ffc6104"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "学",
                HanViet = "HỌC",
                Meaning = "Học tập",
                StrokeCount = 8,
                KunReading = "まなぶ",
                OnReading = "ガク",
                Mnemonic = "Đứa trẻ (子) ngồi học dưới mái che (冖) tạo thành học tập (学).",
                ComponentMapJson = @"[{""component"": ""⺍"", ""name"": ""mái nhỏ / ánh sáng"", ""meaning"": ""mái nhỏ / ánh sáng"", ""position"": ""unknown""}, {""component"": ""冖"", ""name"": ""mái che"", ""meaning"": ""mái che"", ""position"": ""unknown""}, {""component"": ""子"", ""name"": ""trẻ em"", ""meaning"": ""trẻ em"", ""position"": ""unknown""}]",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("64d836b9-4fd7-50ac-81f8-6da9defab802"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "語",
                HanViet = "NGỮ",
                Meaning = "Ngôn ngữ, tiếng nói",
                StrokeCount = 14,
                KunReading = "かたる",
                OnReading = "ゴ",
                Mnemonic = "Dùng lời nói (言) của bản thân (吾) để tạo thành ngôn ngữ (語).",
                ComponentMapJson = @"[{""component"": ""言"", ""name"": ""lời nói"", ""meaning"": ""lời nói"", ""position"": ""unknown""}, {""component"": ""吾"", ""name"": ""ta / bản thân"", ""meaning"": ""ta / bản thân"", ""position"": ""unknown""}]",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("e095aca4-eeea-553a-bf95-716527c4e6b0"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "校",
                HanViet = "HIỆU",
                Meaning = "Trường học",
                StrokeCount = 10,
                KunReading = "",
                OnReading = "コウ",
                Mnemonic = "Nơi mọi người gặp gỡ (交) dưới sân trường có cây (木) tạo thành trường học (校).",
                ComponentMapJson = @"[{""component"": ""木"", ""name"": ""cây"", ""meaning"": ""cây"", ""position"": ""unknown""}, {""component"": ""交"", ""name"": ""giao nhau"", ""meaning"": ""giao nhau"", ""position"": ""unknown""}]",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("aabacd7a-7cf8-5318-a09f-7db6bd98ca00"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "生",
                HanViet = "SINH",
                Meaning = "Sinh sống, sinh ra",
                StrokeCount = 5,
                KunReading = "いきる、うまれる、なま",
                OnReading = "セイ、ショウ",
                Mnemonic = "Mầm cây mọc lên từ đất tạo thành sự sống (生).",
                ComponentMapJson = @"[{""component"": ""土"", ""name"": ""đất"", ""meaning"": ""đất"", ""position"": ""unknown""}, {""component"": ""ノ"", ""name"": ""mầm mọc lên"", ""meaning"": ""mầm mọc lên"", ""position"": ""unknown""}]",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("22d23b80-2c89-5e0b-8b0d-0789864c4909"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "人",
                HanViet = "NHÂN",
                Meaning = "Người",
                StrokeCount = 2,
                KunReading = "ひと",
                OnReading = "ジン、ニン",
                Mnemonic = "Hai nét giống dáng người đang bước đi tạo thành chữ người (人).",
                ComponentMapJson = @"[]",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("b72ae3b6-8a3a-58a9-a43e-e73f8d0f3c3d"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "才",
                HanViet = "TÀI",
                Meaning = "Tuổi, tài năng",
                StrokeCount = 3,
                KunReading = "",
                OnReading = "サイ",
                Mnemonic = "Mỗi năm thêm một dấu mốc trưởng thành tạo thành tuổi tác, tài năng (才).",
                ComponentMapJson = @"[{""component"": ""ノ"", ""name"": ""nét phẩy"", ""meaning"": ""nét phẩy"", ""position"": ""unknown""}, {""component"": ""十"", ""name"": ""mười"", ""meaning"": ""mười"", ""position"": ""unknown""}]",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems1)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems1 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("cdaeb163-9a25-5b9e-86f2-ddbda4570e56"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("28bf8b0c-7514-5252-b44f-4cae34d664a9"),
                Level = "N5",
                Word = "私",
                Reading = "わたし",
                Meaning = "tôi",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c35c5d80-1543-5d1e-9282-bc7ff0188c52"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("22d23b80-2c89-5e0b-8b0d-0789864c4909"),
                Level = "N5",
                Word = "人",
                Reading = "ひと",
                Meaning = "Con người",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("00aba2de-0b52-55ce-9c4c-ef99ddd712c7"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("22d23b80-2c89-5e0b-8b0d-0789864c4909"),
                Level = "N5",
                Word = "あの人",
                Reading = "あのひと",
                Meaning = "Người kia",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("7a0f53b8-22ef-5e68-906f-b98442cffc6b"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("b72ae3b6-8a3a-58a9-a43e-e73f8d0f3c3d"),
                Level = "N5",
                Word = "2才",
                Reading = "にさい",
                Meaning = "2 tuổi",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("09d05fa8-94bf-5901-b162-d24c937614c7"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("b72ae3b6-8a3a-58a9-a43e-e73f8d0f3c3d"),
                Level = "N5",
                Word = "8才",
                Reading = "はっさい",
                Meaning = "8 tuổi",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("909a8d9b-54bc-5309-b39b-4e83b0f6ce67"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("b72ae3b6-8a3a-58a9-a43e-e73f8d0f3c3d"),
                Level = "N5",
                Word = "何才",
                Reading = "なんさい",
                Meaning = "Mấy tuổi",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2e01b767-d065-5f41-abe3-a26b9d519e93"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("beac45f0-e750-5dfd-8226-a870eb4cbd24"),
                Level = "N5",
                Word = "大学",
                Reading = "だいがく",
                Meaning = "Đại học",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6f5eb9ba-6995-5d48-8d59-c7e8507c390b"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("8e5758ae-b26b-57b7-a0d9-bdd56ffc6104"),
                Level = "N5",
                Word = "学生",
                Reading = "がくせい",
                Meaning = "Học sinh",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("786a48f5-5070-5272-bd32-69ba3eade111"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("aabacd7a-7cf8-5318-a09f-7db6bd98ca00"),
                Level = "N5",
                Word = "先生",
                Reading = "せんせい",
                Meaning = "Thầy/cô giáo",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("870163db-a872-5eb9-8c5e-0d59e025c9d1"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("8e5758ae-b26b-57b7-a0d9-bdd56ffc6104"),
                Level = "N5",
                Word = "学校",
                Reading = "がっこう",
                Meaning = "Trường học",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("0ac16f00-14f0-560a-8f6a-2dc2b440b9d9"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "10日",
                Reading = "とおか",
                Meaning = "Mùng 10",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f3bd669a-18ae-5b47-99de-5858f02522d3"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "日曜日",
                Reading = "にちようび",
                Meaning = "Chủ nhật",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("e382eca1-bbb2-5cf7-b073-b2ac3afcc42d"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "日",
                Reading = "ひ",
                Meaning = "Ngày",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("78718cc7-f237-5d4e-b592-9767dc47e421"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "休日",
                Reading = "きゅうじつ",
                Meaning = "Ngày nghỉ",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b7dacb17-7678-528f-9456-63108dbbbb12"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("5653afa6-d7a0-5c1f-b172-e8808f8fb825"),
                Level = "N5",
                Word = "本",
                Reading = "ほん",
                Meaning = "sách",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c015e707-2305-52da-a71a-cf6213e495be"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "日本",
                Reading = "にほん",
                Meaning = "Nhật Bản",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("df1a9cd5-8c1e-5581-85a2-9baefd99e9c5"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "日本人",
                Reading = "にほんじん",
                Meaning = "Người Nhật Bản",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4653c7f0-4500-54b8-a727-8b03738c36a4"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "日本語",
                Reading = "にほんご",
                Meaning = "Tiếng Nhật",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("92426bbc-31c9-562b-8410-9772baa1bbd6"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("64d836b9-4fd7-50ac-81f8-6da9defab802"),
                Level = "N5",
                Word = "ベトナム語",
                Reading = "ベトナムご",
                Meaning = "Tiếng Việt",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b84bd322-84dc-5a48-86a2-e2aecbaf9ecf"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("22d23b80-2c89-5e0b-8b0d-0789864c4909"),
                Level = "N5",
                Word = "～人",
                Reading = "〜じん",
                Meaning = "người (nước ~)",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems1)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 2: Số đếm và Đơn vị tiền tệ (JPD113)
        var lesson2Id = Guid.Parse("5ef50301-f4f5-5cc9-a8a0-c49fe2168bd8");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson2Id))
        {
            db.KanjiLessons.Add(new KanjiLesson
            {
                Id = lesson2Id,
                Level = "N5",
                LessonNumber = 2,
                Title = "Số đếm và Đơn vị tiền tệ",
                Description = "Số đếm và Đơn vị tiền tệ - JPD113 Kanji N5.",
                AccessTier = "free",
                PackageCode = "kanji_jpd113",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            });
        }

        var kanjiItems2 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "一",
                HanViet = "NHẤT",
                Meaning = "Số một",
                StrokeCount = 1,
                KunReading = "ひと.つ",
                OnReading = "イチ、イツ",
                Mnemonic = "Chỉ một nét ngang đơn giản, biểu thị số một (一).",
                ComponentMapJson = @"[]",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "二",
                HanViet = "NHỊ",
                Meaning = "Số hai",
                StrokeCount = 2,
                KunReading = "ふた.つ",
                OnReading = "ニ",
                Mnemonic = "Hai nét ngang song song xếp chồng lên nhau, biểu thị số hai (二).",
                ComponentMapJson = @"[]",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "三",
                HanViet = "TAM",
                Meaning = "Số ba",
                StrokeCount = 3,
                KunReading = "みっ.つ",
                OnReading = "サン",
                Mnemonic = "Ba nét ngang kích thước khác nhau xếp chồng lên nhau, biểu thị số ba (三).",
                ComponentMapJson = @"[]",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("5281ba82-3a72-5b09-960c-017fcda7e538"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "四",
                HanViet = "TỨ",
                Meaning = "Số bốn",
                StrokeCount = 5,
                KunReading = "よん、よっ.つ",
                OnReading = "シ",
                Mnemonic = "Trong một khu vực kín (囗), con số bốn (四) như có hai cái chân (儿) đang tung tăng nhảy múa.",
                ComponentMapJson = @"[]",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("3157f1ba-0189-5b6e-9ffd-4626d4bac967"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "五",
                HanViet = "NGŨ",
                Meaning = "Số năm",
                StrokeCount = 4,
                KunReading = "いつ.つ",
                OnReading = "ゴ",
                Mnemonic = "Trông giống như một cái nắp hộp bị méo mó, biến dạng tạo thành hình số năm (五).",
                ComponentMapJson = @"[]",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "六",
                HanViet = "LỤC",
                Meaning = "Số sáu",
                StrokeCount = 4,
                KunReading = "むっ.つ",
                OnReading = "ロク",
                Mnemonic = "Một cái đầu (亠) to nhưng chỉ mọc lơ thơ có tám (ハ) trừ hai bằng sáu (六) sợi tóc.",
                ComponentMapJson = @"[]",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("4ac63e13-411a-5b19-a116-c6778cabcc8b"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "七",
                HanViet = "THẤT",
                Meaning = "Số bảy",
                StrokeCount = 2,
                KunReading = "なな、なな.つ",
                OnReading = "シチ",
                Mnemonic = "Chữ này nhìn y hệt con số 7 nhưng bị lật ngược lại.",
                ComponentMapJson = @"[]",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "八",
                HanViet = "BÁT",
                Meaning = "Số tám",
                StrokeCount = 2,
                KunReading = "やって.つ",
                OnReading = "ハチ",
                Mnemonic = "Hai nét phẩy xòe rộng tách chẻ ra hai bên, giống như đang chia đều chiếc bánh ra thành tám (八) phần.",
                ComponentMapJson = @"[]",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("baf49199-9e6c-5f39-b696-c1ae140ebb7b"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "九",
                HanViet = "CỬU",
                Meaning = "Số chín",
                StrokeCount = 2,
                KunReading = "ここの.つ",
                OnReading = "キュウ、ク",
                Mnemonic = "Một người đang hít đất và gập cong người lại, tạo thành hình số chín (九).",
                ComponentMapJson = @"[]",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("ddb5d53b-5213-599d-b9be-3536802be0e7"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "十",
                HanViet = "THẬP",
                Meaning = "Số mười",
                StrokeCount = 2,
                KunReading = "とお",
                OnReading = "ジュウ",
                Mnemonic = "Hai nét gạch chéo tạo thành hình chữ thập, biểu thị sự trọn vẹn, đầy đủ của số mười (十).",
                ComponentMapJson = @"[]",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("5b211445-142d-507e-9963-47430a88e47b"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "百",
                HanViet = "BÁCH",
                Meaning = "Một trăm",
                StrokeCount = 6,
                KunReading = "",
                OnReading = "ヒャク",
                Mnemonic = "Đạt được một (一) tờ giấy trắng (白) tinh khôi không tì vết là biểu tượng của một trăm (百) điểm tuyệt đối.",
                ComponentMapJson = @"[]",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("7e864a3b-ca27-5a05-abd8-a6c16639a2b5"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "千",
                HanViet = "THIÊN",
                Meaning = "Một nghìn",
                StrokeCount = 3,
                KunReading = "ち",
                OnReading = "セン",
                Mnemonic = "Chỉ cần thêm một nét phẩy (ノ) vào trên đầu số mười (十), giá trị lập tức tăng lên gấp trăm lần thành một nghìn (千).",
                ComponentMapJson = @"[]",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("9270b873-9f8c-55cc-abfb-d278201b7001"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "万",
                HanViet = "VẠN",
                Meaning = "Mười nghìn",
                StrokeCount = 3,
                KunReading = "",
                OnReading = "マン、バン",
                Mnemonic = "Một (一) vòng tay dang rộng bao bọc lấy hàng vạn (万) sinh linh bé nhỏ.",
                ComponentMapJson = @"[]",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("c0154f99-b8b8-557c-bb1f-0497014dadfb"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "円",
                HanViet = "VIÊN",
                Meaning = "Hình tròn, đồng Yên",
                StrokeCount = 4,
                KunReading = "まる.い",
                OnReading = "エン",
                Mnemonic = "Nhìn qua cái khung cửa sổ (冂), thấy bên trong có những thanh vàng xếp ngang dọc để đổi thành đồng Yên (円) Nhật.",
                ComponentMapJson = @"[]",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems2)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems2 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("c02f132b-5453-55ed-9663-f1c879ee6d19"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一日",
                Reading = "ついたち",
                Meaning = "Ngày mồng 1",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1d427a64-eb02-559d-b8c5-b3c0cab0f31a"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一月",
                Reading = "いちがつ",
                Meaning = "tháng 1",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9a74e688-ab52-5520-b253-104ec75e4c53"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一人",
                Reading = "ひとり",
                Meaning = "1 người",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6975a269-95cb-584d-b5af-896a1a6d1b54"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一つ",
                Reading = "ひとつ",
                Meaning = "1 chiếc",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("76a13148-f6c6-5735-a295-100fa3e69955"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一",
                Reading = "いち",
                Meaning = "Số 1",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b5e67006-ce50-5073-9a5f-5267eab523cb"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                Level = "N5",
                Word = "二日",
                Reading = "ふつか",
                Meaning = "Ngày mồng 2",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1a97650f-b776-54a3-926a-544e202cb128"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                Level = "N5",
                Word = "二月",
                Reading = "にがつ",
                Meaning = "Tháng 2",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("7a05ad0c-3fa0-5b02-afb1-667835bf6be7"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                Level = "N5",
                Word = "二つ",
                Reading = "ふたつ",
                Meaning = "2 cái",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("99f20241-3623-52d0-b16b-69fde006c7da"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                Level = "N5",
                Word = "二",
                Reading = "に",
                Meaning = "Số 2",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("eda0dc35-b4c5-55dc-9bb3-bc108947a2bf"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三日",
                Reading = "みっか",
                Meaning = "Ngày mồng 3",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("95b90564-778b-51bf-8fbb-35a49afb2247"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三月",
                Reading = "さんがつ",
                Meaning = "tháng 3",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1eb74bfd-3f6e-58f4-a144-be02ab2a6448"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三人",
                Reading = "さんにん",
                Meaning = "3 người",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d7e1f0e2-28d4-51e8-a601-934bc5cf3b68"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三",
                Reading = "さん",
                Meaning = "Số 3",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4bde79ce-c4d9-5093-89c3-4065a03fdf40"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("5281ba82-3a72-5b09-960c-017fcda7e538"),
                Level = "N5",
                Word = "四日",
                Reading = "よっか",
                Meaning = "Ngày mồng 4",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8ad8377e-b5e2-5679-9c46-a85d5ae1cf47"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("5281ba82-3a72-5b09-960c-017fcda7e538"),
                Level = "N5",
                Word = "四月",
                Reading = "しがつ",
                Meaning = "tháng 4",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("80e86aae-ab69-5a58-800e-0330608dcdac"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("5281ba82-3a72-5b09-960c-017fcda7e538"),
                Level = "N5",
                Word = "四人",
                Reading = "よにん",
                Meaning = "4 người",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("a4648fde-583c-539e-9dd1-6fd695663282"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("5281ba82-3a72-5b09-960c-017fcda7e538"),
                Level = "N5",
                Word = "四",
                Reading = "よん",
                Meaning = "Số 4",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c968d2eb-8bb4-5bcf-9cd5-974ff2d45a7a"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("3157f1ba-0189-5b6e-9ffd-4626d4bac967"),
                Level = "N5",
                Word = "五日",
                Reading = "いつか",
                Meaning = "Ngày mồng 5",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("15f1a6cd-674f-5628-aef7-e65311e8a610"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("3157f1ba-0189-5b6e-9ffd-4626d4bac967"),
                Level = "N5",
                Word = "五月",
                Reading = "ごがつ",
                Meaning = "tháng 5",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("22f86824-eff5-5cf8-a27c-3d602f3c8853"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("3157f1ba-0189-5b6e-9ffd-4626d4bac967"),
                Level = "N5",
                Word = "五つ",
                Reading = "いつつ",
                Meaning = "5 cái",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("eb2567ee-b289-5097-b9c0-0b55e3fe630e"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("3157f1ba-0189-5b6e-9ffd-4626d4bac967"),
                Level = "N5",
                Word = "五",
                Reading = "ご",
                Meaning = "Số 5",
                OrderIndex = 21,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("52955b18-4d87-56a0-87b6-4a0f9d996171"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                Level = "N5",
                Word = "六日",
                Reading = "むいか",
                Meaning = "Ngày mồng 6",
                OrderIndex = 22,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("e03340bf-48a7-5ead-a197-666a75a26a68"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                Level = "N5",
                Word = "六月",
                Reading = "ろくがつ",
                Meaning = "tháng 6",
                OrderIndex = 23,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4aa4d5bd-2526-5550-ae23-908645dcef5a"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                Level = "N5",
                Word = "六つ",
                Reading = "むっつ",
                Meaning = "6 cái",
                OrderIndex = 24,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("686d5947-3b29-54dd-8ccc-8bffea484164"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                Level = "N5",
                Word = "六",
                Reading = "ろく",
                Meaning = "Số 6",
                OrderIndex = 25,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("df4dd369-9eab-575f-8933-1a0e4948a486"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("4ac63e13-411a-5b19-a116-c6778cabcc8b"),
                Level = "N5",
                Word = "七日",
                Reading = "なのか",
                Meaning = "Ngày mồng 7",
                OrderIndex = 26,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("fb869af9-e272-5f1f-ad8e-24cbd08c7925"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("4ac63e13-411a-5b19-a116-c6778cabcc8b"),
                Level = "N5",
                Word = "七月",
                Reading = "しちがつ",
                Meaning = "tháng 7",
                OrderIndex = 27,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d5b34734-ce46-516f-a981-f928b7cc5d34"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("4ac63e13-411a-5b19-a116-c6778cabcc8b"),
                Level = "N5",
                Word = "七つ",
                Reading = "ななつ",
                Meaning = "7 cái",
                OrderIndex = 28,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8b54890e-ea98-5530-b678-3a939a995d6d"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("4ac63e13-411a-5b19-a116-c6778cabcc8b"),
                Level = "N5",
                Word = "七",
                Reading = "なな/しち",
                Meaning = "Số 7",
                OrderIndex = 29,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("08d25db3-3b95-530e-b475-34531b9e4451"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八日",
                Reading = "ようか",
                Meaning = "Ngày mồng 8",
                OrderIndex = 30,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("db9066c2-df92-55b9-91a9-c7082b73a6c0"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八月",
                Reading = "はちがつ",
                Meaning = "Tháng 8",
                OrderIndex = 31,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("af233439-d793-5740-848d-ff33577ef25c"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八つ",
                Reading = "やっつ",
                Meaning = "8 cái",
                OrderIndex = 32,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("0a135f3c-630a-5ca6-8425-399a85797880"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八",
                Reading = "はち",
                Meaning = "Số 8",
                OrderIndex = 33,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("99be3c1f-1dee-5545-b4d7-a0fe8ca006fc"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("baf49199-9e6c-5f39-b696-c1ae140ebb7b"),
                Level = "N5",
                Word = "九日",
                Reading = "ここのか",
                Meaning = "Ngày mồng 9",
                OrderIndex = 34,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b48b8626-2480-5c75-84ee-fb5be0fdca81"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("baf49199-9e6c-5f39-b696-c1ae140ebb7b"),
                Level = "N5",
                Word = "九月",
                Reading = "くがつ",
                Meaning = "tháng 9",
                OrderIndex = 35,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c841cfac-3db6-5380-98bf-ddbee3da4d8b"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("baf49199-9e6c-5f39-b696-c1ae140ebb7b"),
                Level = "N5",
                Word = "九つ",
                Reading = "ここのつ",
                Meaning = "9 cái",
                OrderIndex = 36,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2dba3b11-3695-51d9-b2f2-5f427ea3cddb"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("baf49199-9e6c-5f39-b696-c1ae140ebb7b"),
                Level = "N5",
                Word = "九",
                Reading = "きゅう",
                Meaning = "Số 9",
                OrderIndex = 37,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1fa838a6-2b72-5889-8aac-1b44fb7280ca"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ddb5d53b-5213-599d-b9be-3536802be0e7"),
                Level = "N5",
                Word = "十日",
                Reading = "とおka",
                Meaning = "Ngày mồng 10",
                OrderIndex = 38,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4bcf50e7-fd7e-56af-a4bc-941cb1908a84"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ddb5d53b-5213-599d-b9be-3536802be0e7"),
                Level = "N5",
                Word = "十月",
                Reading = "じゅうがつ",
                Meaning = "tháng 10",
                OrderIndex = 39,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d57df543-b5f4-517b-b7da-53dbc41ad79a"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ddb5d53b-5213-599d-b9be-3536802be0e7"),
                Level = "N5",
                Word = "十",
                Reading = "じゅう",
                Meaning = "Số 10",
                OrderIndex = 40,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("05b067a8-f748-50c0-9905-fe48aaecfe98"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                Level = "N5",
                Word = "二百",
                Reading = "にひゃく",
                Meaning = "200",
                OrderIndex = 41,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b51c8a24-0206-5547-bd69-59a27fccca9d"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三百",
                Reading = "さんびゃく",
                Meaning = "300",
                OrderIndex = 42,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("26e61cfb-03a1-5206-a99a-e5e2c53a3509"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                Level = "N5",
                Word = "六百",
                Reading = "ろっぴゃく",
                Meaning = "600",
                OrderIndex = 43,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2920a7cc-b553-5034-8674-2498c5ae0024"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八百",
                Reading = "はっぴゃく",
                Meaning = "800",
                OrderIndex = 44,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("99bdce10-eadb-53fe-a6e5-50d3c1049674"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("7e864a3b-ca27-5a05-abd8-a6c16639a2b5"),
                Level = "N5",
                Word = "千",
                Reading = "せん",
                Meaning = "1000",
                OrderIndex = 45,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c0ee16d2-5312-598d-818e-2c957b13179a"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三千",
                Reading = "さんぜん",
                Meaning = "3000",
                OrderIndex = 46,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ed3e3a34-1560-5c95-98a8-2df2ca8b6ff7"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八千",
                Reading = "はっせん",
                Meaning = "8000",
                OrderIndex = 47,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("07a0e297-2b71-5e29-abeb-8ca96cc3f469"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一万",
                Reading = "いちまん",
                Meaning = "10.000",
                OrderIndex = 48,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("e284d007-e9dc-5e99-bde7-9083145508cc"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("5b211445-142d-507e-9963-47430a88e47b"),
                Level = "N5",
                Word = "百円",
                Reading = "ひゃくえん",
                Meaning = "100Yên",
                OrderIndex = 49,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d6ff9178-c5d7-534c-b12f-de1141adbdcc"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一万円",
                Reading = "いちまんえん",
                Meaning = "10.000Yên",
                OrderIndex = 50,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems2)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 3: Thời gian và Ngày trong tuần (JPD113)
        var lesson3Id = Guid.Parse("d7b94290-67f0-5d2c-bb16-88d3b4d23d7e");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson3Id))
        {
            db.KanjiLessons.Add(new KanjiLesson
            {
                Id = lesson3Id,
                Level = "N5",
                LessonNumber = 3,
                Title = "Thời gian và Ngày trong tuần",
                Description = "Thời gian và Ngày trong tuần - JPD113 Kanji N5.",
                AccessTier = "free",
                PackageCode = "kanji_jpd113",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            });
        }

        var kanjiItems3 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "月",
                HanViet = "NGUYỆT",
                Meaning = "Mặt trăng, tháng, thứ Hai",
                StrokeCount = 4,
                KunReading = "つき",
                OnReading = "ゲツ、ガツ",
                Mnemonic = "Hình ảnh mặt trăng (月) khuyết mỏng manh chênh chếch trên bầu trời, có hai đám mây trôi ngang qua ở giữa.",
                ComponentMapJson = @"[]",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("26528581-0a3a-5beb-a4e1-4bb8802c74fa"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "火",
                HanViet = "HỎA",
                Meaning = "Lửa, thứ Ba",
                StrokeCount = 4,
                KunReading = "ひ、び、ほ",
                OnReading = "カ",
                Mnemonic = "Người (人) vung hai tay mạnh mẽ tạo ra những tia lửa (火) bùng cháy rực rỡ.",
                ComponentMapJson = @"[]",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("bff300d8-ac44-5fef-853d-f2c449f59b34"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "水",
                HanViet = "THỦY",
                Meaning = "Nước, thứ Tư",
                StrokeCount = 4,
                KunReading = "みず",
                OnReading = "スイ",
                Mnemonic = "Dòng thác chảy thẳng tắp ở giữa (亅) đập vào đá làm nước (水) bắn tung tóe ra hai bên.",
                ComponentMapJson = @"[]",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("37abdf45-fc81-5dda-8566-2c993fdac3d1"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "木",
                HanViet = "MỘC",
                Meaning = "Cây, thứ Năm",
                StrokeCount = 4,
                KunReading = "き、こ",
                OnReading = "モク、ボク",
                Mnemonic = "Cành và thân cây dang ra như hình chữ thập (十), bên dưới là rễ cắm sâu xuống đất tạo thành cái cây (木).",
                ComponentMapJson = @"[]",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("576b6447-5b94-5a7a-957b-aac336483316"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "金",
                HanViet = "KIM",
                Meaning = "Tiền, vàng, thứ Sáu",
                StrokeCount = 8,
                KunReading = "かね、かな",
                OnReading = "キン、コン",
                Mnemonic = "Được giấu kín dưới mái nhà (𠆢) của nhà vua (王) chính là hai thỏi vàng (ハ) vô giá (金).",
                ComponentMapJson = @"[]",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("118222bb-2f95-564f-8ff5-beb7c16e97e8"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "土",
                HanViet = "THỔ",
                Meaning = "Đất, thứ Bảy",
                StrokeCount = 3,
                KunReading = "つち",
                OnReading = "ド、ト",
                Mnemonic = "Cắm một cây thập tự giá (十) sâu xuống mặt đất (一) bằng phẳng (土).",
                ComponentMapJson = @"[]",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "何",
                HanViet = "HÀ",
                Meaning = "Cái gì",
                StrokeCount = 7,
                KunReading = "なに、なん",
                OnReading = "カ",
                Mnemonic = "Một người (亻) luôn tò mò hỏi xem bản thân có thể (可) làm được cái gì (何).",
                ComponentMapJson = @"[]",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("2473b071-dd7a-561b-9b5a-67dabe649765"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "年",
                HanViet = "NIÊN",
                Meaning = "Năm",
                StrokeCount = 6,
                KunReading = "とし",
                OnReading = "ネン",
                Mnemonic = "Người (𠂉) nông dân làm việc vất vả thu hoạch được một (一) chục (十) vụ mùa là trôi qua hết một năm (年).",
                ComponentMapJson = @"[]",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("eff056fa-5214-5e6d-b2a3-c677dc8655fc"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "時",
                HanViet = "THỜI",
                Meaning = "Thời gian, giờ",
                StrokeCount = 10,
                KunReading = "とき",
                OnReading = "ジ",
                Mnemonic = "Ngày xưa người ta nhìn mặt trời (日) mọc trên đỉnh nóc chùa (寺) để biết thời gian (時) và giờ giấc.",
                ComponentMapJson = @"[]",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("53df3cf0-0b28-5b80-bb4d-07a39190aabd"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "間",
                HanViet = "GIAN",
                Meaning = "Khoảng trống, ở giữa, trong~, gian phòng",
                StrokeCount = 12,
                KunReading = "あいだ、ま",
                OnReading = "カン、ケン",
                Mnemonic = "Ánh mặt trời (日) len lỏi chiếu qua khe hở ở giữa hai cánh cổng (門) lớn tạo ra một không gian (間) rực sáng.",
                ComponentMapJson = @"[]",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("77624de6-8922-5e86-aa9a-824acd663ed6"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "分",
                HanViet = "PHÂN",
                Meaning = "Phân chia, hiểu, phút",
                StrokeCount = 4,
                KunReading = "わ.ける、わ.かる",
                OnReading = "ブン、フン、ブ",
                Mnemonic = "Dùng con dao (刀) sắc bén để chia đồ vật ra làm tám (ハ) phần (分).",
                ComponentMapJson = @"[]",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems3)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems3 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("f1c4fbcb-1919-5386-9d15-05c2785af3dc"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                Level = "N5",
                Word = "月",
                Reading = "つき",
                Meaning = "mặt trăng",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("938d1a34-229f-59cd-aa2c-2623e36dd065"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                Level = "N5",
                Word = "一月",
                Reading = "いちがつ",
                Meaning = "Tháng 1",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2416a85b-540f-5999-ad05-529dd1d327f4"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                Level = "N5",
                Word = "十二月",
                Reading = "じゅうにがつ",
                Meaning = "Tháng 12",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8b80eb00-4895-5b75-9ad8-b3be350a9086"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                Level = "N5",
                Word = "月曜日",
                Reading = "げつようび",
                Meaning = "Thứ 2",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2f80fec5-4a5c-51bb-ac79-b157c26f32fc"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("26528581-0a3a-5beb-a4e1-4bb8802c74fa"),
                Level = "N5",
                Word = "花火",
                Reading = "はなび",
                Meaning = "pháo hoa",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("dd40b42e-6683-567a-98f3-398ba71b98d8"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("26528581-0a3a-5beb-a4e1-4bb8802c74fa"),
                Level = "N5",
                Word = "火",
                Reading = "ひ",
                Meaning = "lửa",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("79189154-b75e-5ea7-8fd4-be73bade0c19"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("26528581-0a3a-5beb-a4e1-4bb8802c74fa"),
                Level = "N5",
                Word = "火曜日",
                Reading = "かようび",
                Meaning = "Thứ 3",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("35efb563-efb7-5520-9f71-218e78c3b1f7"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("bff300d8-ac44-5fef-853d-f2c449f59b34"),
                Level = "N5",
                Word = "水",
                Reading = "みず",
                Meaning = "nước",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2bbbb796-31f8-5ba2-b7f8-b711c2c547c4"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("bff300d8-ac44-5fef-853d-f2c449f59b34"),
                Level = "N5",
                Word = "水曜日",
                Reading = "すいようび",
                Meaning = "Thứ 4",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("49ebfa10-9656-515f-8fdc-23edb27c4e16"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("37abdf45-fc81-5dda-8566-2c993fdac3d1"),
                Level = "N5",
                Word = "木",
                Reading = "き",
                Meaning = "Cây",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9be0f3b0-3256-5926-afb6-57f1658916ea"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("37abdf45-fc81-5dda-8566-2c993fdac3d1"),
                Level = "N5",
                Word = "木曜日",
                Reading = "もくようび",
                Meaning = "Thứ 5",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4c71c876-00b8-5b3c-bec6-7793137d67db"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("576b6447-5b94-5a7a-957b-aac336483316"),
                Level = "N5",
                Word = "お金",
                Reading = "おかね",
                Meaning = "tiền",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c00e9ddb-4523-58d1-b69c-df48006c4add"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("576b6447-5b94-5a7a-957b-aac336483316"),
                Level = "N5",
                Word = "金曜日",
                Reading = "きんようび",
                Meaning = "Thứ 6",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("af495d7a-3a86-5709-85a2-f676672fee79"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("118222bb-2f95-564f-8ff5-beb7c16e97e8"),
                Level = "N5",
                Word = "土",
                Reading = "つち",
                Meaning = "Đất",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("dc7b6369-9c06-5ac8-ad81-e5def41c340a"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("118222bb-2f95-564f-8ff5-beb7c16e97e8"),
                Level = "N5",
                Word = "土曜日",
                Reading = "どようび",
                Meaning = "Thứ 7",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8a0abdd5-3e25-5b58-8718-a08e67968179"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何",
                Reading = "なん/なに",
                Meaning = "Cái gì",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6e0c9738-e15e-502e-bc49-017fbd26db1d"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                Level = "N5",
                Word = "何月",
                Reading = "なんがつ",
                Meaning = "tháng mấy",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("78d67df1-50b7-57ce-8b97-05b899d11a6d"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何人",
                Reading = "なんにん",
                Meaning = "mấy người",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("cc3001e3-be65-54df-a3c4-51ae81582a7c"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何曜日",
                Reading = "なんようび",
                Meaning = "Thứ mấy",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("3b7f6356-e24a-5abb-b50c-c956b18a3aaa"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何年",
                Reading = "なんねん",
                Meaning = "Năm bao nhiêu",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8e1f8556-95f0-59fe-bfc6-cc6e6fff2f81"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("2473b071-dd7a-561b-9b5a-67dabe649765"),
                Level = "N5",
                Word = "今年",
                Reading = "ことし",
                Meaning = "Năm nay",
                OrderIndex = 21,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("dd35f64e-a882-5156-8976-dddb72cdc739"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("eff056fa-5214-5e6d-b2a3-c677dc8655fc"),
                Level = "N5",
                Word = "九時",
                Reading = "くじ",
                Meaning = "9 giờ",
                OrderIndex = 22,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("a51d249a-0dfb-54cc-9eb3-bba55fd858b5"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何時",
                Reading = "なんじ",
                Meaning = "Mấy giờ",
                OrderIndex = 23,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("48e22b70-d933-576f-91d2-4dd2e3af9710"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("eff056fa-5214-5e6d-b2a3-c677dc8655fc"),
                Level = "N5",
                Word = "時間",
                Reading = "じかん",
                Meaning = "Thời gian",
                OrderIndex = 24,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1bee6f8d-ff3a-50a4-8d0f-248bc515afe1"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何時間",
                Reading = "なんじかん",
                Meaning = "Mấy tiếng",
                OrderIndex = 25,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("52425a55-871f-5c4a-89e6-e3bd22bca5c9"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("77624de6-8922-5e86-aa9a-824acd663ed6"),
                Level = "N5",
                Word = "分かります",
                Reading = "わかります",
                Meaning = "Hiểu",
                OrderIndex = 26,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c37bdf0c-1424-5961-b1d7-fb4994326401"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("77624de6-8922-5e86-aa9a-824acd663ed6"),
                Level = "N5",
                Word = "半分",
                Reading = "はんぶん",
                Meaning = "1/2",
                OrderIndex = 27,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b5497a56-fa9d-580a-9764-6382b857d3dc"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("77624de6-8922-5e86-aa9a-824acd663ed6"),
                Level = "N5",
                Word = "六分",
                Reading = "ろっぷん",
                Meaning = "6 phút",
                OrderIndex = 28,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems3)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 4: Địa điểm và Phương hướng (JPD123)
        var lesson4Id = Guid.Parse("dbebdd02-f36c-58da-9c3f-8335e2f73e18");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson4Id))
        {
            db.KanjiLessons.Add(new KanjiLesson
            {
                Id = lesson4Id,
                Level = "N3",
                LessonNumber = 4,
                Title = "Địa điểm và Phương hướng",
                Description = "Địa điểm và Phương hướng - JPD123 Kanji N3.",
                AccessTier = "free",
                PackageCode = "kanji_jpd123",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            });
        }

        var kanjiItems4 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("ad4d2793-71e4-556e-a0f3-0ebaf4447021"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "東",
                HanViet = "ĐÔNG",
                Meaning = "Phía Đông",
                StrokeCount = 8,
                KunReading = "ひがし",
                OnReading = "トウ",
                Mnemonic = "Mặt trời (日) ló sau cây (木) ở phía đông (東).",
                ComponentMapJson = @"[]",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("64d8c3c5-11f7-5719-aca2-6028ab33b401"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "京",
                HanViet = "KINH",
                Meaning = "Kinh đô, thủ đô",
                StrokeCount = 8,
                KunReading = "みやこ",
                OnReading = "キョウ、ケイ",
                Mnemonic = "Khu dân cư (口) dưới mái che (亠) tạo thành kinh đô (京).",
                ComponentMapJson = @"[]",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("2daea2c1-2fc4-51dd-8d60-20539c5ed6b2"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "名",
                HanViet = "DANH",
                Meaning = "Tên gọi",
                StrokeCount = 6,
                KunReading = "な",
                OnReading = "メイ、ミョウ",
                Mnemonic = "Buổi tối (夕) dùng miệng (口) gọi tên nhau thành danh (名).",
                ComponentMapJson = @"[]",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("65a224f0-0273-56ce-a540-8f4458b7304a"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "前",
                HanViet = "TIỀN",
                Meaning = "Phía trước",
                StrokeCount = 9,
                KunReading = "まえ",
                OnReading = "ゼン",
                Mnemonic = "Cầm dao (刂) hướng về phía trước cơ thể (月) tạo thành trước mặt (前).",
                ComponentMapJson = @"[]",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("cf563118-7293-503e-b9b2-1828e82103ee"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "国",
                HanViet = "QUỐC",
                Meaning = "Quốc gia",
                StrokeCount = 8,
                KunReading = "くに",
                OnReading = "コク",
                Mnemonic = "Vua (王) nằm trong lãnh thổ bao quanh (囗) tạo thành quốc gia (国).",
                ComponentMapJson = @"[]",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("56840c05-f8d1-5b3d-bcdc-7284a7d660ef"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "南",
                HanViet = "NAM",
                Meaning = "Phía Nam",
                StrokeCount = 9,
                KunReading = "みなみ",
                OnReading = "ナン",
                Mnemonic = "Vùng đất bao quanh (冂) phía nam (南) có nhiều người tụ họp dưới chữ mười (十).",
                ComponentMapJson = @"[]",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("a44d56c2-f6a7-5d8b-826c-059016900958"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "女",
                HanViet = "NỮ",
                Meaning = "Phụ nữ",
                StrokeCount = 3,
                KunReading = "おんな",
                OnReading = "ジョ、ニョ",
                Mnemonic = "Hình dáng người phụ nữ quỳ tạo thành chữ nữ (女).",
                ComponentMapJson = @"[]",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("1828b3e7-255d-54ee-80d6-b2972cf5b7a5"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "男",
                HanViet = "NAM",
                Meaning = "Nam giới",
                StrokeCount = 7,
                KunReading = "おとこ",
                OnReading = "ダン、ナン",
                Mnemonic = "Dùng sức mạnh (力) làm ruộng (田) là đàn ông (男).",
                ComponentMapJson = @"[]",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("eab57142-b7f2-5ae2-86b2-142856a3c4a0"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "区",
                HanViet = "KHU",
                Meaning = "Khu vực",
                StrokeCount = 4,
                KunReading = "",
                OnReading = "ク",
                Mnemonic = "Không gian được chia (㐅) trong khung (匚) thành khu vực (区).",
                ComponentMapJson = @"[]",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("53a1dfd9-c926-58fc-b1fe-b56a4e64375e"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "市",
                HanViet = "THỊ",
                Meaning = "Thành phố / chợ",
                StrokeCount = 5,
                KunReading = "いち",
                OnReading = "シ",
                Mnemonic = "Những tấm vải (巾) dưới mái che (亠) tạo nên khu chợ, thành thị (市).",
                ComponentMapJson = @"[]",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems4)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems4 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("38246502-81c1-55d7-800b-ab6038f2f9fc"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("ad4d2793-71e4-556e-a0f3-0ebaf4447021"),
                Level = "N3",
                Word = "東",
                Reading = "ひがし",
                Meaning = "phía đông",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2eab0700-9e82-58e0-99e2-5749bf6f468b"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("ad4d2793-71e4-556e-a0f3-0ebaf4447021"),
                Level = "N3",
                Word = "東京",
                Reading = "とうきょう",
                Meaning = "Tokyo",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9a885a75-435b-577a-8401-cfb2c202a0bb"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("65a224f0-0273-56ce-a540-8f4458b7304a"),
                Level = "N3",
                Word = "午前",
                Reading = "ごぜん",
                Meaning = "a.m",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b70898fd-3a3c-55ce-ac41-ddb100637e13"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("65a224f0-0273-56ce-a540-8f4458b7304a"),
                Level = "N3",
                Word = "前日",
                Reading = "ぜんじつ",
                Meaning = "ngày trước đó",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("62fa9519-d830-5954-baba-dd2673ea3b7b"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("2daea2c1-2fc4-51dd-8d60-20539c5ed6b2"),
                Level = "N3",
                Word = "名前",
                Reading = "なまえ",
                Meaning = "tên",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b34c1852-6eba-5054-abc6-fd7552893535"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("cf563118-7293-503e-b9b2-1828e82103ee"),
                Level = "N3",
                Word = "国",
                Reading = "くに",
                Meaning = "đất nước, quốc gia",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8052d08b-11f8-5206-8116-bb614a917811"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("cf563118-7293-503e-b9b2-1828e82103ee"),
                Level = "N3",
                Word = "外国",
                Reading = "がいこく",
                Meaning = "nước ngoài",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("565889bb-d243-5bbb-8902-358013696892"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("1828b3e7-255d-54ee-80d6-b2972cf5b7a5"),
                Level = "N3",
                Word = "男の人",
                Reading = "おとこのひと",
                Meaning = "người đàn ông",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2b894757-e270-5cc5-82a9-627a1f431f93"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("a44d56c2-f6a7-5d8b-826c-059016900958"),
                Level = "N3",
                Word = "女の人",
                Reading = "おんなのひと",
                Meaning = "người phụ nữ",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("912c2820-2c4c-55c2-be32-bdfa47830ba8"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("eab57142-b7f2-5ae2-86b2-142856a3c4a0"),
                Level = "N3",
                Word = "～区",
                Reading = "〜く",
                Meaning = "quận ~",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ac34fcf5-b055-58cf-bc16-ffc3151b9587"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("53a1dfd9-c926-58fc-b1fe-b56a4e64375e"),
                Level = "N3",
                Word = "～市",
                Reading = "〜し",
                Meaning = "Thành phố ~",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("02a077ae-757b-5e67-91e9-5deb88fb2ac8"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("65a224f0-0273-56ce-a540-8f4458b7304a"),
                Level = "N3",
                Word = "前",
                Reading = "まえ",
                Meaning = "phía trước",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("60864bcf-a671-51e7-af63-394fe6a9adc1"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("64d8c3c5-11f7-5719-aca2-6028ab33b401"),
                Level = "N3",
                Word = "京都",
                Reading = "きょうと",
                Meaning = "Kyoto",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b44ad0ba-048a-5563-943d-cc1fe21025a7"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("cf563118-7293-503e-b9b2-1828e82103ee"),
                Level = "N3",
                Word = "外国人",
                Reading = "がいこくじん",
                Meaning = "người nước ngoài",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1d66bea9-1afc-5c33-9a63-df608a15af8a"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("a44d56c2-f6a7-5d8b-826c-059016900958"),
                Level = "N3",
                Word = "男女",
                Reading = "だんじょ",
                Meaning = "nam nữ",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems4)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 5: Hành động và Nghỉ ngơi (JPD123)
        var lesson5Id = Guid.Parse("d3096310-4ab4-55d7-9084-720334835ccc");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson5Id))
        {
            db.KanjiLessons.Add(new KanjiLesson
            {
                Id = lesson5Id,
                Level = "N3",
                LessonNumber = 5,
                Title = "Hành động và Nghỉ ngơi",
                Description = "Hành động và Nghỉ ngơi - JPD123 Kanji N3.",
                AccessTier = "free",
                PackageCode = "kanji_jpd123",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            });
        }

        var kanjiItems5 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("f8571dc2-b128-582f-afe8-56183685f8d5"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "先",
                HanViet = "TIÊN",
                Meaning = "Trước, đầu tiên",
                StrokeCount = 6,
                KunReading = "さき",
                OnReading = "セン",
                Mnemonic = "Con trâu (⺧) mọc hai đôi chân người (儿) chạy vượt lên trước tiên.",
                ComponentMapJson = @"[]",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("1d97f9ed-5faa-57cf-bf8b-3adb806a8f0b"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "週",
                HanViet = "CHU",
                Meaning = "Tuần lễ",
                StrokeCount = 11,
                KunReading = "",
                OnReading = "シュウ",
                Mnemonic = "Bước đi (⻌) trọn vẹn một vòng chu kỳ (周) là kết thúc một tuần lễ.",
                ComponentMapJson = @"[]",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("f719f4c9-db44-595f-8a75-f3530018ae3d"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "毎",
                HanViet = "MỖI",
                Meaning = "Mỗi, mọi",
                StrokeCount = 7,
                KunReading = "ごと",
                OnReading = "マイ",
                Mnemonic = "Người (𠂉) mẹ (毋) đội nón làm việc vất vả mỗi ngày.",
                ComponentMapJson = @"[]",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("b1762e10-ea10-50cb-9513-ce389737a925"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "午",
                HanViet = "NGỌ",
                Meaning = "Buổi trưa, giờ Ngọ",
                StrokeCount = 4,
                KunReading = "",
                OnReading = "ゴ",
                Mnemonic = "Đúng mười (十) giờ sáng có một tia nắng chói chang (ノ) chiếu xuống báo hiệu sắp đến giờ Ngọ (trưa).",
                ComponentMapJson = @"[]",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("95b380a4-8169-5d38-b87a-bca35a0ac341"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "後",
                HanViet = "HẬU",
                Meaning = "Phía sau, sau này",
                StrokeCount = 9,
                KunReading = "あと、うしろ、のち",
                OnReading = "ゴ、コウ",
                Mnemonic = "Bước chân (彳) đi chậm chạp (夂) lùi lại phía sau, nhường đường cho đứa nhỏ (幺).",
                ComponentMapJson = @"[]",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("a7b0cfdd-9764-55e2-a5f5-adf8d3f005f7"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "見",
                HanViet = "KIẾN",
                Meaning = "Nhìn, thấy",
                StrokeCount = 7,
                KunReading = "みる、みえる",
                OnReading = "ケン",
                Mnemonic = "Con mắt (目) mọc thêm hai cái chân (儿) chạy lon ton đi nhìn ngắm khắp nơi.",
                ComponentMapJson = @"[]",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("d3679ee3-f082-5ca0-8920-cf1336840317"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "食",
                HanViet = "THỰC",
                Meaning = "Ăn, thức ăn",
                StrokeCount = 9,
                KunReading = "たべる",
                OnReading = "ショク、ジキ",
                Mnemonic = "Người (人) được dọn cho đồ ăn ngon (良) sẽ có một bữa thực (ăn) thịnh soạn.",
                ComponentMapJson = @"[]",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("a03537e6-2ed4-5467-b66a-dc384eaa4687"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "飲",
                HanViet = "ẨM",
                Meaning = "Uống",
                StrokeCount = 12,
                KunReading = "のむ",
                OnReading = "イン",
                Mnemonic = "Khi cơ thể cảm thấy thiếu (欠) thốn, phải há miệng nạp ngay đồ ăn thức uống (飠).",
                ComponentMapJson = @"[]",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("c57d7cdf-eb30-5672-a5a4-2f911246487d"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "買",
                HanViet = "MÃI",
                Meaning = "Mua",
                StrokeCount = 12,
                KunReading = "かう",
                OnReading = "バイ",
                Mnemonic = "Giăng tấm lưới (罒) lớn gom thật nhiều tiền (貝) để đi mua sắm.",
                ComponentMapJson = @"[]",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("befa0080-7734-5a82-831c-7e2a56ef604e"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "物",
                HanViet = "VẬT",
                Meaning = "Đồ vật",
                StrokeCount = 8,
                KunReading = "もの",
                OnReading = "ブツ、モツ",
                Mnemonic = "Xin đừng (勿) đối xử với con bò (牛) như một món đồ vật vô tri.",
                ComponentMapJson = @"[]",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("312ced8d-b411-57d0-9d7e-37aeefa5f63c"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "行",
                HanViet = "HÀNH",
                Meaning = "Đi, thực hiện",
                StrokeCount = 6,
                KunReading = "いく、ゆく、おこなう",
                OnReading = "コウ、ギョウ",
                Mnemonic = "Bước chân trái (彳) và bước chân phải (亍) cùng phối hợp tiến lên để đi.",
                ComponentMapJson = @"[]",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("f50b90fa-dbd3-5630-9e71-662bc64e0a5d"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "休",
                HanViet = "HƯU",
                Meaning = "Nghỉ ngơi",
                StrokeCount = 6,
                KunReading = "やすむ",
                OnReading = "キュウ",
                Mnemonic = "Người (亻) tựa lưng vào gốc cây (木) để nghỉ ngơi.",
                ComponentMapJson = @"[]",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems5)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems5 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("ebcc7cb9-2fbf-52f3-9fac-d82ed44d7615"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f8571dc2-b128-582f-afe8-56183685f8d5"),
                Level = "N3",
                Word = "先生",
                Reading = "せんせい",
                Meaning = "thầy/cô giáo",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6704da3c-ef4f-5191-88e4-7c51ff538d86"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f8571dc2-b128-582f-afe8-56183685f8d5"),
                Level = "N3",
                Word = "先日",
                Reading = "せんじつ",
                Meaning = "vài ngày trước",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("e49ca78f-4fc6-5f2e-a4ec-b9f8564af74d"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f8571dc2-b128-582f-afe8-56183685f8d5"),
                Level = "N3",
                Word = "先月",
                Reading = "せんげつ",
                Meaning = "tháng trước",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("bbe3845e-362c-5fa7-869b-129f1bcbbb05"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f8571dc2-b128-582f-afe8-56183685f8d5"),
                Level = "N3",
                Word = "先週",
                Reading = "せんしゅう",
                Meaning = "tuần trước",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("81e8cd2c-2c30-5b6b-bae6-c0ee4de6d138"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("1d97f9ed-5faa-57cf-bf8b-3adb806a8f0b"),
                Level = "N3",
                Word = "一週間",
                Reading = "いっしゅうかん",
                Meaning = "1 tuần",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f6269366-7e45-52e9-88e5-de8bab4cdf26"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f719f4c9-db44-595f-8a75-f3530018ae3d"),
                Level = "N3",
                Word = "毎日",
                Reading = "まいにち",
                Meaning = "mỗi ngày",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6ec29966-a8ce-5d88-9594-472ecee97d0f"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("1d97f9ed-5faa-57cf-bf8b-3adb806a8f0b"),
                Level = "N3",
                Word = "毎週",
                Reading = "まいしゅう",
                Meaning = "mỗi tuần",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ed8fe2c4-5afd-53e2-a68b-93a0154e33e4"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f719f4c9-db44-595f-8a75-f3530018ae3d"),
                Level = "N3",
                Word = "毎年",
                Reading = "まいとし/まいねん",
                Meaning = "mỗi năm",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("7ac3b330-0316-58d8-a473-dba1c3a80ceb"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f719f4c9-db44-595f-8a75-f3530018ae3d"),
                Level = "N3",
                Word = "毎月",
                Reading = "まいつき",
                Meaning = "mỗi tháng",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8dce2a1a-28c2-5cff-a946-1d2843f216e6"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("95b380a4-8169-5d38-b87a-bca35a0ac341"),
                Level = "N3",
                Word = "後ろ",
                Reading = "うしろ",
                Meaning = "phía sau",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ffae1439-e319-54bf-b2e3-322c330e25b8"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("95b380a4-8169-5d38-b87a-bca35a0ac341"),
                Level = "N3",
                Word = "後",
                Reading = "あと",
                Meaning = "sau đó",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("bded48d1-de4e-589e-a5d1-70a983b9b0c8"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("b1762e10-ea10-50cb-9513-ce389737a925"),
                Level = "N3",
                Word = "午後",
                Reading = "ごご",
                Meaning = "p.m",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8617f98f-fb68-5fe6-920c-0cf6dee21612"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("95b380a4-8169-5d38-b87a-bca35a0ac341"),
                Level = "N3",
                Word = "前後",
                Reading = "ぜんご",
                Meaning = "trước sau",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("01396178-b469-5fef-b00a-4faff5a9a6b1"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a7b0cfdd-9764-55e2-a5f5-adf8d3f005f7"),
                Level = "N3",
                Word = "見ます",
                Reading = "みます",
                Meaning = "nhìn",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4375f611-fe31-564c-952a-7f9cd31dd987"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a7b0cfdd-9764-55e2-a5f5-adf8d3f005f7"),
                Level = "N3",
                Word = "見学",
                Reading = "けんがく",
                Meaning = "tham quan kiến tập",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b0ca1a6d-b37f-5e92-b9a9-1d7474776ff9"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("d3679ee3-f082-5ca0-8920-cf1336840317"),
                Level = "N3",
                Word = "食べます",
                Reading = "たべます",
                Meaning = "ăn",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("7e8bcd97-9072-5d79-b395-a63a7d9d279a"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("d3679ee3-f082-5ca0-8920-cf1336840317"),
                Level = "N3",
                Word = "食事",
                Reading = "しょくじ",
                Meaning = "ăn, bữa ăn",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("895eda7a-c984-5c40-bf58-65a445339b14"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a03537e6-2ed4-5467-b66a-dc384eaa4687"),
                Level = "N3",
                Word = "飲みます",
                Reading = "のみます",
                Meaning = "uống",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9ef77259-c78e-547c-b8ac-416bee4cb2e1"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("d3679ee3-f082-5ca0-8920-cf1336840317"),
                Level = "N3",
                Word = "飲食",
                Reading = "いんしょく",
                Meaning = "ẩm thực",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1eb1c03b-fd74-553b-bb04-a54bfa20f01b"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a03537e6-2ed4-5467-b66a-dc384eaa4687"),
                Level = "N3",
                Word = "飲み水",
                Reading = "のみみず",
                Meaning = "nước uống",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("63cd79da-6145-597f-b52e-1bb3d9999737"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f50b90fa-dbd3-5630-9e71-662bc64e0a5d"),
                Level = "N3",
                Word = "休みます",
                Reading = "やすみます",
                Meaning = "nghỉ",
                OrderIndex = 21,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b01c3b1b-3f93-58f5-9f0f-46d2e040f27c"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f50b90fa-dbd3-5630-9e71-662bc64e0a5d"),
                Level = "N3",
                Word = "休みの日",
                Reading = "やすみのひ",
                Meaning = "ngày nghỉ",
                OrderIndex = 22,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("0e3b4753-c54c-512e-8b20-a14f35af0b3e"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f50b90fa-dbd3-5630-9e71-662bc64e0a5d"),
                Level = "N3",
                Word = "休日",
                Reading = "きゅうじつ",
                Meaning = "ngày nghỉ",
                OrderIndex = 23,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8a2dfc71-b224-5d65-bf28-2644246b9702"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("c57d7cdf-eb30-5672-a5a4-2f911246487d"),
                Level = "N3",
                Word = "買います",
                Reading = "かいます",
                Meaning = "mua",
                OrderIndex = 24,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("23995647-53f8-5554-9345-fc499f0ee552"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("befa0080-7734-5a82-831c-7e2a56ef604e"),
                Level = "N3",
                Word = "物",
                Reading = "もの",
                Meaning = "đồ vật",
                OrderIndex = 25,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("91e3e106-d51b-5f04-9f8e-c42702bfce20"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("c57d7cdf-eb30-5672-a5a4-2f911246487d"),
                Level = "N3",
                Word = "買い物",
                Reading = "かいもの",
                Meaning = "mua sắm",
                OrderIndex = 26,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("3c097986-b70b-501c-a5e1-70c6b6eec967"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("d3679ee3-f082-5ca0-8920-cf1336840317"),
                Level = "N3",
                Word = "食べ物",
                Reading = "たべもの",
                Meaning = "đồ ăn",
                OrderIndex = 27,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("049782a3-32c7-5d80-be6a-bcc4a7695233"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a03537e6-2ed4-5467-b66a-dc384eaa4687"),
                Level = "N3",
                Word = "飲み物",
                Reading = "のみもの",
                Meaning = "đồ uống",
                OrderIndex = 28,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f98a1aa8-5a01-5cfc-9ac0-13b44e6fa0b3"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("befa0080-7734-5a82-831c-7e2a56ef604e"),
                Level = "N3",
                Word = "人物",
                Reading = "じんぶつ",
                Meaning = "nhân vật",
                OrderIndex = 29,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("aa379166-1812-5d7e-898c-37d45a9b46b7"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a7b0cfdd-9764-55e2-a5f5-adf8d3f005f7"),
                Level = "N3",
                Word = "見物",
                Reading = "けんぶつ",
                Meaning = "tham quan",
                OrderIndex = 30,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8233df81-2e23-599c-910f-5b33c75fe36e"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("312ced8d-b411-57d0-9d7e-37aeefa5f63c"),
                Level = "N3",
                Word = "行きます",
                Reading = "いきます",
                Meaning = "đi",
                OrderIndex = 31,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems5)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 6: Giao tiếp và Sinh hoạt (JPD123)
        var lesson6Id = Guid.Parse("7e64f2ed-a054-568a-8109-16708122f2f3");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson6Id))
        {
            db.KanjiLessons.Add(new KanjiLesson
            {
                Id = lesson6Id,
                Level = "N3",
                LessonNumber = 6,
                Title = "Giao tiếp và Sinh hoạt",
                Description = "Giao tiếp và Sinh hoạt - JPD123 Kanji N3.",
                AccessTier = "free",
                PackageCode = "kanji_jpd123",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            });
        }

        var kanjiItems6 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "今",
                HanViet = "KIM",
                Meaning = "Bây giờ, hiện tại",
                StrokeCount = 4,
                KunReading = "いま",
                OnReading = "コン、キン",
                Mnemonic = "Người (𠆢) vội vã chạy (ラ) về nhà ngay bây giờ (今).",
                ComponentMapJson = @"[]",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "来",
                HanViet = "LAI",
                Meaning = "Đến",
                StrokeCount = 7,
                KunReading = "く.る、き.たる",
                OnReading = "ライ",
                Mnemonic = "Mang theo hai (丷) hạt giống đến (来) gộp thành một (一) đống dưới gốc cây (木).",
                ComponentMapJson = @"[]",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("ccd5f9fb-158b-58ea-9717-1806bf5c0d89"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "帰",
                HanViet = "QUY",
                Meaning = "Trở về",
                StrokeCount = 10,
                KunReading = "かえ.る",
                OnReading = "キ",
                Mnemonic = "Cầm đao (刂) quét chổi (ヨ) dọn dẹp, sau đó trùm khăn (冖) che đồ lại để trở về (帰) nhà.",
                ComponentMapJson = @"[]",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "会",
                HanViet = "HỘI",
                Meaning = "Gặp gỡ, hội họp",
                StrokeCount = 6,
                KunReading = "あ.う",
                OnReading = "カイ、エ",
                Mnemonic = "Hai (二) người (𠆢) lén lút gặp gỡ ở nơi riêng tư (厶) để bàn chuyện đại hội (会).",
                ComponentMapJson = @"[]",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("dcacdd9c-da20-511a-a01c-3ab61322d652"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "社",
                HanViet = "XÃ",
                Meaning = "Xã hội, công ty, đền thờ",
                StrokeCount = 7,
                KunReading = "やしろ",
                OnReading = "シャ",
                Mnemonic = "Lập bàn thờ thần (礻) trên bãi đất (土) rộng để cúng tế cho cả xã hội (社).",
                ComponentMapJson = @"[]",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("825fb13d-2b5f-5a47-aefc-0fe1c0c3c290"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "聞",
                HanViet = "VĂN",
                Meaning = "Nghe, hỏi",
                StrokeCount = 14,
                KunReading = "き.く、き.こえる",
                OnReading = "ブン、モン",
                Mnemonic = "Ghé sát tai (耳) vào cổng (門) để nghe ngóng và hỏi thăm tin tức.",
                ComponentMapJson = @"[]",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("2d9ed815-cb0f-5465-9d86-9c9c69412804"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "読",
                HanViet = "ĐỘC",
                Meaning = "Đọc",
                StrokeCount = 14,
                KunReading = "よ.む",
                OnReading = "ドク、トク",
                Mnemonic = "Dùng ngôn từ (言) để rao bán (売) cuốn sách mà mình đang đọc (読).",
                ComponentMapJson = @"[]",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("af7bb69e-c0ff-5d82-b2fc-ace3112451cb"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "書",
                HanViet = "THƯ",
                Meaning = "Viết, sách",
                StrokeCount = 10,
                KunReading = "か.く",
                OnReading = "ショ",
                Mnemonic = "Cầm cây bút (聿) mải mê viết sách (書) suốt cả một ngày (日) dài.",
                ComponentMapJson = @"[]",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("823b9d3b-c02f-5bab-bba7-12f8c7751748"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "話",
                HanViet = "THOẠI",
                Meaning = "Trò chuyện, câu chuyện",
                StrokeCount = 13,
                KunReading = "はな.す、はなし",
                OnReading = "ワ",
                Mnemonic = "Kết hợp ngôn từ (言) khéo léo và uốn cái lưỡi (舌) để trò chuyện (話) với mọi người.",
                ComponentMapJson = @"[]",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems6)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems6 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("d5f99b7b-0954-5eb1-a8d9-1950720ba379"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                Level = "N3",
                Word = "今年",
                Reading = "ことし",
                Meaning = "năm nay",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("28f98604-2d1e-50ce-801c-31b6662b141a"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                Level = "N3",
                Word = "今日",
                Reading = "きょう",
                Meaning = "hôm nay",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("e1682384-80fb-5201-93fc-e319c19ec606"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                Level = "N3",
                Word = "今月",
                Reading = "こんげつ",
                Meaning = "tháng này",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("577ffdf1-a054-5f93-8229-bc69673ebbc4"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                Level = "N3",
                Word = "今週",
                Reading = "こんしゅう",
                Meaning = "tuần này",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("17a4a42f-7bbb-5a1a-8f97-91d66ca983b2"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                Level = "N3",
                Word = "来ます",
                Reading = "きます",
                Meaning = "đến",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ad20e938-b710-56e8-b870-d3b235bf506d"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                Level = "N3",
                Word = "来週",
                Reading = "らいしゅう",
                Meaning = "tuần sau",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ebd4c716-5b92-504f-88f1-91900b816fb4"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                Level = "N3",
                Word = "来月",
                Reading = "らいげつ",
                Meaning = "tháng sau",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("35cb15c5-f3fb-50f5-9eaf-1351200ca131"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                Level = "N3",
                Word = "来年",
                Reading = "らいねん",
                Meaning = "năm sau",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("120d8005-61e0-586f-b1bf-ccaa8af2a087"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                Level = "N3",
                Word = "来日",
                Reading = "らいにち",
                Meaning = "đến Nhật Bản",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("95f75d8b-ffe7-5f6e-9b07-6e247859dafc"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("ccd5f9fb-158b-58ea-9717-1806bf5c0d89"),
                Level = "N3",
                Word = "帰ります",
                Reading = "かえります",
                Meaning = "về",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("19bd0255-1509-5ab0-bfe5-5db20932f0f3"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("ccd5f9fb-158b-58ea-9717-1806bf5c0d89"),
                Level = "N3",
                Word = "日帰り",
                Reading = "ひがえり",
                Meaning = "đi về trong ngày",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2e1f4c0c-02dc-578b-9f4e-6108e0ba0103"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("ccd5f9fb-158b-58ea-9717-1806bf5c0d89"),
                Level = "N3",
                Word = "帰国",
                Reading = "きこく",
                Meaning = "về nước",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("95a273a5-ae9f-5fa8-b0c8-3fbfaedcd3cb"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "会います",
                Reading = "あいます",
                Meaning = "gặp",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("44339486-acbb-53fb-b606-aee358b5f7b7"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "飲み会",
                Reading = "のみかい",
                Meaning = "nhậu",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("01ac7cf4-ad81-5a4f-b664-95c660153162"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "国会",
                Reading = "こっかい",
                Meaning = "quốc hội",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("431299c9-d53a-5405-a7b6-4d0540244269"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "会見",
                Reading = "かいけん",
                Meaning = "họp báo",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4498c872-603e-5771-98b3-9d3d7d88ed05"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "会社",
                Reading = "かいしゃ",
                Meaning = "công ty",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("684af827-188c-55bc-8bb3-06993bbbebf8"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "社会",
                Reading = "しゃかい",
                Meaning = "xã hội",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8e5e0456-8177-5310-b715-ffd794885866"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("825fb13d-2b5f-5a47-aefc-0fe1c0c3c290"),
                Level = "N3",
                Word = "聞きます",
                Reading = "ききます",
                Meaning = "nghe",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d69ba01b-66d4-5e9c-b96b-e8f4e41a55df"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("825fb13d-2b5f-5a47-aefc-0fe1c0c3c290"),
                Level = "N3",
                Word = "新聞",
                Reading = "しんぶん",
                Meaning = "báo",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("a10ecc51-a7b4-5991-99e3-f5a3f891c427"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("2d9ed815-cb0f-5465-9d86-9c9c69412804"),
                Level = "N3",
                Word = "読みます",
                Reading = "よみます",
                Meaning = "đọc",
                OrderIndex = 21,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ddc1c91e-d3ab-5261-8483-de9e6f97c6fe"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("2d9ed815-cb0f-5465-9d86-9c9c69412804"),
                Level = "N3",
                Word = "読み物",
                Reading = "よみもの",
                Meaning = "tài liệu đọc",
                OrderIndex = 22,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d708e91d-6027-5c0a-a795-8c3b1606c16a"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("af7bb69e-c0ff-5d82-b2fc-ace3112451cb"),
                Level = "N3",
                Word = "書きます",
                Reading = "かきます",
                Meaning = "viết",
                OrderIndex = 23,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8f88ed0a-9d49-5eaa-86e0-081a40045d83"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("2d9ed815-cb0f-5465-9d86-9c9c69412804"),
                Level = "N3",
                Word = "読書",
                Reading = "どくしょ",
                Meaning = "đọc sách",
                OrderIndex = 24,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("96d406a9-4355-5202-8d95-0131da1d476d"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("823b9d3b-c02f-5bab-bba7-12f8c7751748"),
                Level = "N3",
                Word = "話します",
                Reading = "はなします",
                Meaning = "nói chuyện",
                OrderIndex = 25,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d833c29f-266a-54a4-8b8f-085701a1ee6c"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("823b9d3b-c02f-5bab-bba7-12f8c7751748"),
                Level = "N3",
                Word = "話",
                Reading = "はなし",
                Meaning = "câu chuyện",
                OrderIndex = 26,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("06e2ae88-d9da-521c-94ec-db937c526e0b"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "会話",
                Reading = "かいわ",
                Meaning = "hội thoại",
                OrderIndex = 27,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("27486b52-1052-575e-83c3-ff155c62d370"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("823b9d3b-c02f-5bab-bba7-12f8c7751748"),
                Level = "N3",
                Word = "電話",
                Reading = "でんわ",
                Meaning = "điện thoại",
                OrderIndex = 28,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("89f112fa-8a16-586f-937f-c1afebb8ac10"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                Level = "N3",
                Word = "今",
                Reading = "いま",
                Meaning = "bây giờ",
                OrderIndex = 29,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8cfa6207-6423-593b-8952-d474bb460f7a"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("af7bb69e-c0ff-5d82-b2fc-ace3112451cb"),
                Level = "N3",
                Word = "辞書",
                Reading = "じしょ",
                Meaning = "từ điển",
                OrderIndex = 30,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f484cb8e-47cb-540f-b9cd-6345870e9710"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("af7bb69e-c0ff-5d82-b2fc-ace3112451cb"),
                Level = "N3",
                Word = "書き物",
                Reading = "かきもの",
                Meaning = "tài liệu viết",
                OrderIndex = 31,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8750a887-7003-5116-9307-e7b1831f2223"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "お寺",
                Reading = "おてら",
                Meaning = "chùa",
                OrderIndex = 32,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("cec15d05-191f-56cd-8dd3-9a34b686e38e"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "言います",
                Reading = "いいます",
                Meaning = "nói",
                OrderIndex = 33,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f65d653b-6569-5e57-acfc-5d55b45970f4"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "言語",
                Reading = "げんご",
                Meaning = "ngôn ngữ",
                OrderIndex = 34,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f634a252-2cdd-5223-9912-b7eabac350ec"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "言葉",
                Reading = "ことば",
                Meaning = "từ vựng",
                OrderIndex = 35,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("a26981a8-3bab-54f3-b037-dd2a9901f98c"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "貝",
                Reading = "かい",
                Meaning = "con sò",
                OrderIndex = 36,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f6063b99-ddf0-53d0-9a59-4fdf8ee28a9f"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "田んぼ",
                Reading = "たんぼ",
                Meaning = "cánh đồng",
                OrderIndex = 37,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("a6ec786e-4d32-546d-8958-cb7b461e50fb"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "水田",
                Reading = "すいでん",
                Meaning = "cánh đồng lúa nước",
                OrderIndex = 38,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("76ed4f2f-d79c-5cbc-ac02-0aca47ecec8c"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "力",
                Reading = "ちから",
                Meaning = "sức lực",
                OrderIndex = 39,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d064e974-eb10-5578-b912-03aa378b5be2"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "水力",
                Reading = "すいりょく",
                Meaning = "sức nước",
                OrderIndex = 40,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b80bec60-a1b4-542d-949d-00f1cafc1b4a"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "火力",
                Reading = "かりょく",
                Meaning = "công suất nhiệt, hỏa lực",
                OrderIndex = 41,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("588460ef-4f5c-5b9f-947c-9d928e190932"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "門",
                Reading = "もん",
                Meaning = "cổng, cửa",
                OrderIndex = 42,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems6)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 7: Tự nhiên và Cơ bản (JPD123)
        var lesson7Id = Guid.Parse("870a45b6-b242-5803-a0e5-35effe8ece29");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson7Id))
        {
            db.KanjiLessons.Add(new KanjiLesson
            {
                Id = lesson7Id,
                Level = "N3",
                LessonNumber = 7,
                Title = "Tự nhiên và Cơ bản",
                Description = "Tự nhiên và Cơ bản - JPD123 Kanji N3.",
                AccessTier = "free",
                PackageCode = "kanji_jpd123",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            });
        }

        var kanjiItems7 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("06718312-d179-57e1-846a-4c94b7492c57"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "寺",
                HanViet = "TỰ",
                Meaning = "Chùa",
                StrokeCount = 6,
                KunReading = "てら",
                OnReading = "ジ",
                Mnemonic = "Dùng tay (寸) đắp từng tấc đất (土) để xây dựng ngôi chùa (寺).",
                ComponentMapJson = @"[]",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("647be324-4dfe-51af-994b-df33182c6398"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "言",
                HanViet = "NGÔN",
                Meaning = "Nói, ngôn từ",
                StrokeCount = 7,
                KunReading = "い.う、こと",
                OnReading = "ゴン、ゲン",
                Mnemonic = "Kẻ cầm đầu (亠) có đến hai (二) cái miệng (口) để thay đổi ngôn từ lắt léo.",
                ComponentMapJson = @"[]",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("15c53e95-a929-5d5a-beb2-b1332ad72428"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "貝",
                HanViet = "BỐI",
                Meaning = "Vỏ sò, tiền tệ",
                StrokeCount = 7,
                KunReading = "かい",
                OnReading = "バイ",
                Mnemonic = "Con sò (貝) mở mắt (目) to và thò hai cái chân (ハ) ra ngoài để đi tìm tiền.",
                ComponentMapJson = @"[]",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("9e44fc27-4fc9-5721-96dd-c202def83d6b"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "田",
                HanViet = "ĐIỀN",
                Meaning = "Ruộng",
                StrokeCount = 5,
                KunReading = "た",
                OnReading = "デン",
                Mnemonic = "Khu đất (囗) được chia thành hình chữ thập (十) để làm bờ ruộng (田).",
                ComponentMapJson = @"[]",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("9265dce0-4e5e-5a3c-8441-dfe88d5d9cab"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "力",
                HanViet = "LỰC",
                Meaning = "Sức lực",
                StrokeCount = 2,
                KunReading = "ちから",
                OnReading = "リョク、リキ",
                Mnemonic = "Chữ này nhìn giống hệt hình ảnh cánh tay đang gồng lên cuộn bắp chuột để khoe sức lực (力).",
                ComponentMapJson = @"[]",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("c6ebc2d7-7d08-5e65-a953-9924c7161f0c"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "門",
                HanViet = "MÔN",
                Meaning = "Cửa, cổng",
                StrokeCount = 8,
                KunReading = "かど",
                OnReading = "モン",
                Mnemonic = "Hình ảnh hai cánh cổng (門) lớn đang được mở tung ra hai bên.",
                ComponentMapJson = @"[]",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("465ad202-c37c-571a-985c-69c96c90a421"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "肉",
                HanViet = "NHỤC",
                Meaning = "Thịt",
                StrokeCount = 6,
                KunReading = "",
                OnReading = "ニク",
                Mnemonic = "Hai người (人) lén lút trốn vào trong kho lạnh (冂) để lén ăn thịt (肉).",
                ComponentMapJson = @"[]",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("8dc8753c-940a-55fa-8156-1436615d7e72"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "料",
                HanViet = "LIỄU",
                Meaning = "Nguyên liệu, tài liệu, phí",
                StrokeCount = 10,
                KunReading = "",
                OnReading = "リョウ",
                Mnemonic = "Dùng cái đấu (斗) để đong lường gạo (米) làm nguyên liệu (料) nấu ăn.",
                ComponentMapJson = @"[]",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("894bb594-19e0-5f35-8ad3-6b11c0de8351"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "理",
                HanViet = "LÝ",
                Meaning = "Lý lẽ, logic, địa lý",
                StrokeCount = 11,
                KunReading = "",
                OnReading = "リ",
                Mnemonic = "Vị Vua (王) cai quản các ngôi làng (里) bằng lý (理) lẽ vô cùng công bằng.",
                ComponentMapJson = @"[]",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("01b783d3-c933-5a04-aba8-e7af559397e1"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "野",
                HanViet = "DÃ",
                Meaning = "Cánh đồng, hoang dã",
                StrokeCount = 11,
                KunReading = "の",
                OnReading = "ヤ",
                Mnemonic = "Người trong làng (里) đã dự (予) đoán trước được sẽ có thú hoang dã (野) chạy ra cánh đồng.",
                ComponentMapJson = @"[]",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "半",
                HanViet = "BÁN",
                Meaning = "Một nửa",
                StrokeCount = 5,
                KunReading = "なか.ba",
                OnReading = "ハン",
                Mnemonic = "Dùng một nhát chém (丨) chia rẽ (丷) mọi thứ ra thành hai (二) phần, tức là chia một nửa (半).",
                ComponentMapJson = @"[]",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems7)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems7 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("97873265-06bf-5583-9426-c0d9429ebc06"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("465ad202-c37c-571a-985c-69c96c90a421"),
                Level = "N3",
                Word = "肉",
                Reading = "にく",
                Meaning = "thịt",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("06084590-1b3f-5674-bb73-59d6dd8587e2"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("465ad202-c37c-571a-985c-69c96c90a421"),
                Level = "N3",
                Word = "牛肉",
                Reading = "ぎゅうにく",
                Meaning = "thịt bò",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9c11ed8c-8145-5f2a-a711-8d3d5af33b4f"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("465ad202-c37c-571a-985c-69c96c90a421"),
                Level = "N3",
                Word = "豚肉",
                Reading = "ぶたにく",
                Meaning = "thịt heo",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1e006f0e-10b0-5e99-a942-e434cedf6f3c"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("465ad202-c37c-571a-985c-69c96c90a421"),
                Level = "N3",
                Word = "鶏肉",
                Reading = "とりにく",
                Meaning = "thịt gà",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("74de8ff4-4705-5c63-bddb-98a187bc291f"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("8dc8753c-940a-55fa-8156-1436615d7e72"),
                Level = "N3",
                Word = "料金",
                Reading = "りょうきん",
                Meaning = "tiền phí",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9c0cf39c-f85e-546b-bd1b-b24ef40006b5"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("8dc8753c-940a-55fa-8156-1436615d7e72"),
                Level = "N3",
                Word = "料理",
                Reading = "りょうり",
                Meaning = "món ăn",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("57f5cc33-682c-5950-b787-8d59818d8b40"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("01b783d3-c933-5a04-aba8-e7af559397e1"),
                Level = "N3",
                Word = "野菜",
                Reading = "やさい",
                Meaning = "rau",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ec302d99-9384-58e1-8636-d145692d35ea"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "半年",
                Reading = "はんとし/はんねん",
                Meaning = "nửa năm",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("67c9f26b-8a93-5ed2-a174-6a49de5641ab"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "半分",
                Reading = "はんぶん",
                Meaning = "Một nửa",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4bdb4f9e-35f6-570d-afc4-088995ab0652"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "半日",
                Reading = "はんじつ/はんにち",
                Meaning = "Nửa ngày",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9eeb45cc-07d3-5851-a691-3c67a37e34f1"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "半月",
                Reading = "はんつき",
                Meaning = "Nửa tháng",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("cbc39499-59a6-5219-8561-fe5eaaa2b137"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "七時半",
                Reading = "しちじはん",
                Meaning = "7h30",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("7dc6cd31-9925-5804-8435-b592928b5fe2"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "大きい",
                Reading = "おおきい",
                Meaning = "to",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("5e359021-838e-57ad-a98b-533873a26101"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "大学生",
                Reading = "だいがくせい",
                Meaning = "Sinh viên đại học",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("746a3f10-add0-5e6d-ad1a-d4d277f59f38"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "大人",
                Reading = "おとな",
                Meaning = "Người lớn",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c538846d-ae24-5e92-8d94-58ee8a6bd64d"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "大会",
                Reading = "たいかい",
                Meaning = "đại hội",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("240d1d5d-b9a0-5ba0-8b6e-1a167a6b8a71"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "大半",
                Reading = "たいはん",
                Meaning = "Phần lớn",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6278ad70-193e-51c6-94d1-d75f75d66f42"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "大学",
                Reading = "だいがく",
                Meaning = "đại học",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("eff532b6-9fc7-5e3a-aa37-50a4d2ab958f"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "小さい",
                Reading = "ちいさい",
                Meaning = "bé, nhỏ",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("00655620-1e0e-5b64-8f5b-aca0a4c81e05"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "小学校",
                Reading = "しょうがっこう",
                Meaning = "Trường tiểu học",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("dd91fb29-edd7-512a-8658-c64426968315"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "小学生",
                Reading = "しょうがくせい",
                Meaning = "học sinh tiểu học",
                OrderIndex = 21,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("594a36d4-9780-598b-aadc-642b55a4a661"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "小人",
                Reading = "こびと",
                Meaning = "đứa trẻ, người tí hon",
                OrderIndex = 22,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems7)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

    }
}