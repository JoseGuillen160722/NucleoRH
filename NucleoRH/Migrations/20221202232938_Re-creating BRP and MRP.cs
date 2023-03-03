using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class RecreatingBRPandMRP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_BitacoraRequisicionPersonal");

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
                    mRPEmpId = table.Column<int>(type: "int", nullable: false),
                    mRPEscolaridadId = table.Column<int>(type: "int", nullable: false),
                    mRPEsquemaContratacion = table.Column<int>(type: "int", nullable: true),
                    mRPEstatusId = table.Column<int>(type: "int", nullable: false),
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
                    mRPUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mov_RequisicionPersonal", x => x.mRPId);
                    table.ForeignKey(
                        name: "FK_mov_RequisicionPersonal_cat_Empleados",
                        column: x => x.mRPEmpId,
                        principalTable: "cat_Empleados",
                        principalColumn: "empID");
                    table.ForeignKey(
                        name: "FK_mov_RequisicionPersonal_cat_Escolaridades",
                        column: x => x.mRPEscolaridadId,
                        principalTable: "cat_Escolaridades",
                        principalColumn: "escoID");
                    table.ForeignKey(
                        name: "FK_mov_RequisicionPersonal_cat_Estatus",
                        column: x => x.mRPEstatusId,
                        principalTable: "cat_Estatus",
                        principalColumn: "estID");
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
                        name: "FK_mov_RequisicionPersonal_cat_Sucursales",
                        column: x => x.mRPSucursalId,
                        principalTable: "cat_Sucursales",
                        principalColumn: "SucuID");
                    table.ForeignKey(
                        name: "FK_mov_RequisicionPersonal_cat_Turnos",
                        column: x => x.mRPTurnoId,
                        principalTable: "cat_TurnosLaborales",
                        principalColumn: "turID");
                });

            migrationBuilder.CreateTable(
                name: "cat_BitacoraRequisicionPersonal",
                columns: table => new
                {
                    bitRPId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bitRPDetFlujoId = table.Column<int>(type: "int", nullable: false),
                    bitRPFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bitRPObservaciones = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    bitRPRPId = table.Column<int>(type: "int", nullable: false),
                    bitRPUserId = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_BitacoraRequisicionPersonal", x => x.bitRPId);
                    table.ForeignKey(
                        name: "FK_cat_BitacoraRequisicionPersonal_cat_DetalleFlujos",
                        column: x => x.bitRPDetFlujoId,
                        principalTable: "cat_DetalleFlujo",
                        principalColumn: "detFlujoId");
                    table.ForeignKey(
                        name: "FK_cat_BitacoraRequisicionPersonal_mov_RequisicionPersonal",
                        column: x => x.bitRPRPId,
                        principalTable: "mov_RequisicionPersonal",
                        principalColumn: "mRPId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_BitacoraRequisicionPersonal_bitRPDetFlujoId",
                table: "cat_BitacoraRequisicionPersonal",
                column: "bitRPDetFlujoId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_BitacoraRequisicionPersonal_bitRPRPId",
                table: "cat_BitacoraRequisicionPersonal",
                column: "bitRPRPId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPEmpId",
                table: "mov_RequisicionPersonal",
                column: "mRPEmpId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPEscolaridadId",
                table: "mov_RequisicionPersonal",
                column: "mRPEscolaridadId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPEstatusId",
                table: "mov_RequisicionPersonal",
                column: "mRPEstatusId");

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
                name: "IX_mov_RequisicionPersonal_mRPSucursalId",
                table: "mov_RequisicionPersonal",
                column: "mRPSucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPTurnoId",
                table: "mov_RequisicionPersonal",
                column: "mRPTurnoId");
        }
    }
}
