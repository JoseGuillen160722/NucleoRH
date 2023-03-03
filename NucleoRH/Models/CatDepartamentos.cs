using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatDepartamentos
    {
        public CatDepartamentos()
        {
            CatAreas = new HashSet<CatAreas>();
        }

        public int DepaId { get; set; }
        public string DepaDescripcion { get; set; }

        public virtual ICollection<CatAreas> CatAreas { get; set; }
        public virtual ICollection<CatPlantillas> CatPlantillas { get; set; }
        
    }
}
