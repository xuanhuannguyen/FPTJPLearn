using JPLearn.Core.Kanji.Entities;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Data.Seed;

public static class KanjiSeedData
{
    private static readonly Guid Lesson2Id = Guid.Parse("33333333-3333-3333-3333-333333333502");
    private static readonly DateTime SeededAt = new(2026, 5, 8, 0, 0, 0, DateTimeKind.Utc);

    public static async Task SeedAsync(AppDbContext db)
    {
        await EnsureLesson2Async(db);
        await db.SaveChangesAsync();
    }

    private static async Task EnsureLesson2Async(AppDbContext db)
    {
        var lessonExists = await db.KanjiLessons.AnyAsync(lesson => lesson.Id == Lesson2Id);
        if (!lessonExists)
        {
            db.KanjiLessons.Add(new KanjiLesson
            {
                Id = Lesson2Id,
                Level = "N5",
                LessonNumber = 2,
                Title = "Ngày tháng và con người",
                Description = "10 Hán tự N5 thường gặp trong ngày tháng, tuần lễ và mô tả người/vật cơ bản.",
                AccessTier = "free",
                PackageCode = "kanji_n5",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            });
        }

        var existingCharacters = await db.KanjiItems
            .Where(item => item.LessonId == Lesson2Id)
            .Select(item => item.Character)
            .ToListAsync();
        var existingCharacterSet = existingCharacters.ToHashSet();

        var kanjiItems = BuildLesson2Kanji();
        db.KanjiItems.AddRange(kanjiItems.Where(item => !existingCharacterSet.Contains(item.Character)));

        var kanjiByCharacter = kanjiItems.ToDictionary(item => item.Character, item => item.Id);
        var existingWords = await db.KanjiVocabularyItems
            .Where(item => item.LessonId == Lesson2Id)
            .Select(item => item.Word)
            .ToListAsync();
        var existingWordSet = existingWords.ToHashSet();

        db.KanjiVocabularyItems.AddRange(BuildLesson2Vocabulary(kanjiByCharacter)
            .Where(item => !existingWordSet.Contains(item.Word)));
    }

