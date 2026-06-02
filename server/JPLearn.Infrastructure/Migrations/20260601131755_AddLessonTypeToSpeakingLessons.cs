using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonTypeToSpeakingLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LessonType",
                table: "speaking_lessons",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "reading");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonType",
                table: "speaking_lessons");
        }
    }
}
