using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMemoryModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "memory_review_sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Scope = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    TotalCards = table.Column<int>(type: "integer", nullable: false),
                    AgainCount = table.Column<int>(type: "integer", nullable: false),
                    HardCount = table.Column<int>(type: "integer", nullable: false),
                    GoodCount = table.Column<int>(type: "integer", nullable: false),
                    EasyCount = table.Column<int>(type: "integer", nullable: false),
                    DurationSeconds = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_memory_review_sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_memory_grammar_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceGrammarPatternId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Pattern = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Structure = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UsageScope = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Formation = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ExampleJapanese = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExampleReading = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExampleMeaning = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    TagsJson = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "new"),
                    Repetitions = table.Column<int>(type: "integer", nullable: false),
                    EaseFactor = table.Column<double>(type: "double precision", nullable: false, defaultValue: 2.5),
                    IntervalMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IntervalDays = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    NextReviewAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LapseCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LearningStepIndex = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_memory_grammar_items", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_memory_review_sessions_UserId_ItemType_StartedAt",
                table: "memory_review_sessions",
                columns: new[] { "UserId", "ItemType", "StartedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_user_memory_grammar_items_UserId_IsActive_NextReviewAt",
                table: "user_memory_grammar_items",
                columns: new[] { "UserId", "IsActive", "NextReviewAt" });

            migrationBuilder.CreateIndex(
                name: "IX_user_memory_grammar_items_UserId_Level",
                table: "user_memory_grammar_items",
                columns: new[] { "UserId", "Level" });

            migrationBuilder.CreateIndex(
                name: "IX_user_memory_grammar_items_UserId_SourceGrammarPatternId",
                table: "user_memory_grammar_items",
                columns: new[] { "UserId", "SourceGrammarPatternId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "memory_review_sessions");

            migrationBuilder.DropTable(
                name: "user_memory_grammar_items");
        }
    }
}
