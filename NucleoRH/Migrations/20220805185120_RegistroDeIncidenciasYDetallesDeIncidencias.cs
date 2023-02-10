using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class RegistroDeIncidenciasYDetallesDeIncidencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "registroIncidencias",
                columns: table => new
                {
                    reInciId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reInciEmpId = table.Column<int>(nullable: false),
                    fecha = table.Column<DateTime>(nullable: false),
                    reInciInciId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registroIncidencias", x => x.reInciId);
                    table.ForeignKey(
                        name: "FK_registroIncidencias_cat_Empleados",
                        column: x => x.reInciEmpId,
                        principalTable: "cat_Empleados",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_registroIncidencias_cat_Incidencias",
                        column: x => x.reInciInciId,
                        principalTable: "cat_Incidencias",
                        principalColumn: "inciID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalleIncidencias",
                columns: table => new
                {
                    detInciID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    detInciReInciID = table.Column<int>(nullable: false),
                    fecha = table.Column<DateTime>(nullable: false),
                    detInciHorarioID = table.Column<int>(nullable: false),
                    medidaAccion = table.Column<string>(nullable: true),
                    permiso = table.Column<string>(nullable: true),
                    asunto = table.Column<string>(nullable: true),
                    destino = table.Column<string>(nullable: true),
                    telDestino = table.Column<string>(maxLength: 12, nullable: true),
                    contacto1 = table.Column<string>(nullable: true),
                    nombreDestino = table.Column<string>(nullable: true),
                    contacto2 = table.Column<string>(nullable: true),
                    observaciones = table.Column<string>(nullable: true),
                    horaSalida = table.Column<TimeSpan>(nullable: false),
                    horaRegreso = table.Column<TimeSpan>(nullable: false),
                    fechaInicio = table.Column<DateTime>(nullable: false),
                    fechaFinal = table.Column<DateTime>(nullable: false),
                    fechaPresentacion = table.Column<DateTime>(nullable: false),
                    diasAusencia = table.Column<int>(nullable: false),
                    personaCubrira = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalleIncidencias", x => x.detInciID);
                    table.ForeignKey(
                        name: "FK_detalleIncidencias_cat_Horarios",
                        column: x => x.detInciHorarioID,
                        principalTable: "cat_Horarios",
                        principalColumn: "horaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalleIncidencias_registroIncidencias",
                        column: x => x.detInciReInciID,
                        principalTable: "registroIncidencias",
                        principalColumn: "reInciId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_detalleIncidencias_detInciHorarioID",
                table: "detalleIncidencias",
                column: "detInciHorarioID");

            migrationBuilder.CreateIndex(
                name: "IX_detalleIncidencias_detInciReInciID",
                table: "detalleIncidencias",
                column: "detInciReInciID");

            migrationBuilder.CreateIndex(
                name: "IX_registroIncidencias_reInciEmpId",
                table: "registroIncidencias",
                column: "reInciEmpId");

            migrationBuilder.CreateIndex(
                name: "IX_registroIncidencias_reInciInciId",
                table: "registroIncidencias",
                column: "reInciInciId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detalleIncidencias");

            migrationBuilder.DropTable(
                name: "registroIncidencias");
        }
    }
}
