using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamePlayService.Migrations
{
    /// <inheritdoc />
    public partial class GamesTableEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moves");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "CurrentTurn",
                table: "Games",
                newName: "Result");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Games",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "BoardStateFEN",
                table: "Games",
                newName: "PlayerWhite");

            migrationBuilder.AddColumn<string>(
                name: "PlayerBlack",
                table: "Games",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerBlack",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "Result",
                table: "Games",
                newName: "CurrentTurn");

            migrationBuilder.RenameColumn(
                name: "PlayerWhite",
                table: "Games",
                newName: "BoardStateFEN");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Games",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    MoveId = table.Column<Guid>(type: "uuid", nullable: false),
                    CapturedPiece = table.Column<string>(type: "text", nullable: true),
                    From = table.Column<string>(type: "text", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Piece = table.Column<string>(type: "text", nullable: false),
                    Player = table.Column<string>(type: "text", nullable: false),
                    Promotion = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    To = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.MoveId);
                    table.ForeignKey(
                        name: "FK_Moves_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Moves_GameId",
                table: "Moves",
                column: "GameId");
        }
    }
}
