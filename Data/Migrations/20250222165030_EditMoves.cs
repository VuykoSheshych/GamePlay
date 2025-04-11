using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamePlay.Migrations
{
	/// <inheritdoc />
	public partial class EditMoves : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "BlackMoveFrom",
				table: "Moves");

			migrationBuilder.DropColumn(
				name: "BlackMoveTo",
				table: "Moves");

			migrationBuilder.DropColumn(
				name: "BlackSanNotation",
				table: "Moves");

			migrationBuilder.DropColumn(
				name: "FenAfter",
				table: "Moves");

			migrationBuilder.DropColumn(
				name: "WhiteMoveFrom",
				table: "Moves");

			migrationBuilder.DropColumn(
				name: "WhiteMoveTo",
				table: "Moves");

			migrationBuilder.RenameColumn(
				name: "WhiteSanNotation",
				table: "Moves",
				newName: "Promotion");

			migrationBuilder.AlterColumn<string>(
				name: "FenBefore",
				table: "Moves",
				type: "text",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "text",
				oldNullable: true);

			migrationBuilder.AddColumn<string>(
				name: "From",
				table: "Moves",
				type: "text",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "PlayerColor",
				table: "Moves",
				type: "text",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "SanNotation",
				table: "Moves",
				type: "text",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "To",
				table: "Moves",
				type: "text",
				nullable: false,
				defaultValue: "");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "From",
				table: "Moves");

			migrationBuilder.DropColumn(
				name: "PlayerColor",
				table: "Moves");

			migrationBuilder.DropColumn(
				name: "SanNotation",
				table: "Moves");

			migrationBuilder.DropColumn(
				name: "To",
				table: "Moves");

			migrationBuilder.RenameColumn(
				name: "Promotion",
				table: "Moves",
				newName: "WhiteSanNotation");

			migrationBuilder.AlterColumn<string>(
				name: "FenBefore",
				table: "Moves",
				type: "text",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "text");

			migrationBuilder.AddColumn<string>(
				name: "BlackMoveFrom",
				table: "Moves",
				type: "text",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "BlackMoveTo",
				table: "Moves",
				type: "text",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "BlackSanNotation",
				table: "Moves",
				type: "text",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "FenAfter",
				table: "Moves",
				type: "text",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "WhiteMoveFrom",
				table: "Moves",
				type: "text",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "WhiteMoveTo",
				table: "Moves",
				type: "text",
				nullable: true);
		}
	}
}
