using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class RemovetableURP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_UsuariosRolesPermisos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_UsuariosRolesPermisos",
                columns: table => new
                {
                    uRPId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uRPAutorizado = table.Column<bool>(type: "bit", nullable: false),
                    uRPCrear = table.Column<bool>(type: "bit", nullable: false),
                    uRPEliminar = table.Column<bool>(type: "bit", nullable: false),
                    uRPModificar = table.Column<bool>(type: "bit", nullable: false),
                    uRPModuloId = table.Column<int>(type: "int", nullable: false),
                    uRPMostrar = table.Column<bool>(type: "bit", nullable: false),
                    uRPRoleId = table.Column<int>(type: "int", nullable: false),
                    uRPSubModuloId = table.Column<int>(type: "int", nullable: false),
                    uRPUserId = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_UsuariosRolesPermisos", x => x.uRPId);
                    table.ForeignKey(
                        name: "FK_cat_UsuariosRolesPermisos_cat_Modulos",
                        column: x => x.uRPModuloId,
                        principalTable: "cat_Modulos",
                        principalColumn: "moduloId");
                    table.ForeignKey(
                        name: "FK_cat_UsuariosRolesPermisos_cat_SubModulos",
                        column: x => x.uRPSubModuloId,
                        principalTable: "cat_SubModulos",
                        principalColumn: "subMId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cat_UsuariosRolesPermisos_uRPModuloId",
                table: "cat_UsuariosRolesPermisos",
                column: "uRPModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_UsuariosRolesPermisos_uRPSubModuloId",
                table: "cat_UsuariosRolesPermisos",
                column: "uRPSubModuloId");
        }
    }
}
