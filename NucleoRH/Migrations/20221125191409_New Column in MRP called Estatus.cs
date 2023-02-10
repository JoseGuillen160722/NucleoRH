using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewColumninMRPcalledEstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "mRPEstatusId",
                table: "mov_RequisicionPersonal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_mov_RequisicionPersonal_mRPEstatusId",
                table: "mov_RequisicionPersonal",
                column: "mRPEstatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_Estatus",
                table: "mov_RequisicionPersonal",
                column: "mRPEstatusId",
                principalTable: "cat_Estatus",
                principalColumn: "estID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mov_RequisicionPersonal_cat_Estatus",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropIndex(
                name: "IX_mov_RequisicionPersonal_mRPEstatusId",
                table: "mov_RequisicionPersonal");

            migrationBuilder.DropColumn(
                name: "mRPEstatusId",
                table: "mov_RequisicionPersonal");
        }
    }
}
