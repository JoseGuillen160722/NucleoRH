using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class FkPlantllas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_cat_Plantillas_plantiDepaID",
                table: "cat_Plantillas",
                column: "plantiDepaID");

            migrationBuilder.CreateIndex(
                name: "IX_cat_Plantillas_plantiPuestoID",
                table: "cat_Plantillas",
                column: "plantiPuestoID");

            migrationBuilder.CreateIndex(
                name: "IX_cat_Plantillas_plantiSucuID",
                table: "cat_Plantillas",
                column: "plantiSucuID");

            migrationBuilder.AddForeignKey(
                name: "FK_cat_Plantilla_cat_Departamentos",
                table: "cat_Plantillas",
                column: "plantiDepaID",
                principalTable: "cat_Departamentos",
                principalColumn: "depaID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cat_Plantilla_cat_Puestos",
                table: "cat_Plantillas",
                column: "plantiPuestoID",
                principalTable: "cat_Puestos",
                principalColumn: "puestoID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cat_Plantilla_cat_Sucursales",
                table: "cat_Plantillas",
                column: "plantiSucuID",
                principalTable: "cat_Sucursales",
                principalColumn: "SucuID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cat_Plantilla_cat_Departamentos",
                table: "cat_Plantillas");

            migrationBuilder.DropForeignKey(
                name: "FK_cat_Plantilla_cat_Puestos",
                table: "cat_Plantillas");

            migrationBuilder.DropForeignKey(
                name: "FK_cat_Plantilla_cat_Sucursales",
                table: "cat_Plantillas");

            migrationBuilder.DropIndex(
                name: "IX_cat_Plantillas_plantiDepaID",
                table: "cat_Plantillas");

            migrationBuilder.DropIndex(
                name: "IX_cat_Plantillas_plantiPuestoID",
                table: "cat_Plantillas");

            migrationBuilder.DropIndex(
                name: "IX_cat_Plantillas_plantiSucuID",
                table: "cat_Plantillas");
        }
    }
}
