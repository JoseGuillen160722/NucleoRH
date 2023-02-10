using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class spConsultaOrganigrama : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE sp_ConsultaOrganigrama 
	                            -- Add the parameters for the stored procedure here
	                            @IdPuesto int
	
                                AS
                                BEGIN
	                            SELECT  cp.[puestoID]
                                ,cp.[puestoDescripcion]
                                ,cp.[puestoAreaID]
                                ,cp.[puestoJerarquiaSuperiorPuestoID]
	                            ,hijos = COUNT(cph.[puestoID])
	                            FROM [Nucleo_RH].[dbo].[cat_Puestos] cp
                                left join [cat_Puestos] as CPH on cph.puestoJerarquiaSuperiorPuestoID = CP.[puestoID]
                                where cp.puestoJerarquiaSuperiorPuestoID = @IdPuesto
                                group by cp.[puestoID]
                                ,cp.[puestoDescripcion]
                                ,cp.[puestoAreaID]
                                ,cp.[puestoJerarquiaSuperiorPuestoID]
                                END";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"DROP PROCEDURE sp_ConsultaOrganigrama";
            migrationBuilder.Sql(procedure);
        }
    }
}
