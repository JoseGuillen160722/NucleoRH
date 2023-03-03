using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatDomiciliosMunicipios
    {
        public CatDomiciliosMunicipios()
        {
            CatDomiciliosColonias = new HashSet<CatDomiciliosColonias>();
            CatEmpleados = new HashSet<CatEmpleados>();
        }

        public int DomiMunicId { get; set; }
        public string DomiMunicDescripcion { get; set; }
        public int DomiMunicEstaId { get; set; }

        public virtual CatDomiciliosEstados DomiMunicEsta { get; set; }
        public virtual ICollection<CatDomiciliosColonias> CatDomiciliosColonias { get; set; }
        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
    }
}
