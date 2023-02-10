using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class FKsetincat_SaldoDeVacaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_cat_SaldoDeVacaciones_sVPeriodoId",
                table: "cat_SaldoDeVacaciones",
                column: "sVPeriodoId");

            migrationBuilder.AddForeignKey(
                name: "FK_cat_SaldoDeVacaciones_cat_Periodos",
                table: "cat_SaldoDeVacaciones",
                column: "sVPeriodoId",
                principalTable: "cat_Periodos",
                principalColumn: "perID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cat_SaldoDeVacaciones_cat_Periodos",
                table: "cat_SaldoDeVacaciones");

            migrationBuilder.DropIndex(
                name: "IX_cat_SaldoDeVacaciones_sVPeriodoId",
                table: "cat_SaldoDeVacaciones");
        }
    }
}
