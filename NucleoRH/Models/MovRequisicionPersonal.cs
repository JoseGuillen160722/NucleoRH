using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class MovRequisicionPersonal
    {
        public int MRPId { get; set; }
        public int MRPNumeroVacantes { get; set; }
        public int MRPSucursalId { get; set; }
        public DateTime MRPFechaElaboracion { get; set; }
        public DateTime? MRPFechaRecepcion { get; set; }
        public int? MRPFolio { get; set; }
        public int MRPPuestoId { get; set; }
        public int MRPTipoVacante { get; set; }
        public int MRPTurnoId { get; set; }
        public bool MRPRolarTurno { get; set; }
        public float MRPTiempoAlimentos { get; set; }
        public int MRPMotivoVacante { get; set; }
        public string MRPMotivoDescripcion { get; set; }
        public int MRPSexoId { get; set; }
        public int MRPEdadMinima { get; set; }
        public int MRPEdadMaxima { get; set; }
        public int MRPEscolaridadId { get; set; }
        public bool MRPTituloIndispensable { get; set; }
        public bool MRPCedulaIndispensable { get; set; }
        public bool MRPExperienciaIndispensable { get; set; }
        public string MRPFuncionesPuesto { get; set; }
        public string MRPConocimientosPuesto { get; set; }
        public float? MRPSueldoMensualInicial { get; set; }
        public float? MRPSueldoMensualPlanta { get; set; }
        public float? MRPSueldoMensualMasCosto { get; set; }
        public float? MRPBonoVariable { get; set; }
        public int? MRPEsquemaContratacion { get; set; }
        public string? MRPCandidato { get; set; }
        public DateTime? MRPFechaIngreso { get; set; }
        public int MRPFlujoId { get; set; }
        public int MRPEstatusId { get; set; }
        public string MRPUserId { get; set; }
        public int MRPEmpId { get; set; }

        public virtual CatPuestos Puestos { get; set; }
        public virtual CatTurnosLaborales Turnos { get; set; }
        public virtual CatSexos Sexos { get; set; }
        public virtual CatEscolaridades Escolaridad { get; set; }
        public virtual CatSucursales Sucursales { get; set; }
        public virtual CatDetalleFlujo DetalleFlujo { get; set; }
        public virtual CatEstatus Estatus { get; set; }
        public virtual CatEmpleados Empleados { get; set; }

        public virtual ICollection<CatBitacoraRequisicionPersonal> CatBitacoraRequisicionPersonal { get; set; }

    }
}
