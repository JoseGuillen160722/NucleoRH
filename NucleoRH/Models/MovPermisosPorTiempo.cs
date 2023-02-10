using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class MovPermisosPorTiempo
    {
        public int PPTId {get;set;}
        public int PPTEmpId { get; set; }
        public DateTime PPTFechaInicio { get; set; }
        public DateTime PPTFechaFinal { get; set; }
        public TimeSpan? PPTHoraEntrada { get; set; }
        public TimeSpan? PPTHoraSalida { get; set; }
        public TimeSpan? PPTHoraSalidaComida { get; set; }
        public TimeSpan? PPTHoraRegresoComida { get; set; }
        public int PPTEstatusId { get; set; }
        public int PPTReInciId { get; set; }

        public virtual CatEmpleados Empleados { get; set; }
        public virtual CatEstatus Estatus { get; set; }
        public virtual CatRegistroIncidencias ReInci { get; set; }
    }
}
