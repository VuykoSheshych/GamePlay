using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamePlayService.Migrations
{
    /// <inheritdoc />
    public partial class ManageMoveModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "Player",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Moves");

            migrationBuilder.RenameColumn(
                name: "SanNotation",
                table: "Moves",
                newName: "WhiteSanNotation");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "WhiteMoveFrom",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "WhiteMoveTo",
                table: "Moves");

            migrationBuilder.RenameColumn(
                name: "WhiteSanNotation",
                table: "Moves",
                newName: "SanNotation");

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "Moves",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Player",
                table: "Moves",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Moves",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
