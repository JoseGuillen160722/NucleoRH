using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewColumninCatDetalleFlujosforvalidationonstepsbyemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "detFlujoCorreoDestino",
                table: "cat_DetalleFlujo",
                unicode: false,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "detFlujoCorreoDestino",
                table: "cat_DetalleFlujo");
        }
    }
}
