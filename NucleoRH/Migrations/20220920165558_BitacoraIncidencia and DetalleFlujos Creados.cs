using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class BitacoraIncidenciaandDetalleFlujosCreados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_DetalleFlujo",
                columns: table => new
                {
                    detFlujoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    detFlujoFlujoId = table.Column<int>(nullable: false),
                    detFlujoDescripcion = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_DetalleFlujo", x => x.detFlujoId);
                    table.ForeignKey(
                        name: "FK_cat_DetalleFlujos_cat_Flujos",
                        column: x => x.detFlujoFlujoId,
                        principalTable: "cat_Flujos",
                        principalColumn: "flujoId");
                });

            migrationBuilder.CreateTable(
                name: "cat_BitacoraIncidencias",
                columns: table => new
                {
                    bitInciId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bitInciReInciId = table.Column<int>(nullable: false),
                    bitInciDetFlujoId = table.Column<int>(nullable: false),
                    bitInciUserId = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    bitInciFecha = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_BitacoraIncidencias", x => x.bitInciId);
                    table.ForeignKey(
                        name: "FK_cat_BitacoraIncidencias_cat_DetalleFlujos",
                        column: x => x.bitInciDetFlujoId,
                        principalTable: "cat_DetalleFlujo",
                        principalColumn: "detFlujoId");
                    table.ForeignKey(
                        name: "FK_cat_BitacoraIncidencias_cat_RegistroIncidencias",
                        column: x => x.bitInciReInciId,
                        principalTable: "CatRegistroIncidencias",
                        principalColumn: "reInciId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_BitacoraIncidencias_bitInciDetFlujoId",
                table: "cat_BitacoraIncidencias",
                column: "bitInciDetFlujoId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_BitacoraIncidencias_bitInciReInciId",
                table: "cat_BitacoraIncidencias",
                column: "bitInciReInciId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_DetalleFlujo_detFlujoFlujoId",
                table: "cat_DetalleFlujo",
                column: "detFlujoFlujoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_BitacoraIncidencias");

            migrationBuilder.DropTable(
                name: "cat_DetalleFlujo");
        }
    }
}
