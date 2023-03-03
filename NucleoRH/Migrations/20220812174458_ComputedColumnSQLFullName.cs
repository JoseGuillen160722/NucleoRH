using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class ComputedColumnSQLFullName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmpNombreCompleto",
                table: "cat_Empleados",
                nullable: true,
                computedColumnSql: "empNombre + ' ' + empPaterno + ' ' + empMaterno");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpNombreCompleto",
                table: "cat_Empleados");
        }
    }
}
