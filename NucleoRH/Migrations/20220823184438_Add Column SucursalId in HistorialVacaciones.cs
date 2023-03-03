using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class AddColumnSucursalIdinHistorialVacaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "hVSucursalId",
                table: "cat_HistorialVacaciones",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_cat_HistorialVacaciones_hVSucursalId",
                table: "cat_HistorialVacaciones",
                column: "hVSucursalId");

            migrationBuilder.AddForeignKey(
                name: "FK_cat_HistorialVacaciones_cat_Sucursales",
                table: "cat_HistorialVacaciones",
                column: "hVSucursalId",
                principalTable: "cat_Sucursales",
                principalColumn: "SucuID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cat_HistorialVacaciones_cat_Sucursales",
                table: "cat_HistorialVacaciones");

            migrationBuilder.DropIndex(
                name: "IX_cat_HistorialVacaciones_hVSucursalId",
                table: "cat_HistorialVacaciones");

            migrationBuilder.DropColumn(
                name: "hVSucursalId",
                table: "cat_HistorialVacaciones");
        }
    }
}
