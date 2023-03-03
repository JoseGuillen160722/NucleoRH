using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatPeriodos
    {
        public int PerId { get; set; }
        public int PerNum { get; set; }
        public DateTime? PerFechaDesde { get; set; }
        public DateTime? PerFechaHasta { get; set; }
        public DateTime? PerCerrado { get; set; }

        public virtual ICollection<CatSaldoDeVacaciones> SaldoDeVacaciones { get; set; }
    }
}
