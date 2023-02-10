using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatBitacoraRequisicionPersonal
    {
        public int BitRPId { get; set; }
        public int BitRPRPId { get; set; }
        public int BitRPDetFlujoId { get; set; }
        public string BitRPUserId { get; set; }
        public DateTime BitRPFecha { get; set; }
        public string BitRPObservaciones { get; set; }

        public virtual MovRequisicionPersonal MovRequisicionPersonal { get; set; }
        public virtual CatDetalleFlujo CatDetalleFlujo { get; set; }
    }
}
