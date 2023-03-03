using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class newcolumninRequiscionPersonal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "mRPSucursalId",
                table: "mov_RequisicionPersonal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPSucursalId",
                table: "mov_RequisicionPersonal",
                column: "mRPSucursalId");

            migrationBuilder.AddForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_Sucursales",
                table: "mov_RequisicionPersonal",
                column: "mRPSucursalId",
                principalTable: "cat_Sucursales",
                principalColumn: "SucuID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_Sucursales",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropIndex(
                name: "IX_mov_RequisicionPersonal_mRPSucursalId",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropColumn(
                name: "mRPSucursalId",
                table: "mov_RequisicionPersonal");
        }
    }
}
