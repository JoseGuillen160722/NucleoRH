using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class removewrongcolumninMovRequisicionPersonal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_DetalleFlujo",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropIndex(
                name: "IX_mov_RequisicionPersonal_mRPFlujoId",
                table: "mov_RequisicionPersonal");

            migrationBuilder.RenameColumn(
                name: "mRPFlujoId",
                table: "mov_RequisicionPersonal",
                newName: "MRPFlujoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MRPFlujoId",
                table: "mov_RequisicionPersonal",
                newName: "mRPFlujoId");

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPFlujoId",
                table: "mov_RequisicionPersonal",
                column: "mRPFlujoId");

            migrationBuilder.AddForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_DetalleFlujo",
                table: "mov_RequisicionPersonal",
                column: "mRPFlujoId",
                principalTable: "cat_DetalleFlujo",
                principalColumn: "detFlujoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
