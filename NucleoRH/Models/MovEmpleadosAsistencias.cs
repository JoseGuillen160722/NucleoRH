using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class MovEmpleadosAsistencias
    {
        public int EmpAsisId { get; set; }
        public int EmpAsisChecId { get; set; }
        public int EmpAsisEmpId { get; set; }
        public int EmpAsisHoraId { get; set; }
        public DateTime? EmpAsisFecha { get; set; }

        public virtual CatChecadores EmpAsisChec { get; set; }
        public virtual CatEmpleados EmpAsisEmp { get; set; }
        public virtual CatHorarios EmpAsisHora { get; set; }
    }
}
