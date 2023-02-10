using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatEscolaridades
    {
        public CatEscolaridades()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
        }

        public int EscoId { get; set; }
        public string EscoDescripcion { get; set; }

        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
        public virtual ICollection<MovRequisicionPersonal> MovRequisicionPersonals { get; set; }
    }
}
