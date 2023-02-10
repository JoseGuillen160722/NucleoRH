using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatChecadores
    {
        public CatChecadores()
        {
            MovEmpleadosAsistencias = new HashSet<MovEmpleadosAsistencias>();
        }

        public int ChecId { get; set; }
        public string ChecDescripcion { get; set; }
        public int ChecSucuId { get; set; }
        public string ChecIp { get; set; }
        public string ChecPathDescarga { get; set; }
        public int? ChecMinutosDescarga { get; set; }
        public DateTime? ChecUltimaDescarga { get; set; }

        public virtual CatSucursales ChecSucu { get; set; }
        public virtual ICollection<MovEmpleadosAsistencias> MovEmpleadosAsistencias { get; set; }
    }
}
