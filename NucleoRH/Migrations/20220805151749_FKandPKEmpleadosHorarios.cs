using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class FKandPKEmpleadosHorarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_mov_EmpleadosHorarios",
                table: "mov_EmpleadosHorarios",
                column: "empHoraID");

            migrationBuilder.CreateIndex(
                name: "IX_mov_EmpleadosHorarios_empHoraEmpID",
                table: "mov_EmpleadosHorarios",
                column: "empHoraEmpID");

            migrationBuilder.CreateIndex(
                name: "IX_mov_EmpleadosHorarios_empHoraHorID",
                table: "mov_EmpleadosHorarios",
                column: "empHoraHorID");

            migrationBuilder.AddForeignKey(
                name: "FK_MovEmpleadosHorarios_cat_Empleados",
                table: "mov_EmpleadosHorarios",
                column: "empHoraEmpID",
                principalTable: "cat_Empleados",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovEmpleadosHorarios_cat_Horarios",
                table: "mov_EmpleadosHorarios",
                column: "empHoraHorID",
                principalTable: "cat_Horarios",
                principalColumn: "horaID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovEmpleadosHorarios_cat_Empleados",
                table: "mov_EmpleadosHorarios");

            migrationBuilder.DropForeignKey(
                name: "FK_MovEmpleadosHorarios_cat_Horarios",
                table: "mov_EmpleadosHorarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mov_EmpleadosHorarios",
                table: "mov_EmpleadosHorarios");

            migrationBuilder.DropIndex(
                name: "IX_mov_EmpleadosHorarios_empHoraEmpID",
                table: "mov_EmpleadosHorarios");

            migrationBuilder.DropIndex(
                name: "IX_mov_EmpleadosHorarios_empHoraHorID",
                table: "mov_EmpleadosHorarios");
        }
    }
}
