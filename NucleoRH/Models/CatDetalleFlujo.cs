using System;
using System.Collections.Generic;


namespace NucleoRH.Models
{
    public class CatDetalleFlujo
    {
        public int DetFlujoId { get; set; }
        public int DetFlujoFlujoId { get; set; }
        public string DetFlujoDescripcion { get; set; }
        public int DetFlujoOrden { get; set; }
        public string DetFlujoPerfiles { get; set; }
        public string DetFlujoCorreoDestino  { get; set; }
        public string DetFlujoEmail { get; set; }


        public virtual CatFlujos Flujo { get; set; }

        public virtual ICollection<CatBitacoraIncidencias> BitacoraIncidencias { get; set; }
        public virtual ICollection<CatRegistroIncidencias> ReInci { get; set; }
        public virtual ICollection<MovRequisicionPersonal> RequisicionPersonalRP { get; set; }
        public virtual ICollection<CatBitacoraRequisicionPersonal> CatBitacoraRequisicionPersonal { get; set; }
    }
}
