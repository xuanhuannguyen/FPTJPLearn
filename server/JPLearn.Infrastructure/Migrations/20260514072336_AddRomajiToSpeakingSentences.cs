using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRomajiToSpeakingSentences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Romaji",
                table: "speaking_sentences",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "OrderCode",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Romaji",
                table: "speaking_sentences");

            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "Orders");
        }
    }
}
