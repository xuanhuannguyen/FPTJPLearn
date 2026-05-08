using System.Text;
using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable enable

namespace JPLearn.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260508103000_SeedGrammarExerciseExpansion")]
public partial class SeedGrammarExerciseExpansion : Migration
{
    private const string CreatedAt = "2026-05-08 00:00:00+00";

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        var rows = BuildSeedRows();
        migrationBuilder.Sql(BuildInsertSql(rows));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        var ids = BuildSeedRows()
            .Select(row => $"'{row.Id}'")
            .ToArray();

        migrationBuilder.Sql(
            $"""
            DELETE FROM grammar_exercises
            WHERE "Id" IN ({string.Join(", ", ids)});
            """);
    }

    private static List<ExerciseRow> BuildSeedRows()
    {
        var rows = new List<ExerciseRow>();
        var nextId = 1;

        foreach (var seed in PatternSeeds)
        {
            AddTextExercises(rows, ref nextId, seed.PatternId, "vi_to_ja", seed.VietnameseToJapanese, seed.BaseHint, seed.BaseExplanation);
            AddTextExercises(rows, ref nextId, seed.PatternId, "ja_to_vi", seed.JapaneseToVietnamese, seed.BaseHint, seed.BaseExplanation);
            AddArrangeExercises(rows, ref nextId, seed.PatternId, seed.Arrange, seed.BaseHint, seed.BaseExplanation);
        }

        return rows;
    }

    private static void AddTextExercises(
        List<ExerciseRow> rows,
        ref int nextId,
        string patternId,
        string exerciseType,
        IReadOnlyList<TextExerciseSeed> exercises,
        string hint,
        string explanation)
    {
        for (var index = 0; index < exercises.Count; index++)
        {
            var exercise = exercises[index];
            rows.Add(new ExerciseRow(
                NextId(ref nextId),
                patternId,
                exerciseType,
                exercise.Prompt,
                exercise.PromptReading,
                exercise.ExpectedAnswer,
                ToJsonArray(exercise.AcceptableAnswers.Append(exercise.ExpectedAnswer)),
                exercise.Hint ?? hint,
                exercise.Explanation ?? explanation,
                null,
                null,
                null,
                null,
                null,
                index + 2));
        }
    }

    private static void AddArrangeExercises(
        List<ExerciseRow> rows,
        ref int nextId,
        string patternId,
        IReadOnlyList<ArrangeExerciseSeed> exercises,
        string hint,
        string explanation)
    {
        for (var index = 0; index < exercises.Count; index++)
        {
            var exercise = exercises[index];
            rows.Add(new ExerciseRow(
                NextId(ref nextId),
                patternId,
                "arrange",
                exercise.Prompt,
                null,
                exercise.ExpectedAnswer,
                null,
                exercise.Hint ?? hint,
                exercise.Explanation ?? explanation,
                exercise.TemplateText,
                ToJsonArray(exercise.Options),
                ToJsonArray(exercise.CorrectOrder),
                exercise.StarPosition,
                exercise.StarAnswer,
                index + 2));
        }
    }

    private static string BuildInsertSql(IReadOnlyList<ExerciseRow> rows)
    {
        var builder = new StringBuilder();
        builder.AppendLine(
            """
            INSERT INTO grammar_exercises
                ("Id", "PatternId", "ExerciseType", "Prompt", "PromptReading", "ExpectedAnswer", "AcceptableAnswersJson", "Hint", "Explanation", "TemplateText", "OptionsJson", "CorrectOrderJson", "StarPosition", "StarAnswer", "OrderIndex", "CreatedAt", "UpdatedAt")
            VALUES
            """);

        for (var index = 0; index < rows.Count; index++)
        {
            var row = rows[index];
            builder.Append("    (");
            builder.Append(Sql(row.Id)).Append(", ");
            builder.Append(Sql(row.PatternId)).Append(", ");
            builder.Append(Sql(row.ExerciseType)).Append(", ");
            builder.Append(Sql(row.Prompt)).Append(", ");
            builder.Append(Sql(row.PromptReading)).Append(", ");
            builder.Append(Sql(row.ExpectedAnswer)).Append(", ");
            builder.Append(Sql(row.AcceptableAnswersJson)).Append(", ");
            builder.Append(Sql(row.Hint)).Append(", ");
            builder.Append(Sql(row.Explanation)).Append(", ");
            builder.Append(Sql(row.TemplateText)).Append(", ");
            builder.Append(Sql(row.OptionsJson)).Append(", ");
            builder.Append(Sql(row.CorrectOrderJson)).Append(", ");
            builder.Append(row.StarPosition.HasValue ? row.StarPosition.Value.ToString() : "NULL").Append(", ");
            builder.Append(Sql(row.StarAnswer)).Append(", ");
            builder.Append(row.OrderIndex).Append(", ");
            builder.Append("TIMESTAMPTZ ").Append(Sql(CreatedAt)).Append(", ");
            builder.Append("TIMESTAMPTZ ").Append(Sql(CreatedAt)).Append(')');
            builder.AppendLine(index == rows.Count - 1 ? string.Empty : ",");
        }

        builder.AppendLine("ON CONFLICT (\"Id\") DO NOTHING;");
        return builder.ToString();
    }

    private static string NextId(ref int nextId)
    {
        var id = $"55555555-5555-5555-5555-{nextId:000000000000}";
        nextId++;
        return id;
    }

    private static string? ToJsonArray(IEnumerable<string> values)
    {
        var escaped = values
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => $"\"{value.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"");

        return $"[{string.Join(",", escaped)}]";
    }

    private static string Sql(string? value)
    {
        return value == null ? "NULL" : $"'{value.Replace("'", "''")}'";
    }

    private static readonly PatternExerciseSeed[] PatternSeeds =
    [
        new(
            "22222222-2222-2222-2222-222222222501",
            "Dùng mẫu N は N です。",
            "Câu giới thiệu hoặc xác nhận danh tính lịch sự.",
            [
                new("Tôi là học sinh.", "私は学生です。", ["私は学生です"]),
                new("Anh Tanaka là giáo viên.", "田中さんは先生です。", ["田中さんは先生です"]),
                new("Đây là sách.", "これは本です。", ["これは本です"]),
                new("Kia là trường học.", "あれは学校です。", ["あれは学校です"])
            ],
            [
                new("田中さんは先生です。", "たなかさんは せんせいです。", "Anh/chị Tanaka là giáo viên.", ["Tanaka là giáo viên.", "Anh Tanaka là giáo viên."]),
                new("これは本です。", "これは ほんです。", "Đây là sách.", ["Cái này là sách."]),
                new("あれは学校です。", "あれは がっこうです。", "Kia là trường học.", ["Đó là trường học."]),
                new("私は会社員です。", "わたしは かいしゃいんです。", "Tôi là nhân viên công ty.", ["Tôi là nhân viên."])
            ],
            [
                new("Sắp xếp để tạo câu: Tôi là học sinh.", "私は学生です。", ["私は", "学生", "です"], ["です", "学生", "私は", "先生"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Đây là sách.", "これは本です。", ["これは", "本", "です"], ["本", "これは", "です", "学校"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Tanaka là giáo viên.", "田中さんは先生です。", ["田中さんは", "先生", "です"], ["です", "田中さんは", "学生", "先生"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Kia là trường học.", "あれは学校です。", ["あれは", "学校", "です"], ["学校", "これ", "です", "あれは"], "____ ____ ★。", 3, "です")
            ]),
        new(
            "22222222-2222-2222-2222-222222222502",
            "Dùng N は N ではありません。",
            "ではありません là phủ định lịch sự của câu danh từ.",
            [
                new("Tôi không phải là học sinh.", "私は学生ではありません。", ["私は学生ではありません"]),
                new("Đây không phải là sách.", "これは本ではありません。", ["これは本ではありません"]),
                new("Anh Tanaka không phải là bác sĩ.", "田中さんは医者ではありません。", ["田中さんは医者ではありません"]),
                new("Đó không phải là trường học.", "それは学校ではありません。", ["それは学校ではありません"])
            ],
            [
                new("私は先生ではありません。", "わたしは せんせいではありません。", "Tôi không phải là giáo viên.", ["Tôi không phải giáo viên."]),
                new("これは本ではありません。", "これは ほんではありません。", "Đây không phải là sách.", ["Cái này không phải là sách."]),
                new("田中さんは医者ではありません。", "たなかさんは いしゃではありません。", "Anh/chị Tanaka không phải là bác sĩ.", ["Tanaka không phải là bác sĩ."]),
                new("それは学校ではありません。", "それは がっこうではありません。", "Đó không phải là trường học.", ["Cái đó không phải là trường học."])
            ],
            [
                new("Sắp xếp để tạo câu: Tôi không phải là học sinh.", "私は学生ではありません。", ["私は", "学生", "ではありません"], ["学生", "ではありません", "私は", "先生"], "____ ____ ★。", 3, "ではありません"),
                new("Sắp xếp để tạo câu: Đây không phải là sách.", "これは本ではありません。", ["これは", "本", "ではありません"], ["本", "これは", "です", "ではありません"], "____ ____ ★。", 3, "ではありません"),
                new("Sắp xếp để tạo câu: Tanaka không phải là bác sĩ.", "田中さんは医者ではありません。", ["田中さんは", "医者", "ではありません"], ["医者", "田中さんは", "ではありません", "学生"], "____ ____ ★。", 3, "ではありません"),
                new("Sắp xếp để tạo câu: Đó không phải là trường học.", "それは学校ではありません。", ["それは", "学校", "ではありません"], ["学校", "それは", "ではありません", "あれ"], "____ ____ ★。", 3, "ではありません")
            ]),
        new(
            "22222222-2222-2222-2222-222222222503",
            "Dùng địa điểm + に + vật/sự việc + があります。",
            "あります dùng cho đồ vật, sự việc hoặc khái niệm không sống.",
            [
                new("Có một cái ghế trong phòng.", "部屋に椅子があります。", ["へやに椅子があります。", "部屋にいすがあります。"]),
                new("Có ngân hàng ở gần nhà ga.", "駅の近くに銀行があります。", ["駅の近くに銀行があります"]),
                new("Có bài kiểm tra vào ngày mai.", "明日、テストがあります。", ["明日テストがあります。"]),
                new("Có sách trên bàn.", "机の上に本があります。", ["机の上に本があります"])
            ],
            [
                new("部屋に椅子があります。", "へやに いすがあります。", "Có một cái ghế trong phòng.", ["Trong phòng có ghế."]),
                new("駅の近くに銀行があります。", "えきのちかくに ぎんこうがあります。", "Có ngân hàng ở gần nhà ga.", ["Gần ga có ngân hàng."]),
                new("明日、テストがあります。", "あした、テストがあります。", "Ngày mai có bài kiểm tra.", ["Ngày mai có kiểm tra."]),
                new("机の上に本があります。", "つくえのうえに ほんがあります。", "Có sách trên bàn.", ["Trên bàn có sách."])
            ],
            [
                new("Sắp xếp để tạo câu: Có ghế trong phòng.", "部屋に椅子があります。", ["部屋に", "椅子が", "あります"], ["椅子が", "あります", "部屋に", "です"], "____ ____ ★。", 3, "あります"),
                new("Sắp xếp để tạo câu: Có ngân hàng gần nhà ga.", "駅の近くに銀行があります。", ["駅の近くに", "銀行が", "あります"], ["銀行が", "駅の近くに", "あります", "行きます"], "____ ____ ★。", 3, "あります"),
                new("Sắp xếp để tạo câu: Ngày mai có kiểm tra.", "明日、テストがあります。", ["明日", "テストが", "あります"], ["あります", "明日", "テストが", "学校"], "____ ____ ★。", 3, "あります"),
                new("Sắp xếp để tạo câu: Có sách trên bàn.", "机の上に本があります。", ["机の上に", "本が", "あります"], ["本が", "机の上に", "あります", "先生"], "____ ____ ★。", 3, "あります")
            ]),
        new(
            "22222222-2222-2222-2222-222222222504",
            "Chọn これ, それ, あれ theo khoảng cách.",
            "これ gần người nói, それ gần người nghe, あれ xa cả hai.",
            [
                new("Đây là máy ảnh.", "これはカメラです。", ["これはカメラです"]),
                new("Đó là từ điển.", "それは辞書です。", ["それは辞書です"]),
                new("Kia là thư viện.", "あれは図書館です。", ["あれは図書館です"]),
                new("Đây là ô.", "これは傘です。", ["これは傘です", "これはかさです"])
            ],
            [
                new("これはカメラです。", "これは カメラです。", "Đây là máy ảnh.", ["Cái này là máy ảnh."]),
                new("それは辞書です。", "それは じしょです。", "Đó là từ điển.", ["Cái đó là từ điển."]),
                new("あれは図書館です。", "あれは としょかんです。", "Kia là thư viện.", ["Đó là thư viện ở xa."]),
                new("これは傘です。", "これは かさです。", "Đây là ô.", ["Cái này là ô."])
            ],
            [
                new("Sắp xếp để tạo câu: Đây là máy ảnh.", "これはカメラです。", ["これは", "カメラ", "です"], ["カメラ", "これは", "です", "それは"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Đó là từ điển.", "それは辞書です。", ["それは", "辞書", "です"], ["辞書", "あれは", "です", "それは"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Kia là thư viện.", "あれは図書館です。", ["あれは", "図書館", "です"], ["図書館", "これは", "あれは", "です"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Đây là ô.", "これは傘です。", ["これは", "傘", "です"], ["傘", "それは", "です", "これは"], "____ ____ ★。", 3, "です")
            ]),
        new(
            "22222222-2222-2222-2222-222222222505",
            "この/その/あの phải đứng trước danh từ.",
            "Các từ chỉ định này bổ nghĩa trực tiếp cho danh từ phía sau.",
            [
                new("Quyển sách này thú vị.", "この本はおもしろいです。", ["この本は面白いです。", "この本はおもしろいです"]),
                new("Người đó là giáo viên.", "その人は先生です。", ["その人は先生です"]),
                new("Tòa nhà kia là trường học.", "あの建物は学校です。", ["あの建物は学校です"]),
                new("Cái túi này mới.", "このかばんは新しいです。", ["この鞄は新しいです。", "このかばんは新しいです"])
            ],
            [
                new("この本はおもしろいです。", "このほんは おもしろいです。", "Quyển sách này thú vị.", ["Sách này thú vị."]),
                new("その人は先生です。", "そのひとは せんせいです。", "Người đó là giáo viên.", ["Người kia là giáo viên."]),
                new("あの建物は学校です。", "あのたてものは がっこうです。", "Tòa nhà kia là trường học.", ["Tòa nhà đó là trường học."]),
                new("このかばんは新しいです。", "このかばんは あたらしいです。", "Cái túi này mới.", ["Chiếc cặp này mới."])
            ],
            [
                new("Sắp xếp để tạo câu: Quyển sách này thú vị.", "この本はおもしろいです。", ["この本は", "おもしろい", "です"], ["この本は", "です", "おもしろい", "それ"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Người đó là giáo viên.", "その人は先生です。", ["その人は", "先生", "です"], ["先生", "その人は", "です", "この"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Tòa nhà kia là trường học.", "あの建物は学校です。", ["あの建物は", "学校", "です"], ["学校", "あの建物は", "です", "この本"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Cái túi này mới.", "このかばんは新しいです。", ["このかばんは", "新しい", "です"], ["新しい", "このかばんは", "です", "その人"], "____ ____ ★。", 3, "です")
            ]),
        new(
            "22222222-2222-2222-2222-222222222506",
            "の nối hai danh từ.",
            "の thể hiện sở hữu, thuộc tính, nguồn gốc hoặc phân loại.",
            [
                new("Đây là xe của tôi.", "これは私の車です。", ["これは私の車です", "これはわたしの車です"]),
                new("Anh ấy là giáo viên tiếng Nhật.", "彼は日本語の先生です。", ["彼は日本語の先生です"]),
                new("Đây là sách của Tanaka.", "これは田中さんの本です。", ["これは田中さんの本です"]),
                new("Tôi là sinh viên của trường đại học.", "私は大学の学生です。", ["私は大学の学生です"])
            ],
            [
                new("これは私の車です。", "これは わたしのくるまです。", "Đây là xe của tôi.", ["Cái này là xe của tôi."]),
                new("彼は日本語の先生です。", "かれは にほんごのせんせいです。", "Anh ấy là giáo viên tiếng Nhật.", ["Anh ấy là thầy giáo tiếng Nhật."]),
                new("これは田中さんの本です。", "これは たなかさんのほんです。", "Đây là sách của Tanaka.", ["Đây là quyển sách của Tanaka."]),
                new("私は大学の学生です。", "わたしは だいがくのがくせいです。", "Tôi là sinh viên đại học.", ["Tôi là sinh viên của trường đại học."])
            ],
            [
                new("Sắp xếp để tạo câu: Đây là xe của tôi.", "これは私の車です。", ["これは", "私の車", "です"], ["私の車", "これは", "です", "先生"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Giáo viên tiếng Nhật.", "日本語の先生です。", ["日本語の", "先生", "です"], ["先生", "日本語の", "です", "本"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Sách của Tanaka.", "田中さんの本です。", ["田中さんの", "本", "です"], ["本", "田中さんの", "です", "車"], "____ ____ ★。", 3, "です"),
                new("Sắp xếp để tạo câu: Sinh viên đại học.", "大学の学生です。", ["大学の", "学生", "です"], ["学生", "大学の", "です", "先生"], "____ ____ ★。", 3, "です")
            ]),
        new(
            "22222222-2222-2222-2222-222222222507",
            "Động từ lịch sự thường kết thúc bằng ます.",
            "V ます dùng cho hiện tại hoặc tương lai trong văn phong lịch sự.",
            [
                new("Tôi đi đến trường.", "学校へ行きます。", ["学校へ行きます", "学校に行きます"]),
                new("Tôi ăn cơm.", "ご飯を食べます。", ["ご飯を食べます", "ごはんを食べます"]),
                new("Tôi xem phim.", "映画を見ます。", ["映画を見ます"]),
                new("Tôi uống nước.", "水を飲みます。", ["水を飲みます"])
            ],
            [
                new("学校へ行きます。", "がっこうへ いきます。", "Tôi đi đến trường.", ["Đi đến trường."]),
                new("ご飯を食べます。", "ごはんを たべます。", "Tôi ăn cơm.", ["Ăn cơm."]),
                new("映画を見ます。", "えいがを みます。", "Tôi xem phim.", ["Xem phim."]),
                new("水を飲みます。", "みずを のみます。", "Tôi uống nước.", ["Uống nước."])
            ],
            [
                new("Sắp xếp để tạo câu: Tôi đi đến trường.", "学校へ行きます。", ["学校へ", "行きます"], ["行きます", "学校へ", "です", "本"], "____ ★。", 2, "行きます"),
                new("Sắp xếp để tạo câu: Tôi ăn cơm.", "ご飯を食べます。", ["ご飯を", "食べます"], ["食べます", "ご飯を", "学校で", "です"], "____ ★。", 2, "食べます"),
                new("Sắp xếp để tạo câu: Tôi xem phim.", "映画を見ます。", ["映画を", "見ます"], ["映画を", "見ます", "飲みます", "です"], "____ ★。", 2, "見ます"),
                new("Sắp xếp để tạo câu: Tôi uống nước.", "水を飲みます。", ["水を", "飲みます"], ["水を", "食べます", "飲みます", "本"], "____ ★。", 2, "飲みます")
            ]),
        new(
            "22222222-2222-2222-2222-222222222508",
            "Địa điểm + で + hành động.",
            "で đánh dấu nơi hành động xảy ra.",
            [
                new("Tôi học ở thư viện.", "図書館で勉強します。", ["図書館で勉強します"]),
                new("Tôi ăn cơm ở nhà.", "家でご飯を食べます。", ["家でご飯を食べます", "うちでご飯を食べます"]),
                new("Tôi xem phim ở rạp chiếu phim.", "映画館で映画を見ます。", ["映画館で映画を見ます"]),
                new("Tôi mua sách ở hiệu sách.", "本屋で本を買います。", ["本屋で本を買います"])
            ],
            [
                new("図書館で勉強します。", "としょかんで べんきょうします。", "Tôi học ở thư viện.", ["Học ở thư viện."]),
                new("家でご飯を食べます。", "いえで ごはんをたべます。", "Tôi ăn cơm ở nhà.", ["Ăn cơm ở nhà."]),
                new("映画館で映画を見ます。", "えいがかんで えいがをみます。", "Tôi xem phim ở rạp chiếu phim.", ["Xem phim ở rạp."]),
                new("本屋で本を買います。", "ほんやで ほんをかいます。", "Tôi mua sách ở hiệu sách.", ["Mua sách ở nhà sách."])
            ],
            [
                new("Sắp xếp để tạo câu: Tôi học ở thư viện.", "図書館で勉強します。", ["図書館で", "勉強します"], ["勉強します", "図書館で", "本を", "です"], "____ ★。", 2, "勉強します"),
                new("Sắp xếp để tạo câu: Tôi ăn cơm ở nhà.", "家でご飯を食べます。", ["家で", "ご飯を", "食べます"], ["ご飯を", "家で", "食べます", "あります"], "____ ____ ★。", 3, "食べます"),
                new("Sắp xếp để tạo câu: Tôi xem phim ở rạp.", "映画館で映画を見ます。", ["映画館で", "映画を", "見ます"], ["映画を", "見ます", "映画館で", "です"], "____ ____ ★。", 3, "見ます"),
                new("Sắp xếp để tạo câu: Tôi mua sách ở hiệu sách.", "本屋で本を買います。", ["本屋で", "本を", "買います"], ["本屋で", "本を", "買います", "あります"], "____ ____ ★。", 3, "買います")
            ]),
        new(
            "22222222-2222-2222-2222-222222222509",
            "Thời điểm cụ thể + に + hành động.",
            "に đánh dấu thời điểm cụ thể của hành động.",
            [
                new("Tôi ngủ lúc 11 giờ.", "十一時に寝ます。", ["十一時に寝ます", "11時に寝ます"]),
                new("Tôi đi đến trường vào thứ hai.", "月曜日に学校へ行きます。", ["月曜日に学校へ行きます", "月曜日に学校に行きます"]),
                new("Tôi dậy lúc 6 giờ.", "六時に起きます。", ["六時に起きます", "6時に起きます"]),
                new("Tôi gặp bạn vào Chủ nhật.", "日曜日に友達に会います。", ["日曜日に友達に会います"])
            ],
            [
                new("十一時に寝ます。", "じゅういちじに ねます。", "Tôi ngủ lúc 11 giờ.", ["Ngủ lúc 11 giờ."]),
                new("月曜日に学校へ行きます。", "げつようびに がっこうへいきます。", "Tôi đi đến trường vào thứ hai.", ["Thứ hai tôi đi học."]),
                new("六時に起きます。", "ろくじに おきます。", "Tôi dậy lúc 6 giờ.", ["Thức dậy lúc 6 giờ."]),
                new("日曜日に友達に会います。", "にちようびに ともだちにあいます。", "Tôi gặp bạn vào Chủ nhật.", ["Chủ nhật tôi gặp bạn."])
            ],
            [
                new("Sắp xếp để tạo câu: Tôi ngủ lúc 11 giờ.", "十一時に寝ます。", ["十一時に", "寝ます"], ["寝ます", "十一時に", "学校で", "です"], "____ ★。", 2, "寝ます"),
                new("Sắp xếp để tạo câu: Tôi đi học vào thứ hai.", "月曜日に学校へ行きます。", ["月曜日に", "学校へ", "行きます"], ["学校へ", "月曜日に", "行きます", "あります"], "____ ____ ★。", 3, "行きます"),
                new("Sắp xếp để tạo câu: Tôi dậy lúc 6 giờ.", "六時に起きます。", ["六時に", "起きます"], ["起きます", "六時に", "本を", "です"], "____ ★。", 2, "起きます"),
                new("Sắp xếp để tạo câu: Tôi gặp bạn vào Chủ nhật.", "日曜日に友達に会います。", ["日曜日に", "友達に", "会います"], ["友達に", "日曜日に", "会います", "学校で"], "____ ____ ★。", 3, "会います")
            ])
    ];

    private sealed record PatternExerciseSeed(
        string PatternId,
        string BaseHint,
        string BaseExplanation,
        IReadOnlyList<TextExerciseSeed> VietnameseToJapanese,
        IReadOnlyList<TextExerciseSeed> JapaneseToVietnamese,
        IReadOnlyList<ArrangeExerciseSeed> Arrange);

    private sealed class TextExerciseSeed
    {
        public TextExerciseSeed(string prompt, string expectedAnswer, IReadOnlyList<string> acceptableAnswers)
            : this(prompt, null, expectedAnswer, acceptableAnswers, null, null)
        {
        }

        public TextExerciseSeed(
            string prompt,
            string? promptReading,
            string expectedAnswer,
            IReadOnlyList<string> acceptableAnswers,
            string? hint = null,
            string? explanation = null)
        {
            Prompt = prompt;
            PromptReading = promptReading;
            ExpectedAnswer = expectedAnswer;
            AcceptableAnswers = acceptableAnswers;
            Hint = hint;
            Explanation = explanation;
        }

        public string Prompt { get; }
        public string? PromptReading { get; }
        public string ExpectedAnswer { get; }
        public IReadOnlyList<string> AcceptableAnswers { get; }
        public string? Hint { get; }
        public string? Explanation { get; }
    }

    private sealed record ArrangeExerciseSeed(
        string Prompt,
        string ExpectedAnswer,
        IReadOnlyList<string> CorrectOrder,
        IReadOnlyList<string> Options,
        string TemplateText,
        int StarPosition,
        string StarAnswer,
        string? Hint = null,
        string? Explanation = null);

    private sealed record ExerciseRow(
        string Id,
        string PatternId,
        string ExerciseType,
        string Prompt,
        string? PromptReading,
        string ExpectedAnswer,
        string? AcceptableAnswersJson,
        string Hint,
        string Explanation,
        string? TemplateText,
        string? OptionsJson,
        string? CorrectOrderJson,
        int? StarPosition,
        string? StarAnswer,
        int OrderIndex);
}
