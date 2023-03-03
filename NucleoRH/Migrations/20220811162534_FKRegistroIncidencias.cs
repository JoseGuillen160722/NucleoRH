using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class FKRegistroIncidencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registroIncidencias_cat_Estatus",
                table: "CatRegistroIncidencias");

            migrationBuilder.AddForeignKey(
                name: "FK_cat_RegistroIncidencias_cat_Estatus",
                table: "CatRegistroIncidencias",
                column: "reInciEstatusId",
                principalTable: "cat_Estatus",
                principalColumn: "estID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cat_RegistroIncidencias_cat_Estatus",
                table: "CatRegistroIncidencias");

            migrationBuilder.AddForeignKey(
                name: "FK_registroIncidencias_cat_Estatus",
                table: "CatRegistroIncidencias",
                column: "reInciEstatusId",
                principalTable: "cat_Estatus",
                principalColumn: "estID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
