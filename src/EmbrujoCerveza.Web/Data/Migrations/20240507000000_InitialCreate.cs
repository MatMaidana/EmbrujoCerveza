using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmbrujoCerveza.Web.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BottleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Material = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    CapacityMl = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BottleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeerStyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Abv = table.Column<decimal>(type: "numeric", nullable: false),
                    Ibu = table.Column<int>(type: "integer", nullable: true),
                    ImageFileName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerStyles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeerLots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BeerStyleId = table.Column<int>(type: "integer", nullable: false),
                    BottleTypeId = table.Column<int>(type: "integer", nullable: false),
                    BottleCount = table.Column<int>(type: "integer", nullable: false),
                    BottledOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
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
