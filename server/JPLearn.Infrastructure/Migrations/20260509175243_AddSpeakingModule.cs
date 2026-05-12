using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpeakingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "speaking_courses",
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
                    table.PrimaryKey("PK_speaking_courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "speaking_lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    LessonNumber = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_speaking_lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_speaking_lessons_speaking_courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "speaking_courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "speaking_sentences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    SentenceNumber = table.Column<int>(type: "integer", nullable: false),
                    PlainText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ContentHtml = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    MeaningVi = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_speaking_sentences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_speaking_sentences_speaking_lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "speaking_lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_speaking_courses_Code",
                table: "speaking_courses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_speaking_courses_IsActive_OrderIndex",
                table: "speaking_courses",
                columns: new[] { "IsActive", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_speaking_lessons_CourseCode_LessonNumber",
                table: "speaking_lessons",
                columns: new[] { "CourseCode", "LessonNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_speaking_lessons_CourseCode_OrderIndex",
                table: "speaking_lessons",
                columns: new[] { "CourseCode", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_speaking_lessons_CourseId",
                table: "speaking_lessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_speaking_sentences_LessonId_OrderIndex",
                table: "speaking_sentences",
                columns: new[] { "LessonId", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_speaking_sentences_LessonId_SentenceNumber",
                table: "speaking_sentences",
                columns: new[] { "LessonId", "SentenceNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "speaking_sentences");

            migrationBuilder.DropTable(
                name: "speaking_lessons");

            migrationBuilder.DropTable(
                name: "speaking_courses");
        }
    }
}
