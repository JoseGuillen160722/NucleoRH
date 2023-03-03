using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewColumnInPPT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "pPTReInciId",
                table: "mov_PermisosPorTiempo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_mov_PermisosPorTiempo_pPTReInciId",
                table: "mov_PermisosPorTiempo",
                column: "pPTReInciId");

            migrationBuilder.AddForeignKey(
                name: "FK_mov_PermisosPorTiempo_cat_RegistroIncidencias",
                table: "mov_PermisosPorTiempo",
                column: "pPTReInciId",
                principalTable: "CatRegistroIncidencias",
                principalColumn: "reInciId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mov_PermisosPorTiempo_cat_RegistroIncidencias",
                table: "mov_PermisosPorTiempo");

            migrationBuilder.DropIndex(
                name: "IX_mov_PermisosPorTiempo_pPTReInciId",
                table: "mov_PermisosPorTiempo");

            migrationBuilder.DropColumn(
                name: "pPTReInciId",
                table: "mov_PermisosPorTiempo");
        }
    }
}
