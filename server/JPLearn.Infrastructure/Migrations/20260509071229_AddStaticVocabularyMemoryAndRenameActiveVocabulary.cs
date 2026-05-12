using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStaticVocabularyMemoryAndRenameActiveVocabulary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_word_progress_vocabulary_items_VocabularyItemId",
                table: "user_word_progress");

            migrationBuilder.DropForeignKey(
                name: "FK_vocabulary_items_vocabulary_lists_ListId",
                table: "vocabulary_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vocabulary_lists",
                table: "vocabulary_lists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vocabulary_items",
                table: "vocabulary_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_word_progress",
                table: "user_word_progress");

            migrationBuilder.RenameTable(
                name: "vocabulary_lists",
                newName: "active_vocabulary_lists");

            migrationBuilder.RenameTable(
                name: "vocabulary_items",
                newName: "active_vocabulary_items");

            migrationBuilder.RenameTable(
                name: "user_word_progress",
                newName: "user_active_word_progress");

            migrationBuilder.RenameIndex(
                name: "IX_vocabulary_lists_UserId",
                table: "active_vocabulary_lists",
                newName: "IX_active_vocabulary_lists_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_vocabulary_items_ListId",
                table: "active_vocabulary_items",
                newName: "IX_active_vocabulary_items_ListId");

            migrationBuilder.RenameIndex(
                name: "IX_user_word_progress_VocabularyItemId",
                table: "user_active_word_progress",
                newName: "IX_user_active_word_progress_VocabularyItemId");

            migrationBuilder.RenameIndex(
                name: "IX_user_word_progress_UserId_VocabularyItemId",
                table: "user_active_word_progress",
                newName: "IX_user_active_word_progress_UserId_VocabularyItemId");

            migrationBuilder.RenameIndex(
                name: "IX_user_word_progress_UserId_NextReviewAt",
                table: "user_active_word_progress",
                newName: "IX_user_active_word_progress_UserId_NextReviewAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_active_vocabulary_lists",
                table: "active_vocabulary_lists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_active_vocabulary_items",
                table: "active_vocabulary_items",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_active_word_progress",
                table: "user_active_word_progress",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "user_memory_vocabulary_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceVocabularyItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    Word = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Reading = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    WordType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExampleJapanese = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExampleReading = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExampleMeaning = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CourseCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    LessonNumber = table.Column<int>(type: "integer", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "new"),
                    Repetitions = table.Column<int>(type: "integer", nullable: false),
                    EaseFactor = table.Column<double>(type: "double precision", nullable: false, defaultValue: 2.5),
                    IntervalMinutes = table.Column<int>(type: "integer", nullable: false),
                    IntervalDays = table.Column<int>(type: "integer", nullable: false),
                    NextReviewAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LapseCount = table.Column<int>(type: "integer", nullable: false),
                    LearningStepIndex = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_memory_vocabulary_items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vocabulary_courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vocabulary_courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vocabulary_lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                    table.PrimaryKey("PK_vocabulary_lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vocabulary_lessons_vocabulary_courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "vocabulary_courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vocabulary_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Word = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Reading = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    WordType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExampleJapanese = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExampleReading = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExampleMeaning = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AccessTierOverride = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PackageCodeOverride = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vocabulary_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vocabulary_items_vocabulary_lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "vocabulary_lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_vocabulary_progress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    VocabularyItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsLearned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastViewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FlashcardPracticeCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    MultipleChoicePracticeCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TypingPracticeCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_vocabulary_progress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_vocabulary_progress_vocabulary_items_VocabularyItemId",
                        column: x => x.VocabularyItemId,
                        principalTable: "vocabulary_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_memory_vocabulary_items_UserId_IsActive_NextReviewAt",
                table: "user_memory_vocabulary_items",
                columns: new[] { "UserId", "IsActive", "NextReviewAt" });

            migrationBuilder.CreateIndex(
                name: "IX_user_memory_vocabulary_items_UserId_SourceVocabularyItemId",
                table: "user_memory_vocabulary_items",
                columns: new[] { "UserId", "SourceVocabularyItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_memory_vocabulary_items_UserId_Status",
                table: "user_memory_vocabulary_items",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_user_vocabulary_progress_UserId_IsLearned",
                table: "user_vocabulary_progress",
                columns: new[] { "UserId", "IsLearned" });

            migrationBuilder.CreateIndex(
                name: "IX_user_vocabulary_progress_UserId_LastViewedAt",
                table: "user_vocabulary_progress",
                columns: new[] { "UserId", "LastViewedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_user_vocabulary_progress_UserId_VocabularyItemId",
                table: "user_vocabulary_progress",
                columns: new[] { "UserId", "VocabularyItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_vocabulary_progress_VocabularyItemId",
                table: "user_vocabulary_progress",
                column: "VocabularyItemId");

            migrationBuilder.CreateIndex(
                name: "IX_vocabulary_courses_Code",
                table: "vocabulary_courses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vocabulary_courses_OrderIndex",
                table: "vocabulary_courses",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_vocabulary_items_CourseCode_OrderIndex",
                table: "vocabulary_items",
                columns: new[] { "CourseCode", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_vocabulary_items_LessonId",
                table: "vocabulary_items",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_vocabulary_items_LessonId_Word",
                table: "vocabulary_items",
                columns: new[] { "LessonId", "Word" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vocabulary_lessons_CourseCode_LessonNumber",
                table: "vocabulary_lessons",
                columns: new[] { "CourseCode", "LessonNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vocabulary_lessons_CourseCode_OrderIndex",
                table: "vocabulary_lessons",
                columns: new[] { "CourseCode", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_vocabulary_lessons_CourseId",
                table: "vocabulary_lessons",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_active_vocabulary_items_active_vocabulary_lists_ListId",
                table: "active_vocabulary_items",
                column: "ListId",
                principalTable: "active_vocabulary_lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_active_word_progress_active_vocabulary_items_VocabularyItemId",
                table: "user_active_word_progress",
                column: "VocabularyItemId",
                principalTable: "active_vocabulary_items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_active_vocabulary_items_active_vocabulary_lists_ListId",
                table: "active_vocabulary_items");

            migrationBuilder.DropForeignKey(
                name: "FK_user_active_word_progress_active_vocabulary_items_VocabularyItemId",
                table: "user_active_word_progress");

            migrationBuilder.DropTable(
                name: "user_memory_vocabulary_items");

            migrationBuilder.DropTable(
                name: "user_vocabulary_progress");

            migrationBuilder.DropTable(
                name: "vocabulary_items");

            migrationBuilder.DropTable(
                name: "vocabulary_lessons");

            migrationBuilder.DropTable(
                name: "vocabulary_courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_active_word_progress",
                table: "user_active_word_progress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_active_vocabulary_lists",
                table: "active_vocabulary_lists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_active_vocabulary_items",
                table: "active_vocabulary_items");

            migrationBuilder.RenameTable(
                name: "user_active_word_progress",
                newName: "user_word_progress");

            migrationBuilder.RenameTable(
                name: "active_vocabulary_lists",
                newName: "vocabulary_lists");

            migrationBuilder.RenameTable(
                name: "active_vocabulary_items",
                newName: "vocabulary_items");

            migrationBuilder.RenameIndex(
                name: "IX_user_active_word_progress_VocabularyItemId",
                table: "user_word_progress",
                newName: "IX_user_word_progress_VocabularyItemId");

            migrationBuilder.RenameIndex(
                name: "IX_user_active_word_progress_UserId_VocabularyItemId",
                table: "user_word_progress",
                newName: "IX_user_word_progress_UserId_VocabularyItemId");

            migrationBuilder.RenameIndex(
                name: "IX_user_active_word_progress_UserId_NextReviewAt",
                table: "user_word_progress",
                newName: "IX_user_word_progress_UserId_NextReviewAt");

            migrationBuilder.RenameIndex(
                name: "IX_active_vocabulary_lists_UserId",
                table: "vocabulary_lists",
                newName: "IX_vocabulary_lists_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_active_vocabulary_items_ListId",
                table: "vocabulary_items",
                newName: "IX_vocabulary_items_ListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_word_progress",
                table: "user_word_progress",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vocabulary_lists",
                table: "vocabulary_lists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vocabulary_items",
                table: "vocabulary_items",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_word_progress_vocabulary_items_VocabularyItemId",
                table: "user_word_progress",
                column: "VocabularyItemId",
                principalTable: "vocabulary_items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vocabulary_items_vocabulary_lists_ListId",
                table: "vocabulary_items",
                column: "ListId",
                principalTable: "vocabulary_lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