    private static List<KanjiItem> BuildLesson2Kanji()
    {
        return
        [
            Kanji("44444444-4444-4444-4444-444444444511", "日", "NHẬT", "ngày; mặt trời", 4, "ひ、か", "ニチ、ジツ", "Mặt trời được vẽ thành khung vuông có một nét ở giữa, dùng để nhớ ngày và ánh sáng.", 1, """{"strokeCount":4,"strokes":[{"order":1,"path":"M48 30 L48 128"},{"order":2,"path":"M48 30 L152 30 L152 128"},{"order":3,"path":"M48 78 L152 78"},{"order":4,"path":"M48 128 L152 128"}]}""", """[{"component":"日","name":"nhật","meaning":"mặt trời/ngày","position":"whole"}]"""),
            Kanji("44444444-4444-4444-4444-444444444512", "月", "NGUYỆT", "tháng; mặt trăng", 4, "つき", "ゲツ、ガツ", "Hình trăng khuyết được giản lược thành thân dọc với hai nét ngang bên trong.", 2, """{"strokeCount":4,"strokes":[{"order":1,"path":"M64 28 L64 136"},{"order":2,"path":"M64 28 L144 28 L144 136"},{"order":3,"path":"M64 72 L144 72"},{"order":4,"path":"M64 106 L144 106"}]}""", """[{"component":"月","name":"nguyệt","meaning":"mặt trăng/tháng","position":"whole"}]"""),
            Kanji("44444444-4444-4444-4444-444444444513", "火", "HỎA", "lửa", 4, "ひ、ほ", "カ", "Hai tia lửa nhỏ hai bên và ngọn lửa lớn ở giữa tạo thành chữ hỏa.", 3, """{"strokeCount":4,"strokes":[{"order":1,"path":"M80 42 L58 76"},{"order":2,"path":"M124 40 L146 74"},{"order":3,"path":"M102 26 L96 84 Q90 118 54 138"},{"order":4,"path":"M104 84 Q116 118 150 138"}]}""", """[{"component":"火","name":"hỏa","meaning":"lửa","position":"whole"}]"""),
            Kanji("44444444-4444-4444-4444-444444444514", "水", "THỦY", "nước", 4, "みず", "スイ", "Nét giữa là dòng nước chính, các nét hai bên như nước bắn ra.", 4, """{"strokeCount":4,"strokes":[{"order":1,"path":"M100 24 L100 140"},{"order":2,"path":"M58 58 L92 82"},{"order":3,"path":"M142 54 L108 86"},{"order":4,"path":"M98 82 Q78 118 48 138"}]}""", """[{"component":"水","name":"thủy","meaning":"nước","position":"whole"}]"""),
            Kanji("44444444-4444-4444-4444-444444444515", "木", "MỘC", "cây", 4, "き、こ", "モク、ボク", "Một thân cây dọc, một cành ngang và hai rễ/cành tỏa xuống.", 5, """{"strokeCount":4,"strokes":[{"order":1,"path":"M100 24 L100 140"},{"order":2,"path":"M44 64 L156 64"},{"order":3,"path":"M98 66 Q78 106 48 134"},{"order":4,"path":"M102 66 Q122 106 152 134"}]}""", """[{"component":"木","name":"mộc","meaning":"cây","position":"whole"}]"""),
            Kanji("44444444-4444-4444-4444-444444444516", "金", "KIM", "vàng; tiền; kim loại", 8, "かね、かな", "キン、コン", "Phần trên như mái che, dưới là đất và hai chấm kim loại lấp lánh.", 6, """{"strokeCount":8,"strokes":[{"order":1,"path":"M100 24 L54 58"},{"order":2,"path":"M100 24 L146 58"},{"order":3,"path":"M58 70 L142 70"},{"order":4,"path":"M76 88 L76 116"},{"order":5,"path":"M124 88 L124 116"},{"order":6,"path":"M54 122 L146 122"},{"order":7,"path":"M78 96 L92 112"},{"order":8,"path":"M122 96 L108 112"}]}""", """[{"component":"人","name":"nhân","meaning":"mái/người phủ lên","position":"top"},{"component":"土","name":"thổ","meaning":"đất","position":"middle"},{"component":"丶","name":"chủ","meaning":"điểm kim loại","position":"bottom"}]"""),
            Kanji("44444444-4444-4444-4444-444444444517", "土", "THỔ", "đất", 3, "つち", "ド、ト", "Cây mọc lên từ mặt đất: nét dọc xuyên qua hai lớp đất.", 7, """{"strokeCount":3,"strokes":[{"order":1,"path":"M100 30 L100 122"},{"order":2,"path":"M58 70 L142 70"},{"order":3,"path":"M42 126 L158 126"}]}""", """[{"component":"土","name":"thổ","meaning":"đất","position":"whole"}]"""),
            Kanji("44444444-4444-4444-4444-444444444518", "人", "NHÂN", "người", 2, "ひと", "ジン、ニン", "Hai nét như dáng người đang đứng và bước đi.", 8, """{"strokeCount":2,"strokes":[{"order":1,"path":"M96 32 Q88 82 54 134"},{"order":2,"path":"M100 66 Q120 108 150 134"}]}""", """[{"component":"人","name":"nhân","meaning":"người","position":"whole"}]"""),
            Kanji("44444444-4444-4444-4444-444444444519", "大", "ĐẠI", "to; lớn", 3, "おお、 おお.きい", "ダイ、タイ", "Người dang rộng tay chân để biểu thị sự to lớn.", 9, """{"strokeCount":3,"strokes":[{"order":1,"path":"M46 66 L154 66"},{"order":2,"path":"M100 28 L92 82 Q80 116 48 138"},{"order":3,"path":"M102 82 Q122 118 154 138"}]}""", """[{"component":"人","name":"nhân","meaning":"người","position":"base"},{"component":"一","name":"nhất","meaning":"tay dang ngang","position":"top"}]"""),
            Kanji("44444444-4444-4444-4444-444444444520", "小", "TIỂU", "nhỏ", 3, "ちい.さい、こ、お", "ショウ", "Một nét chính nhỏ ở giữa, hai chấm hai bên như những phần nhỏ tách ra.", 10, """{"strokeCount":3,"strokes":[{"order":1,"path":"M100 30 L100 136"},{"order":2,"path":"M68 72 L42 112"},{"order":3,"path":"M132 72 L158 112"}]}""", """[{"component":"小","name":"tiểu","meaning":"nhỏ","position":"whole"}]""")
        ];
    }

