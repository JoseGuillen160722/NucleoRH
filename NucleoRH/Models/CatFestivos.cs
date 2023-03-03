using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatFestivos
    {
        public int FestId { get; set; }
        public string FestDescripcion { get; set; }
        public DateTime? FestFechaDesde { get; set; }
        public DateTime? FestFechaHasta { get; set; }
        public int? FestGuardia { get; set; }
        public string FestObservaciones { get; set; }
    }
}
