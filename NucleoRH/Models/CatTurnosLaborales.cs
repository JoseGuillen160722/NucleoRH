using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatTurnosLaborales
    {
        public CatTurnosLaborales()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
        }

        public int TurId { get; set; }
        public string TurDescripcion { get; set; }

        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
        public virtual ICollection<MovRequisicionPersonal> MovRequisicionPersonal { get; set; }
    }
}
