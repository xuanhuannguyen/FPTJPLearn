using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExamPracticeModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exam_attempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 30),
                    TotalQuestions = table.Column<int>(type: "integer", nullable: false, defaultValue: 30),
                    CorrectCount = table.Column<int>(type: "integer", nullable: false),
                    ScorePercent = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_attempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exam_passages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    Level = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Topic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_passages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exam_questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassageId = table.Column<Guid>(type: "uuid", nullable: true),
                    QuestionType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Topic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Level = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    QuestionText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Explanation = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exam_questions_exam_passages_PassageId",
                        column: x => x.PassageId,
                        principalTable: "exam_passages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "exam_question_options",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Label = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_question_options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exam_question_options_exam_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "exam_questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_attempt_answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedOptionId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: true),
                    AnsweredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SequenceNumber = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_attempt_answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exam_attempt_answers_exam_attempts_AttemptId",
                        column: x => x.AttemptId,
                        principalTable: "exam_attempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exam_attempt_answers_exam_question_options_SelectedOptionId",
                        column: x => x.SelectedOptionId,
                        principalTable: "exam_question_options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_exam_attempt_answers_exam_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "exam_questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempt_answers_AttemptId_QuestionId",
                table: "exam_attempt_answers",
                columns: new[] { "AttemptId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempt_answers_AttemptId_SequenceNumber",
                table: "exam_attempt_answers",
                columns: new[] { "AttemptId", "SequenceNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempt_answers_QuestionId",
                table: "exam_attempt_answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempt_answers_SelectedOptionId",
                table: "exam_attempt_answers",
                column: "SelectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_ExpiresAt",
                table: "exam_attempts",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_UserId_StartedAt",
                table: "exam_attempts",
                columns: new[] { "UserId", "StartedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_UserId_Status",
                table: "exam_attempts",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_passages_Level_Topic_IsActive",
                table: "exam_passages",
                columns: new[] { "Level", "Topic", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_passages_OrderIndex",
                table: "exam_passages",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_exam_question_options_QuestionId",
                table: "exam_question_options",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_question_options_QuestionId_Label",
                table: "exam_question_options",
                columns: new[] { "QuestionId", "Label" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_Level_OrderIndex",
                table: "exam_questions",
                columns: new[] { "Level", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_PassageId",
                table: "exam_questions",
                column: "PassageId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_Topic_Level_IsActive",
                table: "exam_questions",
                columns: new[] { "Topic", "Level", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_attempt_answers");

            migrationBuilder.DropTable(
                name: "exam_attempts");

            migrationBuilder.DropTable(
                name: "exam_question_options");

            migrationBuilder.DropTable(
                name: "exam_questions");

            migrationBuilder.DropTable(
                name: "exam_passages");
        }
    }
}
