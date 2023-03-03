using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatModulos
    {
        public int ModuloId { get; set; }
        public string ModuloNombre { get; set; }

        public virtual ICollection<CatUsuariosPermisos> UsuariosPermisos { get; set; }
        public virtual ICollection<CatSubModulos> SubModulos { get; set; }
    }
}
