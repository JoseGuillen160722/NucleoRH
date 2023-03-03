using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewTableMov_EmpleadosHorarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mov_EmpleadosHorarios",
                columns: table => new
                {
                    empHoraId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    empHoraFechaRegistro = table.Column<DateTime>(nullable: false),
                    empHoraEmpId = table.Column<int>(nullable: false),
                    empHoraHorId = table.Column<int>(nullable: false),
                    empHoraFechaDesde = table.Column<DateTime>(nullable: true),
                    empHoraFechaHasta = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mov_EmpleadosHorarios", x => x.empHoraId);
                    table.ForeignKey(
                        name: "FK_mov_EmpleadosHorarios_cat_Empleados",
                        column: x => x.empHoraEmpId,
                        principalTable: "cat_Empleados",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mov_EmpleadosHorarios_cat_Horarios",
                        column: x => x.empHoraHorId,
                        principalTable: "cat_Horarios",
                        principalColumn: "horaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mov_EmpleadosHorarios_empHoraEmpId",
                table: "mov_EmpleadosHorarios",
                column: "empHoraEmpId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_EmpleadosHorarios_empHoraHorId",
                table: "mov_EmpleadosHorarios",
                column: "empHoraHorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mov_EmpleadosHorarios");
        }
    }
}
