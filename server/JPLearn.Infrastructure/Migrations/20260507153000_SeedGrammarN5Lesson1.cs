using JPLearn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260507153000_SeedGrammarN5Lesson1")]
public partial class SeedGrammarN5Lesson1 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            INSERT INTO grammar_lessons
                ("Id", "Level", "LessonNumber", "Title", "Description", "AccessTier", "PackageCode", "OrderIndex", "CreatedAt", "UpdatedAt")
            VALUES
                ('11111111-1111-1111-1111-111111111501', 'N5', 1, 'Câu cơ bản', 'Các mẫu câu nền tảng để giới thiệu, phủ định và nói về sự tồn tại.', 'free', 'grammar_n5', 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00');

            INSERT INTO grammar_patterns
                ("Id", "LessonId", "Level", "Pattern", "Title", "Meaning", "Structure", "UsageScope", "Formation", "Notes", "TagsJson", "OrderIndex", "CreatedAt", "UpdatedAt")
            VALUES
                ('22222222-2222-2222-2222-222222222501', '11111111-1111-1111-1111-111111111501', 'N5', '〜です', 'Câu khẳng định lịch sự', 'là...', 'Noun + です', 'Dùng để nói A là B một cách lịch sự. Thường dùng trong giới thiệu, mô tả danh tính, nghề nghiệp hoặc trạng thái.', '学生 + です = 学生です', 'Không dùng trực tiếp sau động từ. Với tính từ đuôi な và danh từ, です tạo sắc thái lịch sự.', '["noun","basic","polite"]', 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('22222222-2222-2222-2222-222222222502', '11111111-1111-1111-1111-111111111501', 'N5', '〜ではありません', 'Câu phủ định lịch sự', 'không phải là...', 'Noun + ではありません', 'Dùng để phủ định câu danh từ một cách lịch sự.', '先生 + ではありません = 先生ではありません', 'Trong hội thoại thân mật có thể dùng じゃありません hoặc じゃないです.', '["noun","negative","polite"]', 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('22222222-2222-2222-2222-222222222503', '11111111-1111-1111-1111-111111111501', 'N5', '〜があります', 'Có, tồn tại', 'có...', 'Noun + があります', 'Dùng để nói sự tồn tại của đồ vật, sự việc, thời gian hoặc khái niệm không sống.', '本 + があります = 本があります', 'Với người và động vật thường dùng います thay vì あります.', '["existence","particle","basic"]', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00');

            INSERT INTO grammar_examples
                ("Id", "PatternId", "Japanese", "Reading", "Meaning", "Note", "OrderIndex", "CreatedAt", "UpdatedAt")
            VALUES
                ('33333333-3333-3333-3333-333333333501', '22222222-2222-2222-2222-222222222501', '私は学生です。', 'わたしは がくせいです。', 'Tôi là học sinh.', 'Câu giới thiệu cơ bản.', 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333502', '22222222-2222-2222-2222-222222222501', '田中さんは先生です。', 'たなかさんは せんせいです。', 'Anh/chị Tanaka là giáo viên.', NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333503', '22222222-2222-2222-2222-222222222502', '私は先生ではありません。', 'わたしは せんせいではありません。', 'Tôi không phải là giáo viên.', 'Phủ định lịch sự của câu danh từ.', 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333504', '22222222-2222-2222-2222-222222222502', 'これは本ではありません。', 'これは ほんではありません。', 'Cái này không phải là sách.', NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333505', '22222222-2222-2222-2222-222222222503', '机の上に本があります。', 'つくえのうえに ほんがあります。', 'Có một quyển sách trên bàn.', 'Dùng cho đồ vật.', 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('33333333-3333-3333-3333-333333333506', '22222222-2222-2222-2222-222222222503', '明日、テストがあります。', 'あした、テストがあります。', 'Ngày mai có bài kiểm tra.', 'Dùng cho sự kiện/sự việc.', 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00');

            INSERT INTO grammar_exercises
                ("Id", "PatternId", "ExerciseType", "Prompt", "PromptReading", "ExpectedAnswer", "AcceptableAnswersJson", "Hint", "Explanation", "TemplateText", "OptionsJson", "CorrectOrderJson", "StarPosition", "StarAnswer", "OrderIndex", "CreatedAt", "UpdatedAt")
            VALUES
                ('44444444-4444-4444-4444-444444444501', '22222222-2222-2222-2222-222222222501', 'vi_to_ja', 'Tôi là Michael.', NULL, '私はマイケルです。', '["私はマイケルです。","私はマイケルです"]', 'Dùng mẫu N は N です。', 'Câu giới thiệu danh tính lịch sự.', NULL, NULL, NULL, NULL, NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444502', '22222222-2222-2222-2222-222222222501', 'ja_to_vi', '私は学生です。', 'わたしは がくせいです。', 'Tôi là học sinh.', '["Tôi là học sinh.","Tôi là học sinh"]', 'Chú ý danh từ trước です.', 'です làm câu trở nên lịch sự.', NULL, NULL, NULL, NULL, NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444503', '22222222-2222-2222-2222-222222222501', 'arrange', 'Sắp xếp để tạo câu: Tôi là học sinh.', NULL, '私は学生です。', NULL, 'Mẫu: N は N です。', 'Thứ tự đúng là chủ đề + は + danh từ + です。', '____ ____ ★ ____。', '["です","学生","私は","先生"]', '["私は","学生","です"]', 3, 'です', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444504', '22222222-2222-2222-2222-222222222502', 'vi_to_ja', 'Tôi không phải là giáo viên.', NULL, '私は先生ではありません。', '["私は先生ではありません。","私は先生ではありません"]', 'Dùng N は N ではありません。', 'ではありません là phủ định lịch sự của です.', NULL, NULL, NULL, NULL, NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444505', '22222222-2222-2222-2222-222222222502', 'ja_to_vi', 'これは本ではありません。', 'これは ほんではありません。', 'Cái này không phải là sách.', '["Cái này không phải là sách.","Đây không phải là sách."]', 'これは = cái này.', 'Câu phủ định danh từ.', NULL, NULL, NULL, NULL, NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444506', '22222222-2222-2222-2222-222222222502', 'arrange', 'Sắp xếp để tạo câu: Cái này không phải là sách.', NULL, 'これは本ではありません。', NULL, 'Mẫu: N は N ではありません。', 'ではありません đứng cuối câu.', '____ ____ ★ ____。', '["本","これは","ではありません","学生"]', '["これは","本","ではありません"]', 3, 'ではありません', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444507', '22222222-2222-2222-2222-222222222503', 'vi_to_ja', 'Có một quyển sách trên bàn.', NULL, '机の上に本があります。', '["机の上に本があります。","つくえの上に本があります。"]', 'Dùng địa điểm + に + vật + があります。', 'あります dùng cho đồ vật hoặc sự việc.', NULL, NULL, NULL, NULL, NULL, 1, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444508', '22222222-2222-2222-2222-222222222503', 'ja_to_vi', '明日、テストがあります。', 'あした、テストがあります。', 'Ngày mai có bài kiểm tra.', '["Ngày mai có bài kiểm tra.","Ngày mai có kiểm tra."]', 'テスト = bài kiểm tra.', 'あります cũng dùng cho sự kiện.', NULL, NULL, NULL, NULL, NULL, 2, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00'),
                ('44444444-4444-4444-4444-444444444509', '22222222-2222-2222-2222-222222222503', 'arrange', 'Sắp xếp để tạo câu: Có sách trên bàn.', NULL, '机の上に本があります。', NULL, 'Địa điểm + に + vật + があります。', 'に đánh dấu nơi tồn tại.', '____ ____ ★ ____。', '["本が","あります","机の上に","学生が"]', '["机の上に","本が","あります"]', 3, 'あります', 3, TIMESTAMPTZ '2026-05-07 00:00:00+00', TIMESTAMPTZ '2026-05-07 00:00:00+00');
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DELETE FROM grammar_lessons
            WHERE "Id" = '11111111-1111-1111-1111-111111111501';
            """);
    }
}
