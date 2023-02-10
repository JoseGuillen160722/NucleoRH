using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatSaldoDeVacaciones
    {
        public int SVId { get; set; }
        public int SVEmpId { get; set; }
        public int SVEjercicio { get; set; }
        public int SVPeriodoId { get; set; }
        public DateTime SVFechaRegistro { get; set; }
        public int SVAniosAntiguedad { get; set; }
        public int SVAntiId { get; set; }
        public int SVDiasDisfrutados { get; set; }
        public int SVDiasRestantes { get; set; }

        public virtual CatEmpleados SVEmpleados { get; set; }
        public virtual CatPeriodos SVPeriodos { get; set; }
        public virtual CatAntiguedad SVAntiguedad { get; set; }


    }
}
