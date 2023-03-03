using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewcolumninHistorialVacaciones2newcolumnsinAntiguedad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "antiAnios",
                table: "cat_Antiguedad");

            migrationBuilder.AddColumn<int>(
                name: "hVSaldoVacaciones",
                table: "cat_HistorialVacaciones",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "antiAniosDesde",
                table: "cat_Antiguedad",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "antiAniosHasta",
                table: "cat_Antiguedad",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hVSaldoVacaciones",
                table: "cat_HistorialVacaciones");

            migrationBuilder.DropColumn(
                name: "antiAniosDesde",
                table: "cat_Antiguedad");

            migrationBuilder.DropColumn(
                name: "antiAniosHasta",
                table: "cat_Antiguedad");

            migrationBuilder.AddColumn<string>(
                name: "antiAnios",
                table: "cat_Antiguedad",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);
        }
    }
}
