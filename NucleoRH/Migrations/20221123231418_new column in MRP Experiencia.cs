using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class newcolumninMRPExperiencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "mRPExperienciaIndispensable",
                table: "mov_RequisicionPersonal",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mRPExperienciaIndispensable",
                table: "mov_RequisicionPersonal");
        }
    }
}
