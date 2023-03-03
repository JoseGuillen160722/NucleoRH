using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatAreas
    {
        public CatAreas()
        {
            CatPuestos = new HashSet<CatPuestos>();
        }

        public int AreaId { get; set; }
        public string AreaDescripcion { get; set; }
        public int AreaDepaId { get; set; }

        public virtual CatDepartamentos AreaDepa { get; set; }
        public virtual ICollection<CatPuestos> CatPuestos { get; set; }
    }
}
