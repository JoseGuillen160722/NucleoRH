using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatEstatus
    {
        public CatEstatus()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
        }

        public int EstId { get; set; }
        public string EstDescripcion { get; set; }

        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
        public virtual ICollection<CatRegistroIncidencias> CatRegistroIncidencias { get; set; }
        public virtual ICollection<MovRequisicionPersonal> MovRequisicionPersonal { get; set; }
        public virtual ICollection<MovPermisosPorTiempo> MovPermisosPorTiempos { get; set; }
    }
}
