using System;
using System.Collections.Generic;


namespace NucleoRH.Models
{
    public class CatHistorialVacaciones
    {
        public int HVId { get; set; }
        public int HVEmpId { get; set; }
        public int HVAntiguedadId { get; set; }
        public int HVDiasSolicitados { get; set; }
        public int HVEjercicio { get; set; }
        public DateTime HVFechaSolicitud { get; set; }
        public DateTime HVFechaInicio { get; set; }
        public DateTime HVFechaCulminacion { get; set; }
        public DateTime HVFechaPresentacion { get; set; }
        public int HVVacacionesPendientesEjercicioActual { get; set; }
        public int HVVacacionesPendientesEjerciciosPasados { get; set; }
        public int HVReInciId { get; set; }
        public int HVPuestoId { get; set; }
        public int HVSucursalId { get; set; }
        public int HVSaldoVacaciones { get; set; }

        public virtual CatEmpleados HVEmp { get; set; }
        public virtual CatAntiguedad HVAnti { get; set; }
        public virtual CatRegistroIncidencias HVReInci { get; set; }
        public virtual CatPuestos HVPuestos { get; set; }
        public virtual CatSucursales HVSucursales { get; set; }

    }
}