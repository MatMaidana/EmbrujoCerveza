using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmbrujoCerveza.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeerStyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Abv = table.Column<decimal>(type: "TEXT", nullable: false),
                    Ibu = table.Column<int>(type: "INTEGER", nullable: true),
                    ImageFileName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerStyles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BottleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Material = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    CapacityMl = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BottleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeerLots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BeerStyleId = table.Column<int>(type: "INTEGER", nullable: false),
                    BottleTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    BottleCount = table.Column<int>(type: "INTEGER", nullable: false),
                    BottledOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerLots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeerLots_BeerStyles_BeerStyleId",
                        column: x => x.BeerStyleId,
                        principalTable: "BeerStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BeerLots_BottleTypes_BottleTypeId",
                        column: x => x.BottleTypeId,
                        principalTable: "BottleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeerLots_BeerStyleId",
                table: "BeerLots",
                column: "BeerStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_BeerLots_BottleTypeId",
                table: "BeerLots",
                column: "BottleTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeerLots");

            migrationBuilder.DropTable(
                name: "BeerStyles");

            migrationBuilder.DropTable(
                name: "BottleTypes");
        }
    }
}
