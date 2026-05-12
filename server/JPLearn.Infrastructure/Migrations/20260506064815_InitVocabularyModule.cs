using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitVocabularyModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ListId = table.Column<Guid>(type: "uuid", nullable: false),
                    Mode = table.Column<string>(type: "text", nullable: false),
                    TotalCards = table.Column<int>(type: "integer", nullable: false),
                    CorrectCount = table.Column<int>(type: "integer", nullable: false),
                    WrongCount = table.Column<int>(type: "integer", nullable: false),
                    DurationSeconds = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vocabulary_lists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    WordCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vocabulary_lists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vocabulary_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ListId = table.Column<Guid>(type: "uuid", nullable: false),
                    Word = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Reading = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    WordType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ExampleSentence = table.Column<string>(type: "text", nullable: true),
                    ExampleMeaning = table.Column<string>(type: "text", nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vocabulary_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vocabulary_items_vocabulary_lists_ListId",
                        column: x => x.ListId,
                        principalTable: "vocabulary_lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_word_progress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    VocabularyItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Repetitions = table.Column<int>(type: "integer", nullable: false),
                    EaseFactor = table.Column<double>(type: "double precision", nullable: false, defaultValue: 2.5),
                    IntervalDays = table.Column<int>(type: "integer", nullable: false),
                    NextReviewAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "new"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_word_progress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_word_progress_vocabulary_items_VocabularyItemId",
                        column: x => x.VocabularyItemId,
                        principalTable: "vocabulary_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_word_progress_UserId_NextReviewAt",
                table: "user_word_progress",
                columns: new[] { "UserId", "NextReviewAt" });

            migrationBuilder.CreateIndex(
                name: "IX_user_word_progress_UserId_VocabularyItemId",
                table: "user_word_progress",
                columns: new[] { "UserId", "VocabularyItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_word_progress_VocabularyItemId",
                table: "user_word_progress",
                column: "VocabularyItemId");

            migrationBuilder.CreateIndex(
                name: "IX_vocabulary_items_ListId",
                table: "vocabulary_items",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_vocabulary_lists_UserId",
                table: "vocabulary_lists",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewSessions");

            migrationBuilder.DropTable(
                name: "user_word_progress");

            migrationBuilder.DropTable(
                name: "vocabulary_items");

            migrationBuilder.DropTable(
                name: "vocabulary_lists");
        }
    }
}
