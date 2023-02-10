using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class RemoveTableMovEmpleadosHorarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mov_EmpleadosHorarios");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mov_EmpleadosHorarios",
                columns: table => new
                {
                    empHoraID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    empHoraEmpID = table.Column<int>(type: "int", nullable: false),
                    empHoraFechaDesde = table.Column<DateTime>(type: "datetime", nullable: true),
                    empHoraFechaHasta = table.Column<DateTime>(type: "datetime", nullable: true),
                    empHoraFechaRegistro = table.Column<DateTime>(type: "datetime", nullable: false),
                    empHoraHorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mov_EmpleadosHorarios", x => x.empHoraID);
                    table.ForeignKey(
                        name: "FK_MovEmpleadosHorarios_cat_Empleados",
                        column: x => x.empHoraEmpID,
                        principalTable: "cat_Empleados",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovEmpleadosHorarios_cat_Horarios",
                        column: x => x.empHoraHorID,
                        principalTable: "cat_Horarios",
                        principalColumn: "horaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mov_EmpleadosHorarios_empHoraEmpID",
                table: "mov_EmpleadosHorarios",
                column: "empHoraEmpID");

            migrationBuilder.CreateIndex(
                name: "IX_mov_EmpleadosHorarios_empHoraHorID",
                table: "mov_EmpleadosHorarios",
                column: "empHoraHorID");
        }
    }
}
