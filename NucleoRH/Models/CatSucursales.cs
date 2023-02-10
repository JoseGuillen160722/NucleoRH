using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatSucursales
    {
        public CatSucursales()
        {
            CatChecadores = new HashSet<CatChecadores>();
            CatEmpleados = new HashSet<CatEmpleados>();
        }

        public int SucuId { get; set; }
        public string SucuNcorto { get; set; }
        public string SucuNombre { get; set; }
        public int? SucuPatId { get; set; }
        public string SucuEmail { get; set; }

        public virtual CatPatrones SucuPat { get; set; }
        public virtual ICollection<CatChecadores> CatChecadores { get; set; }
        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
        public virtual ICollection<CatPlantillas> CatPlantillas { get; set; }
        public virtual ICollection<CatHistorialVacaciones> HistorialVacaciones { get; set; }
        public virtual ICollection<MovRequisicionPersonal> RequisicionPersonal { get; set; }
    }
}
