using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class NewColumnURPSubModuloIdAsFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "uRPSubModuloId",
                table: "cat_UsuariosRolesPermisos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_cat_UsuariosRolesPermisos_uRPSubModuloId",
                table: "cat_UsuariosRolesPermisos",
                column: "uRPSubModuloId");

            migrationBuilder.AddForeignKey(
                name: "FK_cat_UsuariosRolesPermisos_cat_SubModulos",
                table: "cat_UsuariosRolesPermisos",
                column: "uRPSubModuloId",
                principalTable: "cat_SubModulos",
                principalColumn: "subMId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cat_UsuariosRolesPermisos_cat_SubModulos",
                table: "cat_UsuariosRolesPermisos");

            migrationBuilder.DropIndex(
                name: "IX_cat_UsuariosRolesPermisos_uRPSubModuloId",
                table: "cat_UsuariosRolesPermisos");

            migrationBuilder.DropColumn(
                name: "uRPSubModuloId",
                table: "cat_UsuariosRolesPermisos");
        }
    }
}
