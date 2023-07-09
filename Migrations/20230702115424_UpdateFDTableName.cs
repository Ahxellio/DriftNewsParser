using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriftNewsParser.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFDTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DriversFD",
                table: "DriversFD");

            migrationBuilder.RenameTable(
                name: "DriversFD",
                newName: "DriversFDPRO");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriversFDPRO",
                table: "DriversFDPRO",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DriversFDPRO",
                table: "DriversFDPRO");

            migrationBuilder.RenameTable(
                name: "DriversFDPRO",
                newName: "DriversFD");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriversFD",
                table: "DriversFD",
                column: "Id");
        }
    }
}
