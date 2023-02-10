using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class MovEmpleadosIncidencias
    {
        public int EmpInciId { get; set; }
        public int EmpInciEmpId { get; set; }
        public int EmpInciInciId { get; set; }
        public DateTime? EmpInciFechaDesde { get; set; }
        public DateTime? EmpInciFechaHasta { get; set; }
        public int? EmpInciHoraId { get; set; }
        public string EmpInciObs { get; set; }

        public virtual CatEmpleados EmpInciEmp { get; set; }
        public virtual CatHorarios EmpInciHora { get; set; }
        public virtual CatIncidencias EmpInciInci { get; set; }
    }
}
