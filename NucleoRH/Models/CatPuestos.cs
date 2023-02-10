using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatPuestos
    {
        public CatPuestos()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
        }

        public int PuestoId { get; set; }
        public string PuestoDescripcion { get; set; }
        public int PuestoAreaId { get; set; }
        public int? PuestoJerarquiaSuperiorPuestoId { get; set; }


        public virtual CatAreas PuestoArea { get; set; }
        public virtual CatPuestos PuestoJefe { get; set; }
        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
        public virtual ICollection<CatPlantillas> CatPlantillas { get; set; }
        public virtual ICollection<CatPuestos> CatPuestosSuperiores { get; set; }
        public virtual ICollection<CatHistorialVacaciones> HistorialVacaciones { get; set; }
        public virtual ICollection<MovRequisicionPersonal> RequisicionPersonal { get; set; }
    }
}
