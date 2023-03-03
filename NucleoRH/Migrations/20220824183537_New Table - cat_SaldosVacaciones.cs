using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewTablecat_SaldosVacaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_SaldosVacaciones",
                columns: table => new
                {
                    sVId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sVEmpId = table.Column<int>(nullable: false),
                    sVAnioIngreso = table.Column<int>(nullable: true),
                    sVFechaRegistro = table.Column<DateTime>(nullable: true),
                    SVAntiguedad = table.Column<int>(nullable: true, computedColumnSql: "sVEjercicio - sVAnioIngreso"),
                    sVDepaId = table.Column<int>(nullable: false),
                    sVEjercicio = table.Column<int>(nullable: true),
                    sVAntiId = table.Column<int>(nullable: false),
                    SVDiasDisfrutados = table.Column<int>(nullable: true, computedColumnSql: "sVQuincena1Enero + sVQuincena2Enero + sVQuincena1Febrero + sVQuincena2Febrero + sVQuincena1Marzo + sVQuincena2Marzo + sVQuincena1Abril + sVQuincena2Abril + sVQuincena1Mayo + sVQuincena2Mayo + sVQuincena1Junio + sVQuincena2Junio + sVQuincena1Julio + sVQuincena2Julio + sVQuincena1Agosto + sVQuincena2Agosto + sVQuincena1Septiembre + sVQuincena2Septiembre + sVQuincena1Octubre + sVQuincena2Octubre + sVQuincena1Noviembre + sVQuincena2Noviembre + sVQuincena1Diciembre + sVQuincena1Diciembre"),
                    sVDiasPorAdelantado = table.Column<int>(nullable: true),
                    sVQuincena1Enero = table.Column<int>(nullable: true),
                    sVQuincena2Enero = table.Column<int>(nullable: true),
                    sVQuincena1Febrero = table.Column<int>(nullable: true),
                    sVQuincena2Febrero = table.Column<int>(nullable: true),
                    sVQuincena1Marzo = table.Column<int>(nullable: true),
                    sVQuincena2Marzo = table.Column<int>(nullable: true),
                    sVQuincena1Abril = table.Column<int>(nullable: true),
                    sVQuincena2Abril = table.Column<int>(nullable: true),
                    sVQuincena1Mayo = table.Column<int>(nullable: true),
                    sVQuincena2Mayo = table.Column<int>(nullable: true),
                    sVQuincena1Junio = table.Column<int>(nullable: true),
                    sVQuincena2Junio = table.Column<int>(nullable: true),
                    sVQuincena1Julio = table.Column<int>(nullable: true),
                    sVQuincena2Julio = table.Column<int>(nullable: true),
                    sVQuincena1Agosto = table.Column<int>(nullable: true),
                    sVQuincena2Agosto = table.Column<int>(nullable: true),
                    sVQuincena1Septiembre = table.Column<int>(nullable: true),
                    sVQuincena2Septiembre = table.Column<int>(nullable: true),
                    sVQuincena1Octubre = table.Column<int>(nullable: true),
                    sVQuincena2Octubre = table.Column<int>(nullable: true),
                    sVQuincena1Noviembre = table.Column<int>(nullable: true),
                    sVQuincena2Noviembre = table.Column<int>(nullable: true),
                    sVQuincena1Diciembre = table.Column<int>(nullable: true),
                    sVQuincena2Diciembre = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_SaldosVacaciones", x => x.sVId);
                    table.ForeignKey(
                        name: "FK_cat_SaldoVacaciones_cat_Antiguedad",
                        column: x => x.sVAntiId,
                        principalTable: "cat_Antiguedad",
                        principalColumn: "antiID");
                    table.ForeignKey(
                        name: "FK_cat_SaldoVacaciones_cat_Departamentos",
                        column: x => x.sVDepaId,
                        principalTable: "cat_Departamentos",
                        principalColumn: "depaID");
                    table.ForeignKey(
                        name: "FK_cat_SaldoVacaciones_cat_Empleados",
                        column: x => x.sVEmpId,
                        principalTable: "cat_Empleados",
                        principalColumn: "empID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_SaldosVacaciones_sVAntiId",
                table: "cat_SaldosVacaciones",
                column: "sVAntiId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_SaldosVacaciones_sVDepaId",
                table: "cat_SaldosVacaciones",
                column: "sVDepaId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_SaldosVacaciones_sVEmpId",
                table: "cat_SaldosVacaciones",
                column: "sVEmpId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_SaldosVacaciones");
        }
    }
}
