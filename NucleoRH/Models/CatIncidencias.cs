using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatIncidencias
    {
        public CatIncidencias()
        {
            MovEmpleadosIncidencias = new HashSet<MovEmpleadosIncidencias>();
        }

        public int InciId { get; set; }
        public string InciDescripcion { get; set; }

        public virtual ICollection<MovEmpleadosIncidencias> MovEmpleadosIncidencias { get; set; }
        public virtual ICollection<CatRegistroIncidencias> RegistroIncidencias { get; set; }
    }
}
