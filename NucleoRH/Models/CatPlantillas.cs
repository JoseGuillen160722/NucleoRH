using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatPlantillas
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int PlantiId { get; set; }

        // Claves externas

        public int PlantiDepaId { get; set; }
        public int PlantiSucuId { get; set; }
        public int PlantiPuestoId { get; set; }

        // Objetos que representan la clave externa

        public virtual CatDepartamentos PlanDepa { get; set;}
        public virtual CatSucursales PlanSucu { get; set; }
        public virtual CatPuestos PlanPuesto { get; set; }
        
    }
}
