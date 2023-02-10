using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class FKRegistroIncidenciasToCatEstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_Status");

            migrationBuilder.AddColumn<int>(
                name: "reInciEstatusId",
                table: "CatRegistroIncidencias",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CatRegistroIncidencias_reInciEstatusId",
                table: "CatRegistroIncidencias",
                column: "reInciEstatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_registroIncidencias_cat_Estatus",
                table: "CatRegistroIncidencias",
                column: "reInciEstatusId",
                principalTable: "cat_Estatus",
                principalColumn: "estID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registroIncidencias_cat_Estatus",
                table: "CatRegistroIncidencias");

            migrationBuilder.DropIndex(
                name: "IX_CatRegistroIncidencias_reInciEstatusId",
                table: "CatRegistroIncidencias");

            migrationBuilder.DropColumn(
                name: "reInciEstatusId",
                table: "CatRegistroIncidencias");

            migrationBuilder.CreateTable(
                name: "cat_Status",
                columns: table => new
                {
                    statusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    statusDescripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_Status", x => x.statusID);
                });
        }
    }
}
