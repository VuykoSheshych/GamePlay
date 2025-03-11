using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamePlayService.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeToGameRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "GameRecords",
                newName: "Started");

            migrationBuilder.AddColumn<DateTime>(
                name: "Finished",
                table: "GameRecords",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "GameRecords");

            migrationBuilder.RenameColumn(
                name: "Started",
                table: "GameRecords",
                newName: "Date");
        }
    }
}
