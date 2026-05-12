using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGrammarModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "grammar_lessons",
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
                    table.PrimaryKey("PK_grammar_lessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "grammar_patterns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Pattern = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Structure = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UsageScope = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Formation = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Notes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    TagsJson = table.Column<string>(type: "text", nullable: true),
                    AccessTierOverride = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PackageCodeOverride = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grammar_patterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_grammar_patterns_grammar_lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "grammar_lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grammar_examples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatternId = table.Column<Guid>(type: "uuid", nullable: false),
                    Japanese = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Reading = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Meaning = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grammar_examples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_grammar_examples_grammar_patterns_PatternId",
                        column: x => x.PatternId,
                        principalTable: "grammar_patterns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grammar_exercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatternId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Prompt = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    PromptReading = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    ExpectedAnswer = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    AcceptableAnswersJson = table.Column<string>(type: "text", nullable: true),
                    Hint = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Explanation = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    TemplateText = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    OptionsJson = table.Column<string>(type: "text", nullable: true),
                    CorrectOrderJson = table.Column<string>(type: "text", nullable: true),
                    StarPosition = table.Column<int>(type: "integer", nullable: true),
                    StarAnswer = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grammar_exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_grammar_exercises_grammar_patterns_PatternId",
                        column: x => x.PatternId,
                        principalTable: "grammar_patterns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_grammar_progress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrammarPatternId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "new"),
                    Repetitions = table.Column<int>(type: "integer", nullable: false),
                    EaseFactor = table.Column<double>(type: "double precision", nullable: false, defaultValue: 2.5),
                    IntervalDays = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_user_grammar_progress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_grammar_progress_grammar_patterns_GrammarPatternId",
                        column: x => x.GrammarPatternId,
                        principalTable: "grammar_patterns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grammar_exercise_attempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrammarExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    SelectedOptionOrderJson = table.Column<string>(type: "text", nullable: true),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: true),
                    Feedback = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    CheckedBy = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "system"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grammar_exercise_attempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_grammar_exercise_attempts_grammar_exercises_GrammarExercise~",
                        column: x => x.GrammarExerciseId,
                        principalTable: "grammar_exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_grammar_examples_PatternId_OrderIndex",
                table: "grammar_examples",
                columns: new[] { "PatternId", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_grammar_exercise_attempts_GrammarExerciseId",
                table: "grammar_exercise_attempts",
                column: "GrammarExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_grammar_exercise_attempts_UserId_GrammarExerciseId_CreatedAt",
                table: "grammar_exercise_attempts",
                columns: new[] { "UserId", "GrammarExerciseId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_grammar_exercises_PatternId_ExerciseType_OrderIndex",
                table: "grammar_exercises",
                columns: new[] { "PatternId", "ExerciseType", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_grammar_lessons_Level_LessonNumber",
                table: "grammar_lessons",
                columns: new[] { "Level", "LessonNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_grammar_lessons_Level_OrderIndex",
                table: "grammar_lessons",
                columns: new[] { "Level", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_grammar_patterns_LessonId",
                table: "grammar_patterns",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_grammar_patterns_LessonId_Pattern",
                table: "grammar_patterns",
                columns: new[] { "LessonId", "Pattern" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_grammar_patterns_Level_OrderIndex",
                table: "grammar_patterns",
                columns: new[] { "Level", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_user_grammar_progress_GrammarPatternId",
                table: "user_grammar_progress",
                column: "GrammarPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_user_grammar_progress_UserId_GrammarPatternId",
                table: "user_grammar_progress",
                columns: new[] { "UserId", "GrammarPatternId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_grammar_progress_UserId_IsActive_NextReviewAt",
                table: "user_grammar_progress",
                columns: new[] { "UserId", "IsActive", "NextReviewAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grammar_examples");

            migrationBuilder.DropTable(
                name: "grammar_exercise_attempts");

            migrationBuilder.DropTable(
                name: "user_grammar_progress");

            migrationBuilder.DropTable(
                name: "grammar_exercises");

            migrationBuilder.DropTable(
                name: "grammar_patterns");

            migrationBuilder.DropTable(
                name: "grammar_lessons");
        }
    }
}
