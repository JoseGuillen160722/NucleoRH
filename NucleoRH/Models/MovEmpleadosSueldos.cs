using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class MovEmpleadosSueldos
    {
        public int EmpSdoId { get; set; }
        public int EmpSdoEmpId { get; set; }
        public DateTime? EmpSdoFechaAlta { get; set; }
        public decimal? EmpSdoSueldoDia { get; set; }
        public decimal? EmpSdoSdi { get; set; }
        public decimal? EmpSdoPremioAsis { get; set; }
        public decimal? EmpSdoPremioPunt { get; set; }
        public decimal? EmpSdoDespensa { get; set; }
        public decimal? EmpSdoApoyoViviTrans { get; set; }
        public decimal? EmpSdoBono { get; set; }

        public virtual CatEmpleados EmpSdoEmp { get; set; }
    }
}
