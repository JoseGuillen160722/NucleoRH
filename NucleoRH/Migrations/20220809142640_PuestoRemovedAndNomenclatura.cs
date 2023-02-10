using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class PuestoRemovedAndNomenclatura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "permiso",
                table: "detalleIncidencias");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "permiso",
                table: "detalleIncidencias",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
