using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawlin.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatorUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decks_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeckInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeckId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckInstances_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckInstances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeckId = table.Column<int>(type: "INTEGER", nullable: false),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    Question = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flashcards_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewDataItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlashcardId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeckInstanceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Repeats = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    EasinessFactor = table.Column<double>(type: "REAL", nullable: false, defaultValue: 2.5),
                    InvervalDays = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Quality = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    NextReviewDateUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewDateUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewDataItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewDataItems_DeckInstances_DeckInstanceId",
                        column: x => x.DeckInstanceId,
                        principalTable: "DeckInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewDataItems_Flashcards_FlashcardId",
                        column: x => x.FlashcardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewDataItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeckInstances_DeckId",
                table: "DeckInstances",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckInstances_UserId",
                table: "DeckInstances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_CreatorUserId",
                table: "Decks",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_DeckId",
                table: "Flashcards",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDataItems_DeckInstanceId",
                table: "ReviewDataItems",
                column: "DeckInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDataItems_DeckInstanceId_NextReviewDateUtc",
                table: "ReviewDataItems",
                columns: new[] { "DeckInstanceId", "NextReviewDateUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDataItems_FlashcardId",
                table: "ReviewDataItems",
                column: "FlashcardId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDataItems_UserId",
                table: "ReviewDataItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewDataItems");

            migrationBuilder.DropTable(
                name: "DeckInstances");

            migrationBuilder.DropTable(
                name: "Flashcards");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
