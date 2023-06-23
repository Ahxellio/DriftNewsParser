using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriftNewsParser.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCarEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Car_CarId",
                table: "Drivers");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_CarId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Drivers");

            migrationBuilder.AddColumn<string>(
                name: "CarEngine",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CarName",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarEngine",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CarName",
                table: "Drivers");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarBrand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Engine = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CarId",
                table: "Drivers",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Car_CarId",
                table: "Drivers",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
