using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriftNewsParser.Migrations
{
    /// <inheritdoc />
    public partial class AddResultsRDSTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultsRDS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Q7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    R7 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultsRDS", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultsRDS");
        }
    }
}
