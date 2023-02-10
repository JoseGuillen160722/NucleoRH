using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class Actualiza_Modelo_Departamento_Puestos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "puestoJerarquiaOrden",
                table: "cat_Puestos");

            migrationBuilder.DropColumn(
                name: "DepaJerarquiaSuperiorID",
                table: "cat_Departamentos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "puestoJerarquiaOrden",
                table: "cat_Puestos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepaJerarquiaSuperiorID",
                table: "cat_Departamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
