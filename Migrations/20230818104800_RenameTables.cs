using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriftNewsParser.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_News",
                table: "News");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Drivers",
                table: "Drivers");

            migrationBuilder.RenameTable(
                name: "News",
                newName: "NewsRDS");

            migrationBuilder.RenameTable(
                name: "Drivers",
                newName: "DriversRDS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsRDS",
                table: "NewsRDS",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriversRDS",
                table: "DriversRDS",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsRDS",
                table: "NewsRDS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriversRDS",
                table: "DriversRDS");

            migrationBuilder.RenameTable(
                name: "NewsRDS",
                newName: "News");

            migrationBuilder.RenameTable(
                name: "DriversRDS",
                newName: "Drivers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_News",
                table: "News",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Drivers",
                table: "Drivers",
                column: "Id");
        }
    }
}
