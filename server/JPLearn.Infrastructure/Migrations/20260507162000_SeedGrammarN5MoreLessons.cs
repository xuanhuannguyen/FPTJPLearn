using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260507162000_SeedGrammarN5MoreLessons")]
public partial class SeedGrammarN5MoreLessons : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            INSERT INTO grammar_lessons
                ("Id", "Level", "LessonNumber", "Title", "Description", "AccessTier", "PackageCode", "OrderIndex", "CreatedAt", "UpdatedAt")
            VALUES
                ('11111111-1111-1111-1111-111111111502', 'N5', 2, 'Chỉ thị và sở hữu', 'Các mẫu câu dùng để chỉ người/vật, nói sở hữu và hỏi thông tin cơ bản.', 'free', 'grammar_n5', 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('11111111-1111-1111-1111-111111111503', 'N5', 3, 'Hành động cơ bản', 'Các mẫu câu dùng để nói hành động, địa điểm và thời gian trong câu đơn giản.', 'free', 'grammar_n5', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00');

            INSERT INTO grammar_patterns
                ("Id", "LessonId", "Level", "Pattern", "Title", "Meaning", "Structure", "UsageScope", "Formation", "Notes", "TagsJson", "OrderIndex", "CreatedAt", "UpdatedAt")
            VALUES
                ('22222222-2222-2222-2222-222222222504', '11111111-1111-1111-1111-111111111502', 'N5', 'これ・それ・あれ', 'Từ chỉ vật', 'cái này / cái đó / cái kia', 'これ / それ / あれ + は + N + です', 'Dùng để chỉ đồ vật dựa trên khoảng cách giữa người nói và người nghe.', 'これは本です。', 'これ gần người nói, それ gần người nghe, あれ xa cả hai.', '["demonstrative","noun","basic"]', 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('22222222-2222-2222-2222-222222222505', '11111111-1111-1111-1111-111111111502', 'N5', 'この・その・あの', 'Từ chỉ định bổ nghĩa danh từ', '... này / ... đó / ... kia', 'この / その / あの + N', 'Dùng trước danh từ để chỉ rõ người/vật đang nói tới.', 'この本は新しいです。', 'Khác với これ, các từ này phải đi kèm danh từ phía sau.', '["demonstrative","modifier","noun"]', 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('22222222-2222-2222-2222-222222222506', '11111111-1111-1111-1111-111111111502', 'N5', 'N の N', 'Sở hữu và liên kết danh từ', 'N của N / N thuộc N', 'N1 + の + N2', 'Dùng để nối hai danh từ, thể hiện sở hữu, thuộc tính, nguồn gốc hoặc phân loại.', '私の本', 'の có nhiều nghĩa tùy ngữ cảnh, không chỉ là sở hữu.', '["particle","noun","possession"]', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('22222222-2222-2222-2222-222222222507', '11111111-1111-1111-1111-111111111503', 'N5', 'V ます', 'Động từ lịch sự', 'làm...', 'Verb stem + ます', 'Dùng để nói hành động ở thể lịch sự, thường dùng trong câu khẳng định hiện tại/tương lai.', '行きます / 食べます / 見ます', 'ます không tự biểu thị quá khứ. Quá khứ dùng ました.', '["verb","polite","present"]', 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('22222222-2222-2222-2222-222222222508', '11111111-1111-1111-1111-111111111503', 'N5', '場所 で V', 'Địa điểm diễn ra hành động', 'làm gì ở đâu', 'Place + で + Verb', 'Dùng để nói nơi một hành động xảy ra.', '学校で勉強します。', 'Không dùng で cho nơi tồn tại với あります/います; trường hợp đó thường dùng に.', '["particle","place","verb"]', 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('22222222-2222-2222-2222-222222222509', '11111111-1111-1111-1111-111111111503', 'N5', '時間 に V', 'Thời điểm hành động', 'làm gì vào lúc...', 'Time + に + Verb', 'Dùng với thời điểm cụ thể để nói khi nào hành động xảy ra.', '七時に起きます。', 'Không dùng に với các từ thời gian tương đối như 今日, 明日 khi không cần nhấn mạnh thời điểm.', '["particle","time","verb"]', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00');

            INSERT INTO grammar_examples
                ("Id", "PatternId", "Japanese", "Reading", "Meaning", "Note", "OrderIndex", "CreatedAt", "UpdatedAt")
            VALUES
                ('33333333-3333-3333-3333-333333333507', '22222222-2222-2222-2222-222222222504', 'これは辞書です。', 'これは じしょです。', 'Đây là từ điển.', NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333508', '22222222-2222-2222-2222-222222222504', 'あれは学校です。', 'あれは がっこうです。', 'Kia là trường học.', NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333509', '22222222-2222-2222-2222-222222222505', 'この本は新しいです。', 'このほんは あたらしいです。', 'Quyển sách này mới.', NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333510', '22222222-2222-2222-2222-222222222505', 'その人は先生です。', 'そのひとは せんせいです。', 'Người đó là giáo viên.', NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333511', '22222222-2222-2222-2222-222222222506', 'これは私の本です。', 'これは わたしのほんです。', 'Đây là sách của tôi.', 'Sở hữu.', 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333512', '22222222-2222-2222-2222-222222222506', '日本語の先生です。', 'にほんごの せんせいです。', 'Là giáo viên tiếng Nhật.', 'Phân loại/chuyên môn.', 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333513', '22222222-2222-2222-2222-222222222507', '毎日、日本語を勉強します。', 'まいにち、にほんごを べんきょうします。', 'Mỗi ngày tôi học tiếng Nhật.', NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333514', '22222222-2222-2222-2222-222222222507', '明日、映画を見ます。', 'あした、えいがを みます。', 'Ngày mai tôi xem phim.', NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333515', '22222222-2222-2222-2222-222222222508', '図書館で本を読みます。', 'としょかんで ほんを よみます。', 'Tôi đọc sách ở thư viện.', NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333516', '22222222-2222-2222-2222-222222222508', '学校で勉強します。', 'がっこうで べんきょうします。', 'Tôi học ở trường.', NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333517', '22222222-2222-2222-2222-222222222509', '七時に起きます。', 'しちじに おきます。', 'Tôi thức dậy lúc 7 giờ.', NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333518', '22222222-2222-2222-2222-222222222509', '月曜日に学校へ行きます。', 'げつようびに がっこうへ いきます。', 'Thứ hai tôi đi đến trường.', NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00');

            INSERT INTO grammar_exercises
                ("Id", "PatternId", "ExerciseType", "Prompt", "PromptReading", "ExpectedAnswer", "AcceptableAnswersJson", "Hint", "Explanation", "TemplateText", "OptionsJson", "CorrectOrderJson", "StarPosition", "StarAnswer", "OrderIndex", "CreatedAt", "UpdatedAt")
            VALUES
                ('44444444-4444-4444-4444-444444444510', '22222222-2222-2222-2222-222222222504', 'vi_to_ja', 'Đây là từ điển.', NULL, 'これは辞書です。', '["これは辞書です。","これは辞書です"]', 'Dùng これ cho vật ở gần người nói.', 'これ は N です。', NULL, NULL, NULL, NULL, NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444511', '22222222-2222-2222-2222-222222222504', 'ja_to_vi', 'あれは学校です。', 'あれは がっこうです。', 'Kia là trường học.', '["Kia là trường học.","Đó là trường học."]', 'あれ chỉ vật ở xa cả người nói và người nghe.', 'あれ = cái kia.', NULL, NULL, NULL, NULL, NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444512', '22222222-2222-2222-2222-222222222504', 'arrange', 'Sắp xếp để tạo câu: Đây là từ điển.', NULL, 'これは辞書です。', NULL, 'Mẫu: これは N です。', 'これ dùng độc lập như danh từ.', '____ ____ ★。', '["辞書","これは","です","あれは"]', '["これは","辞書","です"]', 3, 'です', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444513', '22222222-2222-2222-2222-222222222505', 'vi_to_ja', 'Quyển sách này mới.', NULL, 'この本は新しいです。', '["この本は新しいです。","この本は新しいです"]', 'この phải đứng trước danh từ.', 'この + N は Adj です。', NULL, NULL, NULL, NULL, NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444514', '22222222-2222-2222-2222-222222222505', 'ja_to_vi', 'その人は先生です。', 'そのひとは せんせいです。', 'Người đó là giáo viên.', '["Người đó là giáo viên.","Người kia là giáo viên."]', 'その人 = người đó.', 'その bổ nghĩa cho 人.', NULL, NULL, NULL, NULL, NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444515', '22222222-2222-2222-2222-222222222505', 'arrange', 'Sắp xếp để tạo câu: Quyển sách này mới.', NULL, 'この本は新しいです。', NULL, 'この đi liền với 本.', 'Không tách この khỏi danh từ nó bổ nghĩa.', '____ ____ ★ ____。', '["新しい","この本は","です","これは"]', '["この本は","新しい","です"]', 3, 'です', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444516', '22222222-2222-2222-2222-222222222506', 'vi_to_ja', 'Đây là sách của tôi.', NULL, 'これは私の本です。', '["これは私の本です。","これはわたしの本です。"]', '私の本 = sách của tôi.', 'の nối 私 và 本.', NULL, NULL, NULL, NULL, NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444517', '22222222-2222-2222-2222-222222222506', 'ja_to_vi', '日本語の先生です。', 'にほんごの せんせいです。', 'Là giáo viên tiếng Nhật.', '["Là giáo viên tiếng Nhật.","Giáo viên tiếng Nhật."]', '日本語の先生 = giáo viên tiếng Nhật.', 'の thể hiện phân loại/chuyên môn.', NULL, NULL, NULL, NULL, NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444518', '22222222-2222-2222-2222-222222222506', 'arrange', 'Sắp xếp để tạo câu: Đây là sách của tôi.', NULL, 'これは私の本です。', NULL, '私 + の + 本.', 'の đặt giữa hai danh từ.', '____ ____ ★。', '["私の本","これは","です","先生"]', '["これは","私の本","です"]', 3, 'です', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444519', '22222222-2222-2222-2222-222222222507', 'vi_to_ja', 'Mỗi ngày tôi học tiếng Nhật.', NULL, '毎日、日本語を勉強します。', '["毎日、日本語を勉強します。","毎日日本語を勉強します。"]', '勉強します = học.', 'ます dùng cho câu lịch sự.', NULL, NULL, NULL, NULL, NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444520', '22222222-2222-2222-2222-222222222507', 'ja_to_vi', '明日、映画を見ます。', 'あした、えいがを みます。', 'Ngày mai tôi xem phim.', '["Ngày mai tôi xem phim.","Ngày mai xem phim."]', '見ます = xem/nhìn.', '明日 có thể dùng cho tương lai.', NULL, NULL, NULL, NULL, NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444521', '22222222-2222-2222-2222-222222222507', 'arrange', 'Sắp xếp để tạo câu: Tôi học tiếng Nhật.', NULL, '日本語を勉強します。', NULL, 'Danh từ + を + động từ.', '勉強します đặt cuối câu.', '____ ★ ____。', '["日本語を","勉強します","学校で","です"]', '["日本語を","勉強します"]', 2, '勉強します', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444522', '22222222-2222-2222-2222-222222222508', 'vi_to_ja', 'Tôi đọc sách ở thư viện.', NULL, '図書館で本を読みます。', '["図書館で本を読みます。","としょかんで本を読みます。"]', 'Địa điểm + で + hành động.', 'で đánh dấu nơi diễn ra hành động.', NULL, NULL, NULL, NULL, NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444523', '22222222-2222-2222-2222-222222222508', 'ja_to_vi', '学校で勉強します。', 'がっこうで べんきょうします。', 'Tôi học ở trường.', '["Tôi học ở trường.","Học ở trường."]', '学校で = ở trường.', 'で dùng với hành động 勉強します.', NULL, NULL, NULL, NULL, NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444524', '22222222-2222-2222-2222-222222222508', 'arrange', 'Sắp xếp để tạo câu: Tôi học ở trường.', NULL, '学校で勉強します。', NULL, '学校 + で + 勉強します.', 'で đứng sau địa điểm.', '____ ★ ____。', '["勉強します","学校で","本を","です"]', '["学校で","勉強します"]', 2, '勉強します', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444525', '22222222-2222-2222-2222-222222222509', 'vi_to_ja', 'Tôi thức dậy lúc 7 giờ.', NULL, '七時に起きます。', '["七時に起きます。","7時に起きます。"]', 'Thời điểm cụ thể + に.', 'に đánh dấu thời điểm.', NULL, NULL, NULL, NULL, NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444526', '22222222-2222-2222-2222-222222222509', 'ja_to_vi', '月曜日に学校へ行きます。', 'げつようびに がっこうへ いきます。', 'Thứ hai tôi đi đến trường.', '["Thứ hai tôi đi đến trường.","Tôi đi học vào thứ hai."]', '月曜日に = vào thứ hai.', 'に dùng với ngày cụ thể.', NULL, NULL, NULL, NULL, NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444527', '22222222-2222-2222-2222-222222222509', 'arrange', 'Sắp xếp để tạo câu: Tôi thức dậy lúc 7 giờ.', NULL, '七時に起きます。', NULL, '七時 + に + 起きます.', 'に đặt sau thời điểm cụ thể.', '____ ★ ____。', '["起きます","七時に","学校で","です"]', '["七時に","起きます"]', 2, '起きます', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00');
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DELETE FROM grammar_lessons
            WHERE "Id" IN (
                '11111111-1111-1111-1111-111111111502',
                '11111111-1111-1111-1111-111111111503'
            );
            """);
    }
}
