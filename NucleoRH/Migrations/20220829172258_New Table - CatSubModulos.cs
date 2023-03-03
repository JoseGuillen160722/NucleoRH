using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewTableCatSubModulos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_SubModulos",
                columns: table => new
                {
                    subMId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subMModuloId = table.Column<int>(nullable: false),
                    subMName = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_SubModulos", x => x.subMId);
                    table.ForeignKey(
                        name: "FK_cat_SubModulos_cat_Modulos",
                        column: x => x.subMModuloId,
                        principalTable: "cat_Modulos",
                        principalColumn: "moduloId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_SubModulos_subMModuloId",
                table: "cat_SubModulos",
                column: "subMModuloId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_SubModulos");
        }
    }
}
