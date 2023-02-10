using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class DeletetableMRP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mov_RequisicionPersonal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mov_RequisicionPersonal",
                columns: table => new
                {
                    mRPId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mRPBonoVariable = table.Column<float>(type: "real", nullable: true),
                    mRPCandidato = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mRPCedulaIndispensable = table.Column<bool>(type: "bit", nullable: false),
                    mRPConocimientosPuesto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mRPEdadMaxima = table.Column<int>(type: "int", nullable: false),
                    mRPEdadMinima = table.Column<int>(type: "int", nullable: false),
                    mRPEscolaridadId = table.Column<int>(type: "int", nullable: false),
                    mRPEsquemaContratacion = table.Column<int>(type: "int", nullable: true),
                    mRPExperienciaIndispensable = table.Column<bool>(type: "bit", nullable: false),
                    mRPFechaElaboracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    mRPFechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    mRPFechaRecepcion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    mRPFlujoId = table.Column<int>(type: "int", nullable: false),
                    mRPFolio = table.Column<int>(type: "int", nullable: true),
                    mRPFuncionesPuesto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mRPMotivoDescripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mRPMotivoVacante = table.Column<int>(type: "int", nullable: false),
                    mRPNumeroVacantes = table.Column<int>(type: "int", nullable: false),
                    mRPPuestoId = table.Column<int>(type: "int", nullable: false),
                    mRPRolarTurno = table.Column<bool>(type: "bit", nullable: false),
                    mRPSexoId = table.Column<int>(type: "int", nullable: false),
                    mRPSucursalId = table.Column<int>(type: "int", nullable: false),
                    mRPSueldoMensualInicial = table.Column<float>(type: "real", nullable: true),
                    mRPSueldoMensualMasCosto = table.Column<float>(type: "real", nullable: true),
                    mRPSueldoMensualPlanta = table.Column<float>(type: "real", nullable: true),
                    mRPTiempoAlimentos = table.Column<float>(type: "real", nullable: false),
                    mRPTipoVacante = table.Column<int>(type: "int", nullable: false),
                    mRPTituloIndispensable = table.Column<bool>(type: "bit", nullable: false),
                    mRPTurnoId = table.Column<int>(type: "int", nullable: false),
                    SucursalesSucuId = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_mov_RequisicionPersonal_cat_DetalleFlujo",
                        column: x => x.mRPFlujoId,
                        principalTable: "cat_DetalleFlujo",
                        principalColumn: "detFlujoId");
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
                    table.ForeignKey(
                        name: "FK_mov_RequisicionPersonal_cat_Sucursales_SucursalesSucuId",
                        column: x => x.SucursalesSucuId,
                        principalTable: "cat_Sucursales",
                        principalColumn: "SucuID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPEscolaridadId",
                table: "mov_RequisicionPersonal",
                column: "mRPEscolaridadId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPFlujoId",
                table: "mov_RequisicionPersonal",
                column: "mRPFlujoId");

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

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_SucursalesSucuId",
                table: "mov_RequisicionPersonal",
                column: "SucursalesSucuId");
        }
    }
}