    private static List<KanjiVocabulary> BuildLesson2Vocabulary(Dictionary<string, Guid> kanjiByCharacter)
    {
        return
        [
            Vocabulary("55555555-5555-5555-5555-555555555516", "日", "日本", "にほん", "Nhật Bản", "日本へ行きます。", "にほんへ いきます。", "Tôi đi Nhật Bản.", 1),
            Vocabulary("55555555-5555-5555-5555-555555555517", "日", "日曜日", "にちようび", "chủ nhật", "日曜日に休みます。", "にちようびに やすみます。", "Tôi nghỉ vào chủ nhật.", 2),
            Vocabulary("55555555-5555-5555-5555-555555555518", "月", "月曜日", "げつようび", "thứ hai", "月曜日にテストがあります。", "げつようびに テストが あります。", "Thứ hai có bài kiểm tra.", 3),
            Vocabulary("55555555-5555-5555-5555-555555555519", "月", "一月", "いちがつ", "tháng một", "一月は寒いです。", "いちがつは さむいです。", "Tháng một thì lạnh.", 4),
            Vocabulary("55555555-5555-5555-5555-555555555520", "火", "火曜日", "かようび", "thứ ba", "火曜日に会いましょう。", "かようびに あいましょう。", "Hãy gặp nhau vào thứ ba.", 5),
            Vocabulary("55555555-5555-5555-5555-555555555521", "火", "花火", "はなび", "pháo hoa", "花火を見ました。", "はなびを みました。", "Tôi đã xem pháo hoa.", 6),
            Vocabulary("55555555-5555-5555-5555-555555555522", "水", "水曜日", "すいようび", "thứ tư", "水曜日は忙しいです。", "すいようびは いそがしいです。", "Thứ tư thì bận.", 7),
            Vocabulary("55555555-5555-5555-5555-555555555523", "水", "水", "みず", "nước", "水を飲みます。", "みずを のみます。", "Tôi uống nước.", 8),
            Vocabulary("55555555-5555-5555-5555-555555555524", "木", "木曜日", "もくようび", "thứ năm", "木曜日に図書館へ行きます。", "もくようびに としょかんへ いきます。", "Thứ năm tôi đi thư viện.", 9),
            Vocabulary("55555555-5555-5555-5555-555555555525", "木", "木", "き", "cây", "大きい木があります。", "おおきい きが あります。", "Có một cái cây lớn.", 10),
            Vocabulary("55555555-5555-5555-5555-555555555526", "金", "金曜日", "きんようび", "thứ sáu", "金曜日に映画を見ます。", "きんようびに えいがを みます。", "Thứ sáu tôi xem phim.", 11),
            Vocabulary("55555555-5555-5555-5555-555555555527", "土", "土曜日", "どようび", "thứ bảy", "土曜日に買い物します。", "どようびに かいものします。", "Thứ bảy tôi đi mua sắm.", 12),
            Vocabulary("55555555-5555-5555-5555-555555555528", "人", "日本人", "にほんじん", "người Nhật", "田中さんは日本人です。", "たなかさんは にほんじんです。", "Anh Tanaka là người Nhật.", 13),
            Vocabulary("55555555-5555-5555-5555-555555555529", "大", "大学", "だいがく", "đại học", "大学で日本語を勉強します。", "だいがくで にほんごを べんきょうします。", "Tôi học tiếng Nhật ở đại học.", 14),
            Vocabulary("55555555-5555-5555-5555-555555555530", "小", "小さい", "ちいさい", "nhỏ", "小さいかばんを買いました。", "ちいさい かばんを かいました。", "Tôi đã mua một cái cặp nhỏ.", 15)
        ];

        KanjiVocabulary Vocabulary(
            string id,
            string kanji,
            string word,
            string reading,
            string meaning,
            string exampleJapanese,
            string exampleReading,
            string exampleMeaning,
            int orderIndex)
        {
            return new KanjiVocabulary
            {
                Id = Guid.Parse(id),
                LessonId = Lesson2Id,
                KanjiItemId = kanjiByCharacter[kanji],
                Level = "N5",
                Word = word,
                Reading = reading,
                Meaning = meaning,
                ExampleJapanese = exampleJapanese,
                ExampleReading = exampleReading,
                ExampleMeaning = exampleMeaning,
                OrderIndex = orderIndex,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            };
        }
    }

    private static KanjiItem Kanji(
        string id,
        string character,
        string hanViet,
        string meaning,
        int strokeCount,
        string kunReading,
        string onReading,
        string mnemonic,
        int orderIndex,
        string strokeDataJson,
        string componentMapJson)
    {
        return new KanjiItem
        {
            Id = Guid.Parse(id),
            LessonId = Lesson2Id,
            Level = "N5",
            Character = character,
            HanViet = hanViet,
            Meaning = meaning,
            StrokeCount = strokeCount,
            KunReading = kunReading,
            OnReading = onReading,
            Mnemonic = mnemonic,
            StrokeDataJson = strokeDataJson,
            ComponentMapJson = componentMapJson,
            OrderIndex = orderIndex,
            CreatedAt = SeededAt,
            UpdatedAt = SeededAt
        };
    }
}
