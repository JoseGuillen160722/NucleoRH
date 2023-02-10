using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class CatEmpleados
    {
        public CatEmpleados()
        {
            MovEmpleadosAsistencias = new HashSet<MovEmpleadosAsistencias>();
            MovEmpleadosIncidencias = new HashSet<MovEmpleadosIncidencias>();
            MovEmpleadosSueldos = new HashSet<MovEmpleadosSueldos>();
        }

        public int EmpId { get; set; }
        public int? EmpNumero { get; set; }
        public string EmpNombre { get; set; }
        public string EmpPaterno { get; set; }
        public string EmpMaterno { get; set; }
        public decimal? EmpTelefono { get; set; }
        public decimal? EmpCelular { get; set; }
        public string EmpEmail { get; set; }
        public DateTime? EmpFechaIngreso { get; set; }
        public string EmpRfc { get; set; }
        public string EmpCurp { get; set; }
        public string EmpImss { get; set; }
        public int? EmpEscoId { get; set; }
        public int EmpEdocId { get; set; }
        public int EmpSexId { get; set; }
        public int EmpEstId { get; set; }
        public string EmpComentarios { get; set; }
        public int EmpPatId { get; set; }
        public int EmpPuestoId { get; set; }
        public int EmpSucuId { get; set; }
        public int EmpJornaId { get; set; }
        public int? EmpTrabTipoId { get; set; }
        public int EmpTurId { get; set; }
        public DateTime? EmpNacFecha { get; set; }
        public int? EmpNacMunicId { get; set; }
        public int? EmpVacId { get; set; }
        public string EmpUserId { get; set; }
        public string EmpNombreCompleto { get; set; }
        public string EmpAlias { get; set; }
        public int? EmpVacaDias { get; set; }


        public virtual CatEstadosCiviles EmpEdoc { get; set; }
        public virtual CatEscolaridades EmpEsco { get; set; }
        public virtual CatEstatus EmpEst { get; set; }
        public virtual CatJornadasLaborales EmpJorna { get; set; }
        public virtual CatDomiciliosMunicipios EmpNacMunic { get; set; }
        public virtual CatPatrones EmpPat { get; set; }
        public virtual CatPuestos EmpPuesto { get; set; }
        public virtual CatSexos EmpSex { get; set; }
        public virtual CatSucursales EmpSucu { get; set; }
        public virtual CatTrabajadorTipos EmpTrabTipo { get; set; }
        public virtual CatTurnosLaborales EmpTur { get; set; }
        public virtual CatVacunacion EmpVac { get; set; }


        public virtual ICollection<MovEmpleadosAsistencias> MovEmpleadosAsistencias { get; set; }
        public virtual ICollection<MovEmpleadosIncidencias> MovEmpleadosIncidencias { get; set; }
        public virtual ICollection<MovEmpleadosSueldos> MovEmpleadosSueldos { get; set; }
        public virtual ICollection<MovEmpleadosHorarios> MovEmpleadosHorarios { get; set; }
        public virtual ICollection<CatRegistroIncidencias> RegistroIncidencias { get; set; }
        public virtual ICollection<CatHistorialVacaciones> HistorialVacaciones { get; set; }
        public virtual ICollection<CatSaldoDeVacaciones> SaldoDeVacaciones { get; set; }
        public virtual ICollection<CatSolicitudIncidencias> SolicitudIncidencias { get; set; }
        public virtual ICollection<CatEmpTrazabilidad> CatEmpTrazabilidads { get; set; }
        public virtual ICollection<MovRequisicionPersonal> MovRequisicionPersonal { get; set; }
        public virtual ICollection<MovPermisosPorTiempo> MovPermisosPorTiempo { get; set; }



    }
}
