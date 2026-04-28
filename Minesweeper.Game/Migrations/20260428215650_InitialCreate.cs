using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Minesweeper.Game.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TimeInSeconds = table.Column<int>(type: "int", nullable: false),
                    MinesCount = table.Column<int>(type: "int", nullable: false),
                    DatePlayed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameScores_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "Username" },
                values: new object[] { 1, new DateTime(2026, 4, 29, 0, 56, 49, 370, DateTimeKind.Local).AddTicks(1856), "$2a$11$qR7E.fBIdJ9V1N3F6mZ.OeZ/v.X0NqFzY7S3p4Q9U8h.vG1M2V4m6", "ProPlayer" });

            migrationBuilder.InsertData(
                table: "GameScores",
                columns: new[] { "Id", "DatePlayed", "MinesCount", "TimeInSeconds", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), 20, 45, 1 },
                    { 2, new DateTime(2026, 4, 20, 14, 15, 0, 0, DateTimeKind.Unspecified), 15, 30, 1 },
                    { 3, new DateTime(2026, 3, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), 40, 120, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameScores_UserId",
                table: "GameScores",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameScores");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
