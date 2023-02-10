using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class ModeloReporte
    {
        public int ReporteEmpId { get; set; }
        public int ReporteIncidenciaId { get; set; }
        public int ReporteSucursalId { get; set; }
        public DateTime ReporteFecha { get; set; }
        public DateTime ReporteFechaDesde { get; set; }
        public DateTime ReporteFechaHasta { get; set; }
        public bool ReporteRadioSucursal { get; set; }
        public bool ReporteRadioEmpleado { get; set; }
        public bool ReporteCheckBoxAllSucursales { get; set; }
        public bool ReporteCheckBoxAllPermisos { get; set; }
        public bool ReporteRadioExcel { get; set; }
        public bool ReporteRadioPDF { get; set; }
    }
}
