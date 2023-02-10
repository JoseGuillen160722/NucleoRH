using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class TwoNewTablesCatFlujosandCatSolicitudIncidencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_Flujos",
                columns: table => new
                {
                    flujoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    flujoDescripcion = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_Flujos", x => x.flujoId);
                });

            migrationBuilder.CreateTable(
                name: "cat_SolicitudIncidencias",
                columns: table => new
                {
                    solInciId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    solInciReInciId = table.Column<int>(nullable: false),
                    solInciEmpId = table.Column<int>(nullable: false),
                    solInciFechaRegistro = table.Column<DateTime>(nullable: true),
                    solInciPuestoSuperior = table.Column<int>(nullable: true),
                    solInciPerfiles = table.Column<int>(nullable: true),
                    solInciFlujoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_SolicitudIncidencias", x => x.solInciId);
                    table.ForeignKey(
                        name: "FK_cat_SolicitudIncidencias_cat_Empleados",
                        column: x => x.solInciEmpId,
                        principalTable: "cat_Empleados",
                        principalColumn: "empID");
                    table.ForeignKey(
                        name: "FK_cat_SolicitudIncidencias_cat_Flujos",
                        column: x => x.solInciFlujoId,
                        principalTable: "cat_Flujos",
                        principalColumn: "flujoId");
                    table.ForeignKey(
                        name: "FK_cat_SolicitudIncidencias_cat_RegistroIncidencias",
                        column: x => x.solInciReInciId,
                        principalTable: "CatRegistroIncidencias",
                        principalColumn: "reInciId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_SolicitudIncidencias_solInciEmpId",
                table: "cat_SolicitudIncidencias",
                column: "solInciEmpId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_SolicitudIncidencias_solInciFlujoId",
                table: "cat_SolicitudIncidencias",
                column: "solInciFlujoId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_SolicitudIncidencias_solInciReInciId",
                table: "cat_SolicitudIncidencias",
                column: "solInciReInciId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_SolicitudIncidencias");

            migrationBuilder.DropTable(
                name: "cat_Flujos");
        }
    }
}
