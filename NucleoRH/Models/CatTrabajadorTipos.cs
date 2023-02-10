using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatTrabajadorTipos
    {
        public CatTrabajadorTipos()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
        }

        public int TrabTipoId { get; set; }
        public string TrabTipoDescripcion { get; set; }

        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
    }
}
