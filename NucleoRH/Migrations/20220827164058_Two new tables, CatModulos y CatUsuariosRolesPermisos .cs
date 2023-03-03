using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class TwonewtablesCatModulosyCatUsuariosRolesPermisos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_Modulos",
                columns: table => new
                {
                    moduloId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    moduloNombre = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_Modulos", x => x.moduloId);
                });

            migrationBuilder.CreateTable(
                name: "cat_UsuariosRolesPermisos",
                columns: table => new
                {
                    uRPId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uRPUserId = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    uRPRoleId = table.Column<int>(nullable: false),
                    uRPModuloId = table.Column<int>(nullable: false),
                    uRPCrear = table.Column<bool>(nullable: false),
                    uRPMostrar = table.Column<bool>(nullable: false),
                    uRPModificar = table.Column<bool>(nullable: false),
                    uRPEliminar = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_UsuariosRolesPermisos", x => x.uRPId);
                    table.ForeignKey(
                        name: "FK_cat_UsuariosRolesPermisos_cat_Modulos",
                        column: x => x.uRPModuloId,
                        principalTable: "cat_Modulos",
                        principalColumn: "moduloId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_UsuariosRolesPermisos_uRPModuloId",
                table: "cat_UsuariosRolesPermisos",
                column: "uRPModuloId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_UsuariosRolesPermisos");

            migrationBuilder.DropTable(
                name: "cat_Modulos");
        }
    }
}
