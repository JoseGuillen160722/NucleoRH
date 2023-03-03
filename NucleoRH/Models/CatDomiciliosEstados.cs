using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatDomiciliosEstados
    {
        public CatDomiciliosEstados()
        {
            CatDomiciliosMunicipios = new HashSet<CatDomiciliosMunicipios>();
        }

        public int DomiEstaId { get; set; }
        public string DomiEstaDescripcion { get; set; }
        public string DomiEstaAbrev { get; set; }

        public virtual ICollection<CatDomiciliosMunicipios> CatDomiciliosMunicipios { get; set; }
    }
}
