using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public partial class CatRegistroIncidencias
    {
        public int ReInciId { get; set; }
        public int ReInciEmpId { get; set; }
        public DateTime Fecha { get; set; }
        public int ReInciInciId { get; set; }
        public int ReInciEstatusId { get; set; }
        public int ReInciEstatusFlujo { get; set; }

        public virtual CatEmpleados Emp { get; set; }
        public virtual CatIncidencias Inci { get; set; }
        public virtual CatEstatus Estatus { get; set; }
        public virtual CatDetalleFlujo DetFlujo { get; set; }

        public virtual ICollection<CatDetalleIncidencias> DetalleIncidencias { get; set; }
        public virtual ICollection<CatHistorialVacaciones> HistorialVacaciones { get; set; }
        public virtual ICollection<CatSolicitudIncidencias> SolicitudIncidencias { get; set; }
        public virtual ICollection<CatBitacoraIncidencias> BitacoraIncidencias { get; set; }
        public virtual ICollection<MovPermisosPorTiempo> MovPermisosPorTiempo { get; set; }

    }
}