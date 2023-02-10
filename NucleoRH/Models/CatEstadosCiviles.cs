using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatEstadosCiviles
    {
        public CatEstadosCiviles()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
        }

        public int EdocId { get; set; }
        public string EdocDescripcion { get; set; }

        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
    }
}
