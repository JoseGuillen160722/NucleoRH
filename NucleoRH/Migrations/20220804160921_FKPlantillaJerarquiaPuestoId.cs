using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class FKPlantillaJerarquiaPuestoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_cat_Puestos_puestoJerarquiaSuperiorPuestoID",
                table: "cat_Puestos",
                column: "puestoJerarquiaSuperiorPuestoID");

            migrationBuilder.AddForeignKey(
                name: "FK_cat_Puestos_cat_Puestos",
                table: "cat_Puestos",
                column: "puestoJerarquiaSuperiorPuestoID",
                principalTable: "cat_Puestos",
                principalColumn: "puestoID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cat_Puestos_cat_Puestos",
                table: "cat_Puestos");

            migrationBuilder.DropIndex(
                name: "IX_cat_Puestos_puestoJerarquiaSuperiorPuestoID",
                table: "cat_Puestos");
        }
    }
}
