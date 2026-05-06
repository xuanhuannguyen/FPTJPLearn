using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewWorkflowStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LapseCount",
                table: "user_word_progress",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LearningStepIndex",
                table: "user_word_progress",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LapseCount",
                table: "user_word_progress");

            migrationBuilder.DropColumn(
                name: "LearningStepIndex",
                table: "user_word_progress");
        }
    }
}
