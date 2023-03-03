using System;
using System.Collections.Generic;


namespace NucleoRH.Models
{
    public class MovEmpleadosHorarios
    {
        public int EmpHoraId { get; set; }
        public DateTime EmpHoraFechaRegistro { get; set; }
        public int EmpHoraEmpId { get; set; }
        public int EmpHoraHorId { get; set; }
        public DateTime? EmpHoraFechaDesde { get; set; }
        public DateTime? EmpHoraFechaHasta { get; set; }
        public string EmpHoraTipoHorario { get; set; }

        public virtual CatEmpleados HoraEmp { get; set; }
        public virtual CatHorarios HoraHor { get; set; }
    }
}
