using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class newcolumninMRPcalledEmpIdMRP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "mRPEmpId",
                table: "mov_RequisicionPersonal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPEmpId",
                table: "mov_RequisicionPersonal",
                column: "mRPEmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_Empleados",
                table: "mov_RequisicionPersonal",
                column: "mRPEmpId",
                principalTable: "cat_Empleados",
                principalColumn: "empID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_Empleados",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropIndex(
                name: "IX_mov_RequisicionPersonal_mRPEmpId",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropColumn(
                name: "mRPEmpId",
                table: "mov_RequisicionPersonal");
        }
    }
}
