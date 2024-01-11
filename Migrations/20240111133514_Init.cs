using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DriftNewsParser.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriversFDPRO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CarName = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<string>(type: "text", nullable: false),
                    Sponsors = table.Column<string>(type: "text", nullable: false),
                    Hometown = table.Column<string>(type: "text", nullable: false),
                    Team = table.Column<string>(type: "text", nullable: false),
                    Href = table.Column<string>(type: "text", nullable: false),
                    ImgSrc = table.Column<string>(type: "text", nullable: false),
                    Championship = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriversFDPRO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DriversRDS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CarName = table.Column<string>(type: "text", nullable: false),
                    CarEngine = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    Team = table.Column<string>(type: "text", nullable: false),
                    Href = table.Column<string>(type: "text", nullable: false),
                    ImgSrc = table.Column<string>(type: "text", nullable: false),
                    Championship = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriversRDS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImgUrl = table.Column<string>(type: "text", nullable: false),
                    Championship = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Championship = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultsDMEC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Place = table.Column<string>(type: "text", nullable: false),
                    AllPoints = table.Column<string>(type: "text", nullable: false),
                    R1 = table.Column<string>(type: "text", nullable: true),
                    R2 = table.Column<string>(type: "text", nullable: true),
                    R3 = table.Column<string>(type: "text", nullable: true),
                    R4 = table.Column<string>(type: "text", nullable: true),
                    R5 = table.Column<string>(type: "text", nullable: true),
                    R6 = table.Column<string>(type: "text", nullable: true),
                    R7 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultsDMEC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultsFDPRO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProfileUrl = table.Column<string>(type: "text", nullable: false),
                    CarNumber = table.Column<string>(type: "text", nullable: false),
                    Place = table.Column<string>(type: "text", nullable: false),
                    AllPoints = table.Column<string>(type: "text", nullable: false),
                    R1 = table.Column<string>(type: "text", nullable: true),
                    R2 = table.Column<string>(type: "text", nullable: true),
                    R3 = table.Column<string>(type: "text", nullable: true),
                    R4 = table.Column<string>(type: "text", nullable: true),
                    R5 = table.Column<string>(type: "text", nullable: true),
                    R6 = table.Column<string>(type: "text", nullable: true),
                    R7 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultsFDPRO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultsRDS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProfileUrl = table.Column<string>(type: "text", nullable: false),
                    CarNumber = table.Column<string>(type: "text", nullable: false),
                    Place = table.Column<string>(type: "text", nullable: false),
                    AllPoints = table.Column<string>(type: "text", nullable: false),
                    Q1 = table.Column<string>(type: "text", nullable: true),
                    Q2 = table.Column<string>(type: "text", nullable: true),
                    Q3 = table.Column<string>(type: "text", nullable: true),
                    Q4 = table.Column<string>(type: "text", nullable: true),
                    Q5 = table.Column<string>(type: "text", nullable: true),
                    Q6 = table.Column<string>(type: "text", nullable: true),
                    Q7 = table.Column<string>(type: "text", nullable: true),
                    R1 = table.Column<string>(type: "text", nullable: true),
                    R2 = table.Column<string>(type: "text", nullable: true),
                    R3 = table.Column<string>(type: "text", nullable: true),
                    R4 = table.Column<string>(type: "text", nullable: true),
                    R5 = table.Column<string>(type: "text", nullable: true),
                    R6 = table.Column<string>(type: "text", nullable: true),
                    R7 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultsRDS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriversFDPRO");

            migrationBuilder.DropTable(
                name: "DriversRDS");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "ResultsDMEC");

            migrationBuilder.DropTable(
                name: "ResultsFDPRO");

            migrationBuilder.DropTable(
                name: "ResultsRDS");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
