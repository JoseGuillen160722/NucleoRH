using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class AdicionColumnaMotivoDetalleIncidencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registroIncidencias_cat_Empleados",
                table: "registroIncidencias");

            migrationBuilder.AddColumn<string>(
                name: "motivo",
                table: "detalleIncidencias",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_registroIncidencias_cat_Empleados",
                table: "registroIncidencias",
                column: "reInciEmpId",
                principalTable: "cat_Empleados",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registroIncidencias_cat_Empleados",
                table: "registroIncidencias");

            migrationBuilder.DropColumn(
                name: "motivo",
                table: "detalleIncidencias");

            migrationBuilder.AddForeignKey(
                name: "FK_registroIncidencias_cat_Empleados",
                table: "registroIncidencias",
                column: "reInciEmpId",
                principalTable: "cat_Empleados",
                principalColumn: "empID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
