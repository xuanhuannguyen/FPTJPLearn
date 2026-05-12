using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExamPracticeBlueprintsAndTopics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE exam_question_options CASCADE; TRUNCATE TABLE exam_questions CASCADE; TRUNCATE TABLE exam_passages CASCADE; TRUNCATE TABLE exam_attempts CASCADE;");
            migrationBuilder.AddColumn<Guid>(
                name: "BlueprintId",
                table: "exam_attempts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "exam_blueprints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TimeLimitMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 30),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_blueprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exam_blueprints_exam_courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "exam_courses",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "exam_practice_progress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Topic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalCompleted = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_practice_progress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exam_topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "jpd113"),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_topics", x => x.Id);
                    table.UniqueConstraint("AK_exam_topics_CourseCode_Code", x => new { x.CourseCode, x.Code });
                    table.ForeignKey(
                        name: "FK_exam_topics_exam_courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "exam_courses",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "exam_blueprint_rules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlueprintId = table.Column<Guid>(type: "uuid", nullable: false),
                    Topic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    QuestionCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_blueprint_rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exam_blueprint_rules_exam_blueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "exam_blueprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exam_passages_CourseCode_Topic",
                table: "exam_passages",
                columns: new[] { "CourseCode", "Topic" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_BlueprintId",
                table: "exam_attempts",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_blueprint_rules_BlueprintId",
                table: "exam_blueprint_rules",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_blueprints_CourseCode_IsActive",
                table: "exam_blueprints",
                columns: new[] { "CourseCode", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_practice_progress_UserId_CourseCode_Topic",
                table: "exam_practice_progress",
                columns: new[] { "UserId", "CourseCode", "Topic" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exam_topics_CourseCode_IsActive_OrderIndex",
                table: "exam_topics",
                columns: new[] { "CourseCode", "IsActive", "OrderIndex" });

            migrationBuilder.AddForeignKey(
                name: "FK_exam_attempts_exam_blueprints_BlueprintId",
                table: "exam_attempts",
                column: "BlueprintId",
                principalTable: "exam_blueprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_exam_passages_exam_topics_CourseCode_Topic",
                table: "exam_passages",
                columns: new[] { "CourseCode", "Topic" },
                principalTable: "exam_topics",
                principalColumns: new[] { "CourseCode", "Code" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_exam_questions_exam_topics_CourseCode_Topic",
                table: "exam_questions",
                columns: new[] { "CourseCode", "Topic" },
                principalTable: "exam_topics",
                principalColumns: new[] { "CourseCode", "Code" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exam_attempts_exam_blueprints_BlueprintId",
                table: "exam_attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_exam_passages_exam_topics_CourseCode_Topic",
                table: "exam_passages");

            migrationBuilder.DropForeignKey(
                name: "FK_exam_questions_exam_topics_CourseCode_Topic",
                table: "exam_questions");

            migrationBuilder.DropTable(
                name: "exam_blueprint_rules");

            migrationBuilder.DropTable(
                name: "exam_practice_progress");

            migrationBuilder.DropTable(
                name: "exam_topics");

            migrationBuilder.DropTable(
                name: "exam_blueprints");

            migrationBuilder.DropIndex(
                name: "IX_exam_passages_CourseCode_Topic",
                table: "exam_passages");

            migrationBuilder.DropIndex(
                name: "IX_exam_attempts_BlueprintId",
                table: "exam_attempts");

            migrationBuilder.DropColumn(
                name: "BlueprintId",
                table: "exam_attempts");
        }
    }
}
