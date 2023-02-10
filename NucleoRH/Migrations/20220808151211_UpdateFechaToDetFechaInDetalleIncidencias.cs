using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class UpdateFechaToDetFechaInDetalleIncidencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fecha",
                table: "detalleIncidencias");

            migrationBuilder.AddColumn<DateTime>(
                name: "detFecha",
                table: "detalleIncidencias",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "detFecha",
                table: "detalleIncidencias");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha",
                table: "detalleIncidencias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
