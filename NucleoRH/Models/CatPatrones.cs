using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatPatrones
    {
        public CatPatrones()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
            CatSucursales = new HashSet<CatSucursales>();
        }

        public int PatId { get; set; }
        public string PatAbrev { get; set; }
        public string PatDescripcion { get; set; }
        public string PatRegistro { get; set; }
        public string PatRfc { get; set; }
        public int? PatColoId { get; set; }

        public virtual CatDomiciliosColonias PatColo { get; set; }
        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
        public virtual ICollection<CatSucursales> CatSucursales { get; set; }
    }
}
