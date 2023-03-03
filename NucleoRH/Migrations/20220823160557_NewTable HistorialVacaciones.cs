using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewTableHistorialVacaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_HistorialVacaciones",
                columns: table => new
                {
                    HVId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hVEmpID = table.Column<int>(nullable: false),
                    hVEAntiguedadID = table.Column<int>(nullable: false),
                    hVDiasSolicitados = table.Column<int>(nullable: true),
                    hVEjercicio = table.Column<int>(nullable: true),
                    hvFechaSolicitud = table.Column<DateTime>(nullable: true),
                    hVFechaInicio = table.Column<DateTime>(nullable: true),
                    hVFechaCulminacion = table.Column<DateTime>(nullable: true),
                    hVFechaPresentacion = table.Column<DateTime>(nullable: true),
                    hVVacacionesPendientesEjercicioActual = table.Column<int>(nullable: true),
                    hVVacacionesPendientesEjerciciosPasados = table.Column<int>(nullable: true),
                    HVReInciId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_HistorialVacaciones", x => x.HVId);
                    table.ForeignKey(
                        name: "FK_cat_HistorialVacaciones_cat_Antiguedad",
                        column: x => x.hVEAntiguedadID,
                        principalTable: "cat_Antiguedad",
                        principalColumn: "antiID"
                        );
                    table.ForeignKey(
                        name: "FK_cat_HistorialVacaciones_cat_CatEmpleados",
                        column: x => x.hVEmpID,
                        principalTable: "cat_Empleados",
                        principalColumn: "empID"
                        );
                    table.ForeignKey(
                        name: "FK_cat_HistorialVacaciones_cat_RegistroIncidencias",
                        column: x => x.HVReInciId,
                        principalTable: "CatRegistroIncidencias",
                        principalColumn: "reInciId"
                        );
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_HistorialVacaciones_hVEAntiguedadID",
                table: "cat_HistorialVacaciones",
                column: "hVEAntiguedadID");

            migrationBuilder.CreateIndex(
                name: "IX_cat_HistorialVacaciones_hVEmpID",
                table: "cat_HistorialVacaciones",
                column: "hVEmpID");

            migrationBuilder.CreateIndex(
                name: "IX_cat_HistorialVacaciones_HVReInciId",
                table: "cat_HistorialVacaciones",
                column: "HVReInciId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_HistorialVacaciones");
        }
    }
}
