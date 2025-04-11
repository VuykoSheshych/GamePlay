using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamePlay.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Games",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					BoardStateFEN = table.Column<string>(type: "text", nullable: false),
					CurrentTurn = table.Column<string>(type: "text", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					IsFinished = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Games", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Moves",
				columns: table => new
				{
					MoveId = table.Column<Guid>(type: "uuid", nullable: false),
					GameId = table.Column<Guid>(type: "uuid", nullable: false),
					Player = table.Column<string>(type: "text", nullable: false),
					From = table.Column<string>(type: "text", nullable: false),
					To = table.Column<string>(type: "text", nullable: false),
					Piece = table.Column<string>(type: "text", nullable: false),
					CapturedPiece = table.Column<string>(type: "text", nullable: true),
					Promotion = table.Column<string>(type: "text", nullable: true),
					Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Moves");

			migrationBuilder.DropTable(
				name: "Games");
		}
	}
}
