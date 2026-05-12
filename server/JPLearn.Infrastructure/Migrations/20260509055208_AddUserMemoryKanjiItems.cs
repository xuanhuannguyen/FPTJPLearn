<<<<<<< HEAD
using System;
=======
﻿using System;
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JPLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMemoryKanjiItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_memory_kanji_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceKanjiItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    Character = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HanViet = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    KunReading = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    OnReading = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Mnemonic = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    KanjiLevel = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    StrokeCount = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_user_memory_kanji_items", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_memory_kanji_items_UserId_IsActive_NextReviewAt",
                table: "user_memory_kanji_items",
                columns: new[] { "UserId", "IsActive", "NextReviewAt" });

            migrationBuilder.CreateIndex(
                name: "IX_user_memory_kanji_items_UserId_Level",
                table: "user_memory_kanji_items",
                columns: new[] { "UserId", "Level" });

            migrationBuilder.CreateIndex(
                name: "IX_user_memory_kanji_items_UserId_SourceKanjiItemId",
                table: "user_memory_kanji_items",
                columns: new[] { "UserId", "SourceKanjiItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_memory_kanji_items");
        }
    }
}
