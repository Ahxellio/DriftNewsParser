using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriftNewsParser.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChampionship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Championship_ChampionshipId",
                table: "Races");

            migrationBuilder.DropTable(
                name: "Championship");

            migrationBuilder.DropIndex(
                name: "IX_Races_ChampionshipId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "ChampionshipId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "ChampionshipId",
                table: "Drivers");

            migrationBuilder.AddColumn<string>(
                name: "Championship",
                table: "Races",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Championship",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Championship",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Championship",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "Championship",
                table: "News");

            migrationBuilder.AddColumn<int>(
                name: "ChampionshipId",
                table: "Races",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Championship",
                table: "Drivers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ChampionshipId",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Championship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Championship", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Races_ChampionshipId",
                table: "Races",
                column: "ChampionshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Championship_ChampionshipId",
                table: "Races",
                column: "ChampionshipId",
                principalTable: "Championship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
