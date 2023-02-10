using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatFlujos
    {
        public int FlujoId { get; set; }
        public string FlujoDescripcion { get; set; }

        public virtual ICollection<CatSolicitudIncidencias> SolicitudIncidencias { get; set; }
        public virtual ICollection<CatDetalleFlujo> DetalleFlujos { get; set; }
    }
}
