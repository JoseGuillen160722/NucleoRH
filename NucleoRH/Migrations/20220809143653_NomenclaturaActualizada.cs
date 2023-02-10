using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NomenclaturaActualizada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalleIncidencias_cat_Horarios",
                table: "detalleIncidencias");

            migrationBuilder.DropForeignKey(
                name: "FK_detalleIncidencias_registroIncidencias",
                table: "detalleIncidencias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_registroIncidencias",
                table: "registroIncidencias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_detalleIncidencias",
                table: "detalleIncidencias");

            migrationBuilder.RenameTable(
                name: "registroIncidencias",
                newName: "CatRegistroIncidencias");

            migrationBuilder.RenameTable(
                name: "detalleIncidencias",
                newName: "CatDetalleIncidencias");

            migrationBuilder.RenameIndex(
                name: "IX_registroIncidencias_reInciInciId",
                table: "CatRegistroIncidencias",
                newName: "IX_CatRegistroIncidencias_reInciInciId");

            migrationBuilder.RenameIndex(
                name: "IX_registroIncidencias_reInciEmpId",
                table: "CatRegistroIncidencias",
                newName: "IX_CatRegistroIncidencias_reInciEmpId");

            migrationBuilder.RenameIndex(
                name: "IX_detalleIncidencias_detInciReInciID",
                table: "CatDetalleIncidencias",
                newName: "IX_CatDetalleIncidencias_detInciReInciID");

            migrationBuilder.RenameIndex(
                name: "IX_detalleIncidencias_detInciHorarioID",
                table: "CatDetalleIncidencias",
                newName: "IX_CatDetalleIncidencias_detInciHorarioID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatRegistroIncidencias",
                table: "CatRegistroIncidencias",
                column: "reInciId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatDetalleIncidencias",
                table: "CatDetalleIncidencias",
                column: "detInciID");

            migrationBuilder.AddForeignKey(
                name: "FK_cat_DetalleIncidencias_cat_Horarios",
                table: "CatDetalleIncidencias",
                column: "detInciHorarioID",
                principalTable: "cat_Horarios",
                principalColumn: "horaID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cat_DetalleIncidencias_cat_RegistroIncidencias",
                table: "CatDetalleIncidencias",
                column: "detInciReInciID",
                principalTable: "CatRegistroIncidencias",
                principalColumn: "reInciId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cat_DetalleIncidencias_cat_Horarios",
                table: "CatDetalleIncidencias");

            migrationBuilder.DropForeignKey(
                name: "FK_cat_DetalleIncidencias_cat_RegistroIncidencias",
                table: "CatDetalleIncidencias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatRegistroIncidencias",
                table: "CatRegistroIncidencias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatDetalleIncidencias",
                table: "CatDetalleIncidencias");

            migrationBuilder.RenameTable(
                name: "CatRegistroIncidencias",
                newName: "registroIncidencias");

            migrationBuilder.RenameTable(
                name: "CatDetalleIncidencias",
                newName: "detalleIncidencias");

            migrationBuilder.RenameIndex(
                name: "IX_CatRegistroIncidencias_reInciInciId",
                table: "registroIncidencias",
                newName: "IX_registroIncidencias_reInciInciId");

            migrationBuilder.RenameIndex(
                name: "IX_CatRegistroIncidencias_reInciEmpId",
                table: "registroIncidencias",
                newName: "IX_registroIncidencias_reInciEmpId");

            migrationBuilder.RenameIndex(
                name: "IX_CatDetalleIncidencias_detInciReInciID",
                table: "detalleIncidencias",
                newName: "IX_detalleIncidencias_detInciReInciID");

            migrationBuilder.RenameIndex(
                name: "IX_CatDetalleIncidencias_detInciHorarioID",
                table: "detalleIncidencias",
                newName: "IX_detalleIncidencias_detInciHorarioID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_registroIncidencias",
                table: "registroIncidencias",
                column: "reInciId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_detalleIncidencias",
                table: "detalleIncidencias",
                column: "detInciID");

            migrationBuilder.AddForeignKey(
                name: "FK_detalleIncidencias_cat_Horarios",
                table: "detalleIncidencias",
                column: "detInciHorarioID",
                principalTable: "cat_Horarios",
                principalColumn: "horaID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_detalleIncidencias_registroIncidencias",
                table: "detalleIncidencias",
                column: "detInciReInciID",
                principalTable: "registroIncidencias",
                principalColumn: "reInciId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
