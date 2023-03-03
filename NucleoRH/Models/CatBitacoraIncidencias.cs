using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatBitacoraIncidencias
    {
        public int BitInciId { get; set; }
        public int BitInciReInciId { get; set; }
        public int BitInciDetFlujoId { get; set; }
        public string BitInciUserId { get; set; }
        public DateTime BitInciFecha { get; set; }
        public string BitInciObservaciones { get; set; }

        public virtual CatRegistroIncidencias ReInci { get; set; }
        public virtual CatDetalleFlujo DetFlujos { get; set; }
    }
}
