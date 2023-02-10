using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class RemovedSaldosVacaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_SaldosVacaciones");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_SaldosVacaciones",
                columns: table => new
                {
                    sVId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sVAnioIngreso = table.Column<int>(type: "int", nullable: false),
                    sVAntiId = table.Column<int>(type: "int", nullable: false),
                    SVAntiguedad = table.Column<int>(type: "int", nullable: false, computedColumnSql: "sVEjercicio - sVAnioIngreso"),
                    sVDepaId = table.Column<int>(type: "int", nullable: false),
                    SVDiasDisfrutados = table.Column<int>(type: "int", nullable: false, computedColumnSql: "sVQuincena1Enero + sVQuincena2Enero + sVQuincena1Febrero + sVQuincena2Febrero + sVQuincena1Marzo + sVQuincena2Marzo + sVQuincena1Abril + sVQuincena2Abril + sVQuincena1Mayo + sVQuincena2Mayo + sVQuincena1Junio + sVQuincena2Junio + sVQuincena1Julio + sVQuincena2Julio + sVQuincena1Agosto + sVQuincena2Agosto + sVQuincena1Septiembre + sVQuincena2Septiembre + sVQuincena1Octubre + sVQuincena2Octubre + sVQuincena1Noviembre + sVQuincena2Noviembre + sVQuincena1Diciembre + sVQuincena1Diciembre"),
                    sVDiasPorAdelantado = table.Column<int>(type: "int", nullable: false),
                    sVEjercicio = table.Column<int>(type: "int", nullable: false),
                    sVEmpId = table.Column<int>(type: "int", nullable: false),
                    sVFechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sVQuincena1Abril = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Agosto = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Diciembre = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Enero = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Febrero = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Julio = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Junio = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Marzo = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Mayo = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Noviembre = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Octubre = table.Column<int>(type: "int", nullable: false),
                    sVQuincena1Septiembre = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Abril = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Agosto = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Diciembre = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Enero = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Febrero = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Julio = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Junio = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Marzo = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Mayo = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Noviembre = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Octubre = table.Column<int>(type: "int", nullable: false),
                    sVQuincena2Septiembre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_SaldosVacaciones", x => x.sVId);
                    table.ForeignKey(
                        name: "FK_cat_SaldoVacaciones_cat_Antiguedad",
                        column: x => x.sVAntiId,
                        principalTable: "cat_Antiguedad",
                        principalColumn: "antiID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cat_SaldoVacaciones_cat_Departamentos",
                        column: x => x.sVDepaId,
                        principalTable: "cat_Departamentos",
                        principalColumn: "depaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cat_SaldoVacaciones_cat_Empleados",
                        column: x => x.sVEmpId,
                        principalTable: "cat_Empleados",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
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
    }
}
