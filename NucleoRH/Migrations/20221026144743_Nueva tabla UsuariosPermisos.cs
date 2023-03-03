using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NuevatablaUsuariosPermisos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_UsuariosPermisos",
                columns: table => new
                {
                    uRPId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uRPUserId = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    uRPModuloId = table.Column<int>(nullable: false),
                    uRPSubModuloId = table.Column<int>(nullable: false),
                    uRPCrear = table.Column<bool>(nullable: false),
                    uRPMostrar = table.Column<bool>(nullable: false),
                    uRPModificar = table.Column<bool>(nullable: false),
                    uRPEliminar = table.Column<bool>(nullable: false),
                    uRPAutorizado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_UsuariosPermisos", x => x.uRPId);
                    table.ForeignKey(
                        name: "FK_cat_UsuariosPermisos_cat_Modulos",
                        column: x => x.uRPModuloId,
                        principalTable: "cat_Modulos",
                        principalColumn: "moduloId");
                    table.ForeignKey(
                        name: "FK_cat_UsuariosPermisos_cat_SubModulos",
                        column: x => x.uRPSubModuloId,
                        principalTable: "cat_SubModulos",
                        principalColumn: "subMId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_UsuariosPermisos_uRPModuloId",
                table: "cat_UsuariosPermisos",
                column: "uRPModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_UsuariosPermisos_uRPSubModuloId",
                table: "cat_UsuariosPermisos",
                column: "uRPSubModuloId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_UsuariosPermisos");
        }
    }
}
