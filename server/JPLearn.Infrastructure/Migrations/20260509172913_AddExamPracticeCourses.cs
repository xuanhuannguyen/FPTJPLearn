using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExamPracticeCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_exam_questions_Level_OrderIndex",
                table: "exam_questions");

            migrationBuilder.DropIndex(
                name: "IX_exam_questions_Topic_Level_IsActive",
                table: "exam_questions");

            migrationBuilder.DropIndex(
                name: "IX_exam_passages_Level_Topic_IsActive",
                table: "exam_passages");

            migrationBuilder.DropIndex(
                name: "IX_exam_attempts_UserId_Status",
                table: "exam_attempts");

            migrationBuilder.AlterColumn<string>(
                name: "CourseCode",
                table: "exam_questions",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "jpd113",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CourseCode",
                table: "exam_passages",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "jpd113",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "exam_attempts",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "jpd113");

            migrationBuilder.CreateTable(
                name: "exam_courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AccessTier = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "free"),
                    PackageCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_courses", x => x.Id);
                    table.UniqueConstraint("AK_exam_courses_Code", x => x.Code);
                });

            migrationBuilder.Sql("""
                INSERT INTO "exam_courses" ("Id", "Code", "Title", "Description", "AccessTier", "PackageCode", "OrderIndex", "IsActive", "CreatedAt", "UpdatedAt")
                VALUES
                    ('77777777-7777-7777-7777-000000000801', 'jpd113', 'JPD113', 'Tiếng Nhật cơ bản 1 - bộ câu hỏi luyện thi nền tảng.', 'free', NULL, 1, TRUE, NOW(), NOW()),
                    ('77777777-7777-7777-7777-000000000802', 'jpd123', 'JPD123', 'Tiếng Nhật cơ bản 2 - bộ câu hỏi luyện thi nâng cao hơn.', 'premium', 'exam_jpd123', 2, TRUE, NOW(), NOW())
                ON CONFLICT ("Code") DO UPDATE SET
                    "Title" = EXCLUDED."Title",
                    "Description" = EXCLUDED."Description",
                    "AccessTier" = EXCLUDED."AccessTier",
                    "PackageCode" = EXCLUDED."PackageCode",
                    "OrderIndex" = EXCLUDED."OrderIndex",
                    "IsActive" = EXCLUDED."IsActive",
                    "UpdatedAt" = NOW();

                UPDATE "exam_questions"
                SET "CourseCode" = 'jpd113'
                WHERE "CourseCode" = '';

                UPDATE "exam_passages"
                SET "CourseCode" = 'jpd113'
                WHERE "CourseCode" = '';
                """);

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_CourseCode_Level_OrderIndex",
                table: "exam_questions",
                columns: new[] { "CourseCode", "Level", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_CourseCode_Topic_Level_IsActive",
                table: "exam_questions",
                columns: new[] { "CourseCode", "Topic", "Level", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_passages_CourseCode_Level_Topic_IsActive",
                table: "exam_passages",
                columns: new[] { "CourseCode", "Level", "Topic", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_CourseCode",
                table: "exam_attempts",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_UserId_CourseCode_Status",
                table: "exam_attempts",
                columns: new[] { "UserId", "CourseCode", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_courses_Code",
                table: "exam_courses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exam_courses_IsActive_OrderIndex",
                table: "exam_courses",
                columns: new[] { "IsActive", "OrderIndex" });

            migrationBuilder.AddForeignKey(
                name: "FK_exam_attempts_exam_courses_CourseCode",
                table: "exam_attempts",
                column: "CourseCode",
                principalTable: "exam_courses",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_exam_passages_exam_courses_CourseCode",
                table: "exam_passages",
                column: "CourseCode",
                principalTable: "exam_courses",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_exam_questions_exam_courses_CourseCode",
                table: "exam_questions",
                column: "CourseCode",
                principalTable: "exam_courses",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exam_attempts_exam_courses_CourseCode",
                table: "exam_attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_exam_passages_exam_courses_CourseCode",
                table: "exam_passages");

            migrationBuilder.DropForeignKey(
                name: "FK_exam_questions_exam_courses_CourseCode",
                table: "exam_questions");

            migrationBuilder.DropTable(
                name: "exam_courses");

            migrationBuilder.DropIndex(
                name: "IX_exam_questions_CourseCode_Level_OrderIndex",
                table: "exam_questions");

            migrationBuilder.DropIndex(
                name: "IX_exam_questions_CourseCode_Topic_Level_IsActive",
                table: "exam_questions");

            migrationBuilder.DropIndex(
                name: "IX_exam_passages_CourseCode_Level_Topic_IsActive",
                table: "exam_passages");

            migrationBuilder.DropIndex(
                name: "IX_exam_attempts_CourseCode",
                table: "exam_attempts");

            migrationBuilder.DropIndex(
                name: "IX_exam_attempts_UserId_CourseCode_Status",
                table: "exam_attempts");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "exam_attempts");

            migrationBuilder.AlterColumn<string>(
                name: "CourseCode",
                table: "exam_questions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldDefaultValue: "jpd113");

            migrationBuilder.AlterColumn<string>(
                name: "CourseCode",
                table: "exam_passages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldDefaultValue: "jpd113");

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_Level_OrderIndex",
                table: "exam_questions",
                columns: new[] { "Level", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_Topic_Level_IsActive",
                table: "exam_questions",
                columns: new[] { "Topic", "Level", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_passages_Level_Topic_IsActive",
                table: "exam_passages",
                columns: new[] { "Level", "Topic", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_UserId_Status",
                table: "exam_attempts",
                columns: new[] { "UserId", "Status" });
        }
    }
}
