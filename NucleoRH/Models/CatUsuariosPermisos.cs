using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatUsuariosPermisos
    {

        public int URPId { get; set; }
        public string URPUserId { get; set; }
        public int URPModuloId { get; set; }
        public int URPSubModuloId { get; set; }
        public bool URPCrear { get; set; }
        public bool URPMostrar { get; set; }
        public bool URPModificar { get; set; }
        public bool URPEliminar { get; set; }
        public bool URPAutorizado { get; set; }

        public virtual CatModulos Modulo { get; set; }
        public virtual CatSubModulos SubModulo { get; set; }
       
    }
}
