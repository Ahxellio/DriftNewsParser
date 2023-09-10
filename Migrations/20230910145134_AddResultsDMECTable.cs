using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriftNewsParser.Migrations
{
    /// <inheritdoc />
    public partial class AddResultsDMECTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultsDMEC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    R2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    R3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    R4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    R5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    R6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    R7 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultsDMEC", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultsDMEC");
        }
    }
}
