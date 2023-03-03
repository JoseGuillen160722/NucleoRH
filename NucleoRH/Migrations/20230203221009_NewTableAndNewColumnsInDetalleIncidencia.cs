using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewTableAndNewColumnsInDetalleIncidencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "empHoraTipoHorario",
                table: "mov_EmpleadosHorarios",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "detInciFechaFinPermisoPersonal",
                table: "CatDetalleIncidencias",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "detInciHoraIngreso",
                table: "CatDetalleIncidencias",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "detInciHoraRegresoComida",
                table: "CatDetalleIncidencias",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "detInciHoraSalida",
                table: "CatDetalleIncidencias",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "detInciHoraSalidaComida",
                table: "CatDetalleIncidencias",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "mov_PermisosPorTiempo",
                columns: table => new
                {
                    pPTId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pPTEmpId = table.Column<int>(nullable: false),
                    pPTFechaInicio = table.Column<DateTime>(nullable: false),
                    pPTFechaFinal = table.Column<DateTime>(nullable: false),
                    pPTHoraEntrada = table.Column<TimeSpan>(nullable: true),
                    pPTHoraSalida = table.Column<TimeSpan>(nullable: true),
                    pPTHoraSalidaComida = table.Column<TimeSpan>(nullable: true),
                    pPTHoraRegresoComida = table.Column<TimeSpan>(nullable: true),
                    pPTEstatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mov_PermisosPorTiempo", x => x.pPTId);
                    table.ForeignKey(
                        name: "FK_mov_PermisosPorTiempo_cat_Empleados",
                        column: x => x.pPTEmpId,
                        principalTable: "cat_Empleados",
                        principalColumn: "empID");
                    table.ForeignKey(
                        name: "FK_mov_PermisosPorTiempo_cat_Estatus",
                        column: x => x.pPTEstatusId,
                        principalTable: "cat_Estatus",
                        principalColumn: "estID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_mov_PermisosPorTiempo_pPTEmpId",
                table: "mov_PermisosPorTiempo",
                column: "pPTEmpId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_PermisosPorTiempo_pPTEstatusId",
                table: "mov_PermisosPorTiempo",
                column: "pPTEstatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mov_PermisosPorTiempo");

            migrationBuilder.DropColumn(
                name: "empHoraTipoHorario",
                table: "mov_EmpleadosHorarios");

            migrationBuilder.DropColumn(
                name: "detInciFechaFinPermisoPersonal",
                table: "CatDetalleIncidencias");

            migrationBuilder.DropColumn(
                name: "detInciHoraIngreso",
                table: "CatDetalleIncidencias");

            migrationBuilder.DropColumn(
                name: "detInciHoraRegresoComida",
                table: "CatDetalleIncidencias");

            migrationBuilder.DropColumn(
                name: "detInciHoraSalida",
                table: "CatDetalleIncidencias");

            migrationBuilder.DropColumn(
                name: "detInciHoraSalidaComida",
                table: "CatDetalleIncidencias");
        }
    }
}
