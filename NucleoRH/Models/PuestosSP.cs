using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class PuestosSP
    {
        public int puestoID { get; set; }
        public string puestoDescripcion { get; set; }
        public int puestoAreaID { get; set; }
        public int? puestoJerarquiaSuperiorPuestoID { get; set; }
        public int hijos { get; set; }

    }
}
