using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewcolumninMovRequisicionPersonalcalledMRPFlujoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_Sucursales",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropIndex(
                name: "IX_mov_RequisicionPersonal_mRPSucursalId",
                table: "mov_RequisicionPersonal");

            migrationBuilder.AddColumn<int>(
                name: "mRPFlujoId",
                table: "mov_RequisicionPersonal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SucursalesSucuId",
                table: "mov_RequisicionPersonal",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPFlujoId",
                table: "mov_RequisicionPersonal",
                column: "mRPFlujoId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_SucursalesSucuId",
                table: "mov_RequisicionPersonal",
                column: "SucursalesSucuId");

            migrationBuilder.AddForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_DetalleFlujo",
                table: "mov_RequisicionPersonal",
                column: "mRPFlujoId",
                principalTable: "cat_DetalleFlujo",
                principalColumn: "detFlujoId");

            migrationBuilder.AddForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_Sucursales_SucursalesSucuId",
                table: "mov_RequisicionPersonal",
                column: "SucursalesSucuId",
                principalTable: "cat_Sucursales",
                principalColumn: "SucuID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_DetalleFlujo",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_Sucursales_SucursalesSucuId",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropIndex(
                name: "IX_mov_RequisicionPersonal_mRPFlujoId",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropIndex(
                name: "IX_mov_RequisicionPersonal_SucursalesSucuId",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropColumn(
                name: "mRPFlujoId",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropColumn(
                name: "SucursalesSucuId",
                table: "mov_RequisicionPersonal");

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
    }
}
