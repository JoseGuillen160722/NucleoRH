using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatSubModulos
    {
        public int SubMId { get; set; }
        public int SubMModuloId { get; set; }
        public string SubMName { get; set; }

        public virtual CatModulos Modulos { get; set; }
        public virtual ICollection<CatUsuariosPermisos> CatUsuariosPermisos { get; set; }
    }
}
