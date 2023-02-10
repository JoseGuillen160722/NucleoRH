using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class Add2ColumnsInHistorialVacacionesReInciIdandPuestosId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HVReInciId",
                table: "cat_HistorialVacaciones",
                newName: "hVReInciId");

            migrationBuilder.RenameIndex(
                name: "IX_cat_HistorialVacaciones_HVReInciId",
                table: "cat_HistorialVacaciones",
                newName: "IX_cat_HistorialVacaciones_hVReInciId");

            migrationBuilder.AddColumn<int>(
                name: "hVPuestoId",
                table: "cat_HistorialVacaciones",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_cat_HistorialVacaciones_hVPuestoId",
                table: "cat_HistorialVacaciones",
                column: "hVPuestoId");

            migrationBuilder.AddForeignKey(
                name: "FK_cat_HistorialVacaciones_cat_Puestos",
                table: "cat_HistorialVacaciones",
                column: "hVPuestoId",
                principalTable: "cat_Puestos",
                principalColumn: "puestoID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cat_HistorialVacaciones_cat_Puestos",
                table: "cat_HistorialVacaciones");

            migrationBuilder.DropIndex(
                name: "IX_cat_HistorialVacaciones_hVPuestoId",
                table: "cat_HistorialVacaciones");

            migrationBuilder.DropColumn(
                name: "hVPuestoId",
                table: "cat_HistorialVacaciones");

            migrationBuilder.RenameColumn(
                name: "hVReInciId",
                table: "cat_HistorialVacaciones",
                newName: "HVReInciId");

            migrationBuilder.RenameIndex(
                name: "IX_cat_HistorialVacaciones_hVReInciId",
                table: "cat_HistorialVacaciones",
                newName: "IX_cat_HistorialVacaciones_HVReInciId");
        }
    }
}
