using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatEmpTrazabilidad
    {

        public int EmpTrazaId { get; set; }
        public int EmpTrazaEmpId { get; set; }
        public string EmpTrazaPassword { get; set; }

        public virtual CatEmpleados Empleados { get; set; }
    }
}
