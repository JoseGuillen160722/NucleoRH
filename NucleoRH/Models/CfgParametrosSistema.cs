using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CfgParametrosSistema
    {
        public int ParamId { get; set; }
        public int? ParamMinutosTolerancia { get; set; }
        public int? ParamMinutosComida { get; set; }
        public string ParamGafetteFrente { get; set; }
        public string ParamGafetteReverso { get; set; }
    }
}
