using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatSolicitudIncidencias
    {
        public int SolInciId { get; set; }
        public int SolInciReInciId { get; set; }
        public int SolInciEmpId { get; set; }
        public DateTime SolInciFechaRegistro { get; set; }
        public int SolInciPuestoSuperior { get; set; }
        public string SolInciPerfiles { get; set; }
        public int SolInciFlujoId { get; set; }

        public virtual CatEmpleados Empleados { get; set; }
        public virtual CatRegistroIncidencias ReInci { get; set; }
        public virtual CatFlujos Flujos { get; set; }

    }
}
