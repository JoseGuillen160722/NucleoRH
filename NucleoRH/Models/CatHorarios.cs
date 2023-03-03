using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatHorarios
    {
        public CatHorarios()
        {
            MovEmpleadosAsistencias = new HashSet<MovEmpleadosAsistencias>();
            MovEmpleadosIncidencias = new HashSet<MovEmpleadosIncidencias>();
        }

        public int HoraId { get; set; }
        public TimeSpan? HoraEntrada { get; set; }
        public TimeSpan? HoraComidaSalida { get; set; }
        public TimeSpan? HoraComidaEntrada { get; set; }
        public TimeSpan? HoraSalida { get; set; }
        public TimeSpan? HoraSabadoEntrada { get; set; }
        public TimeSpan? HoraSabadoComidaSalida { get; set; }
        public TimeSpan? HoraSabadoComidaEntrada { get; set; }
        public TimeSpan? HoraSabadoSalida { get; set; }

        public virtual ICollection<MovEmpleadosAsistencias> MovEmpleadosAsistencias { get; set; }
        public virtual ICollection<MovEmpleadosIncidencias> MovEmpleadosIncidencias { get; set; }
        public virtual ICollection<MovEmpleadosHorarios> MovEmpleadosHorarios { get; set; }
        public virtual ICollection<CatDetalleIncidencias> DetallesIncidencias { get; set; }
    }
}
