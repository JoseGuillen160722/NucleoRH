using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewTableMov_Requisiciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mov_RequisicionPersonal",
                columns: table => new
                {
                    mRPId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mRPNumeroVacantes = table.Column<int>(nullable: false),
                    mRPFechaElaboracion = table.Column<DateTime>(nullable: false),
                    mRPFechaRecepcion = table.Column<DateTime>(nullable: true),
                    mRPFolio = table.Column<int>(nullable: true),
                    mRPPuestoId = table.Column<int>(nullable: false),
                    mRPTipoVacante = table.Column<int>(nullable: false),
                    mRPTurnoId = table.Column<int>(nullable: false),
                    mRPRolarTurno = table.Column<bool>(nullable: false),
                    mRPTiempoAlimentos = table.Column<float>(nullable: false),
                    mRPMotivoVacante = table.Column<int>(nullable: false),
                    mRPMotivoDescripcion = table.Column<string>(nullable: true),
                    mRPSexoId = table.Column<int>(nullable: false),
                    mRPEdadMinima = table.Column<int>(nullable: false),
                    mRPEdadMaxima = table.Column<int>(nullable: false),
                    mRPEscolaridadId = table.Column<int>(nullable: false),
                    mRPTituloIndispensable = table.Column<bool>(nullable: false),
                    mRPCedulaIndispensable = table.Column<bool>(nullable: false),
                    mRPFuncionesPuesto = table.Column<string>(nullable: true),
                    mRPConocimientosPuesto = table.Column<string>(nullable: true),
                    mRPSueldoMensualInicial = table.Column<float>(nullable: true),
                    mRPSueldoMensualPlanta = table.Column<float>(nullable: true),
                    mRPSueldoMensualMasCosto = table.Column<float>(nullable: true),
                    mRPBonoVariable = table.Column<float>(nullable: true),
                    mRPEsquemaContratacion = table.Column<int>(nullable: true),
                    mRPCandidato = table.Column<string>(nullable: true),
                    mRPFechaIngreso = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mov_RequisicionPersonal", x => x.mRPId);
                    table.ForeignKey(
                        name: "FK_mov_RequisicionPersonal_cat_Escolaridades",
                        column: x => x.mRPEscolaridadId,
                        principalTable: "cat_Escolaridades",
                        principalColumn: "escoID");
                    table.ForeignKey(
                        name: "FK_mov_RequisicionPersonal_cat_Puestos",
                        column: x => x.mRPPuestoId,
                        principalTable: "cat_Puestos",
                        principalColumn: "puestoID");
                    table.ForeignKey(
                        name: "FK_mov_RequisicionPersonal_cat_Sexos",
                        column: x => x.mRPSexoId,
                        principalTable: "cat_Sexos",
                        principalColumn: "sexID");
                    table.ForeignKey(
                        name: "FK_mov_RequisicionPersonal_cat_Turnos",
                        column: x => x.mRPTurnoId,
                        principalTable: "cat_TurnosLaborales",
                        principalColumn: "turID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPEscolaridadId",
                table: "mov_RequisicionPersonal",
                column: "mRPEscolaridadId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPPuestoId",
                table: "mov_RequisicionPersonal",
                column: "mRPPuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPSexoId",
                table: "mov_RequisicionPersonal",
                column: "mRPSexoId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPTurnoId",
                table: "mov_RequisicionPersonal",
                column: "mRPTurnoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mov_RequisicionPersonal");
        }
    }
}
