using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKanjiModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kanji_lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    LessonNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AccessTier = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "free"),
                    PackageCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kanji_lessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "kanji_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Character = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    HanViet = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    StrokeCount = table.Column<int>(type: "integer", nullable: false),
                    KunReading = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    OnReading = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Mnemonic = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    StrokeSvg = table.Column<string>(type: "text", nullable: true),
                    StrokeDataJson = table.Column<string>(type: "text", nullable: true),
                    ComponentMapJson = table.Column<string>(type: "text", nullable: true),
                    AccessTierOverride = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PackageCodeOverride = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kanji_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_kanji_items_kanji_lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "kanji_lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kanji_vocabulary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    KanjiItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    Level = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Word = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Reading = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExampleJapanese = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExampleReading = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExampleMeaning = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kanji_vocabulary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_kanji_vocabulary_kanji_items_KanjiItemId",
                        column: x => x.KanjiItemId,
                        principalTable: "kanji_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_kanji_vocabulary_kanji_lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "kanji_lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_kanji_progress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    KanjiItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsLearned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastViewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WritingPracticeCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    FlashcardPracticeCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_kanji_progress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_kanji_progress_kanji_items_KanjiItemId",
                        column: x => x.KanjiItemId,
                        principalTable: "kanji_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kanji_items_Character",
                table: "kanji_items",
                column: "Character",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kanji_items_LessonId",
                table: "kanji_items",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_kanji_items_Level_OrderIndex",
                table: "kanji_items",
                columns: new[] { "Level", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_kanji_lessons_Level_LessonNumber",
                table: "kanji_lessons",
                columns: new[] { "Level", "LessonNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kanji_lessons_Level_OrderIndex",
                table: "kanji_lessons",
                columns: new[] { "Level", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_kanji_vocabulary_KanjiItemId",
                table: "kanji_vocabulary",
                column: "KanjiItemId");

            migrationBuilder.CreateIndex(
                name: "IX_kanji_vocabulary_LessonId",
                table: "kanji_vocabulary",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_kanji_vocabulary_LessonId_Word",
                table: "kanji_vocabulary",
                columns: new[] { "LessonId", "Word" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kanji_vocabulary_Level_OrderIndex",
                table: "kanji_vocabulary",
                columns: new[] { "Level", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_user_kanji_progress_KanjiItemId",
                table: "user_kanji_progress",
                column: "KanjiItemId");

            migrationBuilder.CreateIndex(
                name: "IX_user_kanji_progress_UserId_IsLearned",
                table: "user_kanji_progress",
                columns: new[] { "UserId", "IsLearned" });

            migrationBuilder.CreateIndex(
                name: "IX_user_kanji_progress_UserId_KanjiItemId",
                table: "user_kanji_progress",
                columns: new[] { "UserId", "KanjiItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_kanji_progress_UserId_LastViewedAt",
                table: "user_kanji_progress",
                columns: new[] { "UserId", "LastViewedAt" });

            migrationBuilder.Sql(
                """
                INSERT INTO kanji_lessons
                    ("Id", "Level", "LessonNumber", "Title", "Description", "AccessTier", "PackageCode", "OrderIndex", "CreatedAt", "UpdatedAt")
                VALUES
                    ('33333333-3333-3333-3333-333333333501', 'N5', 1, 'Số đếm cơ bản', '10 Hán tự N5 nền tảng dùng cho số đếm, ngày tháng và lượng từ thường gặp.', 'free', 'kanji_n5', 1, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00');

                INSERT INTO kanji_items
                    ("Id", "LessonId", "Level", "Character", "HanViet", "Meaning", "StrokeCount", "KunReading", "OnReading", "Mnemonic", "StrokeSvg", "StrokeDataJson", "ComponentMapJson", "OrderIndex", "CreatedAt", "UpdatedAt")
                VALUES
                    ('44444444-4444-4444-4444-444444444501', '33333333-3333-3333-3333-333333333501', 'N5', '一', 'NHẤT', 'một', 1, 'ひと、ひと.つ', 'イチ、イツ', 'Một nét ngang duy nhất biểu thị số một.', NULL, '{"strokeCount":1,"strokes":[{"order":1,"path":"M20 50 L180 50"}]}', '[{"component":"一","name":"nhất","meaning":"một nét ngang","position":"whole","note":"Kanji đơn giản gồm một nét ngang."}]', 1, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('44444444-4444-4444-4444-444444444502', '33333333-3333-3333-3333-333333333501', 'N5', '二', 'NHỊ', 'hai', 2, 'ふた、ふた.つ', 'ニ', 'Hai nét ngang song song biểu thị số hai.', NULL, '{"strokeCount":2,"strokes":[{"order":1,"path":"M45 38 L155 38"},{"order":2,"path":"M25 92 L175 92"}]}', '[{"component":"一","name":"nhất","meaning":"một nét ngang","position":"top"},{"component":"一","name":"nhất","meaning":"một nét ngang","position":"bottom"}]', 2, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('44444444-4444-4444-4444-444444444503', '33333333-3333-3333-3333-333333333501', 'N5', '三', 'TAM', 'ba', 3, 'み、み.つ、みっ.つ', 'サン', 'Ba nét ngang xếp tầng biểu thị số ba.', NULL, '{"strokeCount":3,"strokes":[{"order":1,"path":"M52 34 L148 34"},{"order":2,"path":"M42 66 L158 66"},{"order":3,"path":"M24 102 L176 102"}]}', '[{"component":"一","name":"nhất","meaning":"một nét ngang","position":"top"},{"component":"二","name":"nhị","meaning":"hai nét ngang","position":"lower"}]', 3, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('44444444-4444-4444-4444-444444444504', '33333333-3333-3333-3333-333333333501', 'N5', '四', 'TỨ', 'bốn', 5, 'よ、よ.つ、よっ.つ、よん', 'シ', 'Một khung bao ngoài chứa phần bên trong, liên tưởng bốn phía.', NULL, '{"strokeCount":5,"strokes":[{"order":1,"path":"M36 30 L36 128"},{"order":2,"path":"M36 30 L164 30 L164 128"},{"order":3,"path":"M74 55 L74 98"},{"order":4,"path":"M126 55 L104 98"},{"order":5,"path":"M36 128 L164 128"}]}', '[{"component":"囗","name":"vi","meaning":"khung bao","position":"outside"},{"component":"儿","name":"nhân đi","meaning":"chân người","position":"inside"}]', 4, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('44444444-4444-4444-4444-444444444505', '33333333-3333-3333-3333-333333333501', 'N5', '五', 'NGŨ', 'năm', 4, 'いつ、いつ.つ', 'ゴ', 'Các nét đan nhau tạo hình số năm trong Hán tự.', NULL, '{"strokeCount":4,"strokes":[{"order":1,"path":"M42 30 L158 30"},{"order":2,"path":"M78 30 L62 102"},{"order":3,"path":"M62 68 L142 68 L136 102"},{"order":4,"path":"M35 118 L165 118"}]}', '[{"component":"二","name":"nhị","meaning":"hai nét ngang","position":"top-bottom"},{"component":"丨","name":"cổn","meaning":"nét dọc","position":"middle"}]', 5, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('44444444-4444-4444-4444-444444444506', '33333333-3333-3333-3333-333333333501', 'N5', '六', 'LỤC', 'sáu', 4, 'む、む.つ、むっ.つ、むい', 'ロク', 'Phần trên như mái, hai nét dưới mở ra thành số sáu.', NULL, '{"strokeCount":4,"strokes":[{"order":1,"path":"M96 22 L104 42"},{"order":2,"path":"M45 54 L155 54"},{"order":3,"path":"M76 70 L44 120"},{"order":4,"path":"M124 70 L158 120"}]}', '[{"component":"亠","name":"đầu","meaning":"nắp/mái","position":"top"},{"component":"八","name":"bát","meaning":"mở ra hai bên","position":"bottom"}]', 6, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('44444444-4444-4444-4444-444444444507', '33333333-3333-3333-3333-333333333501', 'N5', '七', 'THẤT', 'bảy', 2, 'なな、なな.つ、なの', 'シチ', 'Một nét ngang bị cắt bởi nét cong như dấu móc của số bảy.', NULL, '{"strokeCount":2,"strokes":[{"order":1,"path":"M38 58 L162 48"},{"order":2,"path":"M92 26 L82 115 Q82 138 116 126"}]}', '[{"component":"一","name":"nhất","meaning":"nét ngang","position":"cross"},{"component":"乙","name":"ất","meaning":"nét móc cong","position":"main"}]', 7, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('44444444-4444-4444-4444-444444444508', '33333333-3333-3333-3333-333333333501', 'N5', '八', 'BÁT', 'tám', 2, 'や、や.つ、やっ.つ、よう', 'ハチ', 'Hai nét tách ra hai bên, như mở rộng thành số tám.', NULL, '{"strokeCount":2,"strokes":[{"order":1,"path":"M88 36 L50 124"},{"order":2,"path":"M112 36 L152 124"}]}', '[{"component":"八","name":"bát","meaning":"hai nét tách ra","position":"whole","note":"Hai nét mở rộng sang trái và phải."}]', 8, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('44444444-4444-4444-4444-444444444509', '33333333-3333-3333-3333-333333333501', 'N5', '九', 'CỬU', 'chín', 2, 'ここの、ここの.つ', 'キュウ、ク', 'Một nét cong lớn kèm nét phụ, ghi nhớ như số chín có đuôi.', NULL, '{"strokeCount":2,"strokes":[{"order":1,"path":"M78 28 L70 82 Q66 125 35 136"},{"order":2,"path":"M55 56 L128 56 Q160 58 144 102 L132 132"}]}', '[{"component":"九","name":"cửu","meaning":"chín","position":"whole","note":"Kanji nguyên khối, nhớ theo hình nét cong có đuôi."}]', 9, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('44444444-4444-4444-4444-444444444510', '33333333-3333-3333-3333-333333333501', 'N5', '十', 'THẬP', 'mười', 2, 'とお、と', 'ジュウ、ジッ', 'Một nét ngang và một nét dọc giao nhau tạo thành số mười.', NULL, '{"strokeCount":2,"strokes":[{"order":1,"path":"M35 62 L165 62"},{"order":2,"path":"M100 26 L100 138"}]}', '[{"component":"一","name":"nhất","meaning":"nét ngang","position":"horizontal"},{"component":"丨","name":"cổn","meaning":"nét dọc","position":"vertical"}]', 10, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00');

                INSERT INTO kanji_vocabulary
                    ("Id", "LessonId", "KanjiItemId", "Level", "Word", "Reading", "Meaning", "ExampleJapanese", "ExampleReading", "ExampleMeaning", "OrderIndex", "CreatedAt", "UpdatedAt")
                VALUES
                    ('55555555-5555-5555-5555-555555555501', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444501', 'N5', '一人', 'ひとり', 'một người', '一人で行きます。', 'ひとりで いきます。', 'Tôi đi một mình.', 1, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555502', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444501', 'N5', '一つ', 'ひとつ', 'một cái', 'りんごを一つください。', 'りんごを ひとつ ください。', 'Cho tôi một quả táo.', 2, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555503', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444502', 'N5', '二人', 'ふたり', 'hai người', '二人で勉強します。', 'ふたりで べんきょうします。', 'Hai người cùng học.', 3, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555504', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444502', 'N5', '二つ', 'ふたつ', 'hai cái', 'いすが二つあります。', 'いすが ふたつ あります。', 'Có hai cái ghế.', 4, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555505', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444503', 'N5', '三日', 'みっか', 'ngày mùng ba; ba ngày', '三日に会いましょう。', 'みっかに あいましょう。', 'Hãy gặp nhau vào ngày mùng ba.', 5, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555506', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444504', 'N5', '四月', 'しがつ', 'tháng tư', '四月に学校が始まります。', 'しがつに がっこうが はじまります。', 'Trường học bắt đầu vào tháng tư.', 6, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555507', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444505', 'N5', '五分', 'ごふん', 'năm phút', '五分待ってください。', 'ごふん まって ください。', 'Vui lòng đợi năm phút.', 7, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555508', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444506', 'N5', '六日', 'むいか', 'ngày mùng sáu; sáu ngày', '六日に帰ります。', 'むいかに かえります。', 'Tôi sẽ về vào ngày mùng sáu.', 8, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555509', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444507', 'N5', '七時', 'しちじ', 'bảy giờ', '七時に起きます。', 'しちじに おきます。', 'Tôi thức dậy lúc bảy giờ.', 9, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555510', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444508', 'N5', '八百', 'はっぴゃく', 'tám trăm', '八百円です。', 'はっぴゃくえんです。', 'Là tám trăm yên.', 10, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555511', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444509', 'N5', '九月', 'くがつ', 'tháng chín', '九月は涼しいです。', 'くがつは すずしいです。', 'Tháng chín thì mát mẻ.', 11, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555512', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444510', 'N5', '十分', 'じゅっぷん', 'mười phút', '十分休みましょう。', 'じゅっぷん やすみましょう。', 'Hãy nghỉ mười phút.', 12, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555513', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444501', 'N5', '一日', 'いちにち', 'một ngày', '一日勉強しました。', 'いちにち べんきょうしました。', 'Tôi đã học cả một ngày.', 13, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555514', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444502', 'N5', '二月', 'にがつ', 'tháng hai', '二月は短いです。', 'にがつは みじかいです。', 'Tháng hai thì ngắn.', 14, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00'),
                    ('55555555-5555-5555-5555-555555555515', '33333333-3333-3333-3333-333333333501', '44444444-4444-4444-4444-444444444503', 'N5', '三人', 'さんにん', 'ba người', '三人で食べます。', 'さんにんで たべます。', 'Ba người cùng ăn.', 15, TIMESTAMPTZ '2026-05-08 00:00:00+00', TIMESTAMPTZ '2026-05-08 00:00:00+00');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kanji_vocabulary");

            migrationBuilder.DropTable(
                name: "user_kanji_progress");

            migrationBuilder.DropTable(
                name: "kanji_items");

            migrationBuilder.DropTable(
                name: "kanji_lessons");
        }
    }
}
