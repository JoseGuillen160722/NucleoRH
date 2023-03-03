using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class createcat_SaldoDeVacaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_SaldoDeVacaciones",
                columns: table => new
                {
                    sVId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sVEmpId = table.Column<int>(nullable: false),
                    sVEjercicio = table.Column<int>(nullable: true),
                    sVPeriodoId = table.Column<int>(nullable: false),
                    sVFechaRegistro = table.Column<DateTime>(nullable: true),
                    sVAniosAntiguedad = table.Column<int>(nullable: true),
                    sVAntiId = table.Column<int>(nullable: false),
                    sVDiasDisfrutados = table.Column<int>(nullable: true),
                    sVDiasRestantes = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_SaldoDeVacaciones", x => x.sVId);
                    table.ForeignKey(
                        name: "FK_cat_SaldoDeVacaciones_cat_Antiguedad",
                        column: x => x.sVAntiId,
                        principalTable: "cat_Antiguedad",
                        principalColumn: "antiID"
                        );
                    table.ForeignKey(
                        name: "FK_cat_SaldoDeVacaciones_cat_Empleados",
                        column: x => x.sVEmpId,
                        principalTable: "cat_Empleados",
                        principalColumn: "empID"
                        );
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_SaldoDeVacaciones_sVAntiId",
                table: "cat_SaldoDeVacaciones",
                column: "sVAntiId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_SaldoDeVacaciones_sVEmpId",
                table: "cat_SaldoDeVacaciones",
                column: "sVEmpId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_SaldoDeVacaciones");
        }
    }
}
