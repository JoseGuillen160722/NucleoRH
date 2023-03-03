using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatSexos
    {
        public CatSexos()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
        }

        public int SexId { get; set; }
        public string SexDescripcion { get; set; }

        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
        public virtual ICollection<MovRequisicionPersonal> MovRequisicionPersonals { get; set; }
    }
}
