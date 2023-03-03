using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewColumntoRegistroIncidencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "reInciEstatusFlujo",
                table: "CatRegistroIncidencias",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CatRegistroIncidencias_reInciEstatusFlujo",
                table: "CatRegistroIncidencias",
                column: "reInciEstatusFlujo");

            migrationBuilder.AddForeignKey(
                name: "FK_cat_RegistroIncidencias_cat_DetalleFlujo",
                table: "CatRegistroIncidencias",
                column: "reInciEstatusFlujo",
                principalTable: "cat_DetalleFlujo",
                principalColumn: "detFlujoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cat_RegistroIncidencias_cat_DetalleFlujo",
                table: "CatRegistroIncidencias");

            migrationBuilder.DropIndex(
                name: "IX_CatRegistroIncidencias_reInciEstatusFlujo",
                table: "CatRegistroIncidencias");

            migrationBuilder.DropColumn(
                name: "reInciEstatusFlujo",
                table: "CatRegistroIncidencias");
        }
    }
}
