using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class MovEmpleadosDomicilios
    {
        public int DomiId { get; set; }
        public int? DomiEmpId { get; set; }
        public string DomiCalle { get; set; }
        public int? DomiNumExt { get; set; }
        public string DomiNumInt { get; set; }
        public int DomiColoId { get; set; }
    }
}
