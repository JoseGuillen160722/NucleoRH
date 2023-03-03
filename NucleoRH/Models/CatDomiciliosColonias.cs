using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatDomiciliosColonias
    {
        public CatDomiciliosColonias()
        {
            CatPatrones = new HashSet<CatPatrones>();
        }

        public int DomiColoId { get; set; }
        public int? DomiColoCp { get; set; }
        public string DomiColoDescripcion { get; set; }
        public int DomiMunicId { get; set; }

        public virtual CatDomiciliosMunicipios DomiMunic { get; set; }
        public virtual ICollection<CatPatrones> CatPatrones { get; set; }
    }
}
