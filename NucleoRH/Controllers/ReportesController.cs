using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NucleoRH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;

namespace NucleoRH.Controllers
{
    public class ReportesController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        public ReportesController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }
        public class ListaIncidenciasFiltradasPorSucursal
        {
            public int ReInciId { get; set; }
            public int ReInciEmpId { get; set; }
            public string ReInciEmpNombreCompleto { get; set; }
            public int? ReInciEmpNumero { get; set; }
            public DateTime Fecha { get; set; }
            public int ReInciInciId { get; set; }
            public string ReInciInciDescripcion { get; set; }
            public int ReInciEstatusId { get; set; }
            public string ReInciEstatusDescripcion { get; set; }
            public int ReInciSucuId { get; set; }
            public string ReInciSucuNombre { get; set; }
            public int DetInciHorarioId { get; set; }
            public string DetInciHorarioDetalles { get; set; }
            public string DetInciMedidaAccion { get; set; }
            public string DetInciAsunto { get; set; }
            public string DetInciDestino { get; set; }
            public string DetInciTelDestino { get; set; }
            public string DetInciContacto1 { get; set; }
            public string DetInciNombreDestino { get; set; }
            public string DetInciContacto2 { get; set; }
            public string DetInciObservaciones { get; set; }
            public TimeSpan DetInciHoraSalida { get; set; }
            public TimeSpan DetInciHoraRegreso { get; set; }
            public DateTime DetInciFechaInicio { get; set; }
            public DateTime DetInciFechaFinal { get; set; }
            public DateTime DetInciFechaPresentacion { get; set; }
            public int DetInciDiasAusencia { get; set; }
            public string DetInciPersonaCubrira { get; set; }
            public string DetInciMotivo { get; set; }
            public DateTime DetInciDetFecha { get; set; }
            public string DetInciFechaFinPermisoPersonal { get; set; }
            public string DetInciHoraIngresoPermiso { get; set; }
            public string DetInciHoraSalidaPermiso { get; set; }
            public string DetInciHoraSalidaComida { get; set; }
            public string DetInciHoraRegresoComida { get; set; }


        }

        [Authorize]
        public IActionResult Index()
        {
            var Sucursales = _context.CatSucursales.OrderBy(x => x.SucuNombre).ToList();
            ViewData["Sucursales"] = new SelectList(Sucursales, "SucuId", "SucuNombre");
            var Permisos = _context.CatIncidencias.Where(x => x.InciDescripcion != "FALTAS" && x.InciDescripcion != "RETARDOS" && x.InciDescripcion != "INCAPACIDADES").ToList();
            ViewData["Permisos"] = new SelectList(Permisos, "InciId", "InciDescripcion");
            return View();
        }

        [HttpGet("PermisosReportes")]
        public async Task<IActionResult> PermisosReportes()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 20 && x.URPUserId == user.Id).ToList();
            bool BanderaPermisos = false;

            if (ListaPermisos.Count == 0)
            {
                BanderaPermisos = true;
            }
            var obj = new
            {
                ListaPermisos,
                BanderaPermisos
            };

            return Ok(obj);
        }

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [HttpGet("ReporteVistaPrevia/{IdSucursal}/{NoNomina}/{FechaInicio}/{FechaFinal}/{IdPermiso}/{TodasSucursales}/{TodosPermisos}")]
        public IActionResult ReporteVistaPrevia(int IdSucursal, int NoNomina, DateTime FechaInicio, DateTime FechaFinal, int IdPermiso, bool TodasSucursales, bool TodosPermisos)
        {
            try
            {
                bool BanderaFiltroPorPermiso = false;
                var TipoDePermiso = _context.CatIncidencias.OrderBy(x => x.InciId).ToList();
                CatIncidencias Permiso = new CatIncidencias();
                string DescripcionPermiso = "";

                if (IdPermiso != 0)
                {
                   Permiso = TipoDePermiso.Where(x => x.InciId == IdPermiso).FirstOrDefault();
                    DescripcionPermiso = Permiso.InciDescripcion;
                }
                if (TodosPermisos == true)
                {
                    DescripcionPermiso = "Todos";
                }
                var IncidenciasBySucursal = (from reinci in _context.CatRegistroIncidencias
                                             join detinci in _context.CatDetalleIncidencias on reinci.ReInciId equals detinci.DetInciReInciId
                                             join inci in _context.CatIncidencias on reinci.ReInciInciId equals inci.InciId
                                             join estatus in _context.CatEstatus on reinci.ReInciEstatusId equals estatus.EstId
                                             join emp in _context.CatEmpleados on reinci.ReInciEmpId equals emp.EmpId
                                             join sucu in _context.CatSucursales on emp.EmpSucuId equals sucu.SucuId
                                             join horario in _context.CatHorarios on detinci.DetInciHorarioId equals horario.HoraId
                                             select new ListaIncidenciasFiltradasPorSucursal
                                             {
                                                 ReInciId = reinci.ReInciId,
                                                 ReInciEmpId = emp.EmpId,
                                                 ReInciEmpNombreCompleto = emp.EmpNombreCompleto,
                                                 ReInciEmpNumero = emp.EmpNumero,
                                                 Fecha = reinci.Fecha,
                                                 ReInciInciId = reinci.ReInciInciId,
                                                 ReInciInciDescripcion = inci.InciDescripcion,
                                                 ReInciEstatusId = estatus.EstId,
                                                 ReInciEstatusDescripcion = estatus.EstDescripcion,
                                                 ReInciSucuId = sucu.SucuId,
                                                 ReInciSucuNombre = sucu.SucuNombre,
                                                 DetInciHorarioId = horario.HoraId,
                                                 DetInciMedidaAccion = detinci.MedidaAccion,
                                                 DetInciAsunto = detinci.Asunto,
                                                 DetInciDestino = detinci.Destino,
                                                 DetInciTelDestino = detinci.TelDestino,
                                                 DetInciContacto1 = detinci.Contacto1,
                                                 DetInciContacto2 = detinci.Contacto2,
                                                 DetInciObservaciones = detinci.Observaciones,
                                                 DetInciHoraSalida = detinci.HoraSalida,
                                                 DetInciHoraRegreso = detinci.HoraRegreso,
                                                 DetInciFechaInicio = detinci.FechaInicio,
                                                 DetInciFechaFinal = detinci.FechaFinal,
                                                 DetInciFechaPresentacion = detinci.FechaPresentacion,
                                                 DetInciDiasAusencia = detinci.DiasAusencia,
                                                 DetInciPersonaCubrira = detinci.PersonaCubrira,
                                                 DetInciNombreDestino = detinci.NombreDestino,
                                                 DetInciMotivo = detinci.Motivo,
                                                 DetInciDetFecha = detinci.DetFecha,
                                                 DetInciHorarioDetalles = "De Lunes a Viernes de: " + horario.HoraEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSalida.Value.ToString("hh':'mm':'ss") + " y los sábados de : " + horario.HoraSabadoEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSabadoSalida.Value.ToString("hh':'mm':'ss"),
                                                 DetInciFechaFinPermisoPersonal = detinci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString(),
                                                 DetInciHoraIngresoPermiso = detinci.DetInciHoraIngreso.ToString(),
                                                 DetInciHoraSalidaPermiso = detinci.DetInciHoraSalida.ToString(),
                                                 DetInciHoraSalidaComida = detinci.DetInciHoraSalidaComida.ToString(),
                                                 DetInciHoraRegresoComida = detinci.DetInciHoraRegresoComida.ToString()

                                             }).OrderBy(x => x.Fecha).ThenBy(x => x.ReInciInciDescripcion).ToList();

                if (IdSucursal == 0 && NoNomina !=0) // Validación por Empleado
                {
                    var Empleado = _context.CatEmpleados.Where(x => x.EmpNumero == NoNomina).FirstOrDefault();
                    if(Empleado != null)
                    {
                        if(TodosPermisos == true) // Sin filtro por permiso
                        {
                            IncidenciasBySucursal = (from reinci in _context.CatRegistroIncidencias
                                                         join detinci in _context.CatDetalleIncidencias on reinci.ReInciId equals detinci.DetInciReInciId
                                                         join inci in _context.CatIncidencias on reinci.ReInciInciId equals inci.InciId
                                                         join estatus in _context.CatEstatus on reinci.ReInciEstatusId equals estatus.EstId
                                                         join emp in _context.CatEmpleados on reinci.ReInciEmpId equals emp.EmpId
                                                         join sucu in _context.CatSucursales on emp.EmpSucuId equals sucu.SucuId
                                                         join horario in _context.CatHorarios on detinci.DetInciHorarioId equals horario.HoraId
                                                         where (reinci.Fecha >= FechaInicio && reinci.Fecha <= FechaFinal && emp.EmpNumero == NoNomina)
                                                         select new ListaIncidenciasFiltradasPorSucursal
                                                         {
                                                             ReInciId = reinci.ReInciId,
                                                             ReInciEmpId = emp.EmpId,
                                                             ReInciEmpNombreCompleto = emp.EmpNombreCompleto,
                                                             ReInciEmpNumero = emp.EmpNumero,
                                                             Fecha = reinci.Fecha,
                                                             ReInciInciId = reinci.ReInciInciId,
                                                             ReInciInciDescripcion = inci.InciDescripcion,
                                                             ReInciEstatusId = estatus.EstId,
                                                             ReInciEstatusDescripcion = estatus.EstDescripcion,
                                                             ReInciSucuId = sucu.SucuId,
                                                             ReInciSucuNombre = sucu.SucuNombre,
                                                             DetInciHorarioId = horario.HoraId,
                                                             DetInciMedidaAccion = detinci.MedidaAccion,
                                                             DetInciAsunto = detinci.Asunto,
                                                             DetInciDestino = detinci.Destino,
                                                             DetInciTelDestino = detinci.TelDestino,
                                                             DetInciContacto1 = detinci.Contacto1,
                                                             DetInciContacto2 = detinci.Contacto2,
                                                             DetInciObservaciones = detinci.Observaciones,
                                                             DetInciHoraSalida = detinci.HoraSalida,
                                                             DetInciHoraRegreso = detinci.HoraRegreso,
                                                             DetInciFechaInicio = detinci.FechaInicio,
                                                             DetInciFechaFinal = detinci.FechaFinal,
                                                             DetInciFechaPresentacion = detinci.FechaPresentacion,
                                                             DetInciDiasAusencia = detinci.DiasAusencia,
                                                             DetInciPersonaCubrira = detinci.PersonaCubrira,
                                                             DetInciNombreDestino = detinci.NombreDestino,
                                                             DetInciMotivo = detinci.Motivo,
                                                             DetInciDetFecha = detinci.DetFecha,
                                                             DetInciHorarioDetalles = "De Lunes a Viernes de: " + horario.HoraEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSalida.Value.ToString("hh':'mm':'ss") + " y los sábados de : " + horario.HoraSabadoEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSabadoSalida.Value.ToString("hh':'mm':'ss"),
                                                             DetInciFechaFinPermisoPersonal = detinci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString(),
                                                             DetInciHoraIngresoPermiso = detinci.DetInciHoraIngreso.ToString(),
                                                             DetInciHoraSalidaPermiso = detinci.DetInciHoraSalida.ToString(),
                                                             DetInciHoraSalidaComida = detinci.DetInciHoraSalidaComida.ToString(),
                                                             DetInciHoraRegresoComida = detinci.DetInciHoraRegresoComida.ToString()
                                                         }).OrderByDescending(x=> x.ReInciInciDescripcion).ThenBy(x=> x.Fecha).ToList();
                            BanderaFiltroPorPermiso = false;
                        }
                        else // Con filtro por permiso
                        {
                             IncidenciasBySucursal = (from reinci in _context.CatRegistroIncidencias
                                                         join detinci in _context.CatDetalleIncidencias on reinci.ReInciId equals detinci.DetInciReInciId
                                                         join inci in _context.CatIncidencias on reinci.ReInciInciId equals inci.InciId
                                                         join estatus in _context.CatEstatus on reinci.ReInciEstatusId equals estatus.EstId
                                                         join emp in _context.CatEmpleados on reinci.ReInciEmpId equals emp.EmpId
                                                         join sucu in _context.CatSucursales on emp.EmpSucuId equals sucu.SucuId
                                                         join horario in _context.CatHorarios on detinci.DetInciHorarioId equals horario.HoraId
                                                         where (reinci.Fecha >= FechaInicio && reinci.Fecha <= FechaFinal && emp.EmpNumero == NoNomina && reinci.ReInciInciId == IdPermiso)
                                                         select new ListaIncidenciasFiltradasPorSucursal
                                                         {
                                                             ReInciId = reinci.ReInciId,
                                                             ReInciEmpId = emp.EmpId,
                                                             ReInciEmpNombreCompleto = emp.EmpNombreCompleto,
                                                             ReInciEmpNumero = emp.EmpNumero,
                                                             Fecha = reinci.Fecha,
                                                             ReInciInciId = reinci.ReInciInciId,
                                                             ReInciInciDescripcion = inci.InciDescripcion,
                                                             ReInciEstatusId = estatus.EstId,
                                                             ReInciEstatusDescripcion = estatus.EstDescripcion,
                                                             ReInciSucuId = sucu.SucuId,
                                                             ReInciSucuNombre = sucu.SucuNombre,
                                                             DetInciHorarioId = horario.HoraId,
                                                             DetInciMedidaAccion = detinci.MedidaAccion,
                                                             DetInciAsunto = detinci.Asunto,
                                                             DetInciDestino = detinci.Destino,
                                                             DetInciTelDestino = detinci.TelDestino,
                                                             DetInciContacto1 = detinci.Contacto1,
                                                             DetInciContacto2 = detinci.Contacto2,
                                                             DetInciObservaciones = detinci.Observaciones,
                                                             DetInciHoraSalida = detinci.HoraSalida,
                                                             DetInciHoraRegreso = detinci.HoraRegreso,
                                                             DetInciFechaInicio = detinci.FechaInicio,
                                                             DetInciFechaFinal = detinci.FechaFinal,
                                                             DetInciFechaPresentacion = detinci.FechaPresentacion,
                                                             DetInciDiasAusencia = detinci.DiasAusencia,
                                                             DetInciPersonaCubrira = detinci.PersonaCubrira,
                                                             DetInciNombreDestino = detinci.NombreDestino,
                                                             DetInciMotivo = detinci.Motivo,
                                                             DetInciDetFecha = detinci.DetFecha,
                                                             DetInciHorarioDetalles = "De Lunes a Viernes de: " + horario.HoraEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSalida.Value.ToString("hh':'mm':'ss") + " y los sábados de : " + horario.HoraSabadoEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSabadoSalida.Value.ToString("hh':'mm':'ss"),
                                                             DetInciFechaFinPermisoPersonal = detinci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString(),
                                                             DetInciHoraIngresoPermiso = detinci.DetInciHoraIngreso.ToString(),
                                                             DetInciHoraSalidaPermiso = detinci.DetInciHoraSalida.ToString(),
                                                             DetInciHoraSalidaComida = detinci.DetInciHoraSalidaComida.ToString(),
                                                             DetInciHoraRegresoComida = detinci.DetInciHoraRegresoComida.ToString()
                                                         }).OrderBy(x => x.Fecha).ThenBy(x => x.ReInciInciDescripcion).ToList();
                            BanderaFiltroPorPermiso = true;
                        }
                    }
                }
                if(NoNomina == 0 && IdSucursal !=0 || NoNomina == 0 && TodasSucursales == true) // Validación por Sucursal
                {
                    if(TodasSucursales == true)
                    {
                        if(TodosPermisos == true) // Todos los permisos TODAS LAS SUCURSALES
                        {
                             IncidenciasBySucursal = (from reinci in _context.CatRegistroIncidencias
                                                         join detinci in _context.CatDetalleIncidencias on reinci.ReInciId equals detinci.DetInciReInciId
                                                         join inci in _context.CatIncidencias on reinci.ReInciInciId equals inci.InciId
                                                         join estatus in _context.CatEstatus on reinci.ReInciEstatusId equals estatus.EstId
                                                         join emp in _context.CatEmpleados on reinci.ReInciEmpId equals emp.EmpId
                                                         join sucu in _context.CatSucursales on emp.EmpSucuId equals sucu.SucuId
                                                         join horario in _context.CatHorarios on detinci.DetInciHorarioId equals horario.HoraId
                                                         where (reinci.Fecha >= FechaInicio && reinci.Fecha <= FechaFinal)
                                                         select new ListaIncidenciasFiltradasPorSucursal
                                                         {
                                                             ReInciId = reinci.ReInciId,
                                                             ReInciEmpId = emp.EmpId,
                                                             ReInciEmpNombreCompleto = emp.EmpNombreCompleto,
                                                             ReInciEmpNumero = emp.EmpNumero,
                                                             Fecha = reinci.Fecha,
                                                             ReInciInciId = reinci.ReInciInciId,
                                                             ReInciInciDescripcion = inci.InciDescripcion,
                                                             ReInciEstatusId = estatus.EstId,
                                                             ReInciEstatusDescripcion = estatus.EstDescripcion,
                                                             ReInciSucuId = sucu.SucuId,
                                                             ReInciSucuNombre = sucu.SucuNombre,
                                                             DetInciHorarioId = horario.HoraId,
                                                             DetInciMedidaAccion = detinci.MedidaAccion,
                                                             DetInciAsunto = detinci.Asunto,
                                                             DetInciDestino = detinci.Destino,
                                                             DetInciTelDestino = detinci.TelDestino,
                                                             DetInciContacto1 = detinci.Contacto1,
                                                             DetInciContacto2 = detinci.Contacto2,
                                                             DetInciObservaciones = detinci.Observaciones,
                                                             DetInciHoraSalida = detinci.HoraSalida,
                                                             DetInciHoraRegreso = detinci.HoraRegreso,
                                                             DetInciFechaInicio = detinci.FechaInicio,
                                                             DetInciFechaFinal = detinci.FechaFinal,
                                                             DetInciFechaPresentacion = detinci.FechaPresentacion,
                                                             DetInciDiasAusencia = detinci.DiasAusencia,
                                                             DetInciPersonaCubrira = detinci.PersonaCubrira,
                                                             DetInciNombreDestino = detinci.NombreDestino,
                                                             DetInciMotivo = detinci.Motivo,
                                                             DetInciDetFecha = detinci.DetFecha,
                                                             DetInciHorarioDetalles = "De Lunes a Viernes de: " + horario.HoraEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSalida.Value.ToString("hh':'mm':'ss") + " y los sábados de : " + horario.HoraSabadoEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSabadoSalida.Value.ToString("hh':'mm':'ss"),
                                                             DetInciFechaFinPermisoPersonal = detinci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString(),
                                                             DetInciHoraIngresoPermiso = detinci.DetInciHoraIngreso.ToString(),
                                                             DetInciHoraSalidaPermiso = detinci.DetInciHoraSalida.ToString(),
                                                             DetInciHoraSalidaComida = detinci.DetInciHoraSalidaComida.ToString(),
                                                             DetInciHoraRegresoComida = detinci.DetInciHoraRegresoComida.ToString()
                                                         }).OrderBy(x => x.ReInciSucuNombre).ThenBy(x => x.Fecha).ToList();
                            BanderaFiltroPorPermiso = false;
                        }
                        else // Un tipo en específico TODAS LAS SUCURSALES
                        {
                             IncidenciasBySucursal = (from reinci in _context.CatRegistroIncidencias
                                                         join detinci in _context.CatDetalleIncidencias on reinci.ReInciId equals detinci.DetInciReInciId
                                                         join inci in _context.CatIncidencias on reinci.ReInciInciId equals inci.InciId
                                                         join estatus in _context.CatEstatus on reinci.ReInciEstatusId equals estatus.EstId
                                                         join emp in _context.CatEmpleados on reinci.ReInciEmpId equals emp.EmpId
                                                         join sucu in _context.CatSucursales on emp.EmpSucuId equals sucu.SucuId
                                                         join horario in _context.CatHorarios on detinci.DetInciHorarioId equals horario.HoraId
                                                         where (reinci.Fecha >= FechaInicio && reinci.Fecha <= FechaFinal && reinci.ReInciInciId == IdPermiso)
                                                         select new ListaIncidenciasFiltradasPorSucursal
                                                         {
                                                             ReInciId = reinci.ReInciId,
                                                             ReInciEmpId = emp.EmpId,
                                                             ReInciEmpNombreCompleto = emp.EmpNombreCompleto,
                                                             ReInciEmpNumero = emp.EmpNumero,
                                                             Fecha = reinci.Fecha,
                                                             ReInciInciId = reinci.ReInciInciId,
                                                             ReInciInciDescripcion = inci.InciDescripcion,
                                                             ReInciEstatusId = estatus.EstId,
                                                             ReInciEstatusDescripcion = estatus.EstDescripcion,
                                                             ReInciSucuId = sucu.SucuId,
                                                             ReInciSucuNombre = sucu.SucuNombre,
                                                             DetInciHorarioId = horario.HoraId,
                                                             DetInciMedidaAccion = detinci.MedidaAccion,
                                                             DetInciAsunto = detinci.Asunto,
                                                             DetInciDestino = detinci.Destino,
                                                             DetInciTelDestino = detinci.TelDestino,
                                                             DetInciContacto1 = detinci.Contacto1,
                                                             DetInciContacto2 = detinci.Contacto2,
                                                             DetInciObservaciones = detinci.Observaciones,
                                                             DetInciHoraSalida = detinci.HoraSalida,
                                                             DetInciHoraRegreso = detinci.HoraRegreso,
                                                             DetInciFechaInicio = detinci.FechaInicio,
                                                             DetInciFechaFinal = detinci.FechaFinal,
                                                             DetInciFechaPresentacion = detinci.FechaPresentacion,
                                                             DetInciDiasAusencia = detinci.DiasAusencia,
                                                             DetInciPersonaCubrira = detinci.PersonaCubrira,
                                                             DetInciNombreDestino = detinci.NombreDestino,
                                                             DetInciMotivo = detinci.Motivo,
                                                             DetInciDetFecha = detinci.DetFecha,
                                                             DetInciHorarioDetalles = "De Lunes a Viernes de: " + horario.HoraEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSalida.Value.ToString("hh':'mm':'ss") + " y los sábados de : " + horario.HoraSabadoEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSabadoSalida.Value.ToString("hh':'mm':'ss"),
                                                             DetInciFechaFinPermisoPersonal = detinci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString(),
                                                             DetInciHoraIngresoPermiso = detinci.DetInciHoraIngreso.ToString(),
                                                             DetInciHoraSalidaPermiso = detinci.DetInciHoraSalida.ToString(),
                                                             DetInciHoraSalidaComida = detinci.DetInciHoraSalidaComida.ToString(),
                                                             DetInciHoraRegresoComida = detinci.DetInciHoraRegresoComida.ToString()
                                                         }).OrderBy(x => x.Fecha).ThenBy(x => x.ReInciInciDescripcion).ToList();
                            
                            BanderaFiltroPorPermiso = true;
                        }
                        
                    }else
                    {
                        if(TodosPermisos == true) // Todos los permisos, filtrado por sucursal
                        {
                             IncidenciasBySucursal = (from reinci in _context.CatRegistroIncidencias
                                                         join detinci in _context.CatDetalleIncidencias on reinci.ReInciId equals detinci.DetInciReInciId
                                                         join inci in _context.CatIncidencias on reinci.ReInciInciId equals inci.InciId
                                                         join estatus in _context.CatEstatus on reinci.ReInciEstatusId equals estatus.EstId
                                                         join emp in _context.CatEmpleados on reinci.ReInciEmpId equals emp.EmpId
                                                         join sucu in _context.CatSucursales on emp.EmpSucuId equals sucu.SucuId
                                                         join horario in _context.CatHorarios on detinci.DetInciHorarioId equals horario.HoraId
                                                         where (sucu.SucuId == IdSucursal && reinci.Fecha >= FechaInicio && reinci.Fecha <= FechaFinal)
                                                         select new ListaIncidenciasFiltradasPorSucursal
                                                         {
                                                             ReInciId = reinci.ReInciId,
                                                             ReInciEmpId = emp.EmpId,
                                                             ReInciEmpNombreCompleto = emp.EmpNombreCompleto,
                                                             ReInciEmpNumero = emp.EmpNumero,
                                                             Fecha = reinci.Fecha,
                                                             ReInciInciId = reinci.ReInciInciId,
                                                             ReInciInciDescripcion = inci.InciDescripcion,
                                                             ReInciEstatusId = estatus.EstId,
                                                             ReInciEstatusDescripcion = estatus.EstDescripcion,
                                                             ReInciSucuId = sucu.SucuId,
                                                             ReInciSucuNombre = sucu.SucuNombre,
                                                             DetInciHorarioId = horario.HoraId,
                                                             DetInciMedidaAccion = detinci.MedidaAccion,
                                                             DetInciAsunto = detinci.Asunto,
                                                             DetInciDestino = detinci.Destino,
                                                             DetInciTelDestino = detinci.TelDestino,
                                                             DetInciContacto1 = detinci.Contacto1,
                                                             DetInciContacto2 = detinci.Contacto2,
                                                             DetInciObservaciones = detinci.Observaciones,
                                                             DetInciHoraSalida = detinci.HoraSalida,
                                                             DetInciHoraRegreso = detinci.HoraRegreso,
                                                             DetInciFechaInicio = detinci.FechaInicio,
                                                             DetInciFechaFinal = detinci.FechaFinal,
                                                             DetInciFechaPresentacion = detinci.FechaPresentacion,
                                                             DetInciDiasAusencia = detinci.DiasAusencia,
                                                             DetInciPersonaCubrira = detinci.PersonaCubrira,
                                                             DetInciNombreDestino = detinci.NombreDestino,
                                                             DetInciMotivo = detinci.Motivo,
                                                             DetInciDetFecha = detinci.DetFecha,
                                                             DetInciHorarioDetalles = "De Lunes a Viernes de: " + horario.HoraEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSalida.Value.ToString("hh':'mm':'ss") + " y los sábados de : " + horario.HoraSabadoEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSabadoSalida.Value.ToString("hh':'mm':'ss"),
                                                             DetInciFechaFinPermisoPersonal = detinci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString(),
                                                             DetInciHoraIngresoPermiso = detinci.DetInciHoraIngreso.ToString(),
                                                             DetInciHoraSalidaPermiso = detinci.DetInciHoraSalida.ToString(),
                                                             DetInciHoraSalidaComida = detinci.DetInciHoraSalidaComida.ToString(),
                                                             DetInciHoraRegresoComida = detinci.DetInciHoraRegresoComida.ToString()
                                                         }).OrderBy(x => x.Fecha).ThenBy(x => x.ReInciInciDescripcion).ToList();
                            BanderaFiltroPorPermiso = false;
                        }
                        else // Filtrado por sucursal, filtrado por permiso
                        {
                             IncidenciasBySucursal = (from reinci in _context.CatRegistroIncidencias
                                                         join detinci in _context.CatDetalleIncidencias on reinci.ReInciId equals detinci.DetInciReInciId
                                                         join inci in _context.CatIncidencias on reinci.ReInciInciId equals inci.InciId
                                                         join estatus in _context.CatEstatus on reinci.ReInciEstatusId equals estatus.EstId
                                                         join emp in _context.CatEmpleados on reinci.ReInciEmpId equals emp.EmpId
                                                         join sucu in _context.CatSucursales on emp.EmpSucuId equals sucu.SucuId
                                                         join horario in _context.CatHorarios on detinci.DetInciHorarioId equals horario.HoraId
                                                         where (sucu.SucuId == IdSucursal && reinci.Fecha >= FechaInicio && reinci.Fecha <= FechaFinal && reinci.ReInciInciId == IdPermiso)
                                                         select new ListaIncidenciasFiltradasPorSucursal
                                                         {
                                                             ReInciId = reinci.ReInciId,
                                                             ReInciEmpId = emp.EmpId,
                                                             ReInciEmpNombreCompleto = emp.EmpNombreCompleto,
                                                             ReInciEmpNumero = emp.EmpNumero,
                                                             Fecha = reinci.Fecha,
                                                             ReInciInciId = reinci.ReInciInciId,
                                                             ReInciInciDescripcion = inci.InciDescripcion,
                                                             ReInciEstatusId = estatus.EstId,
                                                             ReInciEstatusDescripcion = estatus.EstDescripcion,
                                                             ReInciSucuId = sucu.SucuId,
                                                             ReInciSucuNombre = sucu.SucuNombre,
                                                             DetInciHorarioId = horario.HoraId,
                                                             DetInciMedidaAccion = detinci.MedidaAccion,
                                                             DetInciAsunto = detinci.Asunto,
                                                             DetInciDestino = detinci.Destino,
                                                             DetInciTelDestino = detinci.TelDestino,
                                                             DetInciContacto1 = detinci.Contacto1,
                                                             DetInciContacto2 = detinci.Contacto2,
                                                             DetInciObservaciones = detinci.Observaciones,
                                                             DetInciHoraSalida = detinci.HoraSalida,
                                                             DetInciHoraRegreso = detinci.HoraRegreso,
                                                             DetInciFechaInicio = detinci.FechaInicio,
                                                             DetInciFechaFinal = detinci.FechaFinal,
                                                             DetInciFechaPresentacion = detinci.FechaPresentacion,
                                                             DetInciDiasAusencia = detinci.DiasAusencia,
                                                             DetInciPersonaCubrira = detinci.PersonaCubrira,
                                                             DetInciNombreDestino = detinci.NombreDestino,
                                                             DetInciMotivo = detinci.Motivo,
                                                             DetInciDetFecha = detinci.DetFecha,
                                                             DetInciHorarioDetalles = "De Lunes a Viernes de: " + horario.HoraEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSalida.Value.ToString("hh':'mm':'ss") + " y los sábados de : " + horario.HoraSabadoEntrada.Value.ToString("hh':'mm':'ss") + " a: " + horario.HoraSabadoSalida.Value.ToString("hh':'mm':'ss"),
                                                             DetInciFechaFinPermisoPersonal = detinci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString(),
                                                             DetInciHoraIngresoPermiso = detinci.DetInciHoraIngreso.ToString(),
                                                             DetInciHoraSalidaPermiso = detinci.DetInciHoraSalida.ToString(),
                                                             DetInciHoraSalidaComida = detinci.DetInciHoraSalidaComida.ToString(),
                                                             DetInciHoraRegresoComida = detinci.DetInciHoraRegresoComida.ToString()
                                                         }).OrderBy(x => x.Fecha).ThenBy(x => x.ReInciInciDescripcion).ToList();
                            BanderaFiltroPorPermiso = true;
                        }
                    }
                }
                var obj = new
                {
                    IncidenciasBySucursal,
                    BanderaFiltroPorPermiso,
                    DescripcionPermiso
                };
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost("ReportePdf")]
        public IActionResult ReportePdf(ListaIncidenciasFiltradasPorSucursal[] data, string Bandera, DateTime FechaPrincipio, DateTime FechaFinalizacion)
        {
            try
            {
                if (data != null)
                {
                    if(Directory.Exists("C:\\ReportesNucleoRH") != false){
                        Directory.CreateDirectory("C:\\ReportesNucleoRH");
                    }
                    DateTime FechaActual = DateTime.Now;
                    string Dia = FechaActual.Day.ToString();
                    string Mes = FechaActual.Month.ToString();
                    string Anio = FechaActual.Year.ToString();
                    string FechaBreve = FechaActual.ToShortDateString();
                    string FechaCorta = Dia + Mes + Anio;
                    string FechaCortaPrincipio = FechaPrincipio.ToShortDateString();
                    string FechaCortaFinal = FechaFinalizacion.ToShortDateString();
                    string TipoReporte = "";

                    if (Bandera == "Vacaciones")
                    {
                        TipoReporte = "Reporte de vacaciones";
                    }
                    else if (Bandera == "Comisión")
                    {
                        TipoReporte = "Reporte de permisos en comisión";
                    } else if(Bandera=="PersonalSH" || Bandera == "PersonalCH")
                    {
                        TipoReporte = "Reporte de permisos personales";
                    }else if (Bandera == "Todos")
                    {
                        TipoReporte = "Reporte de todos los permisos";
                    }else if(Bandera == "Entradas/Salidas")
                    {
                        TipoReporte = "Reporte de permisos de entradas y salidas";
                    }
                    else
                    {
                        TipoReporte = "";
                    }

                    using (FileStream fs = new FileStream("C:\\ReportesNucleoRH\\ReportePermisos"+ FechaCorta +"_"+ TipoReporte +".pdf",FileMode.Create))
                    {
                        Document doc = new Document(PageSize.LETTER.Rotate(), 18.3465f, 18.3465f, 78.3465f, 28.3465f);
                        PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                        //Agregando información al documento 
                        doc.AddAuthor("Sistemas, Núcleo de diagnóstico");
                        doc.AddCreator("Núcleo de diagnóstico");
                        doc.AddKeywords("Sistemas ND");
                        doc.AddTitle("Reporte de permisos");

                        writer.PageEvent = new HeaderFooter(TipoReporte, FechaBreve, FechaCortaPrincipio, FechaCortaFinal);

                        doc.Open();

                        if (Bandera == "Vacaciones")
                        {
                            PdfPTable TablaVacaciones = new PdfPTable(12);
                            TablaVacaciones.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            TablaVacaciones.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                            TablaVacaciones.WidthPercentage = 100;
                            float[] TablaWidth = { 5.33f, 8.33f, 12.33f, 8.33f, 8.33f, 8.33f, 8.33f, 8.33f, 8.33f, 9.33f, 6.33f, 8.33f };
                            TablaVacaciones.SetWidths(TablaWidth);

                            var font = FontFactory.GetFont("Arial", size: 10, iTextSharp.text.Font.BOLD);
                            var fontContext = FontFactory.GetFont("Arial", size: 7);

                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Folio",  font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Número de nómina", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Nombre del empleado", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Nombre de sucursal", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Fecha del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Estatus del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Tipo de permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Inicio de vacaciones", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Fin de vacaciones", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Fecha de presentación", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Días de ausencia", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaVacaciones.AddCell(new PdfPCell(new Phrase("Persona que cubrirá", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            
                            for(int i=0; i<data.Length; i++)
                            {
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].ReInciId.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].ReInciEmpNumero.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].ReInciEmpNombreCompleto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].ReInciSucuNombre, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].Fecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].ReInciEstatusDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].ReInciInciDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].DetInciFechaInicio.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].DetInciFechaFinal.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].DetInciFechaPresentacion.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].DetInciDiasAusencia.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase(data[i].DetInciPersonaCubrira, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            }
                            doc.Add(TablaVacaciones);
                        }
                        else if (Bandera == "Comisión")
                        {
                            PdfPTable TablaComision = new PdfPTable(12);
                            TablaComision.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            TablaComision.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                            TablaComision.WidthPercentage = 100;
                            float[] TablaWidth = { 4.33f, 7.33f, 13.33f, 8.33f, 8.33f, 8.33f, 8.33f, 8.33f, 8.33f, 10.33f, 7.33f, 7.33f };
                            TablaComision.SetWidths(TablaWidth);

                            var font = FontFactory.GetFont("Arial", size: 10, iTextSharp.text.Font.BOLD);
                            var fontContext = FontFactory.GetFont("Arial", size: 7);

                            TablaComision.AddCell(new PdfPCell(new Phrase("Folio", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Número de nómina", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Nombre del empleado", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Nombre de sucursal", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Fecha del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Estatus del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Tipo de permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Asunto", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Destino", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Observaciones", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Hora de salida", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaComision.AddCell(new PdfPCell(new Phrase("Hora de regreso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            for(int i=0; i<data.Length; i++)
                            {
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].ReInciId.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].ReInciEmpNumero.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].ReInciEmpNombreCompleto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].ReInciSucuNombre, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].Fecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].ReInciEstatusDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].ReInciInciDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].DetInciAsunto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].DetInciDestino, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].DetInciObservaciones, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].DetInciHoraSalida.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase(data[i].DetInciHoraRegreso.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            }

                            doc.Add(TablaComision);

                        }
                        else if (Bandera == "PersonalSH")
                        {
                            PdfPTable TablaPersonalSH= new PdfPTable(10);
                            TablaPersonalSH.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            TablaPersonalSH.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                            TablaPersonalSH.WidthPercentage = 100;
                            float[] TablaWidth = { 6f, 8f, 16f, 10f, 8f, 8f, 8f, 10f, 13f, 13f };
                            TablaPersonalSH.SetWidths(TablaWidth);

                            var font = FontFactory.GetFont("Arial", size: 10, iTextSharp.text.Font.BOLD);
                            var fontContext = FontFactory.GetFont("Arial", size: 7);

                            TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Folio", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Número de nómina", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Nombre del empleado", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Nombre de sucursal", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Fecha del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Estatus del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Tipo de permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Fecha para el permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Motivo", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Medida de acción", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            for (int i = 0; i < data.Length; i++)
                            {
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase(data[i].ReInciId.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase(data[i].ReInciEmpNumero.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase(data[i].ReInciEmpNombreCompleto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase(data[i].ReInciSucuNombre, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase(data[i].Fecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase(data[i].ReInciEstatusDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase(data[i].ReInciInciDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase(data[i].DetInciDetFecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase(data[i].DetInciMotivo, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase(data[i].DetInciMedidaAccion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            }

                            doc.Add(TablaPersonalSH);

                        }
                        else if (Bandera == "PersonalCH")
                        {
                            PdfPTable TablaPersonalCH = new PdfPTable(11);
                            TablaPersonalCH.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            TablaPersonalCH.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                            TablaPersonalCH.WidthPercentage = 100;
                            float[] TablaWidth = { 5f, 8.09f, 11.09f, 9.09f, 8f, 8.09f, 10.18f, 9.09f, 9.09f, 9.09f, 13.09f };
                            TablaPersonalCH.SetWidths(TablaWidth);
                            var font = FontFactory.GetFont("Arial", size: 10, iTextSharp.text.Font.BOLD);
                            var fontContext = FontFactory.GetFont("Arial", size: 7);

                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Folio", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Número de nómina", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Nombre del empleado", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Nombre de sucursal", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Fecha del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Estatus del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Tipo de permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Fecha para el permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Motivo", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Medida de acción", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Horario", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            for (int i = 0; i < data.Length; i++)
                            {
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].ReInciId.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].ReInciEmpNumero.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].ReInciEmpNombreCompleto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].ReInciSucuNombre, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].Fecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].ReInciEstatusDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].ReInciInciDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].DetInciDetFecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].DetInciMotivo, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].DetInciMedidaAccion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase(data[i].DetInciHorarioDetalles, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            }
                            doc.Add(TablaPersonalCH);
                        }
                        else if(Bandera == "Entradas/Salidas"){
                            PdfPTable TablaPersonalES = new PdfPTable(15);
                            TablaPersonalES.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            TablaPersonalES.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                            TablaPersonalES.WidthPercentage = 100;
                            float[] TablaWidth = { 4.0f, 6.66f, 6.66f, 7.58f, 6.66f, 6.66f, 9.32f, 6.66f, 6.66f, 6.66f, 6.66f, 6.66f, 6.66f, 6.0f, 6.0f };
                            TablaPersonalES.SetWidths(TablaWidth);
                            var font = FontFactory.GetFont("Arial", size: 8, iTextSharp.text.Font.BOLD);
                            var fontContext = FontFactory.GetFont("Arial", size: 6.3f);

                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Folio", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Número de nómina", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Nombre del empleado", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Nombre de sucursal", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Fecha del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Estatus del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Tipo de permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Fecha para el permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Fecha para fin de permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Hora de entrada", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Hora de salida", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Salida a comer", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Regresa de comer", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Motivo", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            TablaPersonalES.AddCell(new PdfPCell(new Phrase("Medida de acción", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            for (int i = 0; i < data.Length; i++)
                            {
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].ReInciId.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].ReInciEmpNumero.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].ReInciEmpNombreCompleto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].ReInciSucuNombre, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].Fecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].ReInciEstatusDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].ReInciInciDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].DetInciDetFecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].DetInciFechaFinPermisoPersonal, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].DetInciHoraIngresoPermiso, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].DetInciHoraSalidaPermiso, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].DetInciHoraSalidaComida, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].DetInciHoraRegresoComida, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].DetInciMotivo, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase(data[i].DetInciMedidaAccion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            }
                            doc.Add(TablaPersonalES);
                        }
                        else if (Bandera == "Todos")
                        {
                            var Vacaciones = data.Where(x => x.ReInciInciId == 4).ToList();
                            var Comision = data.Where(x => x.ReInciInciId == 11).ToList();
                            var PersonalSH = data.Where(x => x.ReInciInciId != 1 && x.ReInciInciId != 2 && x.ReInciInciId != 3 && x.ReInciInciId != 4 && x.ReInciInciId != 5 && x.ReInciInciId != 8 && x.ReInciInciId != 11).ToList();
                            var PersonalCH = data.Where(x => x.ReInciInciId == 8).ToList();
                            var PersonalES = data.Where(x => x.ReInciInciId == 5).ToList();

                            var font = FontFactory.GetFont("Arial", size: 10, iTextSharp.text.Font.BOLD);
                            var fontContext = FontFactory.GetFont("Arial", size: 7);

                            if(Vacaciones.Count != 0)
                            {
                                PdfPTable TablaVacaciones = new PdfPTable(12);
                                TablaVacaciones.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                TablaVacaciones.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                                TablaVacaciones.WidthPercentage = 100;
                                float[] TablaWidth = { 5.33f, 8.33f, 12.33f, 8.33f, 8.33f, 8.33f, 8.33f, 8.33f, 8.33f, 9.33f, 6.33f, 8.33f };
                                TablaVacaciones.SetWidths(TablaWidth);

                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Folio", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Número de nómina", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Nombre del empleado", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Nombre de sucursal", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Fecha del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Estatus del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Tipo de permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Inicio de vacaciones", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Fin de vacaciones", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Fecha de presentación", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Días de ausencia", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaVacaciones.AddCell(new PdfPCell(new Phrase("Persona que cubrirá", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                                for (int i = 0; i < Vacaciones.Count; i++)
                                {
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].ReInciId.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].ReInciEmpNumero.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].ReInciEmpNombreCompleto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].ReInciSucuNombre, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].Fecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].ReInciEstatusDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].ReInciInciDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].DetInciFechaInicio.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].DetInciFechaFinal.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].DetInciFechaPresentacion.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].DetInciDiasAusencia.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaVacaciones.AddCell(new PdfPCell(new Phrase(Vacaciones[i].DetInciPersonaCubrira, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                }

                                doc.Add(TablaVacaciones);
                                doc.NewPage();
                            }

                            if(Comision.Count != 0)
                            {
                                PdfPTable TablaComision = new PdfPTable(12);
                                TablaComision.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                TablaComision.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                                TablaComision.WidthPercentage = 100;
                                float[] TablaWidthComision = { 4.33f, 7.33f, 13.33f, 8.33f, 8.33f, 8.33f, 8.33f, 8.33f, 8.33f, 10.33f, 7.33f, 7.33f };
                                TablaComision.SetWidths(TablaWidthComision);

                                TablaComision.AddCell(new PdfPCell(new Phrase("Folio", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Número de nómina", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Nombre del empleado", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Nombre de sucursal", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Fecha del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Estatus del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Tipo de permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Asunto", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Destino", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Observaciones", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Hora de salida", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaComision.AddCell(new PdfPCell(new Phrase("Hora de regreso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                                for (int i = 0; i < Comision.Count; i++)
                                {
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].ReInciId.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].ReInciEmpNumero.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].ReInciEmpNombreCompleto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].ReInciSucuNombre, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].Fecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].ReInciEstatusDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].ReInciInciDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].DetInciAsunto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].DetInciDestino, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].DetInciObservaciones, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].DetInciHoraSalida.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaComision.AddCell(new PdfPCell(new Phrase(Comision[i].DetInciHoraRegreso.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });

                                }

                                doc.Add(TablaComision);
                                doc.NewPage();
                            }

                            if(PersonalSH.Count != 0)
                            {
                                PdfPTable TablaPersonalSH = new PdfPTable(10);
                                TablaPersonalSH.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                TablaPersonalSH.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                                TablaPersonalSH.WidthPercentage = 100;
                                float[] TablaWidthSH = { 6f, 8f, 16f, 10f, 8f, 8f, 8f, 10f, 13f, 13f };
                                TablaPersonalSH.SetWidths(TablaWidthSH);

                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Folio", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Número de nómina", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Nombre del empleado", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Nombre de sucursal", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Fecha del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Estatus del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Tipo de permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Fecha para el permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Motivo", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalSH.AddCell(new PdfPCell(new Phrase("Medida de acción", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                                for (int i = 0; i < PersonalSH.Count; i++)
                                {
                                    TablaPersonalSH.AddCell(new PdfPCell(new Phrase(PersonalSH[i].ReInciId.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalSH.AddCell(new PdfPCell(new Phrase(PersonalSH[i].ReInciEmpNumero.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalSH.AddCell(new PdfPCell(new Phrase(PersonalSH[i].ReInciEmpNombreCompleto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalSH.AddCell(new PdfPCell(new Phrase(PersonalSH[i].ReInciSucuNombre, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalSH.AddCell(new PdfPCell(new Phrase(PersonalSH[i].Fecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalSH.AddCell(new PdfPCell(new Phrase(PersonalSH[i].ReInciEstatusDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalSH.AddCell(new PdfPCell(new Phrase(PersonalSH[i].ReInciInciDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalSH.AddCell(new PdfPCell(new Phrase(PersonalSH[i].DetInciDetFecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalSH.AddCell(new PdfPCell(new Phrase(PersonalSH[i].DetInciMotivo, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalSH.AddCell(new PdfPCell(new Phrase(PersonalSH[i].DetInciMedidaAccion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                }

                                doc.Add(TablaPersonalSH);
                                doc.NewPage();
                            }

                            if(PersonalCH.Count != 0)
                            {
                                PdfPTable TablaPersonalCH = new PdfPTable(11);
                                TablaPersonalCH.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                TablaPersonalCH.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                                TablaPersonalCH.WidthPercentage = 100;
                                float[] TablaWidthCH = { 5f, 8.09f, 11.09f, 9.09f, 8f, 8.09f, 10.18f, 9.09f, 9.09f, 9.09f, 13.09f };
                                TablaPersonalCH.SetWidths(TablaWidthCH);

                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Folio", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Número de nómina", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Nombre del empleado", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Nombre de sucursal", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Fecha del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Estatus del permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Tipo de permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Fecha para el permiso", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Motivo", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Medida de acción", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalCH.AddCell(new PdfPCell(new Phrase("Horario", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                                for (int i = 0; i < PersonalCH.Count; i++)
                                {
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].ReInciId.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].ReInciEmpNumero.ToString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].ReInciEmpNombreCompleto, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].ReInciSucuNombre, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].Fecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].ReInciEstatusDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].ReInciInciDescripcion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].DetInciDetFecha.ToShortDateString(), fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].DetInciMotivo, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].DetInciMedidaAccion, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalCH.AddCell(new PdfPCell(new Phrase(PersonalCH[i].DetInciHorarioDetalles, fontContext)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                }
                                doc.Add(TablaPersonalCH);
                            }
                            if(PersonalES.Count != 0)
                            {
                                PdfPTable TablaPersonalES = new PdfPTable(15);
                                TablaPersonalES.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                TablaPersonalES.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                                TablaPersonalES.WidthPercentage = 100;
                                float[] TablaWidth = { 4.0f, 6.66f, 6.66f, 7.58f, 6.66f, 6.66f, 9.32f, 6.66f, 6.66f, 6.66f, 6.66f, 6.66f, 6.66f, 6.0f, 6.0f };
                                TablaPersonalES.SetWidths(TablaWidth);
                                var fontES = FontFactory.GetFont("Arial", size: 8, iTextSharp.text.Font.BOLD);
                                var fontContextES = FontFactory.GetFont("Arial", size: 6.3f);

                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Folio", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Número de nómina", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Nombre del empleado", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Nombre de sucursal", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Fecha del permiso", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Estatus del permiso", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Tipo de permiso", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Fecha para el permiso", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Fecha para fin de permiso", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Hora de entrada", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Hora de salida", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Salida a comer", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Regresa de comer", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Motivo", fontES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                TablaPersonalES.AddCell(new PdfPCell(new Phrase("Medida de acción", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                                for (int i = 0; i < data.Length; i++)
                                {
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].ReInciId.ToString(), fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].ReInciEmpNumero.ToString(), fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].ReInciEmpNombreCompleto, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].ReInciSucuNombre, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].Fecha.ToShortDateString(), fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].ReInciEstatusDescripcion, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].ReInciInciDescripcion, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].DetInciDetFecha.ToShortDateString(), fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].DetInciFechaFinPermisoPersonal, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].DetInciHoraIngresoPermiso, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].DetInciHoraSalidaPermiso, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].DetInciHoraSalidaComida, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].DetInciHoraRegresoComida, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].DetInciMotivo, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                    TablaPersonalES.AddCell(new PdfPCell(new Phrase(PersonalES[i].DetInciMedidaAccion, fontContextES)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                }
                                doc.Add(TablaPersonalES);
                            }

                           
                        }
                        else
                        {
                        }
                        doc.Close();
                    }
                    var NombreArchivo = "ReportePermisos" + FechaCorta + "_" + TipoReporte + ".pdf";
                    //HttpContext.Response.Headers.Add("content-disposition", "attachment;filename=" + NombreArchivo);
                    //byte[] myfile = System.IO.File.ReadAllBytes("C:\\FormatosRequisicionPersonal\\" + NombreArchivo);
                    //return new FileContentResult(myfile, "application/pdf");

                    return Ok(NombreArchivo);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.StackTrace);
            }
        } // Aquí se puede poner otro método. 

        [HttpGet("DescargarPdf/{NombreArchivo}")]
        public IActionResult GetNombreArchivo(string NombreArchivo)
        {
            HttpContext.Response.Headers.Add("content-disposition", "inline;filename=" + NombreArchivo);
            byte[] myfile = System.IO.File.ReadAllBytes("C:\\ReportesNucleoRH\\" + NombreArchivo);
            return new FileContentResult(myfile, "application/pdf");
        }
    }

    class HeaderFooter : PdfPageEventHelper
    {
        string reporte = null;
        string FechaHoy = null;
        string FechaInicio = null;
        string FechaFinal = null;
        public HeaderFooter(string TipoReporte, string FechaBreve, string FechaCortaPrincipio, string FechaCortaFinal)
        {
            reporte = TipoReporte;
            FechaHoy = FechaBreve;
            FechaInicio = FechaCortaPrincipio;
            FechaFinal = FechaCortaFinal;
        }
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable TablaEncabezado = new PdfPTable(3);
            TablaEncabezado.WidthPercentage = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            var V = TablaEncabezado.WidthPercentage / 3;
            float[] width = { V, V, V };
            TablaEncabezado.SetTotalWidth(width);
            TablaEncabezado.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            TablaEncabezado.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
            /*TablaEncabezado.WidthPercentage = 100;*/ //
            int[] firstTablecellwidth = { 15, 50, 35 };
            TablaEncabezado.SetWidths(firstTablecellwidth);

            iTextSharp.text.Image Logo = iTextSharp.text.Image.GetInstance(new Uri("https://i.imgur.com/XEv6wYz.png")); // Cargamos el logo desde una URL
            float percentage = 0.0f;
            percentage = 85 / Logo.Width;
            Logo.ScalePercent(percentage * 100);

            TablaEncabezado.AddCell(new PdfPCell(Logo) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER});
            TablaEncabezado.AddCell(new PdfPCell(new Phrase("Núcleo de Diagnóstico")) { Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
            TablaEncabezado.AddCell(new PdfPCell(new Phrase("Fecha de hoy: " + FechaHoy)) { HorizontalAlignment = Element.ALIGN_CENTER });
            TablaEncabezado.AddCell(new PdfPCell(new Phrase("Desde: " + FechaInicio)) { HorizontalAlignment = Element.ALIGN_CENTER });
            TablaEncabezado.AddCell(new PdfPCell(new Phrase(reporte)) { HorizontalAlignment = Element.ALIGN_CENTER });
            TablaEncabezado.AddCell(new PdfPCell(new Phrase("Hasta: " + FechaFinal)) { HorizontalAlignment = Element.ALIGN_CENTER });
            TablaEncabezado.AddCell(new PdfPCell(new Phrase("\n\n")) { Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, Border = iTextSharp.text.Rectangle.NO_BORDER });
            TablaEncabezado.AddCell(new PdfPCell(new Phrase("\n\n ")) { HorizontalAlignment = Element.ALIGN_CENTER, Border = iTextSharp.text.Rectangle.NO_BORDER });
            TablaEncabezado.AddCell(new PdfPCell(new Phrase("\n\n")) { HorizontalAlignment = Element.ALIGN_CENTER, Border = iTextSharp.text.Rectangle.NO_BORDER });

            TablaEncabezado.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 70, writer.DirectContent);

            PdfPTable TbFooter = new PdfPTable(3);
            TbFooter.WidthPercentage = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            var L = TbFooter.WidthPercentage / 3;
            float[] widthFooter = { L, L, L };
            TbFooter.SetTotalWidth(widthFooter);
            TbFooter.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //TbFooter.WidthPercentage = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            TbFooter.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            TbFooter.AddCell(new PdfPCell(new Phrase()) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = iTextSharp.text.Rectangle.NO_BORDER });
            TbFooter.AddCell(new PdfPCell(new Phrase()) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = iTextSharp.text.Rectangle.NO_BORDER });
            TbFooter.AddCell(new PdfPCell(new Phrase("Página " + writer.PageNumber, FontFactory.GetFont(FontFactory.TIMES, size: 8))) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = iTextSharp.text.Rectangle.NO_BORDER});

            TbFooter.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin) -5, writer.DirectContent);
        }
    }
}