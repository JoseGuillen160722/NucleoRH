using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatVacunacion
    {
        public CatVacunacion()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
        }

        public int VacId { get; set; }
        public string VacDescripcion { get; set; }

        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
    }
}
