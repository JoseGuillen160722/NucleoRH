using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatAntiguedad
    {
        public int AntiId { get; set; }
        public int? AntiAniosDesde { get; set; }
        public int? AntiAniosHasta { get; set; }

        //public string AntiAnios { get; set; }
        public int AntiDias { get; set; }

        public virtual ICollection<CatHistorialVacaciones> HistorialVacaciones { get; set; }
        public virtual ICollection<CatSaldoDeVacaciones> SaldoDeVacaciones { get; set; }
    }
}
