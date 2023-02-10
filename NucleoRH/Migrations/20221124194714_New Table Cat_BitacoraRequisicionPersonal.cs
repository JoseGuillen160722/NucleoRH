using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewTableCat_BitacoraRequisicionPersonal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_BitacoraRequisicionPersonal",
                columns: table => new
                {
                    bitRPId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bitRPRPId = table.Column<int>(nullable: false),
                    bitRPDetFlujoId = table.Column<int>(nullable: false),
                    bitRPUserId = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    bitRPFecha = table.Column<DateTime>(nullable: false),
                    bitRPObservaciones = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_BitacoraRequisicionPersonal", x => x.bitRPId);
                    table.ForeignKey(
                        name: "FK_cat_BitacoraRequisicionPersonal_cat_DetalleFlujos",
                        column: x => x.bitRPDetFlujoId,
                        principalTable: "cat_DetalleFlujo",
                        principalColumn: "detFlujoId");
                    table.ForeignKey(
                        name: "FK_cat_BitacoraRequisicionPersonal_mov_RequisicionPersonal",
                        column: x => x.bitRPRPId,
                        principalTable: "mov_RequisicionPersonal",
                        principalColumn: "mRPId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_BitacoraRequisicionPersonal_bitRPDetFlujoId",
                table: "cat_BitacoraRequisicionPersonal",
                column: "bitRPDetFlujoId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_BitacoraRequisicionPersonal_bitRPRPId",
                table: "cat_BitacoraRequisicionPersonal",
                column: "bitRPRPId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_BitacoraRequisicionPersonal");
        }
    }
}
