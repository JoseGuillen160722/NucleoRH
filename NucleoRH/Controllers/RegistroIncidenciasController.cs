using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Utils;
using NucleoRH.Models;
using NucleoRH.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace NucleoRH.Controllers
{
    
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class CatRegistroIncidenciasController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITemplateHelper _templateHelper;
        public CatRegistroIncidenciasController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ITemplateHelper templateHelper)
        {
            this._context = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
            this._templateHelper = templateHelper;
        }
        public class ListaIncidenciasAbiertasEnPrimerPaso
        {
            public int ReInciId { get; set; }
            public string ReInciEmpId { get; set; }
            public DateTime Fecha { get; set; }
            public string ReInciInciId { get; set; }
            public string ReInciEstatusId { get; set; }
            public string ReInciEstatusFlujo { get; set; }
            public int RIEmpId { get; set; }
            public int RIInciId { get; set; }
            public int RIEstatusId { get; set; }
            public int RIEstatusFlujo { get; set; }
        }
        public async Task<IActionResult> Index()
        {
                ViewBag.RegistroIncidencias = _context.CatRegistroIncidencias.Include(r => r.Emp).Include(r => r.Inci).Include(r => r.Estatus);
                var Emp = _context.CatEmpleados.OrderBy(x => x.EmpId);
                ViewData["Emp"] = new SelectList(Emp, "EmpId", "EmpNombreCompleto");
                var Inci = _context.CatIncidencias.OrderBy(x => x.InciId).Where(x=> x.InciDescripcion != "FALTAS" && x.InciDescripcion != "RETARDOS" && x.InciDescripcion != "INCAPACIDADES" );
                ViewData["Inci"] = new SelectList(Inci, "InciId", "InciDescripcion");
                var Horario = _context.CatHorarios.OrderBy(x => x.HoraId);
                ViewData["Horario"] = new SelectList(Horario, "HoraId", "HoraEntrada");
                var Estatus = _context.CatEstatus.OrderBy(x => x.EstId).Where(x => x.EstDescripcion != "ACTIVO" && x.EstDescripcion != "BAJA" && x.EstDescripcion != "PROCESADO" && x.EstDescripcion != "ACEPTADO");
                ViewData["Estatus"] = new SelectList(Estatus, "EstId", "EstDescripcion");
                var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                var RegistroDeIncidencias = _context.CatRegistroIncidencias.OrderBy(x => x.ReInciId).ToList();
                var Empleados = _context.CatEmpleados.ToList();
                var IdEmpleado = 0;
                for (int i = 0; i < Empleados.Count; i++)
                {
                    if (user.UserName == Empleados[i].EmpUserId)
                    {
                        IdEmpleado = Empleados[i].EmpId;
                        break;
                    }
                }
                var FlujosComision = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == 10).ToList();
                var PrimerPaso = FlujosComision.Where(x => x.DetFlujoOrden == 1).First();
                var UltimoPaso = FlujosComision.Where(x => x.DetFlujoOrden == 4).First();
                var TodosLosFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoOrden == 1).ToList();

                var q = (from p in _context.CatRegistroIncidencias
                         join pp in _context.CatDetalleFlujo on p.ReInciEstatusFlujo equals pp.DetFlujoId
                         join emp in _context.CatEmpleados on p.ReInciEmpId equals emp.EmpId
                         join inci in _context.CatIncidencias on p.ReInciInciId equals inci.InciId
                         join estatus in _context.CatEstatus on p.ReInciEstatusId equals estatus.EstId
                         where pp.DetFlujoOrden == 1
                         select new ListaIncidenciasAbiertasEnPrimerPaso { ReInciId = p.ReInciId, ReInciEmpId = emp.EmpNombreCompleto, ReInciInciId = inci.InciDescripcion, ReInciEstatusId = estatus.EstDescripcion, RIEstatusFlujo = pp.DetFlujoId, Fecha = p.Fecha, RIEmpId = emp.EmpId, RIEstatusId = estatus.EstId, RIInciId = inci.InciId }).ToList();

                var CH = (from p in _context.CatRegistroIncidencias
                          join pp in _context.CatDetalleFlujo on p.ReInciEstatusFlujo equals pp.DetFlujoId
                          join emp in _context.CatEmpleados on p.ReInciEmpId equals emp.EmpId
                          join inci in _context.CatIncidencias on p.ReInciInciId equals inci.InciId
                          join estatus in _context.CatEstatus on p.ReInciEstatusId equals estatus.EstId
                          where pp.DetFlujoOrden != 1 && pp.DetFlujoOrden != 4 && p.ReInciInciId != 11
                          select new ListaIncidenciasAbiertasEnPrimerPaso { ReInciId = p.ReInciId, ReInciEmpId = emp.EmpNombreCompleto, ReInciInciId = inci.InciDescripcion, ReInciEstatusId = estatus.EstDescripcion, RIEstatusFlujo = pp.DetFlujoId, Fecha = p.Fecha, RIEmpId = emp.EmpId, RIEstatusId = estatus.EstId, RIInciId = inci.InciId }).ToList();
                
                ViewBag.IncidenciasAbiertasEnPrimerPaso = q.Where(x => x.RIEstatusId == 3 && x.RIEmpId != IdEmpleado || x.RIEstatusId == 9 && x.RIEmpId != IdEmpleado);
                ViewBag.RegistroIncidenciasCH = CH.Where(x => x.RIEstatusId == 3 && x.RIEmpId != IdEmpleado || x.RIEstatusId == 9 && x.RIEmpId != IdEmpleado);
                ViewBag.RegistroIncidenciasCerradas = _context.CatRegistroIncidencias.Include(r => r.Emp).Include(r => r.Inci).Include(r => r.Estatus).Include(r => r.DetFlujo).Where(x => x.ReInciEstatusId == 6);
                ViewBag.RegistroIncidenciasPropias = _context.CatRegistroIncidencias.Include(r => r.Emp).Include(r => r.Inci).Include(r => r.Estatus).Include(r => r.DetFlujo).Where(x => x.ReInciEmpId == IdEmpleado);
                ViewBag.RegistroIncidenciasDeComision = _context.CatRegistroIncidencias.Include(r => r.Emp).Include(r => r.Inci).Include(r => r.Estatus).Include(r => r.DetFlujo).Where(x => x.ReInciInciId == 11 && x.ReInciEmpId != IdEmpleado && x.ReInciEstatusId != 6 && x.ReInciEstatusFlujo != PrimerPaso.DetFlujoId);
                ViewBag.RegistrosHechosPorCH = _context.CatRegistroIncidencias.Include(r => r.Emp).Include(r => r.Inci).Include(r => r.Estatus).Include(r => r.DetFlujo).Where(x=> x.ReInciEstatusFlujo == 1061);

            return View();
        }
        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS. ESTA VEZ LO HACE CON DOS TABLAS

        [Microsoft.AspNetCore.Mvc.HttpPost("AddRegistroIncidencias")]
        public async Task<IActionResult> AddRegistroIncidencias(CatRegistroIncidencias incidencias, CatDetalleIncidencias detincidencias, CatBitacoraIncidencias BitInci, int NominaEmpleado, CatHistorialVacaciones vaca)
        {
            try
            {
                if (incidencias != null)
                {
                    // Configuración del SMTP

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    var Empleados = _context.CatEmpleados.Where(x => x.EmpNumero == NominaEmpleado).FirstOrDefault();

                    if(Empleados.EmpEstId == 2)
                    {
                        return BadRequest();
                    }

                    if (Empleados != null) // Validación por si se ingresa un número de nómina NO válido
                    {
                        var ListaIncidencias = _context.CatIncidencias.Where(x => x.InciId == incidencias.ReInciInciId).First();
                        var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                        var PerfilesDelUsuario = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
                        bool BanderaDeFlujos = false;
                        bool BanderaCapitalHumano = false;
                        var NombreIncidenciaParaFlujo = ListaIncidencias.InciDescripcion;
                        var Administracion = " DIRECCIÓN";
                        for (int i = 0; i < PerfilesDelUsuario.Count; i++)
                        {
                            if (PerfilesDelUsuario[i].RoleId == "2")
                            {
                                BanderaDeFlujos = true;
                                break;
                            }
                        }
                        for (int i = 0; i < PerfilesDelUsuario.Count; i++)
                        {
                            if (PerfilesDelUsuario[i].RoleId == "8")
                            {
                                BanderaCapitalHumano = true;
                                break;
                            }
                        }

                        if (BanderaDeFlujos == true)
                        {
                            NombreIncidenciaParaFlujo = NombreIncidenciaParaFlujo + Administracion;
                        }

                        if (BanderaCapitalHumano == true)
                        {
                            CatRegistroIncidencias inci = new CatRegistroIncidencias() // SE HACE EL OBJETO CON LOS DATOS
                            {
                                ReInciEmpId = Empleados.EmpId,
                                Fecha = incidencias.Fecha,
                                ReInciInciId = incidencias.ReInciInciId,
                                ReInciEstatusId = 6,
                                ReInciEstatusFlujo = 1061
                            };

                            var HorarioDefault = 2055;
                            if (detincidencias.DetInciHorarioId == 0)
                            {
                                detincidencias.DetInciHorarioId = HorarioDefault;
                            }
                            TimeSpan? RestaFechas;
                            int Dias = 0;
                            RestaFechas = detincidencias.FechaFinal - detincidencias.FechaInicio;
                            if (inci.ReInciInciId == 4)
                            {
                                Dias = RestaFechas.Value.Days + 1;
                                DateTime Fecha = detincidencias.FechaInicio;
                                do
                                {
                                    if (Fecha.DayOfWeek == DayOfWeek.Sunday) Dias--;

                                    Fecha = Fecha.AddDays(1);

                                } while (!(Fecha > detincidencias.FechaFinal));
                                var Festivos = _context.CatFestivos.OrderBy(x => x.FestId).ToList();
                                for (int i = 0; i < Festivos.Count; i++)
                                {
                                    if ((Festivos[i].FestFechaDesde >= detincidencias.FechaInicio) && (Festivos[i].FestFechaHasta <= detincidencias.FechaFinal))
                                    {
                                        Dias = Dias - 1;
                                    }
                                }

                                var HorarioEmpleado = _context.MovEmpleadosHorarios.Where(x => x.EmpHoraEmpId == inci.ReInciEmpId && x.EmpHoraFechaHasta == null).FirstOrDefault();
                                var Horario = _context.CatHorarios.Where(x => x.HoraId == HorarioEmpleado.EmpHoraHorId).FirstOrDefault();
                                bool BanderaLV = false;

                                if (Horario.HoraSabadoEntrada == null)
                                {
                                    BanderaLV = true;
                                }

                                if (BanderaLV == true)
                                {
                                    do
                                    {
                                        if (Fecha.DayOfWeek == DayOfWeek.Saturday) Dias--;

                                        Fecha = Fecha.AddDays(1);

                                    } while (!(Fecha > detincidencias.FechaFinal));
                                }
                            }

                            CatDetalleIncidencias detIn = new CatDetalleIncidencias() // SE HACE EL OBJETO CON LOS DATOS DE LA TABLA DEPENDIENTE
                            {
                                DetInciReInciId = inci.ReInciId,
                                DetInciHorarioId = detincidencias.DetInciHorarioId,
                                MedidaAccion = detincidencias.MedidaAccion,
                                Asunto = detincidencias.Asunto,
                                Destino = detincidencias.Destino,
                                TelDestino = detincidencias.TelDestino,
                                Contacto1 = detincidencias.Contacto1,
                                NombreDestino = detincidencias.NombreDestino,
                                Contacto2 = detincidencias.Contacto2,
                                Observaciones = detincidencias.Observaciones,
                                HoraSalida = detincidencias.HoraSalida,
                                HoraRegreso = detincidencias.HoraRegreso,
                                FechaInicio = detincidencias.FechaInicio,
                                FechaFinal = detincidencias.FechaFinal,
                                FechaPresentacion = detincidencias.FechaPresentacion,
                                DiasAusencia = Dias,
                                PersonaCubrira = detincidencias.PersonaCubrira,
                                Motivo = detincidencias.Motivo,
                                DetFecha = detincidencias.DetFecha,
                                DetInciFechaFinPermisoPersonal = detincidencias.DetInciFechaFinPermisoPersonal,
                                DetInciHoraIngreso = detincidencias.DetInciHoraIngreso,
                                DetInciHoraSalida = detincidencias.DetInciHoraSalida,
                                DetInciHoraSalidaComida = detincidencias.DetInciHoraSalidaComida,
                                DetInciHoraRegresoComida = detincidencias.DetInciHoraRegresoComida
                            };

                            List<CatDetalleIncidencias> Details = new List<CatDetalleIncidencias>(); // SE HACE UNA NUEVA LISTA
                            Details.Add(detIn); // La lista tomará los valores del objeto

                            inci.DetalleIncidencias = Details;
                            _context.CatRegistroIncidencias.Add(inci);
                            _context.SaveChanges();

                            BitInci.BitInciReInciId = inci.ReInciId;
                            BitInci.BitInciDetFlujoId = inci.ReInciEstatusFlujo;
                            BitInci.BitInciUserId = user.UserName;
                            BitInci.BitInciFecha = inci.Fecha;
                            BitInci.BitInciObservaciones = "Incidencia creada por capital humano. Incidencia cerrada.";
                            _context.CatBitacoraIncidencias.Add(BitInci);
                            _context.SaveChanges();
                            DateTime FechaActual = DateTime.Now;

                            if (inci.ReInciInciId == 4)
                            {
                                var Empleado = _context.CatEmpleados.Where(x => x.EmpNumero == NominaEmpleado).FirstOrDefault();
                                var Permiso = _context.CatRegistroIncidencias.Where(x => x.ReInciId == inci.ReInciId).FirstOrDefault();
                                var DetallesPermiso = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == inci.ReInciId).FirstOrDefault();
                                var AñoEmpleado = Empleado.EmpFechaIngreso.Value.Year;
                                var DiasPermiso = Dias;
                                var Resultado = FechaActual.Year - AñoEmpleado;
                                if (Resultado == 0)
                                {
                                    vaca.HVAntiguedadId = 11;
                                }
                                else if (Resultado == 1)
                                {
                                    vaca.HVAntiguedadId = 12;
                                }
                                else if (Resultado == 2)
                                {
                                    vaca.HVAntiguedadId = 13;
                                }
                                else if (Resultado == 3)
                                {
                                    vaca.HVAntiguedadId = 14;
                                }
                                else if (Resultado == 4)
                                {
                                    vaca.HVAntiguedadId = 15;
                                }
                                else if (Resultado == 5)
                                {
                                    vaca.HVAntiguedadId = 16;
                                }
                                else if (Resultado >= 6 && Resultado <= 10 )
                                {
                                    vaca.HVAntiguedadId = 17;
                                }
                                else if (Resultado >= 11 && Resultado <= 15 )
                                {
                                    vaca.HVAntiguedadId = 18;
                                }
                                else if (Resultado >= 16 && Resultado <= 20)
                                {
                                    vaca.HVAntiguedadId = 19;
                                }
                                else if (Resultado >= 21 && Resultado <= 25)
                                {
                                    vaca.HVAntiguedadId = 20;
                                }
                                else if (Resultado >= 26 && Resultado <= 30)
                                {
                                    vaca.HVAntiguedadId = 22;
                                }
                                else if (Resultado >= 30 && Resultado <= 35)
                                {
                                    vaca.HVAntiguedadId = 23;
                                }
                                else
                                {
                                    vaca.HVAntiguedadId = 23;
                                }
                                var Antiguedad = _context.CatAntiguedad.Where(x => x.AntiId == vaca.HVAntiguedadId).First();
                                var DiasCorrespondientes = Antiguedad.AntiDias;

                                // Asignación de valores a vaca (vacaciones)
                                vaca.HVEmpId = Empleado.EmpId;
                                vaca.HVDiasSolicitados = DiasPermiso;
                                vaca.HVEjercicio = FechaActual.Year;
                                vaca.HVFechaSolicitud = Permiso.Fecha;
                                vaca.HVFechaInicio = DetallesPermiso.FechaInicio;
                                vaca.HVFechaCulminacion = DetallesPermiso.FechaFinal;
                                vaca.HVFechaPresentacion = DetallesPermiso.FechaPresentacion;
                                vaca.HVReInciId = Permiso.ReInciId;
                                vaca.HVPuestoId = Empleado.EmpPuestoId;
                                vaca.HVSucursalId = Empleado.EmpSucuId;

                                // validamos para registros anteriores
                                var DiasDisfrutadosRegistro = _context.CatHistorialVacaciones.Where(x => x.HVEmpId == Empleado.EmpId && x.HVEjercicio == vaca.HVEjercicio).Sum(suma => suma.HVDiasSolicitados);
                                var RegistrosHistorialEmpleado = _context.CatHistorialVacaciones.Where(x => x.HVEmpId == Empleado.EmpId && x.HVEjercicio == vaca.HVEjercicio).OrderByDescending(x => x.HVId).ToList();
                                var DaysAvaliable = 0;
                                var SumaDiasTomados = 0;
                                if (RegistrosHistorialEmpleado == null) // Si no tiene registros previos
                                {
                                    DaysAvaliable = DiasCorrespondientes - vaca.HVDiasSolicitados;
                                    vaca.HVSaldoVacaciones = DaysAvaliable;
                                }
                                else // Si tiene registros previos
                                {
                                    SumaDiasTomados = vaca.HVDiasSolicitados + DiasDisfrutadosRegistro;
                                    DaysAvaliable = DiasCorrespondientes - SumaDiasTomados;
                                    vaca.HVSaldoVacaciones = DaysAvaliable;
                                }

                                _context.CatHistorialVacaciones.Add(vaca);
                                _context.SaveChanges();
                                var x = _context.CatHistorialVacaciones.Where(x => x.HVEmpId == vaca.HVEmpId).First();
                            }

                            if(inci.ReInciInciId == 5)
                            {
                                MovPermisosPorTiempo PPT = new MovPermisosPorTiempo()
                                {
                                    PPTEmpId = inci.ReInciEmpId,
                                    PPTFechaInicio = detincidencias.DetFecha,
                                    PPTFechaFinal = detincidencias.FechaFinal,
                                    PPTHoraEntrada = detincidencias.DetInciHoraSalida,
                                    PPTHoraSalida = detincidencias.DetInciHoraSalida,
                                    PPTHoraSalidaComida = detincidencias.DetInciHoraSalidaComida,
                                    PPTHoraRegresoComida = detincidencias.DetInciHoraRegresoComida,
                                    PPTEstatusId = 10,
                                    PPTReInciId = inci.ReInciId
                                };

                                _context.MovPermisosPorTiempo.Add(PPT);
                                _context.SaveChanges();
                                
                            }
                            return Ok(inci);
                        }
                        else
                        {
                            var ListaFlujos = _context.CatFlujos.Where(x => x.FlujoDescripcion == NombreIncidenciaParaFlujo).First();
                            var ListaDetalleFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == ListaFlujos.FlujoId).First();

                            CatRegistroIncidencias inci = new CatRegistroIncidencias() // SE HACE EL OBJETO CON LOS DATOS
                            {
                                ReInciEmpId = Empleados.EmpId,
                                Fecha = incidencias.Fecha,
                                ReInciInciId = incidencias.ReInciInciId,
                                ReInciEstatusId = 3,
                                ReInciEstatusFlujo = ListaDetalleFlujos.DetFlujoId
                            };
                            var HorarioDefault = 2055;
                            if (detincidencias.DetInciHorarioId == 0)
                            {
                                detincidencias.DetInciHorarioId = HorarioDefault;
                            }

                            TimeSpan? RestaFechas;
                            int Dias = 0;
                            RestaFechas = detincidencias.FechaFinal - detincidencias.FechaInicio;

                            if (inci.ReInciInciId == 4)
                            {
                                Dias = RestaFechas.Value.Days + 1;
                                DateTime Fecha = detincidencias.FechaInicio;
                                do
                                {
                                    if (Fecha.DayOfWeek == DayOfWeek.Sunday) Dias--;

                                    Fecha = Fecha.AddDays(1);

                                } while (!(Fecha > detincidencias.FechaFinal));
                                var Festivos = _context.CatFestivos.OrderBy(x => x.FestId).ToList();
                                for (int i = 0; i < Festivos.Count; i++)
                                {
                                    if ((Festivos[i].FestFechaDesde >= detincidencias.FechaInicio) && (Festivos[i].FestFechaHasta <= detincidencias.FechaFinal))
                                    {
                                        Dias = Dias - 1;
                                    }
                                }

                                var HorarioEmpleado = _context.MovEmpleadosHorarios.Where(x => x.EmpHoraEmpId == inci.ReInciEmpId && x.EmpHoraFechaHasta == null).FirstOrDefault();
                                var Horario = _context.CatHorarios.Where(x => x.HoraId == HorarioEmpleado.EmpHoraHorId).FirstOrDefault();
                                bool BanderaLV = false;
                                if (Horario.HoraSabadoEntrada == null)
                                {
                                    BanderaLV = true;
                                }
                                if (BanderaLV == true)
                                {
                                    do
                                    {
                                        if (Fecha.DayOfWeek == DayOfWeek.Saturday) Dias--;

                                        Fecha = Fecha.AddDays(1);

                                    } while (!(Fecha > detincidencias.FechaFinal));
                                }
                            }

                            CatDetalleIncidencias detIn = new CatDetalleIncidencias() // SE HACE EL OBJETO CON LOS DATOS DE LA TABLA DEPENDIENTE
                            {
                                DetInciReInciId = inci.ReInciId,
                                DetInciHorarioId = detincidencias.DetInciHorarioId,
                                MedidaAccion = detincidencias.MedidaAccion,
                                Asunto = detincidencias.Asunto,
                                Destino = detincidencias.Destino,
                                TelDestino = detincidencias.TelDestino,
                                Contacto1 = detincidencias.Contacto1,
                                NombreDestino = detincidencias.NombreDestino,
                                Contacto2 = detincidencias.Contacto2,
                                Observaciones = detincidencias.Observaciones,
                                HoraSalida = detincidencias.HoraSalida,
                                HoraRegreso = detincidencias.HoraRegreso,
                                FechaInicio = detincidencias.FechaInicio,
                                FechaFinal = detincidencias.FechaFinal,
                                FechaPresentacion = detincidencias.FechaPresentacion,
                                DiasAusencia = Dias,
                                PersonaCubrira = detincidencias.PersonaCubrira,
                                Motivo = detincidencias.Motivo,
                                DetFecha = detincidencias.DetFecha,
                                DetInciFechaFinPermisoPersonal = detincidencias.DetInciFechaFinPermisoPersonal,
                                DetInciHoraIngreso = detincidencias.DetInciHoraIngreso,
                                DetInciHoraSalida = detincidencias.DetInciHoraSalida,
                                DetInciHoraSalidaComida = detincidencias.DetInciHoraSalidaComida,
                                DetInciHoraRegresoComida = detincidencias.DetInciHoraRegresoComida
                            };

                            List<CatDetalleIncidencias> Details = new List<CatDetalleIncidencias>(); // SE HACE UNA NUEVA LISTA
                            Details.Add(detIn); // La lista tomará los valores del objeto

                            inci.DetalleIncidencias = Details;
                            _context.CatRegistroIncidencias.Add(inci);
                            _context.SaveChanges();

                            BitInci.BitInciReInciId = inci.ReInciId;
                            BitInci.BitInciDetFlujoId = inci.ReInciEstatusFlujo;
                            BitInci.BitInciUserId = user.UserName;
                            BitInci.BitInciFecha = inci.Fecha;
                            BitInci.BitInciObservaciones = "Incidencia creada por " + Empleados.EmpNombreCompleto;
                            _context.CatBitacoraIncidencias.Add(BitInci);
                            _context.SaveChanges();

                            if (inci.ReInciInciId == 5)
                            {
                                MovPermisosPorTiempo PPT = new MovPermisosPorTiempo()
                                {
                                    PPTEmpId = inci.ReInciEmpId,
                                    PPTFechaInicio = detincidencias.DetFecha,
                                    PPTFechaFinal = detincidencias.FechaFinal,
                                    PPTHoraEntrada = detincidencias.DetInciHoraSalida,
                                    PPTHoraSalida = detincidencias.DetInciHoraSalida,
                                    PPTHoraSalidaComida = detincidencias.DetInciHoraSalidaComida,
                                    PPTHoraRegresoComida = detincidencias.DetInciHoraRegresoComida,
                                    PPTEstatusId = 3,
                                    PPTReInciId = inci.ReInciId
                                };

                                _context.MovPermisosPorTiempo.Add(PPT);
                                _context.SaveChanges();

                            }

                            // ---------------------------------- Envío de correos -------------------------------------

                            var empleado = _context.CatEmpleados.Where(x => x.EmpId == inci.ReInciEmpId).First();
                            var puesto = _context.CatPuestos.Where(x => x.PuestoId == empleado.EmpPuestoId).First();
                            var EmpleadoPuestoSuperior = _context.CatEmpleados.Where(x => x.EmpPuestoId == puesto.PuestoJerarquiaSuperiorPuestoId).First();
                            var UsuarioJefeSuperior = _userManager.Users.Where(x => x.UserName == EmpleadoPuestoSuperior.EmpUserId).First();  // <---------- Este es el chido
                            var TodasLasIncidencias = _context.CatIncidencias.Where(x => x.InciId == inci.ReInciInciId).First();
                            var NombreIncidencia = TodasLasIncidencias.InciDescripcion;
                            var destinatario = UsuarioJefeSuperior.Email;
                            var remitente = user.Email;
                            var FechaCorta = inci.Fecha.ToShortDateString();
                            var asunto = "Nueva Solicitud de permiso";
                            var asuntoremitente = "Estatus de tu solicitud de permiso";
                            var urlGlobal = "192.168.0.5/CatRegistroIncidencias?";
                            string Bandera = "3424hjlk234"; // Aceptada
                            var urlaceptada = urlGlobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioJefeSuperior.Id + "&Fkch23=" /* Folio */ + inci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                            Bandera = "jfnROs34"; // Rechazada
                            var urlrechazada = urlGlobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioJefeSuperior.Id + "&Fkch23=" /* Folio */ + inci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                            Bandera = "4RT55cgd6FOR"; // Reenvío
                            var UrlDetallesIncidencia = urlGlobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioJefeSuperior.Id + "&Fkch23=" /* Folio */ + inci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                            var bodyBuilder = new BodyBuilder();

                            bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                              <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                              <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                              <th width='15%'>{0}</th> </tr> <tr>
                              <td colspan='3'> <br /> <br /><br />
                              <div style='display:block; text-align:center;'> <p>¡Se ha creado una nueva solicitud de permiso de tipo {1} a nombre de {2}  para su revisión!</p>
                            <p>Para aceptar el permiso, pulse el botón verde, para rechazarlo, pulse el botón rojo...</p> Petición de permiso No. {4}<br />
                             <a href=""{3}""><img src=""https://i.imgur.com/Q7D7Kz5.png"" style='width:30%;'></a><a href=""{5}""><img src=""https://i.imgur.com/bHsgWWI.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, NombreIncidencia, empleado.EmpNombreCompleto, urlaceptada, inci.ReInciId, urlrechazada);

                            var bodyBuilderRemitente = new BodyBuilder();
                            bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                              <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                              <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                              <th width='15%'>{0}</th> </tr> <tr>
                              <td colspan='3'> <br /> <br /><br />
                              <div style='display:block; text-align:center;'> 
                              <p>¡ {1} tu permiso de tipo  {2} y ha sido enviada a {3} para su revisión!</p>
                              <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {5}</p><br /><a href=""{4}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, empleado.EmpNombreCompleto, NombreIncidencia, EmpleadoPuestoSuperior.EmpNombreCompleto, UrlDetallesIncidencia, inci.ReInciId);

                            MailMessage correo = new MailMessage(); // <- Correo mandado para el jefe
                            correo.From = new MailAddress("nucleorh@nucleodediagnostico.com");
                            correo.To.Add(destinatario);
                            correo.Subject = asunto;
                            correo.Body = bodyBuilder.HtmlBody;
                            correo.IsBodyHtml = true;
                            correo.Priority = MailPriority.Normal;

                            MailMessage correoresultado = new MailMessage(); // <- Correo mandado para el remitente, es decir, el empleado de la incidencia
                            correoresultado.From = new MailAddress("nucleorh@nucleodediagnostico.com");
                            correoresultado.To.Add(remitente);
                            correoresultado.Subject = asuntoremitente;
                            correoresultado.Body = bodyBuilderRemitente.HtmlBody;
                            correoresultado.IsBodyHtml = true;
                            correoresultado.Priority = MailPriority.Normal;

                            smtp.Send(correo);
                            smtp.Send(correoresultado);

                            return Ok(inci);
                        }
                    }
                    
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.StackTrace);
            }
        }
        // ************************* MÉTODO DE DETALLES  - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [Microsoft.AspNetCore.Mvc.HttpGet("DetallesDeLaIncidencia/{Id}")]
        public IActionResult MostrarModalDeDetallesIncidencias(int Id)
        {           
            try
            {
                var inci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Id).First();
                var detinci = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == Id ).First();
                var HorariosConcatenados = _context.CatHorarios.Where(x => x.HoraId == detinci.DetInciHorarioId).First();
                var HorarioEnt = HorariosConcatenados.HoraEntrada.ToString();
                var HorarioSal = HorariosConcatenados.HoraSalida.ToString();
                var HorarioEntSabado = HorariosConcatenados.HoraSabadoEntrada.ToString();
                var HorarioSalSabado = HorariosConcatenados.HoraSabadoSalida.ToString();
                string HorarioFinal = "L-V: " + HorarioEnt + " - " + HorarioSal + " Y S de: " + HorarioEntSabado + " - " + HorarioSalSabado;
                var FullName = _context.CatEmpleados.Where(x => x.EmpId == inci.ReInciEmpId).First();
                var Nombre = FullName.EmpNombre.ToString();
                var Paterno = FullName.EmpPaterno.ToString();
                var Materno = FullName.EmpMaterno.ToString();
                string NombreCompleto = Nombre + " " + Paterno + " " + Materno;
                var EstadoListado = _context.CatEstatus.Where(x => x.EstId == inci.ReInciEstatusId).First();
                string EstatusConversion = EstadoListado.EstDescripcion.ToString();
                var ListadoIncidencias = _context.CatIncidencias.Where(x => x.InciId == inci.ReInciInciId).First();
                string IncidenciasConversion = ListadoIncidencias.InciDescripcion.ToString();
                int IdPersonaCubrira = 0;
                if(detinci.PersonaCubrira == null)
                {
                    //IdPersonaCubrira = 2085;
                    IdPersonaCubrira = 2075;
                }
                else
                {
                    IdPersonaCubrira = int.Parse(detinci.PersonaCubrira);
                }
                
                var EmpleadoCubrira = _context.CatEmpleados.Where(x => x.EmpId == IdPersonaCubrira).FirstOrDefault();

                var fefinper = "";
                var HoraIngreso = "";
                var HoraSalida = "";
                var HoraSComida = "";
                var HoraRComida = "";

                if (detinci.DetInciFechaFinPermisoPersonal != null)
                {
                    fefinper = detinci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString();
                }

                if (detinci.DetInciHoraSalida != null)
                {
                    HoraIngreso = detinci.DetInciHoraSalida.ToString();
                }

                if (detinci.DetInciHoraIngreso != null)
                {
                    HoraSalida = detinci.DetInciHoraIngreso.ToString();
                }

                if (detinci.DetInciHoraSalidaComida != null)
                {
                    HoraSComida = detinci.DetInciHoraSalidaComida.ToString();
                }

                if (detinci.DetInciHoraRegresoComida != null)
                {
                    HoraRComida = detinci.DetInciHoraRegresoComida.ToString();
                }

                var obj = new
                {
                    inci.ReInciId,
                    inci.ReInciEmpId,
                    inci.ReInciEstatusId,
                    NombreCompleto,
                    FeGlobal = inci.Fecha.ToShortDateString(),
                    IncidenciasConversion,
                    // inci.ReInciInciId,
                    EstatusConversion,
                    //inci.ReInciEstatusId,
                    detinci.DetInciId,
                    detinci.DetInciReInciId,
                    detinci.DetInciHorarioId,
                    HorarioFinal, // String de los horarios y concatenados
                    detinci.MedidaAccion,
                    detinci.Asunto,
                    detinci.Destino,
                    detinci.TelDestino,
                    detinci.Contacto1,
                    detinci.Contacto2,
                    detinci.NombreDestino,
                    detinci.Observaciones,
                    Hos = detinci.HoraSalida.ToString(),
                    Hor = detinci.HoraRegreso.ToString(),
                    FeIni = detinci.FechaInicio.ToShortDateString(),
                    FeFin = detinci.FechaFinal.ToShortDateString(),
                    FePres = detinci.FechaPresentacion.ToShortDateString(),
                    detinci.DiasAusencia,
                    detinci.PersonaCubrira,
                    FechaDetails = detinci.DetFecha.ToShortDateString(),
                    detinci.Motivo,
                    EmpleadoCubrira.EmpNombreCompleto,
                    fefinper,
                    HoraIngreso,
                    HoraSalida,
                    HoraSComida,
                    HoraRComida,
                };

                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        public class HorariosFiltrados
        {
            public int HoraID { get; set; }
            public string Hora { get; set; }
            public string HoraSali { get; set; }
            public string HoraIngresoSabado { get; set; }
            public string HoraSalidaSabado { get; set; }
        }


        [Microsoft.AspNetCore.Mvc.HttpGet("MostrarFormularios/{Id}")]
        public IActionResult SeleccionarFormularioPorIncidencia(int Id)
        {
            var Horario = _context.CatHorarios.ToList();
            List<HorariosFiltrados> horarios = new List<HorariosFiltrados>();
            foreach (var item in Horario)
            {
                horarios.Add(new HorariosFiltrados()
                {
                    HoraID = item.HoraId,
                    Hora = item.HoraEntrada.ToString(),
                    HoraSali = item.HoraSalida.ToString(),
                    HoraIngresoSabado = item.HoraSabadoEntrada.ToString(),
                    HoraSalidaSabado = item.HoraSabadoSalida.ToString()
                });
            }
            try
            {
                var contar = _context.CatRegistroIncidencias.Count(x => x.ReInciInciId == Id);
                CatRegistroIncidencias validacion = null;
                if (contar > 0)
                {
                    validacion = _context.CatRegistroIncidencias.Where(x => x.ReInciInciId == Id).First();
                }
                var obj = new
                {
                    //Formulario,
                    horarios,
                    validacion
                };
                
                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("PermisosRegistroIncidencias")]
        public async Task<IActionResult> PermisosRegistroIncidencias()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 11 && x.URPUserId == user.Id).ToList();
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

        [Microsoft.AspNetCore.Mvc.HttpGet("RegistroIncidenciaById/{Id}")]
        public async Task<IActionResult> RegistroIncidenciaById(int Id)
        {
            try
            {
                var ReInci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Id).First();
                var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                var GetFlujoActual = ReInci.ReInciEstatusFlujo;
                var DetallesDeFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == ReInci.ReInciEstatusFlujo).First();
                bool PermisoBotonEditar = false;
                var IdEmpleado = ReInci.ReInciEmpId;
                var FechaDelRegistro = ReInci.Fecha;
                var Perfiles = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
                var GetPerfilesDelFlujo = DetallesDeFlujos.DetFlujoPerfiles;
                var coma = ",";
                string[] ListaPerfiles = GetPerfilesDelFlujo.Split(coma);
                for (var i = 0; i < ListaPerfiles.Length; i++)
                {
                    for (var j = 0; j < Perfiles.Count; j++)
                    {
                        if (Perfiles[j].RoleId == ListaPerfiles[i])
                        {
                            PermisoBotonEditar = true;
                            break;
                        }
                    }
                }
                var EstadoIncidencia = ReInci.ReInciEstatusId;
                bool BanderaEstadoIncidencia = false;
                if(EstadoIncidencia == 6 || EstadoIncidencia == 5 || EstadoIncidencia == 4)
                {
                    BanderaEstadoIncidencia = false;
                }
                else
                {
                    BanderaEstadoIncidencia = true;
                }
                var FechaCortada = ReInci.Fecha.ToShortDateString();
                var detinci = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == Id).First();
                var HorariosConcatenados = _context.CatHorarios.Where(x => x.HoraId == detinci.DetInciHorarioId).First();
                var HorarioEnt = HorariosConcatenados.HoraEntrada.ToString();
                var HorarioSal = HorariosConcatenados.HoraSalida.ToString();
                var HorarioEntSabado = HorariosConcatenados.HoraSabadoEntrada.ToString();
                var HorarioSalSabado = HorariosConcatenados.HoraSabadoSalida.ToString();
                string HorarioFinalOME = "L-V: " + HorarioEnt + " - " + HorarioSal + " Y S de: " + HorarioEntSabado + " - " + HorarioSalSabado;
                var ListadoIncidencias = _context.CatIncidencias.Where(x => x.InciId == ReInci.ReInciInciId).First();
                string IncidenciasConversion = ListadoIncidencias.InciDescripcion.ToString();
                int IdPersonaCubriraOME = int.Parse(detinci.PersonaCubrira);
                var EmpleadoCubriraOME = _context.CatEmpleados.Where(x => x.EmpId == int.Parse(detinci.PersonaCubrira)).FirstOrDefault();

                var fefinper = "";
                var HoraIngreso = "";
                var HoraSalidaPermisoPersonal = "";
                var HoraSComida = "";
                var HoraRComida = "";

                if (detinci.DetInciFechaFinPermisoPersonal != null)
                {
                    fefinper = detinci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString();
                }

                if (detinci.DetInciHoraSalida != null)
                {
                    HoraSalidaPermisoPersonal = detinci.DetInciHoraSalida.ToString();
                }

                if (detinci.DetInciHoraIngreso != null)
                {
                    HoraIngreso = detinci.DetInciHoraIngreso.ToString();
                }

                if (detinci.DetInciHoraSalidaComida != null)
                {
                    HoraSComida = detinci.DetInciHoraSalidaComida.ToString();
                }

                if (detinci.DetInciHoraRegresoComida != null)
                {
                    HoraRComida = detinci.DetInciHoraRegresoComida.ToString();
                }

                var obj = new
                {
                    // ReInci
                    FechaCortada,
                    ReInci.ReInciEmpId,
                    ReInci.Fecha,
                    ReInci.ReInciInciId,
                    ReInci.ReInciEstatusId,
                    ReInci.ReInciId,
                    IdEmpleado,
                    PermisoBotonEditar,
                    BanderaEstadoIncidencia,
                    IncidenciasConversion,
                    diiOME = detinci.DetInciId,
                    diriiOME= detinci.DetInciReInciId,
                    dihiOME= detinci.DetInciHorarioId,
                    HorarioFinalOME, // String de los horarios y concatenados
                    maOME= detinci.MedidaAccion,
                    asuntoOME = detinci.Asunto,
                    destinoOME = detinci.Destino,
                    teldestinoOME = detinci.TelDestino,
                    con1OME= detinci.Contacto1,
                    con2OME = detinci.Contacto2,
                    nombredestinoOME = detinci.NombreDestino,
                    observacionesOME = detinci.Observaciones,
                    HosOME = detinci.HoraSalida.ToString(),
                    HorOME = detinci.HoraRegreso.ToString(),
                    FeIniOME = detinci.FechaInicio.ToShortDateString(),
                    FeFinOME = detinci.FechaFinal.ToShortDateString(),
                    FePresOME = detinci.FechaPresentacion.ToShortDateString(),
                    diasausenciaOME= detinci.DiasAusencia,
                    personacubriraOME= EmpleadoCubriraOME.EmpNombreCompleto,
                    FechaDetailsOME = detinci.DetFecha.ToShortDateString(),
                    motivoOME = detinci.Motivo,
                    fefinper,
                    HoraIngreso,
                    HoraSalidaPermisoPersonal,
                    HoraSComida,
                    HoraRComida
            };

                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("AddBitacoraIncidencias")]
        public async Task<IActionResult> AddBitacoraIncidencias(CatBitacoraIncidencias BitInci, int EstatusNew)
        {
            try
            {
                if (BitInci != null)
                {
                    // ------------ Configuración del SMTP --------------

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    // ------------------------------------------

                    var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));

                    DateTime FechaActual = DateTime.Now;
                    var RegInci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == BitInci.BitInciReInciId).First();
                    var ListaDetalleFlujos = _context.CatDetalleFlujo.OrderBy(x => x.DetFlujoId).ToList();
                    var DetalleFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == RegInci.ReInciEstatusFlujo).First();
                    var Flujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == DetalleFlujos.DetFlujoFlujoId).ToList();
                    var OrdenNuevo = 0;
                    var NuevoIdFlujo = 0;

                    for (int i = 0; i < Flujos.Count; i++)
                    {
                        var OrdenActual = DetalleFlujos.DetFlujoOrden;
                        if (RegInci.ReInciEstatusId != 6 || RegInci.ReInciEstatusId != 5) // Validación para que si llega un registro YA cerrado o cancelado, no permita seguir
                        {
                            if (EstatusNew == 5 || EstatusNew == 4 || EstatusNew == 6) // Validación para cerrar registro
                            {
                                var UltimoFlujo = Flujos.Where(x => x.DetFlujoFlujoId == DetalleFlujos.DetFlujoFlujoId).Last();
                                var RegistroCerrado = UltimoFlujo.DetFlujoId;
                                NuevoIdFlujo = RegistroCerrado;
                                var RegistroEstatus = _context.CatEstatus.Where(x => x.EstId == EstatusNew).First();
                                var EstatusDescripcion = RegistroEstatus.EstDescripcion;

                                BitInci.BitInciReInciId = RegInci.ReInciId;
                                BitInci.BitInciUserId = user.UserName;
                                BitInci.BitInciFecha = FechaActual;
                                BitInci.BitInciObservaciones = BitInci.BitInciObservaciones;
                                BitInci.BitInciDetFlujoId = NuevoIdFlujo;

                                RegInci.ReInciEstatusFlujo = BitInci.BitInciDetFlujoId;
                                RegInci.ReInciEstatusId = EstatusNew;

                                _context.CatBitacoraIncidencias.Add(BitInci);
                                _context.Update(RegInci);
                                _context.SaveChanges();

                                var empleados = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).First();
                                var UsuarioDeLaIncidencia = _userManager.Users.Where(x => x.UserName == empleados.EmpUserId).First();
                                var Paso = DetalleFlujos.DetFlujoDescripcion;
                                var destinatario = UsuarioDeLaIncidencia.Email;
                                var TodasLasIncidencias = _context.CatIncidencias.OrderBy(x => x.InciId).ToList();
                                var IncidenciaDelPermiso = TodasLasIncidencias.Where(x => x.InciId == RegInci.ReInciInciId).First();
                                var NombreIncidencia = IncidenciaDelPermiso.InciDescripcion;
                                var mensaje = "El permiso de tipo " + NombreIncidencia + " con el folio # " + RegInci.ReInciId + " ahora tiene el estatus de: " + EstatusDescripcion + ". La solicitud sufrió un cambio de estado en el paso de " + Paso + " y las observaciones que se hicieron fueron las siguientes:"  + BitInci.BitInciObservaciones + ". Si el mismo está cancelado o denegado, por favor revise su solicitud, haga las correcciones pertinentes y vuelva a realizar el registro. Gracias.";
                                var asunto = "Cambio de estatus de tu solicitud de permiso";
                                var Bandera = "NewStatus";
                                Bandera = "4RT55cgd6FOR"; // Reenvío
                                var urlglobal = "http://192.168.0.5/CatRegistroIncidencias?";
                                var UrlRegistroEstatusModificado = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioDeLaIncidencia.Id + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                                DateTime FechaActualMail = DateTime.Now;
                                var FechaCorta = FechaActualMail.ToShortDateString();
                                var empleado = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).FirstOrDefault();
                                var EmpleadoQueCierraIncidencia = _context.CatEmpleados.Where(x => x.EmpUserId == user.UserName).FirstOrDefault();

                                var bodyBuilderRemitente = new BodyBuilder();
                                bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} tu permiso de tipo  {2} y ha sido cerrado por {3}, por el siguiente motivo: {6}!</p>
                <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {5}</p><br /><a href=""{4}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, empleado.EmpNombreCompleto, NombreIncidencia, EmpleadoQueCierraIncidencia.EmpNombreCompleto, UrlRegistroEstatusModificado, RegInci.ReInciId, BitInci.BitInciObservaciones);

                                MailMessage correo = new MailMessage();
                                correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                                correo.To.Add(destinatario);
                                correo.Subject = asunto;
                                correo.Body = bodyBuilderRemitente.HtmlBody;
                                correo.IsBodyHtml = true;
                                correo.Priority = MailPriority.Normal;

                                smtp.Send(correo);

                                break;
                            }
                            else if (EstatusNew == 9)
                            { // Validación para cuando el registro es regresado para revisión
                                if (RegInci.ReInciEstatusId != EstatusNew) // Para cuando cambia de pendiente a regresado
                                {
                                    var BitAnterior = _context.CatBitacoraIncidencias.Where(x => x.BitInciReInciId == BitInci.BitInciReInciId).ToList();
                                    var BitacoraAnterior = BitAnterior.Last();
                                    var RegistroReiniciado = BitacoraAnterior.BitInciDetFlujoId;
                                    NuevoIdFlujo = RegistroReiniciado;

                                    BitInci.BitInciReInciId = RegInci.ReInciId;
                                    BitInci.BitInciUserId = user.UserName;
                                    BitInci.BitInciFecha = FechaActual;
                                    BitInci.BitInciObservaciones = BitInci.BitInciObservaciones;
                                    BitInci.BitInciDetFlujoId = NuevoIdFlujo;

                                    RegInci.ReInciEstatusFlujo = BitInci.BitInciDetFlujoId;
                                    RegInci.ReInciEstatusId = EstatusNew;

                                    _context.CatBitacoraIncidencias.Add(BitInci);
                                    _context.Update(RegInci);
                                    _context.SaveChanges();

                                    var Estatus = _context.CatEstatus.Where(x => x.EstId == EstatusNew).First();
                                    var EstatusDescripcion = Estatus.EstDescripcion;
                                    var empleados = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).First();
                                    var UsuarioDeLaIncidencia = _userManager.Users.Where(x => x.UserName == empleados.EmpUserId).First();
                                    var Paso = DetalleFlujos.DetFlujoDescripcion;
                                    var TodasLasIncidencias = _context.CatIncidencias.OrderBy(x => x.InciId).ToList();
                                    var IncidenciaDelPermiso = TodasLasIncidencias.Where(x => x.InciId == RegInci.ReInciInciId).First();
                                    var NombreIncidencia = IncidenciaDelPermiso.InciDescripcion;
                                    var destinatario = UsuarioDeLaIncidencia.Email;
                                    var mensaje = "El permiso de tipo " + NombreIncidencia + " con el folio # " + RegInci.ReInciId + " ahora tiene el estatus de: " + EstatusDescripcion + ". La solicitud sufrió un cambio de estado en el paso de " + Paso + " y las observaciones que se hicieron fueron las siguientes:" + BitInci.BitInciObservaciones + ". Por favor revise su solicitud, haga las correcciones pertinentes y vuelva a realizar el registro. Gracias.";
                                    var asunto = "Cambio de estatus de tu solicitud de permiso";
                                    var Bandera = "4RT55cgd6FOR"; // Reenvío
                                    var urlglobal = "http://192.168.0.5/CatRegistroIncidencias?";
                                    var UrlRegistroRegresado = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioDeLaIncidencia.Id + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                                    DateTime FechaActualMail = DateTime.Now;
                                    var FechaCorta = FechaActualMail.ToShortDateString();
                                    var empleado = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).FirstOrDefault();
                                    var EmpleadoRegresaIncidencia = _context.CatEmpleados.Where(x => x.EmpUserId == user.UserName).FirstOrDefault();

                                    var bodyBuilderRemitente = new BodyBuilder();
                                    bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} tu permiso de tipo  {2} y ha sido regresada a revisión por {3}, por el siguiente motivo: {6}!</p>
                <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {5}</p><br /><a href=""{4}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, empleado.EmpNombreCompleto, NombreIncidencia, EmpleadoRegresaIncidencia.EmpNombreCompleto, UrlRegistroRegresado, RegInci.ReInciId, BitInci.BitInciObservaciones);

                                    MailMessage correo = new MailMessage();
                                    correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                                    correo.To.Add(destinatario);
                                    correo.Subject = asunto;
                                    correo.Body = bodyBuilderRemitente.HtmlBody;
                                    correo.IsBodyHtml = true;
                                    correo.Priority = MailPriority.Normal;

                                    smtp.Send(correo);

                                    break;
                                }
                                else // Cuando de regresado pasará a estar como pendiente.
                                {
                                    var BitAnterior = _context.CatBitacoraIncidencias.Where(x => x.BitInciReInciId == BitInci.BitInciReInciId).ToList();
                                    var BitacoraAnterior = BitAnterior.Last();
                                    var RegistroReiniciado = BitacoraAnterior.BitInciDetFlujoId;
                                    NuevoIdFlujo = RegistroReiniciado;

                                    BitInci.BitInciReInciId = RegInci.ReInciId;
                                    BitInci.BitInciUserId = user.UserName;
                                    BitInci.BitInciFecha = FechaActual;
                                    BitInci.BitInciObservaciones = BitInci.BitInciObservaciones;
                                    BitInci.BitInciDetFlujoId = NuevoIdFlujo;

                                    RegInci.ReInciEstatusFlujo = BitInci.BitInciDetFlujoId;
                                    RegInci.ReInciEstatusId = 3;

                                    _context.CatBitacoraIncidencias.Add(BitInci);
                                    _context.Update(RegInci);
                                    _context.SaveChanges();

                                    var Estatus = _context.CatEstatus.Where(x => x.EstId == EstatusNew).First();
                                    var EstatusDescripcion = Estatus.EstDescripcion;
                                    var empleados = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).First();
                                    var puesto = _context.CatPuestos.Where(x => x.PuestoId == empleados.EmpPuestoId).First();
                                    var EmpleadoPuestoSuperior = _context.CatEmpleados.Where(x => x.EmpPuestoId == puesto.PuestoJerarquiaSuperiorPuestoId).First();
                                    var UsuarioJefeSuperior = _userManager.Users.Where(x => x.UserName == EmpleadoPuestoSuperior.EmpUserId).First();
                                    var UsuarioDeLaIncidencia = _userManager.Users.Where(x => x.UserName == empleados.EmpUserId).First();
                                    var Paso = DetalleFlujos.DetFlujoDescripcion;
                                    var destinatario = UsuarioDeLaIncidencia.Email;
                                    var TodasLasIncidencias = _context.CatIncidencias.OrderBy(x => x.InciId).ToList();
                                    var IncidenciaDelPermiso = TodasLasIncidencias.Where(x => x.InciId == RegInci.ReInciInciId).First();
                                    var NombreIncidencia = IncidenciaDelPermiso.InciDescripcion;
                                    var mensaje = "El permiso de tipo " + NombreIncidencia + " con el folio # " + RegInci.ReInciId + " ahora tiene el estatus de: " + EstatusDescripcion + " y las observaciones fueron: " + BitInci.BitInciObservaciones;
                                    var asunto = "Cambio de estatus de tu solicitud de permiso";
                                    var DestinoJefe = UsuarioJefeSuperior.Email; // <---------------- Este es el bueno
                                    var ObservacionesBitacora = BitacoraAnterior.BitInciObservaciones;
                                    var AsuntoJefe = "La solicitud de permiso con folio: " + RegInci.ReInciId + ", ha sido modificada.";
                                    var MensajeJefe = "El permiso con el folio # " + RegInci.ReInciId + " a nombre de " + empleados.EmpNombreCompleto + " ha sido modificado para su nueva revisión. Lo que se modificó fue lo siguiente: " + ObservacionesBitacora;
                                    var urlglobal = "http://192.168.0.5/CatRegistroIncidencias?";
                                    string Bandera = "3424hjlk234"; // Aceptada
                                    var urlaceptada = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioJefeSuperior.Id + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                                    Bandera = "jfnROs34"; // Rechazada
                                    var urlrechazada = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioJefeSuperior.Id + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                                    Bandera = "4RT55cgd6FOR"; // Reenvío
                                    var UrlRegistroPendiente = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioDeLaIncidencia + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                                    DateTime FechaActualMail = DateTime.Now;
                                    var FechaCorta = FechaActualMail.ToShortDateString();
                                    var empleado = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).FirstOrDefault();
                                    
                                    var bodyBuilder = new BodyBuilder();

                                    bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> <p>¡Se ha creado una nueva solicitud de permiso de tipo {1} a nombre de {2}  para su revisión!</p><br />
                <p> En esta solicitud, se regresó para su correspondiente revisión y edición. Y el resultado es el siguiente: {6} </p>
              <p>Para aceptar el permiso, pulse el botón verde, para rechazarlo, pulse el botón rojo...</p> Petición de permiso No. {4}<br />
               <a href=""{3}""><img src=""https://i.imgur.com/Q7D7Kz5.png"" style='width:30%;'></a><a href=""{5}""><img src=""https://i.imgur.com/bHsgWWI.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, NombreIncidencia, empleado.EmpNombreCompleto, urlaceptada, RegInci.ReInciId, urlrechazada, ObservacionesBitacora);

                                    var bodyBuilderRemitente = new BodyBuilder();
                                    bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} tu permiso de tipo  {2} y ha sido enviada a {3} para su revisión!</p>
                <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {5}</p><br /><a href=""{4}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, empleado.EmpNombreCompleto, NombreIncidencia, EmpleadoPuestoSuperior.EmpNombreCompleto, UrlRegistroPendiente, RegInci.ReInciId);

                                    MailMessage correo = new MailMessage(); // ES EL CORREO QUE SE ENVIARÁ AL USUARIO COMO ESTATUS DE LA PROPIA INCIDENCIA
                                    correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                                    correo.To.Add(destinatario);
                                    correo.Subject = asunto;
                                    correo.Body = bodyBuilderRemitente.HtmlBody;
                                    correo.IsBodyHtml = true;
                                    correo.Priority = MailPriority.Normal;

                                    MailMessage CorreoJefe = new MailMessage(); // ES EL CORREO QUE SE MANDARÁ AL JEFE AHORA CON LAS INCIDENCIAS MODIFICADAS
                                    CorreoJefe.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                                    CorreoJefe.To.Add(DestinoJefe); // Cambiar a DestinoJefe
                                    CorreoJefe.Subject = AsuntoJefe;
                                    CorreoJefe.Body = bodyBuilder.HtmlBody;
                                    CorreoJefe.IsBodyHtml = true;
                                    CorreoJefe.Priority = MailPriority.Normal;

                                    smtp.Send(correo);
                                    smtp.Send(CorreoJefe);

                                    break;
                                }
                            }
                            else
                            {
                                if (DetalleFlujos.DetFlujoId == Flujos[i].DetFlujoId) // Validación para flujo normal
                                {
                                    if (Flujos.Count != i)
                                    {
                                        if (OrdenActual != Flujos.Count)
                                        {
                                            OrdenNuevo = OrdenActual + 1;
                                            var NuevoFlujo = Flujos.Where(x => x.DetFlujoOrden == OrdenNuevo).First();
                                            NuevoIdFlujo = NuevoFlujo.DetFlujoId;
                                            if (BitInci.BitInciObservaciones == null)
                                            {
                                                BitInci.BitInciObservaciones = "Aprobado sin problemas.";
                                            }

                                            if (OrdenNuevo == 3)
                                            {

                                                OrdenNuevo = OrdenNuevo + 1;
                                                NuevoFlujo = Flujos.Where(x => x.DetFlujoOrden == OrdenNuevo).First();
                                                NuevoIdFlujo = NuevoFlujo.DetFlujoId;
                                                BitInci.BitInciReInciId = RegInci.ReInciId;
                                                BitInci.BitInciUserId = user.UserName;
                                                BitInci.BitInciFecha = FechaActual;
                                                BitInci.BitInciObservaciones = "Registro cerrado, llegó al final del flujo. Aprobado por capital humano.";
                                                BitInci.BitInciDetFlujoId = NuevoIdFlujo;
                                                RegInci.ReInciEstatusFlujo = BitInci.BitInciDetFlujoId;
                                                RegInci.ReInciEstatusId = 6;

                                                _context.CatBitacoraIncidencias.Add(BitInci);
                                                _context.Update(RegInci);
                                                _context.SaveChanges();

                                                var asunto = "Cambio de estatus de tu solicitud de permiso";
                                                var empleados = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).First();
                                                var UsuarioDeLaIncidencia = _userManager.Users.Where(x => x.UserName == empleados.EmpUserId).First();
                                                DateTime FechaActualMail = DateTime.Now;
                                                var FechaCorta = FechaActualMail.ToShortDateString();
                                                var remitente = UsuarioDeLaIncidencia.Email;
                                                var empleado = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).FirstOrDefault();
                                                var IncidenciaDelPermiso = _context.CatIncidencias.Where(x => x.InciId == RegInci.ReInciInciId).First();
                                                var NombreIncidencia = IncidenciaDelPermiso.InciDescripcion;
                                                var Bandera = "4RT55cgd6FOR"; // Reenvío
                                                var urlglobal = "https://localhost:44392/CatRegistroIncidencias?";
                                                //var urlglobal = "http://192.168.0.5/CatRegistroIncidencias?";
                                                var UrlReenvio = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioDeLaIncidencia + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;

                                                var bodyBuilderRemitente = new BodyBuilder();
                                                bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} tu permiso de tipo  {2} y ha sido enviado ha sido aceptado y cerrado por Capital Humano !</p>
                <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {4}</p><br /><a href=""{3}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, empleado.EmpNombreCompleto, NombreIncidencia, UrlReenvio, RegInci.ReInciId);

                                                MailMessage correo = new MailMessage(); // ES EL CORREO QUE SE ENVIARÁ AL USUARIO COMO ESTATUS DE LA PROPIA INCIDENCIA
                                                correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                                                correo.To.Add(remitente);
                                                correo.Subject = asunto;
                                                correo.Body = bodyBuilderRemitente.HtmlBody;
                                                correo.IsBodyHtml = true;
                                                correo.Priority = MailPriority.Normal;
                                                smtp.Send(correo);

                                                if (RegInci.ReInciInciId == 4)
                                                {
                                                    TimeSpan? RestaFechas;
                                                    int Dias = 0;
                                                    var detincidencias = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == RegInci.ReInciId).First();
                                                    RestaFechas = detincidencias.FechaFinal - detincidencias.FechaInicio;
                                                    Dias = RestaFechas.Value.Days + 1;
                                                    DateTime Fecha = detincidencias.FechaInicio;

                                                    do
                                                    {
                                                      if (Fecha.DayOfWeek == DayOfWeek.Sunday) Dias--;
                                                         Fecha = Fecha.AddDays(1);
                                                    }while (!(Fecha > detincidencias.FechaFinal));
                                                    var Festivos = _context.CatFestivos.OrderBy(x => x.FestId).ToList();
                                                    for (int f = 0; f < Festivos.Count; f++)
                                                    {
                                                    if ((Festivos[f].FestFechaDesde >= detincidencias.FechaInicio) && (Festivos[f].FestFechaHasta <= detincidencias.FechaFinal))
                                                    {
                                                      Dias = Dias - 1;
                                                    }
                                                    }
                                                    var HorarioEmpleado = _context.MovEmpleadosHorarios.Where(x => x.EmpHoraEmpId == RegInci.ReInciEmpId && x.EmpHoraFechaHasta == null).FirstOrDefault();
                                                    var Horario = _context.CatHorarios.Where(x => x.HoraId == HorarioEmpleado.EmpHoraHorId).FirstOrDefault();
                                                    bool BanderaLV = false;
                                                    if (Horario.HoraSabadoEntrada == null)
                                                    {
                                                      BanderaLV = true;
                                                    }
                                                    if (BanderaLV == true)
                                                    {
                                                      do
                                                      {
                                                         if (Fecha.DayOfWeek == DayOfWeek.Saturday) Dias--;
                                                         Fecha = Fecha.AddDays(1);
                                                      }while (!(Fecha > detincidencias.FechaFinal));
                                                    }

                                                    var Empleado = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).FirstOrDefault();
                                                    var Permiso = _context.CatRegistroIncidencias.Where(x => x.ReInciId == RegInci.ReInciId).FirstOrDefault();
                                                    var DetallesPermiso = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == RegInci.ReInciId).FirstOrDefault();
                                                    var AñoEmpleado = Empleado.EmpFechaIngreso.Value.Year;
                                                    var DiasPermiso = Dias;
                                                    var Resultado = FechaActual.Year - AñoEmpleado;
                                                    CatHistorialVacaciones vaca = new CatHistorialVacaciones();
                                                    if (Resultado == 0)
                                                    {
                                                        vaca.HVAntiguedadId = 11;
                                                    }
                                                    else if (Resultado == 1)
                                                    {
                                                        vaca.HVAntiguedadId = 12;
                                                    }
                                                    else if (Resultado == 2)
                                                    {
                                                        vaca.HVAntiguedadId = 13;
                                                    }
                                                    else if (Resultado == 3)
                                                    {
                                                        vaca.HVAntiguedadId = 14;
                                                    }
                                                    else if (Resultado == 4)
                                                    {
                                                        vaca.HVAntiguedadId = 15;
                                                    }
                                                    else if (Resultado == 5)
                                                    {
                                                        vaca.HVAntiguedadId = 16;
                                                    }
                                                    else if (Resultado >= 6 && Resultado <= 10)
                                                    {
                                                        vaca.HVAntiguedadId = 17;
                                                    }
                                                    else if (Resultado >= 11 && Resultado <= 15)
                                                    {
                                                        vaca.HVAntiguedadId = 18;
                                                    }
                                                    else if (Resultado >= 16 && Resultado <= 20)
                                                    {
                                                        vaca.HVAntiguedadId = 19;
                                                    }
                                                    else if (Resultado >= 21 && Resultado <= 25)
                                                    {
                                                        vaca.HVAntiguedadId = 20;
                                                    }
                                                    else if (Resultado >= 26 && Resultado <= 30)
                                                    {
                                                        vaca.HVAntiguedadId = 22;
                                                    }
                                                    else if (Resultado >= 30 && Resultado <= 35)
                                                    {
                                                        vaca.HVAntiguedadId = 23;
                                                    }
                                                    else
                                                    {
                                                        vaca.HVAntiguedadId = 23;
                                                    }

                                                    var Antiguedad = _context.CatAntiguedad.Where(x => x.AntiId == vaca.HVAntiguedadId).First();
                                                    var DiasCorrespondientes = Antiguedad.AntiDias;

                                                    // Asignación de valores a vaca (vacaciones)
                                                    vaca.HVEmpId = Empleado.EmpId;
                                                    vaca.HVDiasSolicitados = DiasPermiso;
                                                    vaca.HVEjercicio = FechaActual.Year;
                                                    vaca.HVFechaSolicitud = Permiso.Fecha;
                                                    vaca.HVFechaInicio = DetallesPermiso.FechaInicio;
                                                    vaca.HVFechaCulminacion = DetallesPermiso.FechaFinal;
                                                    vaca.HVFechaPresentacion = DetallesPermiso.FechaPresentacion;
                                                    vaca.HVReInciId = Permiso.ReInciId;
                                                    vaca.HVPuestoId = Empleado.EmpPuestoId;
                                                    vaca.HVSucursalId = Empleado.EmpSucuId;

                                                    // validamos para registros anteriores
                                                    var DiasDisfrutadosRegistro = _context.CatHistorialVacaciones.Where(x => x.HVEmpId == Empleado.EmpId && x.HVEjercicio == vaca.HVEjercicio).Sum(suma => suma.HVDiasSolicitados);
                                                    var RegistrosHistorialEmpleado = _context.CatHistorialVacaciones.Where(x => x.HVEmpId == Empleado.EmpId && x.HVEjercicio == vaca.HVEjercicio).OrderByDescending(x => x.HVId).ToList();
                                                    var DaysAvaliable = 0;
                                                    var SumaDiasTomados = 0;
                                                    if (RegistrosHistorialEmpleado == null) // Si no tiene registros previos
                                                    {
                                                        DaysAvaliable = DiasCorrespondientes - vaca.HVDiasSolicitados;
                                                        vaca.HVSaldoVacaciones = DaysAvaliable;
                                                    }
                                                    else // Si tiene registros previos
                                                    {
                                                        SumaDiasTomados = vaca.HVDiasSolicitados + DiasDisfrutadosRegistro;
                                                        DaysAvaliable = DiasCorrespondientes - SumaDiasTomados;
                                                        vaca.HVSaldoVacaciones = DaysAvaliable;
                                                    }

                                                    _context.CatHistorialVacaciones.Add(vaca);
                                                    _context.SaveChanges();
                                                }

                                                if (RegInci.ReInciInciId == 5)
                                                {
                                                    var detincidencias = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == RegInci.ReInciId).First();
                                                    MovPermisosPorTiempo PPT = new MovPermisosPorTiempo()
                                                    {
                                                        PPTEmpId = RegInci.ReInciEmpId,
                                                        PPTFechaInicio = detincidencias.DetFecha,
                                                        PPTFechaFinal = detincidencias.FechaFinal,
                                                        PPTHoraEntrada = detincidencias.DetInciHoraSalida,
                                                        PPTHoraSalida = detincidencias.DetInciHoraSalida,
                                                        PPTHoraSalidaComida = detincidencias.DetInciHoraSalidaComida,
                                                        PPTHoraRegresoComida = detincidencias.DetInciHoraRegresoComida,
                                                        PPTEstatusId = 10,
                                                        PPTReInciId = RegInci.ReInciId
                                                    };

                                                    _context.MovPermisosPorTiempo.Add(PPT);
                                                    _context.SaveChanges();

                                                }

                                            }
                                            else
                                            {
                                                BitInci.BitInciReInciId = RegInci.ReInciId;
                                                BitInci.BitInciUserId = user.UserName;
                                                BitInci.BitInciFecha = FechaActual;
                                                BitInci.BitInciObservaciones = BitInci.BitInciObservaciones;
                                                BitInci.BitInciDetFlujoId = NuevoIdFlujo;
                                                RegInci.ReInciEstatusFlujo = BitInci.BitInciDetFlujoId;

                                                _context.CatBitacoraIncidencias.Add(BitInci);
                                                _context.Update(RegInci);
                                                _context.SaveChanges();
                                            }
                                            var ValidacionCorreoDestino = NuevoFlujo.DetFlujoCorreoDestino;

                                            if(ValidacionCorreoDestino != null)
                                            {
                                                if(ValidacionCorreoDestino == "CapitalHumano")
                                                {
                                                    var TodosLosEmpleados = _context.CatEmpleados.OrderBy(x => x.EmpNombreCompleto).ToList();
                                                    var empleados = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).First();
                                                    var UsuarioDeLaIncidencia = _userManager.Users.Where(x => x.UserName == empleados.EmpUserId).First();
                                                    var BitacoraPreviamenteAprobada = _context.CatBitacoraIncidencias.Where(x=> x.BitInciReInciId == RegInci.ReInciId).ToList();
                                                    var UltimaBitacora = BitacoraPreviamenteAprobada.Last();
                                                    var UsuarioAprobo = UltimaBitacora.BitInciUserId;
                                                    var JefeQueAprobo = TodosLosEmpleados.Where(x => x.EmpUserId == UsuarioAprobo).First();
                                                    var NombreJefe = JefeQueAprobo.EmpNombreCompleto;
                                                    var remitente = UsuarioDeLaIncidencia.Email;
                                                    var TodasLasIncidencias = _context.CatIncidencias.OrderBy(x => x.InciId).ToList();
                                                    var IncidenciaDelPermiso = TodasLasIncidencias.Where(x => x.InciId == RegInci.ReInciInciId).First();
                                                    var NombreIncidencia = IncidenciaDelPermiso.InciDescripcion;
                                                    var mensaje = "El permiso de tipo " + NombreIncidencia + " con el folio # " + RegInci.ReInciId + " fue aprobado y ahora se encuentra en Capital Humano para su revisión y validación.";
                                                    var asunto = "Cambio de estatus de tu solicitud de permiso";
                                                    var CorreoCHDestino = NuevoFlujo.DetFlujoEmail;
                                                    var AsuntoJefe = "La solicitud de permiso con folio: " + RegInci.ReInciId + ", ha sido modificada.";
                                                    var MensajeJefe = "El permiso de tipo " + NombreIncidencia + " con el folio # " + RegInci.ReInciId + " a nombre de " + empleados.EmpNombreCompleto + " ha sido aprobado por " + NombreJefe + " para su revisión y validación en el departamento de Capital Humano";
                                                    var urlglobal = "http://192.168.0.5/CatRegistroIncidencias?";
                                                    DateTime FechaActualMail = DateTime.Now;
                                                    string Bandera = "3424hjlk234"; // Aceptada
                                                    var urlaceptada = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioAprobo + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                                                    Bandera = "jfnROs34"; // Rechazada
                                                    var urlrechazada = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioAprobo + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                                                    Bandera = "4RT55cgd6FOR"; // Reenvío
                                                    var UrlReenvio = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioDeLaIncidencia + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                                                    var FechaCorta = FechaActualMail.ToShortDateString();
                                                    var empleado = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).FirstOrDefault();
                                                    var EmpleadoRegresaIncidencia = _context.CatEmpleados.Where(x => x.EmpUserId == user.UserName).FirstOrDefault();
                                                    
                                                    var bodyBuilderCH = new BodyBuilder();

                                                    bodyBuilderCH.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> <p>¡Se ha creado una nueva solicitud de permiso de tipo {1} a nombre de {2}  para su revisión!</p><br />
                <p> La misma solicitud de permiso ya fue revisada y aprobada previamente por: {6}. </p>
              <p>Para aceptar el permiso, pulse el botón verde, para rechazarlo, pulse el botón rojo...</p> Petición de permiso No. {4}<br />
               <a href=""{3}""><img src=""https://i.imgur.com/Q7D7Kz5.png"" style='width:30%;'></a><a href=""{5}""><img src=""https://i.imgur.com/bHsgWWI.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, NombreIncidencia, empleado.EmpNombreCompleto, urlaceptada, RegInci.ReInciId, urlrechazada, NombreJefe);

                                                    var bodyBuilderRemitente = new BodyBuilder();
                                                    bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} tu permiso de tipo  {2} y ha sido enviado a Capital Humano para su revisión!</p>
                <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {4}</p><br /><a href=""{3}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, empleado.EmpNombreCompleto, NombreIncidencia, UrlReenvio, RegInci.ReInciId);

                                                    MailMessage correo = new MailMessage(); // ES EL CORREO QUE SE ENVIARÁ AL USUARIO COMO ESTATUS DE LA PROPIA INCIDENCIA
                                                    correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                                                    correo.To.Add(remitente);
                                                    correo.Subject = asunto;
                                                    correo.Body = bodyBuilderRemitente.HtmlBody;
                                                    correo.IsBodyHtml = true;
                                                    correo.Priority = MailPriority.Normal;
                                                    smtp.Send(correo);

                                                    MailMessage CorreoCH = new MailMessage(); // ES EL CORREO QUE SE MANDARÁ AL JEFE AHORA CON LAS INCIDENCIAS MODIFICADAS
                                                    CorreoCH.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                                                    CorreoCH.To.Add(CorreoCHDestino);
                                                    CorreoCH.Subject = AsuntoJefe;
                                                    CorreoCH.Body = bodyBuilderCH.HtmlBody;
                                                    CorreoCH.IsBodyHtml = true;
                                                    CorreoCH.Priority = MailPriority.Normal;
                                                    
                                                    smtp.Send(CorreoCH);
                                                }
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            var BitAnterior = _context.CatBitacoraIncidencias.Where(x => x.BitInciReInciId == BitInci.BitInciReInciId).ToList();
                                            var BitacoraAnterior = BitAnterior.Last();
                                            var RegistroReiniciado = BitacoraAnterior.BitInciDetFlujoId;
                                            NuevoIdFlujo = RegistroReiniciado;

                                            BitInci.BitInciReInciId = RegInci.ReInciId;
                                            BitInci.BitInciUserId = user.UserName;
                                            BitInci.BitInciFecha = FechaActual;
                                            BitInci.BitInciObservaciones = "Registro cerrado, llegó al paso final";
                                            BitInci.BitInciDetFlujoId = NuevoIdFlujo;

                                            RegInci.ReInciEstatusFlujo = BitInci.BitInciDetFlujoId;
                                            RegInci.ReInciEstatusId = 6;

                                            _context.CatBitacoraIncidencias.Add(BitInci);
                                            _context.Update(RegInci);
                                            _context.SaveChanges();
                                            break;
                                        }

                                    }
                                }
                            }

                        }
                    }
                        var x = _context.CatBitacoraIncidencias.Where(x => x.BitInciId == BitInci.BitInciId).First();
                        var obj = new
                        {
                            x.BitInciFecha,
                            x.BitInciReInciId,
                            x.BitInciObservaciones,
                            x.BitInciUserId,
                            x.BitInciDetFlujoId
                        };
                        return Ok(BitInci);
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
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("ValidacionIncidenciaById/{Id}/{IdUsuario}/{Bandera}")]
        public IActionResult ValidacionIncidenciaById(int Id, string IdUsuario, string Bandera)
        {
            try
            {
                var PasoIncidencia = 0;
                bool redflag = false;
                var ReInci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Id).FirstOrDefault();
                if (ReInci == null)
                {
                    return BadRequest();
                }
                else
                {
                    var BitInci = _context.CatBitacoraIncidencias.Where(x => x.BitInciReInciId == Id).ToList();
                    if(BitInci.Count == 0)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        var UsuarioBitacora = BitInci.LastOrDefault();
                        if (UsuarioBitacora == null)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            var FlujoDeLaBitacora = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == UsuarioBitacora.BitInciDetFlujoId).FirstOrDefault();
                            var Flujo = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == ReInci.ReInciEstatusFlujo).FirstOrDefault();
                            if(FlujoDeLaBitacora.DetFlujoId != Flujo.DetFlujoId)
                            {
                                return BadRequest();
                            }
                            if (Flujo == null)
                            {
                                return BadRequest();
                            }
                            else
                            {
                                if(Flujo.DetFlujoCorreoDestino == "Jefe")
                                {
                                    var EmpleadoDeLaIncidencia = _context.CatEmpleados.Where(x => x.EmpId == ReInci.ReInciEmpId).FirstOrDefault();
                                    var PuestoEmpleado = _context.CatPuestos.Where(x => x.PuestoId == EmpleadoDeLaIncidencia.EmpPuestoId).FirstOrDefault();
                                    var JefeEmpleado = _context.CatEmpleados.Where(x => x.EmpPuestoId == PuestoEmpleado.PuestoJerarquiaSuperiorPuestoId).FirstOrDefault();
                                    var UserJefe = _userManager.Users.Where(x => x.UserName == JefeEmpleado.EmpUserId).FirstOrDefault();
                                    if(UserJefe.Id == IdUsuario)
                                    {
                                        PasoIncidencia = Flujo.DetFlujoOrden;
                                        var Bitacora = _context.CatBitacoraIncidencias.Where(x => x.BitInciReInciId == ReInci.ReInciId && x.BitInciDetFlujoId == Flujo.DetFlujoId).FirstOrDefault();

                                        if (Bandera == "3424hjlk234")
                                        {
                                            redflag = true;
                                        }
                                        else if (Bandera == "4RT55cgd6FOR")
                                        {
                                            redflag = true;
                                        }
                                        else if (Bandera == "jfnROs34")
                                        {
                                            redflag = true;
                                        }
                                        else
                                        {
                                            redflag = false;
                                        }
                                    }
                                }else if(Flujo.DetFlujoCorreoDestino == "CapitalHumano")
                                {
                                    var UsuarioAproboIncidencia = BitInci.LastOrDefault();
                                    var EmpleadoDeLaIncidencia = _context.CatEmpleados.Where(x => x.EmpId == ReInci.ReInciEmpId).FirstOrDefault();
                                    if(UsuarioAproboIncidencia.BitInciUserId == IdUsuario)
                                    {
                                        if (Bandera == "3424hjlk234")
                                        {
                                            redflag = true;
                                        }
                                        else if (Bandera == "4RT55cgd6FOR")
                                        {
                                            redflag = true;
                                        }
                                        else if (Bandera == "jfnROs34")
                                        {
                                            redflag = true;
                                        }
                                        else
                                        {
                                            redflag = false;
                                        }
                                    }else if(EmpleadoDeLaIncidencia.EmpUserId == UsuarioAproboIncidencia.BitInciUserId)
                                    {
                                        if (Bandera == "3424hjlk234")
                                        {
                                            redflag = true;
                                        }
                                        else if (Bandera == "4RT55cgd6FOR")
                                        {
                                            redflag = true;
                                        }
                                        else if (Bandera == "jfnROs34")
                                        {
                                            redflag = true;
                                        }
                                        else
                                        {
                                            redflag = false;
                                        }
                                    }
                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                        }
                    }
                }

                var obj = new
                {
                    redflag
                };
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("ValidacionPorPerfiles")]
        public async Task<IActionResult> ValidacionPorPerfiles()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var UsuarioId = user.Id;
            var RegistroDeIncidencias = _context.CatRegistroIncidencias.OrderBy(x => x.ReInciId).ToList();
            var Empleados = _context.CatEmpleados.ToList();
            var IdEmpleado = 0;
            for (int i = 0; i < Empleados.Count; i++)
            {
                if (user.UserName == Empleados[i].EmpUserId)
                {
                    IdEmpleado = Empleados[i].EmpId;
                    break;
                }
            }

            var TablaTodosLosRegistros = _context.CatRegistroIncidencias.ToList();
            var TablaPropiosRegistros = _context.CatRegistroIncidencias.Where(x => x.ReInciEmpId == IdEmpleado);
            var TablaRegistrosAbiertos = _context.CatRegistroIncidencias.Where(x => x.ReInciEstatusId == 3 || x.ReInciEstatusId == 9);
            var TablaRegostrosPorComision = _context.CatRegistroIncidencias.Where(x => x.ReInciInciId == 11 && x.ReInciEmpId != IdEmpleado);
            bool TodosLosRegistros = false; // Administrador
            bool PropiosRegistros = false; // Empleado
            bool RegistrosAbiertos = false; // Gerente, Direccion o Director General, etc.
            bool RegistrosVigilancia = false; // Vigilancia
            bool RegistrosCapitalHumano = false; // Capital Humano
            bool RegistrosHechosCapitalHumano = false; 
            var Perfiles = _context.UserRoles.Where(x => x.UserId == UsuarioId).ToList();
            foreach (var perfile in Perfiles)
            {
                if(perfile.RoleId == "4")
                {
                    PropiosRegistros = true;
                }else if(perfile.RoleId == "5"){
                    TodosLosRegistros = true;
                }else if(perfile.RoleId == "3" || perfile.RoleId == "2" || perfile.RoleId == "1")
                {
                    RegistrosAbiertos = true;
                }else if(perfile.RoleId == "7")
                {
                    RegistrosVigilancia = true;
                }else if(perfile.RoleId == "5")
                {
                    RegistrosAbiertos = true;
                    TodosLosRegistros = true;
                    PropiosRegistros = true;
                    RegistrosVigilancia = false;
                }else if (perfile.RoleId == "8")
                {
                    RegistrosCapitalHumano = true;
                    RegistrosHechosCapitalHumano = true;
                }
                else
                {
                    RegistrosAbiertos = false;
                    TodosLosRegistros = false;
                    PropiosRegistros = false;
                    RegistrosVigilancia = false;
                    RegistrosCapitalHumano = false;
                }

            }
            var obj = new
            {
                RegistrosAbiertos,
                TodosLosRegistros,
                PropiosRegistros,
                RegistrosVigilancia,
                RegistrosCapitalHumano,
                RegistrosHechosCapitalHumano
            };
            return Ok(obj);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("DetallesIncidenciaById/{Id}")]
        public IActionResult DetallesIncidenciaById(int Id)
        {
            try
            {
                var DetInci = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == Id).First();
                var ReInci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Id).First();
                var Incidencia = ReInci.ReInciInciId;
                bool ValidacionVacaciones = false;
                bool ValidacionPersonal = false;
                bool ValidacionComision = false;
                bool ValidacionMuestraHorarios = false;
                var FechaCortita = ReInci.Fecha.ToShortDateString();
                var EstadoListado = _context.CatEstatus.Where(x => x.EstId == ReInci.ReInciEstatusId).First();
                string EstatusReadOnly = EstadoListado.EstDescripcion.ToString();
                var ListadoIncidencias = _context.CatIncidencias.Where(x => x.InciId == ReInci.ReInciInciId).First();
                string IncidenciasReadOnly = ListadoIncidencias.InciDescripcion.ToString();
                var FullName = _context.CatEmpleados.Where(x => x.EmpId == ReInci.ReInciEmpId).First();
                var Nombre = FullName.EmpNombre.ToString();
                var Paterno = FullName.EmpPaterno.ToString();
                var Materno = FullName.EmpMaterno.ToString();
                string NombreCompletoReadOnly = Nombre + " " + Paterno + " " + Materno;
                var Horario = _context.CatHorarios.ToList();
                List<HorariosFiltrados> horariosedicion = new List<HorariosFiltrados>();
                foreach (var item in Horario)
                {
                    horariosedicion.Add(new HorariosFiltrados()
                    {
                        HoraID = item.HoraId,
                        Hora = item.HoraEntrada.ToString(),
                        HoraSali = item.HoraSalida.ToString(),
                        HoraIngresoSabado = item.HoraSabadoEntrada.ToString(),
                        HoraSalidaSabado = item.HoraSabadoSalida.ToString()
                    });
                }
                var FechaDetalle = DetInci.DetFecha;
                var FechaAnio = FechaDetalle.Year;
                var FechaMes = FechaDetalle.Month;
                var FechaDia = FechaDetalle.Day;
                var FechaMesS = "";
                var FechaDiaS = "";
                if (FechaMes < 10)
                {
                    FechaMesS = "0" + FechaMes;
                }
                if (FechaDia < 10)
                {
                    FechaDiaS = "0" + FechaDia;
                }
                var FechaDetalleCorta = FechaDiaS + "/" + FechaMesS + "/" + FechaAnio;
                var FechaInicioVaca = DetInci.FechaInicio;
                var FechaAnioIV = FechaInicioVaca.Year;
                var FechaMesIV = FechaInicioVaca.Month;
                var FechaDiaIV = FechaInicioVaca.Day;
                var FechaInicioVacaCorta = FechaDiaIV + "/" + FechaMesIV + "/" + FechaAnioIV;
                var FechaFinVaca = DetInci.FechaFinal;
                var FechaAnioFV = FechaFinVaca.Year;
                var FechaMesFV = FechaFinVaca.Month;
                var FechaDiaFV = FechaFinVaca.Day;
                var FechaFinVacaCorta = FechaDiaFV + "/" + FechaMesFV + "/" + FechaAnioFV;
                var FechaPVaca = DetInci.FechaPresentacion;
                var FechaAnioPV = FechaPVaca.Year;
                var FechaMesPV = FechaPVaca.Month;
                var FechaDiaPV = FechaPVaca.Day;
                var FechaPVacaCorta = FechaDiaPV + "/" + FechaMesPV + "/" + FechaAnioPV;
                if (Incidencia == 4)
                {
                    ValidacionVacaciones = true;
                }
                else if (Incidencia == 5 || Incidencia == 6 || Incidencia == 7 || Incidencia == 8)
                {
                    ValidacionPersonal = true;
                    if(Incidencia == 5)
                    {
                        ValidacionMuestraHorarios = true;
                    }
                }
                else if (Incidencia == 11)
                {
                    ValidacionComision = true;
                }
                else
                {
                    ValidacionVacaciones = false;
                    ValidacionPersonal = false;
                    ValidacionComision = false;
                }
                var EstadoIncidencia = ReInci.ReInciEstatusId;
                bool BanderaEstadoIncidencia = false;
                if (EstadoIncidencia == 6 || EstadoIncidencia == 5 || EstadoIncidencia == 4)
                {
                    BanderaEstadoIncidencia = false;
                }
                else
                {
                    BanderaEstadoIncidencia = true;
                }
                string TipoIncidencia = "";

                if(ReInci.ReInciInciId == 4)
                {
                    TipoIncidencia = "Vacaciones";
                }else if(ReInci.ReInciInciId == 11)
                {
                    TipoIncidencia = "Comisión";
                }else if (ReInci.ReInciInciId == 5 || ReInci.ReInciInciId == 6 || ReInci.ReInciInciId == 7 || ReInci.ReInciInciId == 8)
                {
                    TipoIncidencia = "Personal";
                }
                else
                {
                }

                var IdPersonaCubrira = 2075;

                if(DetInci.PersonaCubrira != null)
                {
                    IdPersonaCubrira = int.Parse(DetInci.PersonaCubrira);
                }
                
                var EmpleadoCubriraDetalles = _context.CatEmpleados.Where(x => x.EmpId == IdPersonaCubrira).FirstOrDefault();

                var fefinper = "";
                var HoraIngreso = "";
                var HoraSalidaPermisoPersonal = "";
                var HoraSComida = "";
                var HoraRComida = "";

                if (DetInci.DetInciFechaFinPermisoPersonal != null)
                {
                    fefinper = DetInci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString();
                }

                if (DetInci.DetInciHoraSalida != null)
                {
                    HoraSalidaPermisoPersonal = DetInci.DetInciHoraSalida.ToString();
                }

                if (DetInci.DetInciHoraIngreso != null)
                {
                    HoraIngreso = DetInci.DetInciHoraIngreso.ToString();
                }

                if (DetInci.DetInciHoraSalidaComida != null)
                {
                    HoraSComida = DetInci.DetInciHoraSalidaComida.ToString();
                }

                if (DetInci.DetInciHoraRegresoComida != null)
                {
                    HoraRComida = DetInci.DetInciHoraRegresoComida.ToString();
                }

                var objeto = new
                {
                    ReInci.ReInciId,
                    ReInci.ReInciEmpId,
                    FechaCortita,
                    ReInci.ReInciInciId,
                    ReInci.ReInciEstatusId,
                    DetInci.DetInciId,
                    DetInci.DetInciReInciId,
                    DetInci.DetInciHorarioId,
                    DetInci.MedidaAccion,
                    DetInci.Asunto,
                    DetInci.Destino,
                    DetInci.TelDestino,
                    DetInci.Contacto1,
                    DetInci.Contacto2,
                    DetInci.NombreDestino,
                    DetInci.Observaciones,
                    DetInci.HoraSalida,
                    DetInci.HoraRegreso,
                    DetInci.FechaInicio,
                    DetInci.FechaFinal,
                    DetInci.FechaPresentacion,
                    DetInci.DiasAusencia,
                    DetInci.PersonaCubrira,
                    DetInci.DetFecha,
                    DetInci.Motivo,
                    ValidacionVacaciones,
                    ValidacionPersonal,
                    ValidacionComision,
                    EstatusReadOnly,
                    IncidenciasReadOnly,
                    NombreCompletoReadOnly,
                    horariosedicion,
                    ValidacionMuestraHorarios,
                    FechaDetalleCorta,
                    FechaInicioVacaCorta,
                    FechaFinVacaCorta,
                    FechaPVacaCorta,
                    BanderaEstadoIncidencia,
                    TipoIncidencia,
                    EmpleadoCubriraDetalles.EmpId,
                    fefinper,
                    HoraIngreso,
                    //HoraSalida,
                    HoraSalidaPermisoPersonal,
                    HoraSComida,
                    HoraRComida
                };
                return Ok(objeto);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("EditDetailsRegistroIncidencias")]
        public async Task<IActionResult> EditDetailsRegistroIncidencias(CatBitacoraIncidencias Bitacora, CatDetalleIncidencias DetailsInci)
        {
            try
            {
                if (Bitacora != null)
                {
                    // ------------ Configuración del SMTP --------------

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    // ------------------------------------------

                    var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));

                    DateTime FechaActual = DateTime.Now;
                    var RegInci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Bitacora.BitInciReInciId).First();
                    var ListaDetalleFlujos = _context.CatDetalleFlujo.OrderBy(x => x.DetFlujoId).ToList();
                    var DetalleFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == RegInci.ReInciEstatusFlujo).First();
                    var Flujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == DetalleFlujos.DetFlujoFlujoId).ToList();
                    var DetInci = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == Bitacora.BitInciReInciId).First();
                    var DiferenciaHorario = "";
                    var DiferenciaMedidaAccion = "";
                    var DiferenciaAsunto = "";
                    var DiferenciaDestino = "";
                    var DiferenciaTelDestino = "";
                    var DiferenciaContacto1 = "";
                    var DiferenciaNombreDestino = "";
                    var DiferenciaContacto2 = "";
                    var DiferenciaObservaciones = "";
                    var DiferenciaHoraSalida = "";
                    var DiferenciaHoraRegreso = "";
                    var DiferenciaFechaInicio = "";
                    var DiferenciaFechaFinal = "";
                    var DiferenciaFechaPresentacion = "";
                    var DiferenciaDiasAusencia = "";
                    var DiferenciaPersonaCubrira = "";
                    var DiferenciaMotivo = "";
                    var DiferenciaDetFecha = "";
                    if(DetInci.DetInciHorarioId != DetailsInci.DetInciHorarioId)
                    {
                        DiferenciaHorario = "Hubo una modificación en el horario.";
                    }
                    else
                    {
                        DiferenciaHorario = "No hubo una modificación en el horario.";
                    }
                    if (DetInci.MedidaAccion != DetailsInci.MedidaAccion)
                    {
                        DiferenciaMedidaAccion = "Hubo una modificación en la medida de acción. Antes era: " + DetInci.MedidaAccion + " y el nuevo valor es: " + DetailsInci.MedidaAccion;
                    }
                    else
                    {
                        DiferenciaMedidaAccion = "No hubo una modificación en la medida de acción.";
                    }
                    if (DetInci.Asunto != DetailsInci.Asunto)
                    {
                        DiferenciaAsunto = "Hubo una modificación en el asunto. Antes era: " + DetInci.Asunto + " y el nuevo valor es: " + DetailsInci.Asunto;
                    }
                    else
                    {
                        DiferenciaAsunto = "No hubo una modificación en el asunto.";
                    }
                    if (DetInci.Destino != DetailsInci.Destino)
                    {
                        DiferenciaDestino = "Hubo una modificación en el destino. Antes era: " + DetInci.Destino + " y el nuevo valor es: " + DetailsInci.Destino;
                    }
                    else
                    {
                        DiferenciaDestino = "No hubo una modificación en el destino.";
                    }
                    if (DetInci.TelDestino != DetailsInci.TelDestino)
                    {
                        DiferenciaTelDestino = "Hubo una modificación en el teléfono destino. Antes era: " + DetInci.TelDestino + " y el nuevo valor es: " + DetailsInci.TelDestino;
                    }
                    else
                    {
                        DiferenciaTelDestino = "No hubo una modificación en el teléfono destino.";
                    }
                    if (DetInci.Contacto1 != DetailsInci.Contacto1)
                    {
                        DiferenciaContacto1 = "Hubo una modificación en el primer contacto. Antes era: " + DetInci.Contacto1 + " y el nuevo valor es: " + DetailsInci.Contacto1;
                    }
                    else
                    {
                        DiferenciaContacto1 = "No hubo una modificación en el primer contacto.";
                    }
                    if (DetInci.NombreDestino != DetailsInci.NombreDestino)
                    {
                        DiferenciaNombreDestino = "Hubo una modificación en el nombre destino. Antes era: " + DetInci.NombreDestino + " y el nuevo valor es: " + DetailsInci.NombreDestino;
                    }
                    else
                    {
                        DiferenciaNombreDestino = "No hubo una modificación en el nombre destino.";
                    }
                    if (DetInci.Contacto2 != DetailsInci.Contacto2)
                    {
                        DiferenciaNombreDestino = "Hubo una modificación en el segundo contacto. Antes era: " + DetInci.Contacto2 + " y el nuevo valor es: " + DetailsInci.Contacto2;
                    }
                    else
                    {
                        DiferenciaNombreDestino = "No hubo una modificación en el segundo contacto.";
                    }
                    if (DetInci.Observaciones != DetailsInci.Observaciones)
                    {
                        DiferenciaObservaciones = "Hubo una modificación en las observaciones. Antes era: " + DetInci.Observaciones + " y el nuevo valor es: " + DetailsInci.Observaciones;
                    }
                    else
                    {
                        DiferenciaObservaciones = "No hubo una modificación en las observaciones.";
                    }
                    if (DetInci.HoraSalida != DetailsInci.HoraSalida)
                    {
                        DiferenciaHoraSalida = "Hubo una modificación en la hora de salida. Antes era: " + DetInci.HoraSalida + " y el nuevo valor es: " + DetailsInci.HoraSalida;
                    }
                    else
                    {
                        DiferenciaHoraSalida = "No hubo una modificación en la hora de salida.";
                    }
                    if (DetInci.HoraRegreso != DetailsInci.HoraRegreso)
                    {
                        DiferenciaHoraRegreso = "Hubo una modificación en la hora de regreso. Antes era: " + DetInci.HoraRegreso + " y el nuevo valor es: " + DetailsInci.HoraRegreso;
                    }
                    else
                    {
                        DiferenciaHoraRegreso = "No hubo una modificación en la hora de regreso.";
                    }
                    if (DetInci.FechaInicio != DetailsInci.FechaInicio)
                    {
                        DiferenciaFechaInicio = "Hubo una modificación en la fecha de inicio. Antes era: " + DetInci.FechaInicio + " y el nuevo valor es: " + DetailsInci.FechaInicio;
                    }
                    else
                    {
                        DiferenciaFechaInicio = "No hubo una modificación en la fecha de inicio.";
                    }
                    if (DetInci.FechaFinal != DetailsInci.FechaFinal)
                    {
                        DiferenciaFechaFinal = "Hubo una modificación en la fecha de fin. Antes era: " + DetInci.FechaFinal + " y el nuevo valor es: " + DetailsInci.FechaFinal;
                    }
                    else
                    {
                        DiferenciaFechaFinal = "No hubo una modificación en la fecha de fin.";
                    }
                    if (DetInci.FechaPresentacion != DetailsInci.FechaPresentacion)
                    {
                        DiferenciaFechaPresentacion = "Hubo una modificación en la fecha de presentación. Antes era: " + DetInci.FechaPresentacion + " y el nuevo valor es: " + DetailsInci.FechaPresentacion;
                    }
                    else
                    {
                        DiferenciaFechaPresentacion = "No hubo una modificación en la fecha de presentación.";
                    }
                    if (DetInci.DiasAusencia != DetailsInci.DiasAusencia)
                    {
                        DiferenciaDiasAusencia = "Hubo una modificación en los días de ausencia. Antes era: " + DetInci.DiasAusencia + " y el nuevo valor es: " + DetailsInci.DiasAusencia;
                    }
                    else
                    {
                        DiferenciaDiasAusencia = "No hubo una modificación en los días de ausencia.";
                    }
                    if (DetInci.PersonaCubrira != DetailsInci.PersonaCubrira)
                    {
                        DiferenciaPersonaCubrira = "Hubo una modificación en la persona que cubrirá.";
                    }
                    else
                    {
                        DiferenciaPersonaCubrira = "No hubo una modificación en la persona que cubrirá.";
                    }
                    if (DetInci.Motivo != DetailsInci.Motivo)
                    {
                        DiferenciaMotivo = "Hubo una modificación en el motivo. Antes era: " + DetInci.Motivo + " y el nuevo valor es: " + DetailsInci.Motivo;
                    }
                    else
                    {
                        DiferenciaMotivo = "No hubo una modificación en el motivo.";
                    }
                    if (DetInci.DetFecha != DetailsInci.DetFecha)
                    {
                        DiferenciaMotivo = "Hubo una modificación en la fecha de detalle. Antes era: " + DetInci.DetFecha + " y el nuevo valor es: " + DetailsInci.DetFecha;
                    }
                    else
                    {
                        DiferenciaMotivo = "No hubo una modificación en la fecha de detalle.";
                    }
                    var DiferenciasTotales = DiferenciaHorario + " " + DiferenciaMedidaAccion + " " + DiferenciaAsunto + " " + DiferenciaDestino + " " + DiferenciaTelDestino + " " + DiferenciaContacto1 + " " + DiferenciaNombreDestino + " " + DiferenciaContacto2 + " " + DiferenciaObservaciones + " " + DiferenciaHoraSalida + " " + DiferenciaHoraRegreso + " " + DiferenciaFechaInicio + " " + DiferenciaFechaFinal + " " + DiferenciaFechaPresentacion + " " + DiferenciaDiasAusencia + " " + DiferenciaPersonaCubrira + " " + DiferenciaMotivo + " " + DiferenciaDetFecha;
                    if (RegInci.ReInciEstatusId == 9)
                    {
                        Bitacora.BitInciReInciId = RegInci.ReInciId;
                        Bitacora.BitInciUserId = user.UserName;
                        Bitacora.BitInciFecha = FechaActual;
                        Bitacora.BitInciDetFlujoId = RegInci.ReInciEstatusFlujo;
                        Bitacora.BitInciObservaciones = DiferenciasTotales;
                        DetInci.DetInciReInciId = RegInci.ReInciId;
                        DetInci.DetInciHorarioId = DetailsInci.DetInciHorarioId;
                        DetInci.MedidaAccion = DetailsInci.MedidaAccion;
                        DetInci.Asunto = DetailsInci.Asunto;
                        DetInci.Destino = DetailsInci.Destino;
                        DetInci.TelDestino = DetailsInci.TelDestino;
                        DetInci.Contacto1 = DetailsInci.Contacto1;
                        DetInci.NombreDestino = DetailsInci.NombreDestino;
                        DetInci.Contacto2 = DetailsInci.Contacto2;
                        DetInci.Observaciones = DetailsInci.Observaciones;
                        DetInci.HoraSalida = DetailsInci.HoraSalida;
                        DetInci.HoraRegreso = DetailsInci.HoraRegreso;
                        DetInci.FechaInicio = DetailsInci.FechaInicio;
                        DetInci.FechaFinal = DetailsInci.FechaFinal;
                        DetInci.FechaPresentacion = DetailsInci.FechaPresentacion;
                        DetInci.DiasAusencia = DetailsInci.DiasAusencia;
                        DetInci.PersonaCubrira = DetailsInci.PersonaCubrira;
                        DetInci.Motivo = DetailsInci.Motivo;
                        DetInci.DetFecha = DetailsInci.DetFecha;
                        RegInci.ReInciEstatusId = 3;

                        _context.CatBitacoraIncidencias.Add(Bitacora);
                        _context.Update(DetInci);
                        _context.Update(RegInci);
                        _context.SaveChanges();

                        var CorroborarFlujoPorCorreo = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == RegInci.ReInciEstatusFlujo).FirstOrDefault();
                        if(CorroborarFlujoPorCorreo.DetFlujoCorreoDestino == "CapitalHumano")
                        {
                            var TodosLosEmpleados = _context.CatEmpleados.OrderBy(x => x.EmpNombreCompleto).ToList();
                            var empleados = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).First();
                            var UsuarioDeLaIncidencia = _userManager.Users.Where(x => x.UserName == empleados.EmpUserId).First();
                            var BitacoraPreviamenteAprobada = _context.CatBitacoraIncidencias.Where(x => x.BitInciReInciId == RegInci.ReInciId).ToList();
                            var UltimaBitacora = BitacoraPreviamenteAprobada.Last();
                            var UsuarioAprobo = UltimaBitacora.BitInciUserId;
                            var JefeQueAprobo = TodosLosEmpleados.Where(x => x.EmpUserId == UsuarioAprobo).First();
                            var NombreJefe = JefeQueAprobo.EmpNombreCompleto;
                            var remitente = UsuarioDeLaIncidencia.Email;
                            var TodasLasIncidencias = _context.CatIncidencias.OrderBy(x => x.InciId).ToList();
                            var IncidenciaDelPermiso = TodasLasIncidencias.Where(x => x.InciId == RegInci.ReInciInciId).First();
                            var NombreIncidencia = IncidenciaDelPermiso.InciDescripcion;
                            var mensaje = "El permiso de tipo " + NombreIncidencia + " con el folio # " + RegInci.ReInciId + " fue aprobado y ahora se encuentra en Capital Humano para su revisión y validación.";
                            var asunto = "Cambio de estatus de tu solicitud de permiso";
                            var CorreoCHDestino = CorroborarFlujoPorCorreo.DetFlujoEmail;
                            var AsuntoJefe = "La solicitud de permiso con folio: " + RegInci.ReInciId + ", ha sido modificada.";
                            var MensajeJefe = "El permiso de tipo " + NombreIncidencia + " con el folio # " + RegInci.ReInciId + " a nombre de " + empleados.EmpNombreCompleto + " ha sido aprobado por " + NombreJefe + " para su revisión y validación en el departamento de Capital Humano";
                            var urlglobal = "https://localhost:44392/CatRegistroIncidencias?";
                            //var urlglobal = "http://192.168.0.5/CatRegistroIncidencias?";
                            var url = "https://localhost:44392/CatRegistroIncidencias?";
                            //var url = "http://192.168.0.5/CatRegistroIncidencias?";
                            DateTime FechaActualMail = DateTime.Now;
                            string Bandera = "3424hjlk234"; // Aceptada
                            var urlaceptada = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioAprobo + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                            Bandera = "jfnROs34"; // Rechazada
                            var urlrechazada = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioAprobo + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                            Bandera = "4RT55cgd6FOR"; // Reenvío
                            var UrlReenvio = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioDeLaIncidencia + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                            var FechaCorta = FechaActualMail.ToShortDateString();
                            var empleado = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).FirstOrDefault();
                            var EmpleadoRegresaIncidencia = _context.CatEmpleados.Where(x => x.EmpUserId == user.UserName).FirstOrDefault();

                            var bodyBuilderCH = new BodyBuilder();
                            bodyBuilderCH.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> <p>¡Se ha creado una nueva solicitud de permiso de tipo {1} a nombre de {2}  para su revisión!</p><br />
                <p> La misma solicitud de permiso ya fue revisada y aprobada previamente por: {6}. </p>
              <p>Para aceptar el permiso, pulse el botón verde, para rechazarlo, pulse el botón rojo...</p> Petición de permiso No. {4}<br />
               <a href=""{3}""><img src=""https://i.imgur.com/Q7D7Kz5.png"" style='width:30%;'></a><a href=""{5}""><img src=""https://i.imgur.com/bHsgWWI.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, NombreIncidencia, empleado.EmpNombreCompleto, urlaceptada, RegInci.ReInciId, urlrechazada, NombreJefe);

                            var bodyBuilderRemitente = new BodyBuilder();
                            bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} tu permiso de tipo  {2} y ha sido enviado a Capital Humano para su revisión!</p>
                <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {4}</p><br /><a href=""{3}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, empleado.EmpNombreCompleto, NombreIncidencia, UrlReenvio, RegInci.ReInciId);

                            MailMessage correo = new MailMessage(); // ES EL CORREO QUE SE ENVIARÁ AL USUARIO COMO ESTATUS DE LA PROPIA INCIDENCIA
                            correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                            correo.To.Add(remitente);
                            correo.Subject = asunto;
                            correo.Body = bodyBuilderRemitente.HtmlBody;
                            correo.IsBodyHtml = true;
                            correo.Priority = MailPriority.Normal;
                            smtp.Send(correo);

                            MailMessage CorreoCH = new MailMessage(); // ES EL CORREO QUE SE MANDARÁ AL JEFE AHORA CON LAS INCIDENCIAS MODIFICADAS
                            CorreoCH.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                            CorreoCH.To.Add(CorreoCHDestino);
                            CorreoCH.Subject = AsuntoJefe;
                            CorreoCH.Body = bodyBuilderCH.HtmlBody;
                            CorreoCH.IsBodyHtml = true;
                            CorreoCH.Priority = MailPriority.Normal;

                            smtp.Send(CorreoCH);
                        }
                        else if(CorroborarFlujoPorCorreo.DetFlujoCorreoDestino == "Jefe")
                        {
                            var Estatus = _context.CatEstatus.Where(x => x.EstId == 3).First();
                            var EstatusDescripcion = Estatus.EstDescripcion;
                            var empleados = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).First();
                            var puesto = _context.CatPuestos.Where(x => x.PuestoId == empleados.EmpPuestoId).First();
                            var EmpleadoPuestoSuperior = _context.CatEmpleados.Where(x => x.EmpPuestoId == puesto.PuestoJerarquiaSuperiorPuestoId).First();
                            var UsuarioJefeSuperior = _userManager.Users.Where(x => x.UserName == EmpleadoPuestoSuperior.EmpUserId).First();
                            var UsuarioDeLaIncidencia = _userManager.Users.Where(x => x.UserName == empleados.EmpUserId).First();
                            var Paso = DetalleFlujos.DetFlujoDescripcion;
                            var destinatario = UsuarioDeLaIncidencia.Email;
                            var TodasLasIncidencias = _context.CatIncidencias.OrderBy(x => x.InciId).ToList();
                            var IncidenciaDelPermiso = TodasLasIncidencias.Where(x => x.InciId == RegInci.ReInciInciId).First();
                            var NombreIncidencia = IncidenciaDelPermiso.InciDescripcion;
                            var mensaje = "El permiso de tipo " + NombreIncidencia + " con el folio # " + RegInci.ReInciId + " ahora tiene el estatus de: " + EstatusDescripcion + " y las observaciones fueron: " + Bitacora.BitInciObservaciones;
                            var asunto = "Cambio de estatus de tu solicitud de permiso";
                            var DestinoJefe = UsuarioJefeSuperior.Email; // <---------------- Este es el bueno
                            var ObservacionesBitacora = Bitacora.BitInciObservaciones;
                            var AsuntoJefe = "La solicitud de permiso con folio: " + RegInci.ReInciId + ", ha sido modificada.";
                            var MensajeJefe = "El permiso con el folio # " + RegInci.ReInciId + " a nombre de " + empleados.EmpNombreCompleto + " ha sido modificado para su nueva revisión. Lo que se modificó fue lo siguiente: " + ObservacionesBitacora;
                            var urlglobal = "http://192.168.0.5/CatRegistroIncidencias?";
                            string Bandera = "3424hjlk234"; // Aceptada
                            var urlaceptada = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioJefeSuperior.Id + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                            Bandera = "jfnROs34"; // Rechazada
                            var urlrechazada = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioJefeSuperior.Id + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                            Bandera = "4RT55cgd6FOR"; // Reenvío
                            var UrlRegistroPendiente = urlglobal + "&a0s9d8f7g6=" /*Id Usuario*/ + UsuarioDeLaIncidencia + "&Fkch23=" /* Folio */ + RegInci.ReInciId + "&gs54gf=" /*Bandera*/ + Bandera;
                            DateTime FechaActualMail = DateTime.Now;
                            var FechaCorta = FechaActualMail.ToShortDateString();
                            var empleado = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).FirstOrDefault();

                            var bodyBuilder = new BodyBuilder();
                            bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> <p>¡Se ha creado una nueva solicitud de permiso de tipo {1} a nombre de {2}  para su revisión!</p><br />
                <p> En esta solicitud, se regresó para su correspondiente revisión y edición. Y el resultado es el siguiente: {6} </p>
              <p>Para aceptar el permiso, pulse el botón verde, para rechazarlo, pulse el botón rojo...</p> Petición de permiso No. {4}<br />
               <a href=""{3}""><img src=""https://i.imgur.com/Q7D7Kz5.png"" style='width:30%;'></a><a href=""{5}""><img src=""https://i.imgur.com/bHsgWWI.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, NombreIncidencia, empleado.EmpNombreCompleto, urlaceptada, RegInci.ReInciId, urlrechazada, ObservacionesBitacora);

                            var bodyBuilderRemitente = new BodyBuilder();
                            bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} tu permiso de tipo  {2} y ha sido enviada a {3} para su revisión!</p>
                <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {5}</p><br /><a href=""{4}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, empleado.EmpNombreCompleto, NombreIncidencia, EmpleadoPuestoSuperior.EmpNombreCompleto, UrlRegistroPendiente, RegInci.ReInciId);

                            MailMessage correo = new MailMessage(); // ES EL CORREO QUE SE ENVIARÁ AL USUARIO COMO ESTATUS DE LA PROPIA INCIDENCIA
                            correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                            correo.To.Add(destinatario);
                            correo.Subject = asunto;
                            correo.Body = bodyBuilderRemitente.HtmlBody;
                            correo.IsBodyHtml = true;
                            correo.Priority = MailPriority.Normal;

                            MailMessage CorreoJefe = new MailMessage(); // ES EL CORREO QUE SE MANDARÁ AL JEFE AHORA CON LAS INCIDENCIAS MODIFICADAS
                            CorreoJefe.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                            CorreoJefe.To.Add(DestinoJefe); // Cambiar a DestinoJefe
                            CorreoJefe.Subject = AsuntoJefe;
                            CorreoJefe.Body = bodyBuilder.HtmlBody;
                            CorreoJefe.IsBodyHtml = true;
                            CorreoJefe.Priority = MailPriority.Normal;

                            smtp.Send(correo);
                            smtp.Send(CorreoJefe);
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                    var x = _context.CatBitacoraIncidencias.Where(x => x.BitInciId == Bitacora.BitInciId).First();
                    var obj = new
                    {
                        x.BitInciFecha,
                        x.BitInciReInciId,
                        x.BitInciObservaciones,
                        x.BitInciUserId,
                        x.BitInciDetFlujoId
                    };
                    return Ok(Bitacora);
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
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("ComisionById/{Id}")]
        public async Task<IActionResult> ComisionById(int Id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                var DetInci = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == Id).First();
                var ReInci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Id).First();
                var Incidencia = ReInci.ReInciInciId;
                bool PermisoBotonEditar = false;
                var FechaCortita = ReInci.Fecha.ToShortDateString();
                var EstadoListado = _context.CatEstatus.Where(x => x.EstId == ReInci.ReInciEstatusId).First();
                string EstatusReadOnly = EstadoListado.EstDescripcion.ToString();
                var ListadoIncidencias = _context.CatIncidencias.Where(x => x.InciId == ReInci.ReInciInciId).First();
                string IncidenciasReadOnly = ListadoIncidencias.InciDescripcion.ToString();
                var FullName = _context.CatEmpleados.Where(x => x.EmpId == ReInci.ReInciEmpId).First();
                var Nombre = FullName.EmpNombre.ToString();
                var Paterno = FullName.EmpPaterno.ToString();
                var Materno = FullName.EmpMaterno.ToString();
                string NombreCompletoReadOnly = Nombre + " " + Paterno + " " + Materno;
                var Horario = _context.CatHorarios.ToList();
                List<HorariosFiltrados> horariosedicion = new List<HorariosFiltrados>();
                foreach (var item in Horario)
                {
                    horariosedicion.Add(new HorariosFiltrados()
                    {
                        HoraID = item.HoraId,
                        Hora = item.HoraEntrada.ToString(),
                        HoraSali = item.HoraSalida.ToString(),
                        HoraIngresoSabado = item.HoraSabadoEntrada.ToString(),
                        HoraSalidaSabado = item.HoraSabadoSalida.ToString()
                    });
                }
                var DetallesDeFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == ReInci.ReInciEstatusFlujo).First();
                var Perfiles = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
                var GetPerfilesDelFlujo = DetallesDeFlujos.DetFlujoPerfiles;
                var coma = ",";
                string[] ListaPerfiles = GetPerfilesDelFlujo.Split(coma);
                for(var i =0; i < ListaPerfiles.Length; i++)
                {
                    for(var j=0; j < Perfiles.Count; j++)
                    {
                        if (Perfiles[j].RoleId == ListaPerfiles[i])
                        {
                            PermisoBotonEditar = true;
                            break;
                        }
                    }
                }

                var objeto = new
                {
                    ReInci.ReInciId,
                    ReInci.ReInciEmpId,
                    FechaCortita,
                    ReInci.ReInciInciId,
                    ReInci.ReInciEstatusId,
                    DetInci.DetInciId,
                    DetInci.DetInciReInciId,
                    DetInci.Asunto,
                    DetInci.Destino,
                    DetInci.TelDestino,
                    DetInci.Contacto1,
                    DetInci.Contacto2,
                    DetInci.NombreDestino,
                    DetInci.Observaciones,
                    DetInci.HoraSalida,
                    DetInci.HoraRegreso,
                    EstatusReadOnly,
                    IncidenciasReadOnly,
                    NombreCompletoReadOnly,
                    horariosedicion,
                    PermisoBotonEditar
                };
                return Ok(objeto);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("EditComision")]
        public async Task<IActionResult> EditComision(CatBitacoraIncidencias Bitacora, CatDetalleIncidencias DetailsInci)
        {
            try
            {
                if (Bitacora != null)
                {
                    // ------------- Configuración del SMTP ---------------

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    // ----------------------------------------------------

                    var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));

                    DateTime FechaActual = DateTime.Now;
                    var RegInci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Bitacora.BitInciReInciId).First();
                    var ListaDetalleFlujos = _context.CatDetalleFlujo.OrderBy(x => x.DetFlujoId).ToList();
                    var DetalleFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == RegInci.ReInciEstatusFlujo).First();
                    var Flujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == DetalleFlujos.DetFlujoFlujoId).ToList();
                    var EstatusNew = RegInci.ReInciEstatusId;
                    var DetInci = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == Bitacora.BitInciReInciId).First();
                    var PasoActual = RegInci.ReInciEstatusFlujo;
                    var OrdenNuevo = 0;
                    var NuevoIdFlujo = 0;
                    var DiferenciaHora = "";
                    if (DetInci.HoraSalida != DetailsInci.HoraSalida)
                    {
                        DiferenciaHora = "Hora de salida modificada. La nueva hora es: " + DetailsInci.HoraSalida;
                        for (int i = 0; i < Flujos.Count; i++)
                        {
                            var OrdenActual = DetalleFlujos.DetFlujoOrden;
                            if (DetalleFlujos.DetFlujoId == Flujos[i].DetFlujoId)
                            {
                                if (Flujos.Count != i)
                                {
                                    if (OrdenActual != Flujos.Count)
                                    {
                                        OrdenNuevo = OrdenActual + 1;
                                        var NuevoFlujo = Flujos.Where(x => x.DetFlujoOrden == OrdenNuevo).First();
                                        NuevoIdFlujo = NuevoFlujo.DetFlujoId;

                                        Bitacora.BitInciReInciId = RegInci.ReInciId;
                                        Bitacora.BitInciUserId = user.UserName;
                                        Bitacora.BitInciFecha = FechaActual;
                                        Bitacora.BitInciObservaciones = DiferenciaHora;
                                        Bitacora.BitInciDetFlujoId = NuevoIdFlujo;

                                        DetInci.DetInciReInciId = RegInci.ReInciId;
                                        DetInci.Asunto = DetailsInci.Asunto;
                                        DetInci.Destino = DetailsInci.Destino;
                                        DetInci.TelDestino = DetailsInci.TelDestino;
                                        DetInci.Contacto1 = DetailsInci.Contacto1;
                                        DetInci.NombreDestino = DetailsInci.NombreDestino;
                                        DetInci.Contacto2 = DetailsInci.Contacto2;
                                        DetInci.Observaciones = DetailsInci.Observaciones;
                                        DetInci.HoraSalida = DetailsInci.HoraSalida;
                                        DetInci.HoraRegreso = DetailsInci.HoraRegreso;

                                        RegInci.ReInciEstatusFlujo = Bitacora.BitInciDetFlujoId;

                                        _context.CatBitacoraIncidencias.Add(Bitacora);
                                        _context.Update(RegInci);
                                        _context.Update(DetInci);
                                        _context.SaveChanges();

                                        var EmpleadoDeLaIncidencia = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).First();
                                        var NombreEmpleadoIncidencia = EmpleadoDeLaIncidencia.EmpNombreCompleto;
                                        var UserEmpleado = _userManager.Users.Where(x => x.UserName == EmpleadoDeLaIncidencia.EmpUserId).First();
                                        var destinatario = UserEmpleado.Email;
                                        var mensaje = NombreEmpleadoIncidencia + ", permiso en comisión. Tu hora de salida ha sido registrada: " + DetailsInci.HoraSalida;
                                        var asunto = NombreEmpleadoIncidencia + ", permiso en comisión. Registro de salida.";
                                        var Bandera = "4RT55cgd6FOR"; // Reenvío
                                        var url = "https://localhost:44392/CatRegistroIncidencias?";
                                        //var url = "http://192.168.0.5/CatRegistroIncidencias?";
                                        var urlReenvio = url + "&a0s9d8f7g6=" + UserEmpleado.Id + "&Fkch23=" + RegInci.ReInciId + "&gs54gf=" + Bandera;
                                        DateTime FechaActualMail = DateTime.Now;
                                        var FechaCorta = FechaActualMail.ToShortDateString();
                                        var empleado = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).FirstOrDefault();

                                        var bodyBuilder = new BodyBuilder();
                                        bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} en tu permiso en comisión se ha registrado la hora de salida: {2}!</p>
                <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {4}</p><br /><a href=""{3}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, empleado.EmpNombreCompleto, DetailsInci.HoraSalida, urlReenvio, RegInci.ReInciId);

                                        MailMessage correo = new MailMessage();
                                        correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- ¿Correo exclusivo para mandar o se manda desde el correo del usuario?
                                        correo.To.Add(destinatario);
                                        correo.Subject = asunto;
                                        correo.Body = bodyBuilder.HtmlBody;
                                        correo.IsBodyHtml = true;
                                        correo.Priority = MailPriority.Normal;

                                        smtp.Send(correo);

                                        break;
                                    }
                                }
                            }

                        }
                    }
                    if(DetInci.HoraRegreso != DetailsInci.HoraRegreso)
                    {
                        DiferenciaHora = "Hora de regreso modificada. La nueva hora es: " + DetailsInci.HoraRegreso;
                        
                        for (int i = 0; i < Flujos.Count; i++)
                        {
                            var OrdenActual = DetalleFlujos.DetFlujoOrden;
                            if (DetalleFlujos.DetFlujoId == Flujos[i].DetFlujoId)
                            {
                                if (Flujos.Count != i)
                                {
                                    if (OrdenActual != Flujos.Count)
                                    {
                                        OrdenNuevo = OrdenActual + 1;
                                        var NuevoFlujo = Flujos.Where(x => x.DetFlujoOrden == OrdenNuevo).First();
                                        NuevoIdFlujo = NuevoFlujo.DetFlujoId;

                                        Bitacora.BitInciReInciId = RegInci.ReInciId;
                                        Bitacora.BitInciUserId = user.UserName;
                                        Bitacora.BitInciFecha = FechaActual;
                                        Bitacora.BitInciObservaciones = DiferenciaHora;
                                        Bitacora.BitInciDetFlujoId = NuevoIdFlujo;

                                        DetInci.DetInciReInciId = RegInci.ReInciId;
                                        DetInci.Asunto = DetailsInci.Asunto;
                                        DetInci.Destino = DetailsInci.Destino;
                                        DetInci.TelDestino = DetailsInci.TelDestino;
                                        DetInci.Contacto1 = DetailsInci.Contacto1;
                                        DetInci.NombreDestino = DetailsInci.NombreDestino;
                                        DetInci.Contacto2 = DetailsInci.Contacto2;
                                        DetInci.Observaciones = DetailsInci.Observaciones;
                                        DetInci.HoraSalida = DetailsInci.HoraSalida;
                                        DetInci.HoraRegreso = DetailsInci.HoraRegreso;

                                        RegInci.ReInciEstatusFlujo = Bitacora.BitInciDetFlujoId;
                                        RegInci.ReInciEstatusId = 6;

                                        _context.CatBitacoraIncidencias.Add(Bitacora);
                                        _context.Update(RegInci);
                                        _context.Update(DetInci);
                                        _context.SaveChanges();

                                        var EmpleadoDeLaIncidencia = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).First();
                                        var NombreEmpleadoIncidencia = EmpleadoDeLaIncidencia.EmpNombreCompleto;
                                        var UserEmpleado = _userManager.Users.Where(x => x.UserName == EmpleadoDeLaIncidencia.EmpUserId).First();
                                        var destinatario = UserEmpleado.Email;
                                        var asunto = NombreEmpleadoIncidencia + ", permiso en comisión. Registro de regreso.";
                                        var Bandera = "4RT55cgd6FOR"; // Reenvío
                                        var url = "https://localhost:44392/CatRegistroIncidencias?";
                                        //var url = "http://192.168.0.5/CatRegistroIncidencias?";
                                        var urlReenvio = url + "&a0s9d8f7g6=" + UserEmpleado.Id + "&Fkch23=" + RegInci.ReInciId + "&gs54gf=" + Bandera;
                                        DateTime FechaActualMail = DateTime.Now;
                                        var FechaCorta = FechaActualMail.ToShortDateString();
                                        var empleado = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).FirstOrDefault();

                                        var bodyBuilder = new BodyBuilder();
                                        bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} en tu permiso en comisión se ha registrado la hora de salida: {2}!</p>
                <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {4}</p><br /><a href=""{3}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, empleado.EmpNombreCompleto, DetailsInci.HoraRegreso, urlReenvio, RegInci.ReInciId);

                                        MailMessage correo = new MailMessage();
                                        correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- ¿Correo exclusivo para mandar o se manda desde el correo del usuario?
                                        correo.To.Add(destinatario);
                                        correo.Subject = asunto;
                                        correo.Body = bodyBuilder.HtmlBody;
                                        correo.IsBodyHtml = true;
                                        correo.Priority = MailPriority.Normal;

                                        smtp.Send(correo);

                                        break;
                                    }
                                }
                            }

                        }
                    }

                    var x = _context.CatBitacoraIncidencias.Where(x => x.BitInciId == Bitacora.BitInciId).First();
                    var obj = new
                    {
                        x.BitInciFecha,
                        x.BitInciReInciId,
                        x.BitInciObservaciones,
                        x.BitInciUserId,
                        x.BitInciDetFlujoId
                    };
                    return Ok(Bitacora);
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
        }
        public class ListaEmpleadosCompartenPerfil
        {
            public string NombreEmpleado { get; set; }
        }

        public class ListaEmpleadosUsuarios
        {
            public string IdUsuarioByEmpleado { get; set; }
        }

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [Microsoft.AspNetCore.Mvc.HttpGet("JefeVacacionesIncidenciaById/{Id}")]
        public IActionResult JefeVacacionesIncidenciaById(int Id)
        {
            try
            {
                bool Bandera = false;
                var empleado = _context.CatEmpleados.Where(x=> x.EmpNumero == Id).First();
                var puesto = _context.CatPuestos.Where(x => x.PuestoId == empleado.EmpPuestoId).First();
                var Jefe = _context.CatEmpleados.Where(x => x.EmpPuestoId == puesto.PuestoJerarquiaSuperiorPuestoId).First();
                DateTime FechaActual = DateTime.Now;
                var IncidenciasDelJefe = _context.CatRegistroIncidencias.Where(x => x.ReInciEmpId == Jefe.EmpId && x.ReInciInciId == 4).ToList();
                var IncidenciaMasReciente = IncidenciasDelJefe.LastOrDefault();
                if(IncidenciaMasReciente == null)
                {
                    return Ok();
                }
                else
                {
                    var DetalleIncidencia = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == IncidenciaMasReciente.ReInciId).First();
                    var UsuarioJefe = _userManager.Users.Where(x => x.UserName == Jefe.EmpUserId).First();
                    var SacarPerfilesDeUsuario = _context.UserRoles.Where(x => x.UserId == UsuarioJefe.Id && x.RoleId != "4").First();
                    var SacarPersonalQueCompartePerfil = _context.UserRoles.Where(x => x.RoleId == SacarPerfilesDeUsuario.RoleId).ToList();
                    var Empleados = _context.CatEmpleados.OrderBy(x => x.EmpId).ToList();
                    var Usuarios = _userManager.Users.OrderBy(x => x.UserName).ToList();

                    List<ListaEmpleadosUsuarios> LUBE = new List<ListaEmpleadosUsuarios>();
                    for (int i = 0; i < Usuarios.Count; i++)
                    {
                        for (int j = 0; j < Empleados.Count; j++)
                        {
                            if (Empleados[j].EmpUserId == Usuarios[i].UserName)
                            {
                                if (Empleados[j].EmpId != Id)
                                {
                                    LUBE.Add(new ListaEmpleadosUsuarios()
                                    {
                                        IdUsuarioByEmpleado = Usuarios[i].Id
                                    });
                                }
                            }
                        }
                    }

                    List<ListaEmpleadosCompartenPerfil> ListaEmpleados = new List<ListaEmpleadosCompartenPerfil>();
                    for (int i = 0; i < SacarPersonalQueCompartePerfil.Count; i++)
                    {
                        for (int j = 0; j < LUBE.Count; j++)
                        {
                            if (LUBE[j].IdUsuarioByEmpleado == SacarPersonalQueCompartePerfil[i].UserId)
                            {
                                ListaEmpleados.Add(new ListaEmpleadosCompartenPerfil()
                                {
                                    NombreEmpleado = Empleados[j].EmpNombreCompleto
                                });
                            }
                        }
                    }
                    string[] NombresEmpleadosArreglo = new string[ListaEmpleados.Count];
                    for (int i = 0; i < ListaEmpleados.Count; i++)
                    {
                        NombresEmpleadosArreglo[i] = " " + ListaEmpleados[i].NombreEmpleado;
                    }
                    if (FechaActual > DetalleIncidencia.FechaInicio && FechaActual < DetalleIncidencia.FechaPresentacion)
                    {
                        Bandera = true;
                    }
                    var obj = new
                    {
                        Bandera,
                        NombresEmpleadosArreglo
                    };
                    return Ok(obj);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("DetallesIncidenciaCancelacion/{Id}")]
        public IActionResult DetallesIncidenciaCancelacion(int Id)
        {
            try
            {
                var inci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Id).First();
                var detinci = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == Id).First();
                var HorariosConcatenados = _context.CatHorarios.Where(x => x.HoraId == detinci.DetInciHorarioId).First();
                var HorarioEnt = HorariosConcatenados.HoraEntrada.ToString();
                var HorarioSal = HorariosConcatenados.HoraSalida.ToString();
                var HorarioEntSabado = HorariosConcatenados.HoraSabadoEntrada.ToString();
                var HorarioSalSabado = HorariosConcatenados.HoraSabadoSalida.ToString();
                string HorarioFinal = "L-V: " + HorarioEnt + " - " + HorarioSal + " Y S de: " + HorarioEntSabado + " - " + HorarioSalSabado;
                var FullName = _context.CatEmpleados.Where(x => x.EmpId == inci.ReInciEmpId).First();
                var Nombre = FullName.EmpNombre.ToString();
                var Paterno = FullName.EmpPaterno.ToString();
                var Materno = FullName.EmpMaterno.ToString();
                string NombreCompleto = Nombre + " " + Paterno + " " + Materno;
                var EstadoListado = _context.CatEstatus.Where(x => x.EstId == inci.ReInciEstatusId).First();
                string EstatusConversion = EstadoListado.EstDescripcion.ToString();
                var ListadoIncidencias = _context.CatIncidencias.Where(x => x.InciId == inci.ReInciInciId).First();
                string IncidenciasConversion = ListadoIncidencias.InciDescripcion.ToString();

                var fefinper = "";
                var HoraIngreso = "";
                var HoraSalidaPermisoPersonal = "";
                var HoraSComida = "";
                var HoraRComida = "";

                if (detinci.DetInciFechaFinPermisoPersonal != null)
                {
                    fefinper = detinci.DetInciFechaFinPermisoPersonal.Value.ToShortDateString();
                }

                if (detinci.DetInciHoraSalida != null)
                {
                    HoraSalidaPermisoPersonal = detinci.DetInciHoraSalida.ToString();
                }

                if (detinci.DetInciHoraIngreso != null)
                {
                    HoraIngreso = detinci.DetInciHoraIngreso.ToString();
                }

                if (detinci.DetInciHoraSalidaComida != null)
                {
                    HoraSComida = detinci.DetInciHoraSalidaComida.ToString();
                }

                if (detinci.DetInciHoraRegresoComida != null)
                {
                    HoraRComida = detinci.DetInciHoraRegresoComida.ToString();
                }

                var obj = new
                {
                    inci.ReInciId,
                    inci.ReInciEmpId,
                    NombreCompleto,
                    FeGlobal = inci.Fecha.ToShortDateString(),
                    IncidenciasConversion,
                    EstatusConversion,
                    inci.ReInciEstatusId,
                    detinci.DetInciId,
                    detinci.DetInciReInciId,
                    detinci.DetInciHorarioId,
                    HorarioFinal, // String de los horarios y concatenados
                    detinci.MedidaAccion,
                    detinci.Asunto,
                    detinci.Destino,
                    detinci.TelDestino,
                    detinci.Contacto1,
                    detinci.Contacto2,
                    detinci.NombreDestino,
                    detinci.Observaciones,
                    Hos = detinci.HoraSalida.ToString(),
                    Hor = detinci.HoraRegreso.ToString(),
                    FeIni = detinci.FechaInicio.ToShortDateString(),
                    FeFin = detinci.FechaFinal.ToShortDateString(),
                    FePres = detinci.FechaPresentacion.ToShortDateString(),
                    detinci.DiasAusencia,
                    detinci.PersonaCubrira,
                    FechaDetails = detinci.DetFecha.ToShortDateString(),
                    detinci.Motivo,
                    fefinper,
                    HoraIngreso,
                    HoraSalidaPermisoPersonal,
                    HoraSComida,
                    HoraRComida
            };
                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("CancelarPermiso")]
        public async Task<IActionResult> CancelarPermiso(CatBitacoraIncidencias Bitacora, int Id, string MotivoCancelacion)
        {
            try
            {
                if (Bitacora != null)
                {
                    // ------------- Configuración del SMTP ---------------

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    // ----------------------------------------------------

                    var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));

                    DateTime FechaActual = DateTime.Now;
                    var RegInci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Id).First();
                    RegInci.ReInciEstatusId = 4;

                    Bitacora.BitInciReInciId = Id;
                    Bitacora.BitInciFecha = FechaActual;
                    Bitacora.BitInciDetFlujoId = RegInci.ReInciEstatusFlujo;
                    Bitacora.BitInciUserId = user.UserName;
                    Bitacora.BitInciObservaciones = MotivoCancelacion;
                   
                    _context.CatBitacoraIncidencias.Add(Bitacora);
                    _context.Update(RegInci);
                    _context.SaveChanges();

                    var TipoPermiso = _context.CatIncidencias.Where(x => x.InciId == RegInci.ReInciInciId).First();
                    var EmpleadoDeLaIncidencia = _context.CatEmpleados.Where(x => x.EmpId == RegInci.ReInciEmpId).First();
                    var UserEmpleado = _userManager.Users.Where(x => x.UserName == EmpleadoDeLaIncidencia.EmpUserId).First();
                    var PuestoEmpleado = _context.CatPuestos.Where(x => x.PuestoId == EmpleadoDeLaIncidencia.EmpPuestoId).First();
                    var PuestoSuperior = _context.CatPuestos.Where(x => x.PuestoId == PuestoEmpleado.PuestoJerarquiaSuperiorPuestoId).First();
                    var Jefe = _context.CatEmpleados.Where(x => x.EmpPuestoId == PuestoSuperior.PuestoId).First();
                    var UserJefe = _userManager.Users.Where(x => x.UserName == Jefe.EmpUserId).First();
                    var FLujoIncidencia = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == RegInci.ReInciEstatusFlujo).First();
                    var ListaFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == FLujoIncidencia.DetFlujoFlujoId).ToList();
                    var CorreoCapitalHumano = ListaFlujos.Where(x => x.DetFlujoCorreoDestino == "CapitalHumano").First();
                    var UserCapitalHumano = CorreoCapitalHumano.DetFlujoEmail; // <----------- Este es el bueno
                    var destinatarioReenvio = UserEmpleado.Email;
                    var destinatarioJefe =  UserJefe.Email; 
                    var destinatarioCH = UserCapitalHumano;
                    var asunto = "Se ha cancelado una solicitud de permiso del tipo: " + TipoPermiso.InciDescripcion + ". Con el folio" + RegInci.ReInciId;
                    var Bandera = "4RT55cgd6FOR"; // Reenvío
                    var url = "https://localhost:44392/CatRegistroIncidencias?";
                    //var url = "http://192.168.0.5/CatRegistroIncidencias?";
                    var urlReenvio = url + "&a0s9d8f7g6=" + UserEmpleado.Id + "&Fkch23=" + RegInci.ReInciId + "&gs54gf=" + Bandera;
                    var FechaCorta = FechaActual.ToShortDateString();

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p> La solicitud de permiso de tipo {1} a nombre de {2} ha sido cancelada por el siguiente motivo </p><br />
                <p> {5} </p><br />
                <p>Para consultarla da click en el siguiente link...</p> <p> Petición de permiso No. {4}</p><br /><a href=""{3}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, TipoPermiso.InciDescripcion, EmpleadoDeLaIncidencia.EmpNombreCompleto, urlReenvio, RegInci.ReInciId, MotivoCancelacion);

                     MailMessage correo = new MailMessage();
                     correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- ¿Correo exclusivo para mandar o se manda desde el correo del usuario?
                     correo.To.Add(destinatarioReenvio);
                     correo.Subject = asunto;
                     correo.Body = bodyBuilder.HtmlBody;
                     correo.IsBodyHtml = true;
                     correo.Priority = MailPriority.Normal;

                    MailMessage correoJefe = new MailMessage();
                    correoJefe.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- ¿Correo exclusivo para mandar o se manda desde el correo del usuario?
                    correoJefe.To.Add(destinatarioJefe);
                    correoJefe.Subject = asunto;
                    correoJefe.Body = bodyBuilder.HtmlBody;
                    correoJefe.IsBodyHtml = true;
                    correoJefe.Priority = MailPriority.Normal;

                    MailMessage correoCH = new MailMessage();
                    correoCH.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- ¿Correo exclusivo para mandar o se manda desde el correo del usuario?
                    correoCH.To.Add(destinatarioCH);
                    correoCH.Subject = asunto;
                    correoCH.Body = bodyBuilder.HtmlBody;
                    correoCH.IsBodyHtml = true;
                    correoCH.Priority = MailPriority.Normal;

                    smtp.Send(correo);
                    smtp.Send(correoJefe);
                    smtp.Send(correoCH);

                    if(TipoPermiso.InciDescripcion == "VACACIONES")
                    {
                        var HistorialVacaciones = _context.CatHistorialVacaciones.Where(x => x.HVReInciId == RegInci.ReInciId).First();
                        HistorialVacaciones.HVSaldoVacaciones = HistorialVacaciones.HVSaldoVacaciones + HistorialVacaciones.HVDiasSolicitados;
                        _context.Update(HistorialVacaciones);
                        _context.SaveChanges();
                    }

                    var x = _context.CatBitacoraIncidencias.Where(x => x.BitInciId == Bitacora.BitInciId).First();
                    var obj = new
                    {
                        x.BitInciFecha,
                        x.BitInciReInciId,
                        x.BitInciObservaciones,
                        x.BitInciUserId,
                        x.BitInciDetFlujoId
                    };
                    return Ok(Bitacora);
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
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("ValidacionEmpleado/{Id}")]
        public IActionResult ValidacionEmpleado(int Id)
        {
            try
            {
                var Empleado = _context.CatEmpleados.Where(x=> x.EmpNumero == Id).FirstOrDefault();
                var NombreEmpleado = "";
                if (Empleado != null)
                {
                    NombreEmpleado = Empleado.EmpNombreCompleto;
                }
                var Horario = _context.MovEmpleadosHorarios.Where(x => x.EmpHoraEmpId == Empleado.EmpId && x.EmpHoraFechaHasta == null).FirstOrDefault();
                var HorarioPermanente = "";
                if(Horario == null)
                {
                    HorarioPermanente = "No tiene";
                }
                else
                {
                    HorarioPermanente = "Sí tiene";
                }
                var Estatus = _context.CatEstatus.Where(x => x.EstId == Empleado.EmpEstId).FirstOrDefault();
                var obj = new
                {
                   NombreEmpleado,
                   HorarioPermanente,
                   Estatus.EstDescripcion
                };
                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
