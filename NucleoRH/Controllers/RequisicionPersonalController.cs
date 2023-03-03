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
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using System.Drawing;

namespace NucleoRH.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class RequisicionPersonalController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RequisicionPersonalController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ITemplateHelper templateHelper)
        {
            this._context = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public class RequisicionesPersonal
        {
            public int Id { get; set; }
            public DateTime FechaDeElaboracion { get; set; }
            public string NombreDelPuesto { get; set; }
            public int NumeroDeVacantes { get; set; }
            public string Turno { get; set; }
            public string Sucursal { get; set; }
            public string Departamento { get; set; }
            public string EstatusId { get; set; }
            public int FlujoId { get; set; }

        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ConsultaRequisicionPersonal = (from rp in _context.MovRequisicionPersonal
                                               join puesto in _context.CatPuestos on rp.MRPPuestoId equals puesto.PuestoId
                                               join turno in _context.CatTurnosLaborales on rp.MRPTurnoId equals turno.TurId
                                               join sucu in _context.CatSucursales on rp.MRPSucursalId equals sucu.SucuId
                                               join area in _context.CatAreas on puesto.PuestoAreaId equals area.AreaId
                                               join depa in _context.CatDepartamentos on area.AreaDepaId equals depa.DepaId
                                               join estatus in _context.CatEstatus on rp.MRPEstatusId equals estatus.EstId
                                               where rp.MRPUserId == user.UserName
                                               select new RequisicionesPersonal
                                               {
                                                   Id = rp.MRPId,
                                                   FechaDeElaboracion = rp.MRPFechaElaboracion,
                                                   NombreDelPuesto = puesto.PuestoDescripcion,
                                                   NumeroDeVacantes = rp.MRPNumeroVacantes,
                                                   Turno = turno.TurDescripcion,
                                                   Sucursal = sucu.SucuNombre,
                                                   Departamento = depa.DepaDescripcion,
                                                   FlujoId = rp.MRPFlujoId,
                                                   EstatusId = estatus.EstDescripcion
                                               }).ToList();

            var ConsultaRequisicionPersonalCH = (from rp in _context.MovRequisicionPersonal
                                               join puesto in _context.CatPuestos on rp.MRPPuestoId equals puesto.PuestoId
                                               join turno in _context.CatTurnosLaborales on rp.MRPTurnoId equals turno.TurId
                                               join sucu in _context.CatSucursales on rp.MRPSucursalId equals sucu.SucuId
                                               join area in _context.CatAreas on puesto.PuestoAreaId equals area.AreaId
                                               join depa in _context.CatDepartamentos on area.AreaDepaId equals depa.DepaId
                                               join estatus in _context.CatEstatus on rp.MRPEstatusId equals estatus.EstId
                                               where rp.MRPFlujoId !=1063
                                               select new RequisicionesPersonal
                                               {
                                                   Id = rp.MRPId,
                                                   FechaDeElaboracion = rp.MRPFechaElaboracion,
                                                   NombreDelPuesto = puesto.PuestoDescripcion,
                                                   NumeroDeVacantes = rp.MRPNumeroVacantes,
                                                   Turno = turno.TurDescripcion,
                                                   Sucursal = sucu.SucuNombre,
                                                   Departamento = depa.DepaDescripcion,
                                                   FlujoId = rp.MRPFlujoId,
                                                   EstatusId = estatus.EstDescripcion
                                               }).ToList();

            var ConsultaRequisicionPersonalDireccion = (from rp in _context.MovRequisicionPersonal
                                               join puesto in _context.CatPuestos on rp.MRPPuestoId equals puesto.PuestoId
                                               join turno in _context.CatTurnosLaborales on rp.MRPTurnoId equals turno.TurId
                                               join sucu in _context.CatSucursales on rp.MRPSucursalId equals sucu.SucuId
                                               join area in _context.CatAreas on puesto.PuestoAreaId equals area.AreaId
                                               join depa in _context.CatDepartamentos on area.AreaDepaId equals depa.DepaId
                                               join estatus in _context.CatEstatus on rp.MRPEstatusId equals estatus.EstId
                                               where rp.MRPFlujoId ==1063
                                               select new RequisicionesPersonal
                                               {
                                                   Id = rp.MRPId,
                                                   FechaDeElaboracion = rp.MRPFechaElaboracion,
                                                   NombreDelPuesto = puesto.PuestoDescripcion,
                                                   NumeroDeVacantes = rp.MRPNumeroVacantes,
                                                   Turno = turno.TurDescripcion,
                                                   Sucursal = sucu.SucuNombre,
                                                   Departamento = depa.DepaDescripcion,
                                                   FlujoId = rp.MRPFlujoId,
                                                   EstatusId = estatus.EstDescripcion
                                               }).ToList();

            ViewBag.RequisicionPersonal = ConsultaRequisicionPersonal.OrderBy(x => x.Id).ToList();
            ViewBag.RequisicionPersonalCH = ConsultaRequisicionPersonalCH.OrderBy(x => x.Id).ToList();
            ViewBag.RequisicionPersonalDireccion = ConsultaRequisicionPersonalDireccion.OrderBy(x => x.Id).ToList();
            var Puestos = _context.CatPuestos.OrderBy(x => x.PuestoDescripcion).ToList();
            ViewData["Puestos"] = new SelectList(Puestos, "PuestoId", "PuestoDescripcion");
            var Turnos = _context.CatTurnosLaborales.OrderBy(x=> x.TurId).ToList();
            ViewData["Turnos"] = new SelectList(Turnos, "TurId", "TurDescripcion");
            var Escolaridades = _context.CatEscolaridades.OrderBy(x => x.EscoId).ToList();
            ViewData["Escolaridades"] = new SelectList(Escolaridades, "EscoId", "EscoDescripcion");
            var Sexos = _context.CatSexos.OrderBy(x=> x.SexId).ToList();
            ViewData["Sexos"] = new SelectList(Sexos, "SexId", "SexDescripcion");
            var Departamentos = _context.CatDepartamentos.OrderBy(x => x.DepaDescripcion).ToList();
            ViewData["Departamentos"] = new SelectList(Departamentos, "DepaId", "DepaDescripcion").ToList();
            var Sucursales = _context.CatSucursales.OrderBy(x => x.SucuNombre).ToList();
            ViewData["Sucursales"] = new SelectList(Sucursales, "SucuId", "SucuNombre");
            var Estatus = _context.CatEstatus.Where(x => x.EstDescripcion != "ACTIVO" && x.EstDescripcion != "BAJA" && x.EstDescripcion != "PROCESADO" && x.EstDescripcion != "ACEPTADO").ToList();
            ViewData["Estatus"] = new SelectList(Estatus, "EstId", "EstDescripcion");

            return View();
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("AddPuesto")]
        public IActionResult AddPuestos(CatPuestos puestos)
        {
            try
            {
                if (puestos != null)
                {
                    _context.CatPuestos.Add(puestos); // El nombre del catálogo
                    _context.SaveChanges();
                    var x = _context.CatPuestos.Include(x => x.PuestoDescripcion).Where(x => x.PuestoId == puestos.PuestoId).First(); // Solo en consulta, puedes poner el primer campo != de ID
                    var obj = new
                    {
                        x.PuestoId,
                        x.PuestoDescripcion,
                        x.PuestoAreaId,
                        x.PuestoJerarquiaSuperiorPuestoId

                    };
                    return Ok(puestos);
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

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [Microsoft.AspNetCore.Mvc.HttpPost("AgregarPuestos")]
        public async Task<IActionResult> AddPuestosAsync(MovRequisicionPersonal MRP, CatBitacoraRequisicionPersonal BitRP)
        {
            try
            {
                if (MRP != null)
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                    var Empleados = _context.CatEmpleados.Where(x => x.EmpNumero == 0).FirstOrDefault();

                    if (MRP.MRPEmpId == 0)
                    {
                        
                        MRP.MRPEmpId = Empleados.EmpId;
                    }
                    if(MRP.MRPMotivoDescripcion == null)
                    {
                        MRP.MRPMotivoDescripcion = "No es nueva creación";
                    }

                    MRP.MRPFlujoId = 1062;
                    MRP.MRPEstatusId = 3;
                    MRP.MRPUserId = user.UserName;
                    _context.MovRequisicionPersonal.Add(MRP); // El nombre del catálogo
                    _context.SaveChanges();

                    // ---------------------------------- Envío de correos -------------------------------------

                    BitRP.BitRPRPId = MRP.MRPId;
                    BitRP.BitRPDetFlujoId = MRP.MRPFlujoId;
                    BitRP.BitRPUserId = user.UserName;
                    BitRP.BitRPFecha = MRP.MRPFechaElaboracion;
                    BitRP.BitRPObservaciones = "Requisición creada. Enviada a Capital Humano para su revisión.";
                    _context.CatBitacoraRequisicionPersonal.Add(BitRP);
                    _context.SaveChanges();

                    var Sucursal = _context.CatSucursales.Where(x => x.SucuId == MRP.MRPSucursalId).First();
                    var asunto = "Estatus de tu requisición de personal. " + Sucursal.SucuNombre + ". Folio interno: " + MRP.MRPId;
                    var asuntoCH = "Nueva Solicitud de requisición de personal. " + Sucursal.SucuNombre + ". Folio interno: " + MRP.MRPId;
                    var destinatario = user.Email;
                    var FlujoRequisicion = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == 1062).First();
                    var destinatarioCH = FlujoRequisicion.DetFlujoEmail;
                    DateTime FechaActual = DateTime.Now;
                    var FechaCorta = FechaActual.ToShortDateString();
                    var Empleado = _context.CatEmpleados.Where(x => x.EmpUserId == user.UserName).FirstOrDefault();
                    var Puesto = _context.CatPuestos.Where(x=> x.PuestoId == MRP.MRPPuestoId).FirstOrDefault();

                    var urlGlobal = "192.168.0.5/RequisicionPersonal?";
                    var Bandera = "7teugr"; // Reenvío para details
                    var UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + MRP.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                              <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                              <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                              <th width='15%'>{0}</th> </tr> <tr>
                              <td colspan='3'> <br /> <br /><br />
                              <div style='display:block; text-align:center;'> 
                              <p>¡ {1} tu requisición de personal del puesto {3} ya ha sido enviada a Capital Humano para su revisión!</p>
                              <p>Para consultarla da click en el siguiente link...</p> <br /><a href=""{2}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleado.EmpNombreCompleto, UrlDetallesRP, Puesto.PuestoDescripcion);

                    Bandera = "B1qazfg4wsx3edc"; // CapitalHumano con flujo
                    UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + MRP.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                    var bodyBuilderCH = new BodyBuilder();
                    bodyBuilderCH.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                              <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                              <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                              <th width='15%'>{0}</th> </tr> <tr>
                              <td colspan='3'> <br /> <br /><br />
                              <div style='display:block; text-align:center;'> 
                              <p>¡ Se ha registrado una nueva requisición de personal a nombre de {1} para el puesto de {3} para su revisión !</p>
                              <p>Para consultarla da click en el siguiente link...</p> <br /><a href=""{2}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleado.EmpNombreCompleto, UrlDetallesRP, Puesto.PuestoDescripcion);

                    MailMessage correo = new MailMessage(); // <- Correo mandado para CH
                    correo.From = new MailAddress("nucleorh@nucleodediagnostico.com");
                    correo.To.Add(destinatario);
                    correo.Subject = asunto;
                    correo.Body = bodyBuilder.HtmlBody;
                    correo.IsBodyHtml = true;
                    correo.Priority = MailPriority.Normal;

                    MailMessage correoCH = new MailMessage(); // <- Correo mandado para CH
                    correo.From = new MailAddress("nucleorh@nucleodediagnostico.com");
                    correo.To.Add(destinatarioCH);
                    correo.Subject = asuntoCH;
                    correo.Body = bodyBuilderCH.HtmlBody;
                    correo.IsBodyHtml = true;
                    correo.Priority = MailPriority.Normal;

                    smtp.Send(correo);
                    smtp.Send(correoCH);

                    var x = _context.MovRequisicionPersonal.Where(x => x.MRPId == MRP.MRPId).First();
                    var obj = new
                    {
                        x.MRPId,
                        x.MRPPuestoId,
                        x.MRPNumeroVacantes,
                        x.MRPTipoVacante,
                        x.MRPRolarTurno,
                        x.MRPTiempoAlimentos,
                        x.MRPMotivoVacante,
                        x.MRPMotivoDescripcion,
                        x.MRPSexoId,
                        x.MRPEdadMaxima,
                        x.MRPEdadMinima,
                        x.MRPEscolaridadId,
                        x.MRPTituloIndispensable,
                        x.MRPCedulaIndispensable,
                        x.MRPExperienciaIndispensable,
                        x.MRPFuncionesPuesto,
                        x.MRPConocimientosPuesto,
                        x.MRPFechaElaboracion,
                        x.MRPTurnoId,

                    };
                    return Ok(MRP);
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

        [Microsoft.AspNetCore.Mvc.HttpGet("SeleccionarDepartamentoRP")]
        public IActionResult AreaByDepaId(int Id)
        {
            
            var AreasDep = _context.CatAreas.Where(x => x.AreaDepaId == Id).ToList();
            var obj = new
            {
                AreasDep
            };
            return Ok(obj);
        }

        public class EmpleadosPuestosByArea
        {
            public int EPBPuestoId { get; set; }
            public string EPBEmpNombreCompleto { get; set; }
            public int EPBEmpId { get; set; }

        }

        [Microsoft.AspNetCore.Mvc.HttpGet("SeleccionarEmpleadoRP")]
        public IActionResult AreaByPuestoId(int Id)
        {
            var PuestoOrigen = _context.CatPuestos.Where(x => x.PuestoId == Id).First();
            var AreasDep = _context.CatAreas.Where(x => x.AreaId == PuestoOrigen.PuestoAreaId).First();

            var EmpleadoJoin = (from emp in _context.CatEmpleados
                                join puesto in _context.CatPuestos on emp.EmpPuestoId equals puesto.PuestoId
                                where puesto.PuestoAreaId == AreasDep.AreaId
                                select new EmpleadosPuestosByArea
                                {
                                    EPBPuestoId = puesto.PuestoId,
                                    EPBEmpId = emp.EmpId,
                                    EPBEmpNombreCompleto = emp.EmpNombreCompleto
                                }).ToList();

            var obj = new
            {
                EmpleadoJoin

            };
            return Ok(obj);
        }

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [Microsoft.AspNetCore.Mvc.HttpGet("RequisicionById/{Id}")]
        public async Task<IActionResult> RequisicionByIdAsync(int Id)
        {
            try
            {
                var requisicion = _context.MovRequisicionPersonal.Where(x => x.MRPId == Id).First();
                var Puesto = _context.CatPuestos.Where(x => x.PuestoId == requisicion.MRPPuestoId).FirstOrDefault();
                var Sucursal = _context.CatSucursales.Where(x => x.SucuId == requisicion.MRPSucursalId).FirstOrDefault();
                var Turno = _context.CatTurnosLaborales.Where(x => x.TurId == requisicion.MRPTurnoId).FirstOrDefault();
                var Escolaridades = _context.CatEscolaridades.Where(x => x.EscoId == requisicion.MRPEscolaridadId).FirstOrDefault();
                var Sexos = _context.CatSexos.Where(x => x.SexId == requisicion.MRPSexoId).FirstOrDefault();
                var TurnoRolado = "";
                var PasoDelFlujo = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == requisicion.MRPFlujoId).FirstOrDefault();

                var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                var userrole = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
                if (PasoDelFlujo.DetFlujoOrden == 1)
                {
                    var ValidacionPerfil = userrole.Where(x => x.RoleId == "1" || x.RoleId == "9" || x.RoleId == "3" || x.RoleId == "8").FirstOrDefault();
                    if (ValidacionPerfil == null)
                    {
                        return BadRequest();
                    }
                }
                else if (PasoDelFlujo.DetFlujoOrden == 2)
                {
                    var ValidacionPerfil = userrole.Where(x => x.RoleId == "1" || x.RoleId == "9").FirstOrDefault();
                    if (ValidacionPerfil == null)
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }

                string BanderaPaso = "";
                if (PasoDelFlujo.DetFlujoOrden == 1)
                {
                    BanderaPaso = "CH";
                }
                else if (PasoDelFlujo.DetFlujoOrden == 2)
                {
                    BanderaPaso = "Dirección";
                }
                else if (PasoDelFlujo.DetFlujoOrden == 3)
                {
                    BanderaPaso = "CapitalHumano";
                }
                else
                {
                    return BadRequest();
                }
                if (requisicion.MRPRolarTurno == true)
                {
                    TurnoRolado = "Sí";
                }
                else
                {
                    TurnoRolado = "No";
                }
                var TipoVacante = "";
                if (requisicion.MRPTipoVacante == 1)
                {
                    TipoVacante = "Eventual";
                }
                else if (requisicion.MRPTipoVacante == 2)
                {
                    TipoVacante = "Permanente";
                }
                else if (requisicion.MRPTipoVacante == 3)
                {
                    TipoVacante = "Cubre-incidencias";
                }
                else
                {
                    TipoVacante = "Tipo de vacante no válido";
                }
                var MotivoVacante = "";
                if (requisicion.MRPMotivoVacante == 1)
                {
                    MotivoVacante = "Nueva creación";
                }
                else if (requisicion.MRPMotivoVacante == 2)
                {
                    MotivoVacante = "Vacaciones";
                }
                else if (requisicion.MRPMotivoVacante == 3)
                {
                    MotivoVacante = "Sustitución de personal";
                }
                else if (requisicion.MRPMotivoVacante == 4)
                {
                    MotivoVacante = "Cubrir incapacidad";
                }
                else if (requisicion.MRPMotivoVacante == 5)
                {
                    MotivoVacante = "Promoción de personal";
                }
                else
                {
                    MotivoVacante = "Motivo no válido";
                }
                var Titulo = "";
                if (requisicion.MRPTituloIndispensable == true)
                {
                    Titulo = "Sí";
                }
                else
                {
                    Titulo = "No";
                }
                var Cedula = "";
                if (requisicion.MRPCedulaIndispensable == true)
                {
                    Cedula = "Si";
                }
                else
                {
                    Cedula = "No";
                }
                var Experiencia = "";
                if (requisicion.MRPExperienciaIndispensable == true)
                {
                    Experiencia = "Si";
                }
                else
                {
                    Experiencia = "No";
                }
                var fecharecepcioncorta = "";

                if(requisicion.MRPFechaRecepcion != null)
                {
                    fecharecepcioncorta = requisicion.MRPFechaRecepcion.Value.ToShortDateString();
                }
                string DiaIngreso = "";
                string MesIngreso = "";
                string AnioIngreso = "";
                if(requisicion.MRPFechaIngreso != null)
                {
                    DiaIngreso = requisicion.MRPFechaIngreso.Value.Day.ToString();
                    MesIngreso = requisicion.MRPFechaIngreso.Value.Month.ToString();
                    AnioIngreso = requisicion.MRPFechaIngreso.Value.Year.ToString();
                }
                
                if(requisicion.MRPFechaIngreso != null)
                {
                    if (requisicion.MRPFechaIngreso.Value.Day < 10)
                    {
                        DiaIngreso = "0" + DiaIngreso;
                    }
                    if (requisicion.MRPFechaIngreso.Value.Month < 10)
                    {
                        MesIngreso = "0" + MesIngreso;
                    }
                }
                var Empleado = _context.CatEmpleados.Where(x => x.EmpId == requisicion.MRPEmpId).FirstOrDefault();
                var NombreEmpleado = Empleado.EmpNombreCompleto;
                string ingreso = AnioIngreso + "-" + MesIngreso + "-" + DiaIngreso;

                var obj = new
                {
                    Puesto.PuestoDescripcion,
                    Sucursal.SucuNombre,
                    Turno.TurDescripcion,
                    Escolaridades.EscoDescripcion,
                    Sexos.SexDescripcion,
                    TurnoRolado,
                    TipoVacante,
                    MotivoVacante,
                    Titulo,
                    Cedula,
                    Experiencia,
                    requisicion.MRPNumeroVacantes,
                    fe = requisicion.MRPFechaElaboracion.ToShortDateString(),
                    requisicion.MRPTiempoAlimentos,
                    requisicion.MRPMotivoDescripcion,
                    requisicion.MRPEdadMaxima,
                    requisicion.MRPEdadMinima,
                    requisicion.MRPFuncionesPuesto,
                    requisicion.MRPConocimientosPuesto,
                    requisicion.MRPEstatusId,
                    requisicion.MRPId,
                    requisicion.MRPFolio,
                    requisicion.MRPSueldoMensualInicial,
                    requisicion.MRPSueldoMensualPlanta,
                    requisicion.MRPSueldoMensualMasCosto,
                    requisicion.MRPBonoVariable,
                    requisicion.MRPEsquemaContratacion,
                    requisicion.MRPCandidato,
                    requisicion.MRPFechaIngreso,
                    fecharecepcioncorta,
                    ingreso,
                    BanderaPaso,
                    NombreEmpleado
                };
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("RequisicionDetallesById/{Id}")]
        public IActionResult RequisicionDetallesById(int Id)
        {
            try
            {
                
                var requisicion = _context.MovRequisicionPersonal.Where(x => x.MRPId == Id).First();
                var Puesto = _context.CatPuestos.Where(x => x.PuestoId == requisicion.MRPPuestoId).FirstOrDefault();
                var Sucursal = _context.CatSucursales.Where(x => x.SucuId == requisicion.MRPSucursalId).FirstOrDefault();
                var Turno = _context.CatTurnosLaborales.Where(x => x.TurId == requisicion.MRPTurnoId).FirstOrDefault();
                var Escolaridades = _context.CatEscolaridades.Where(x => x.EscoId == requisicion.MRPEscolaridadId).FirstOrDefault();
                var Sexos = _context.CatSexos.Where(x => x.SexId == requisicion.MRPSexoId).FirstOrDefault();
                var TurnoRolado = "";
                var PasoDelFlujo = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == requisicion.MRPFlujoId).FirstOrDefault();
                string BanderaPaso = "";
                if (PasoDelFlujo.DetFlujoOrden == 1)
                {
                    BanderaPaso = "CH";
                }
                else if (PasoDelFlujo.DetFlujoOrden == 2)
                {
                    BanderaPaso = "Dirección";
                }
                else if (PasoDelFlujo.DetFlujoOrden == 3)
                {
                    BanderaPaso = "CapitalHumano";
                }
                else
                {
                    return BadRequest();
                }
                if (requisicion.MRPRolarTurno == true)
                {
                    TurnoRolado = "Sí";
                }
                else
                {
                    TurnoRolado = "No";
                }
                var TipoVacante = "";
                if (requisicion.MRPTipoVacante == 1)
                {
                    TipoVacante = "Eventual";
                }
                else if (requisicion.MRPTipoVacante == 2)
                {
                    TipoVacante = "Permanente";
                }
                else if (requisicion.MRPTipoVacante == 3)
                {
                    TipoVacante = "Cubre-incidencias";
                }
                else
                {
                    TipoVacante = "Tipo de vacante no válido";
                }
                var MotivoVacante = "";
                if (requisicion.MRPMotivoVacante == 1)
                {
                    MotivoVacante = "Nueva creación";
                }
                else if (requisicion.MRPMotivoVacante == 2)
                {
                    MotivoVacante = "Vacaciones";
                }
                else if (requisicion.MRPMotivoVacante == 3)
                {
                    MotivoVacante = "Sustitución de personal";
                }
                else if (requisicion.MRPMotivoVacante == 4)
                {
                    MotivoVacante = "Cubrir incapacidad";
                }
                else if (requisicion.MRPMotivoVacante == 5)
                {
                    MotivoVacante = "Promoción de personal";
                }
                else
                {
                    MotivoVacante = "Motivo no válido";
                }
                var Titulo = "";
                if (requisicion.MRPTituloIndispensable == true)
                {
                    Titulo = "Sí";
                }
                else
                {
                    Titulo = "No";
                }
                var Cedula = "";
                if (requisicion.MRPCedulaIndispensable == true)
                {
                    Cedula = "Si";
                }
                else
                {
                    Cedula = "No";
                }
                var Experiencia = "";
                if (requisicion.MRPExperienciaIndispensable == true)
                {
                    Experiencia = "Si";
                }
                else
                {
                    Experiencia = "No";
                }
                var fecharecepcioncorta = "";

                if (requisicion.MRPFechaRecepcion != null)
                {
                    fecharecepcioncorta = requisicion.MRPFechaRecepcion.Value.ToShortDateString();
                }
                string DiaIngreso = "";
                string MesIngreso = "";
                string AnioIngreso = "";
                if (requisicion.MRPFechaIngreso != null)
                {
                    DiaIngreso = requisicion.MRPFechaIngreso.Value.Day.ToString();
                    MesIngreso = requisicion.MRPFechaIngreso.Value.Month.ToString();
                    AnioIngreso = requisicion.MRPFechaIngreso.Value.Year.ToString();
                }

                if(requisicion.MRPFechaIngreso != null)
                {
                    if (requisicion.MRPFechaIngreso.Value.Day < 10)
                    {
                        DiaIngreso = "0" + DiaIngreso;
                    }
                    if (requisicion.MRPFechaIngreso.Value.Month < 10)
                    {
                        MesIngreso = "0" + MesIngreso;
                    }
                }
                string ingreso = AnioIngreso + "-" + MesIngreso + "-" + DiaIngreso;
                var obj = new
                {
                    Puesto.PuestoDescripcion,
                    Sucursal.SucuNombre,
                    Turno.TurDescripcion,
                    Escolaridades.EscoDescripcion,
                    Sexos.SexDescripcion,
                    TurnoRolado,
                    TipoVacante,
                    MotivoVacante,
                    Titulo,
                    Cedula,
                    Experiencia,
                    requisicion.MRPNumeroVacantes,
                    fe = requisicion.MRPFechaElaboracion.ToShortDateString(),
                    requisicion.MRPTiempoAlimentos,
                    requisicion.MRPMotivoDescripcion,
                    requisicion.MRPEdadMaxima,
                    requisicion.MRPEdadMinima,
                    requisicion.MRPFuncionesPuesto,
                    requisicion.MRPConocimientosPuesto,
                    requisicion.MRPEstatusId,
                    requisicion.MRPId,
                    requisicion.MRPFolio,
                    requisicion.MRPSueldoMensualInicial,
                    requisicion.MRPSueldoMensualPlanta,
                    requisicion.MRPSueldoMensualMasCosto,
                    requisicion.MRPBonoVariable,
                    requisicion.MRPEsquemaContratacion,
                    requisicion.MRPCandidato,
                    requisicion.MRPFechaIngreso,
                    fecharecepcioncorta,
                    ingreso,
                    BanderaPaso
                };
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS
        [Microsoft.AspNetCore.Mvc.HttpPost("AddBitacoraRequisicion")]
        public async Task<IActionResult> AddBitacoraRequisicion(CatBitacoraRequisicionPersonal BRP, int Id, MovRequisicionPersonal MRPSueldos)
        {
            try
            {
                if (BRP != null)
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    var Requisicion = _context.MovRequisicionPersonal.Where(x=> x.MRPId == Id).FirstOrDefault();
                    var Flujo = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == Requisicion.MRPFlujoId).FirstOrDefault();
                    var ListaFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == Flujo.DetFlujoFlujoId).ToList();
                    var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                    DateTime FechaActual = DateTime.Now;
                    
                        var FlujoNuevo = Flujo.DetFlujoOrden + 1;
                        var NeoFlujo = _context.CatDetalleFlujo.Where(x=> x.DetFlujoFlujoId == Flujo.DetFlujoFlujoId && x.DetFlujoOrden == FlujoNuevo).FirstOrDefault();
                        if (FlujoNuevo > ListaFlujos.Count || NeoFlujo==null)
                        {
                            Requisicion.MRPEstatusId = 6;
                            Requisicion.MRPFlujoId = Flujo.DetFlujoId;
                            BRP.BitRPRPId = Requisicion.MRPId;
                            BRP.BitRPDetFlujoId = Flujo.DetFlujoId;
                            BRP.BitRPUserId = user.UserName;
                            BRP.BitRPFecha = FechaActual;
                            BRP.BitRPObservaciones = "Registro cerrado, excepción de pasos.";
                        }
                        else
                        {
                            BRP.BitRPRPId = Requisicion.MRPId;
                            BRP.BitRPDetFlujoId = NeoFlujo.DetFlujoId;
                            BRP.BitRPUserId = user.UserName;
                            BRP.BitRPFecha = FechaActual;
                            Requisicion.MRPFlujoId = NeoFlujo.DetFlujoId;
                            if(Flujo.DetFlujoOrden == 1)
                            {
                                DateTime FechaRecepcion = DateTime.Now;
                                Requisicion.MRPSueldoMensualInicial = MRPSueldos.MRPSueldoMensualInicial;
                                Requisicion.MRPSueldoMensualPlanta = MRPSueldos.MRPSueldoMensualPlanta;
                                Requisicion.MRPSueldoMensualMasCosto = MRPSueldos.MRPSueldoMensualMasCosto;
                                Requisicion.MRPBonoVariable = MRPSueldos.MRPBonoVariable;
                                Requisicion.MRPEsquemaContratacion = MRPSueldos.MRPEsquemaContratacion;
                                Requisicion.MRPFechaIngreso = MRPSueldos.MRPFechaIngreso;
                                Requisicion.MRPFolio = MRPSueldos.MRPFolio;
                                Requisicion.MRPCandidato = MRPSueldos.MRPCandidato;
                                Requisicion.MRPFechaRecepcion = FechaRecepcion;
                            }
                            if (Flujo.DetFlujoOrden == 2)
                            {
                            Requisicion.MRPSueldoMensualInicial = MRPSueldos.MRPSueldoMensualInicial;
                            Requisicion.MRPSueldoMensualPlanta = MRPSueldos.MRPSueldoMensualPlanta;
                            Requisicion.MRPSueldoMensualMasCosto = MRPSueldos.MRPSueldoMensualMasCosto;
                            Requisicion.MRPBonoVariable = MRPSueldos.MRPBonoVariable;
                            Requisicion.MRPEsquemaContratacion = MRPSueldos.MRPEsquemaContratacion;
                            Requisicion.MRPFechaIngreso = MRPSueldos.MRPFechaIngreso;
                            Requisicion.MRPFolio = MRPSueldos.MRPFolio;
                            Requisicion.MRPEstatusId = 10;
                            }
                        }

                        _context.CatBitacoraRequisicionPersonal.Add(BRP);
                        _context.Update(Requisicion);
                        _context.SaveChanges();

                        // Envío de correos

                        if (NeoFlujo != null)
                        {
                            if (NeoFlujo.DetFlujoCorreoDestino == "Dirección")
                            {
                                var Sucursal = _context.CatSucursales.Where(x => x.SucuId == Requisicion.MRPSucursalId).First();
                                var asuntoReenvio = "Estatus de tu requisición de personal. " + Sucursal.SucuNombre + ". Folio interno: " + Requisicion.MRPId;
                                var asuntoDireccion = "Nueva Solicitud de requisición de personal. " + Sucursal.SucuNombre + ". Folio interno: " + Requisicion.MRPId;
                                var FechaCorta = FechaActual.ToShortDateString();
                                var BitacoraRequisicion = _context.CatBitacoraRequisicionPersonal.Where(x => x.BitRPRPId == Requisicion.MRPId).FirstOrDefault();
                                var usuarioRequisicion = _userManager.Users.Where(x => x.UserName == BitacoraRequisicion.BitRPUserId).FirstOrDefault();
                                var Empleados = _context.CatEmpleados.Where(x => x.EmpUserId == usuarioRequisicion.UserName).FirstOrDefault();
                                var puestos = _context.CatPuestos.Where(x => x.PuestoId == Requisicion.MRPPuestoId).FirstOrDefault();
                                var destinatario = usuarioRequisicion.Email;
                                var destinatarioDireccion = NeoFlujo.DetFlujoEmail;
                                var urlGlobal = "192.168.0.5/RequisicionPersonal?";
                                var Bandera = "7teugr"; // Reenvío para detalles 
                                var UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + Requisicion.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                                var bodyBuilder = new BodyBuilder(); // Cuerpo reenvío
                                bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                              <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                              <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                              <th width='15%'>{0}</th> </tr> <tr>
                              <td colspan='3'> <br /> <br /><br />
                              <div style='display:block; text-align:center;'> 
                              <p>¡ {1} tu requisición de personal para el puesto de {3} ya ha sido enviada a Dirección para su revisión!</p>
                              <p>Para consultarla da click en el siguiente botón...</p> <br /><a href=""{2}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleados.EmpNombreCompleto, UrlDetallesRP, puestos.PuestoDescripcion);

                                MailMessage correo = new MailMessage(); // <- Correo mandado para el usuario
                                correo.From = new MailAddress("nucleorh@nucleodediagnostico.com");
                                correo.To.Add(destinatario);
                                correo.Subject = asuntoReenvio;
                                correo.Body = bodyBuilder.HtmlBody;
                                correo.IsBodyHtml = true;
                                correo.Priority = MailPriority.Normal;

                                Bandera = "oudg42ljnrg355fushd"; // Reenvío para dirección
                                UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + Requisicion.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                                var bodyBuilderDireccion = new BodyBuilder(); // Cuerpo para dirección
                                bodyBuilderDireccion.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                              <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                              <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                              <th width='15%'>{0}</th> </tr> <tr>
                              <td colspan='3'> <br /> <br /><br />
                              <div style='display:block; text-align:center;'> 
                              <p>¡ Se ha registrado una nueva requisición de personal a nombre de {1} para el puesto de {3} para su revisión. La misma ya ha sido revisada y aprobada por Capital Humano !</p>
                              <p>Para consultarla da click en el siguiente link...</p> <br /><a href=""{2}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleados.EmpNombreCompleto, UrlDetallesRP, puestos.PuestoDescripcion);

                                MailMessage correoDireccion = new MailMessage(); // <- Correo mandado para CH
                                correoDireccion.From = new MailAddress("nucleorh@nucleodediagnostico.com");
                                correoDireccion.To.Add(destinatarioDireccion);
                                correoDireccion.Subject = asuntoDireccion;
                                correoDireccion.Body = bodyBuilderDireccion.HtmlBody;
                                correoDireccion.IsBodyHtml = true;
                                correoDireccion.Priority = MailPriority.Normal;

                                smtp.Send(correoDireccion);
                                smtp.Send(correo);

                            }
                            else if (NeoFlujo.DetFlujoCorreoDestino == "CapitalHumano")
                            {
                                var Sucursal = _context.CatSucursales.Where(x => x.SucuId == Requisicion.MRPSucursalId).First();
                                var asuntoReenvio = "Estatus de tu requisición de personal. " + Sucursal.SucuNombre + ". Folio interno: " + Requisicion.MRPId;
                                var asuntoCH = "Nueva Solicitud de requisición de personal. " + Sucursal.SucuNombre + ". Folio interno: " + Requisicion.MRPId;
                                var destinatario = user.Email;
                                var destinatarioCH = NeoFlujo.DetFlujoEmail;
                                var FechaCorta = FechaActual.ToShortDateString();
                                var BitacoraRequisicion = _context.CatBitacoraRequisicionPersonal.Where(x => x.BitRPRPId == Requisicion.MRPId).FirstOrDefault();
                                var usuarioRequisicion = _userManager.Users.Where(x => x.UserName == BitacoraRequisicion.BitRPUserId).FirstOrDefault();
                                var Empleados = _context.CatEmpleados.Where(x => x.EmpUserId == usuarioRequisicion.UserName).FirstOrDefault();
                                var UltimaBitacora = _context.CatBitacoraRequisicionPersonal.Where(x => x.BitRPRPId == Requisicion.MRPId).LastOrDefault();
                                var usuarioAproboRequisicion = _context.CatEmpleados.Where(x => x.EmpUserId == UltimaBitacora.BitRPUserId).FirstOrDefault();
                                var puesto = _context.CatPuestos.Where(x => x.PuestoId == Requisicion.MRPPuestoId).FirstOrDefault();
                                var urlGlobal = "192.168.0.5/RequisicionPersonal?";
                                var Bandera = "7teugr"; // Reenvío
                                var UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + Requisicion.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                                var bodyBuilder = new BodyBuilder();
                                bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                              <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                              <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                              <th width='15%'>{0}</th> </tr> <tr>
                              <td colspan='3'> <br /> <br /><br />
                              <div style='display:block; text-align:center;'> 
                              <p>¡ {1} tu requisición de personal para el puesto de {3} ya ha sido enviada de vuelta a Capital Humano para su revisión final!</p>
                              <p>Para consultarla da click en el siguiente botón...</p> <br /><a href=""{2}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleados.EmpNombreCompleto, UrlDetallesRP, puesto.PuestoDescripcion);

                                MailMessage correo = new MailMessage(); // <- Correo mandado para CH
                                correo.From = new MailAddress("nucleorh@nucleodediagnostico.com");
                                correo.To.Add(destinatario);
                                correo.Subject = asuntoReenvio;
                                correo.Body = bodyBuilder.HtmlBody;
                                correo.IsBodyHtml = true;
                                correo.Priority = MailPriority.Normal;

                                Bandera = "9734hyr3u4f30"; // Bandera para CapitalHumano
                                UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + Requisicion.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                                var bodyBuilderCapitalHumano = new BodyBuilder();
                                bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                              <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                              <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                              <th width='15%'>{0}</th> </tr> <tr>
                              <td colspan='3'> <br /> <br /><br />
                              <div style='display:block; text-align:center;'> 
                              <p>¡ Dirección ha aprobado una requisición de personal para el puesto de {3} a nombre de {1} para su revisión. La misma fue aprobada por {3}.</p>
                              <p>Para consultarla da click en el siguiente botón...</p> <br /><a href=""{2}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleados.EmpNombreCompleto, UrlDetallesRP, puesto.PuestoDescripcion);

                                MailMessage correoCH = new MailMessage(); // <- Correo mandado para CH
                                correoCH.From = new MailAddress("nucleorh@nucleodediagnostico.com");
                                correoCH.To.Add(destinatarioCH);
                                correoCH.Subject = asuntoCH;
                                correoCH.Body = bodyBuilderCapitalHumano.HtmlBody;
                                correoCH.IsBodyHtml = true;
                                correoCH.Priority = MailPriority.Normal;

                                smtp.Send(correoCH);
                                smtp.Send(correo);
                            }
                        }
                    
                    
                    return Ok(BRP);
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

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS
        [Microsoft.AspNetCore.Mvc.HttpPost("RechazarRequisicion")]
        public async Task<IActionResult> RechazarRequisicionAsync(CatBitacoraRequisicionPersonal BRP, int Id, int EstatusId)
        {
            try
            {
                if (BRP != null)
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    var Requisicion = _context.MovRequisicionPersonal.Where(x => x.MRPId == Id).FirstOrDefault();
                    var Flujo = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == Requisicion.MRPFlujoId).FirstOrDefault();
                    var ListaFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == Flujo.DetFlujoFlujoId).ToList();
                    var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                    DateTime FechaActual = DateTime.Now;
                    var Sucursal = _context.CatSucursales.Where(x => x.SucuId == Requisicion.MRPSucursalId).First();
                    var asuntoReenvio = "Tu requisición de personal ha sufrido un cambio de estatus. " + Sucursal.SucuNombre + ". Folio interno: " + Requisicion.MRPId;

                    
                        BRP.BitRPRPId = Id;
                        BRP.BitRPDetFlujoId = Requisicion.MRPFlujoId;
                        BRP.BitRPUserId = user.UserName;
                        BRP.BitRPFecha = FechaActual;
                        Requisicion.MRPEstatusId = 5;

                        _context.CatBitacoraRequisicionPersonal.Add(BRP);
                        _context.Update(Requisicion);
                        _context.SaveChanges();

                        var x = _context.CatBitacoraRequisicionPersonal.Where(x => x.BitRPId == BRP.BitRPId).First();

                        var BitacoraRequisicion = _context.CatBitacoraRequisicionPersonal.Where(x => x.BitRPRPId == Requisicion.MRPId).FirstOrDefault();
                        var usuarioRequisicion = _userManager.Users.Where(x => x.UserName == BitacoraRequisicion.BitRPUserId).FirstOrDefault();
                        var Empleados = _context.CatEmpleados.Where(x => x.EmpUserId == usuarioRequisicion.UserName).FirstOrDefault();
                        var FechaCorta = FechaActual.ToShortDateString();
                        var EmpleadoCierraIncidencia = _context.CatEmpleados.Where(x => x.EmpUserId == user.UserName).FirstOrDefault();
                        var puesto = _context.CatPuestos.Where(x => x.PuestoId == Requisicion.MRPPuestoId).FirstOrDefault();
                        var destinatario = usuarioRequisicion.Email;
                        var urlGlobal = "192.168.0.5/RequisicionPersonal?";
                        var Bandera = "7teugr"; // Reenvío para reenvío 
                        var UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + Requisicion.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                        var bodyBuilderRemitente = new BodyBuilder();
                        bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} tu requisición de personal para el puesto de {5} ha sido cerrada por {2}, por el siguiente motivo: {4}!</p>
                <p>Para consultarla da click en el siguiente link...</p> <br /><a href=""{3}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleados.EmpNombreCompleto, EmpleadoCierraIncidencia.EmpNombreCompleto, UrlDetallesRP, BRP.BitRPObservaciones, puesto.PuestoDescripcion);

                        MailMessage correo = new MailMessage();
                        correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                        correo.To.Add(destinatario);
                        correo.Subject = asuntoReenvio;
                        correo.Body = bodyBuilderRemitente.HtmlBody;
                        correo.IsBodyHtml = true;
                        correo.Priority = MailPriority.Normal;

                        smtp.Send(correo);
                    

                    _context.CatBitacoraRequisicionPersonal.Add(BRP);
                    _context.SaveChanges();
                    
                    return Ok(BRP);
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

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS
        [Microsoft.AspNetCore.Mvc.HttpPost("RegresarRequisicion")]
        public async Task<IActionResult> RegresarRequisicionAsync(CatBitacoraRequisicionPersonal BRP, int Id)
        {
            try
            {
                if (BRP != null)
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    var Requisicion = _context.MovRequisicionPersonal.Where(x => x.MRPId == Id).FirstOrDefault();
                    var Flujo = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == Requisicion.MRPFlujoId).FirstOrDefault();
                    var ListaFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == Flujo.DetFlujoFlujoId).ToList();
                    var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                    DateTime FechaActual = DateTime.Now;
                    var Sucursal = _context.CatSucursales.Where(x => x.SucuId == Requisicion.MRPSucursalId).First();
                    var asuntoReenvio = "Tu requisición de personal ha sufrido un cambio de estatus. " + Sucursal.SucuNombre + ". Folio interno: " + Requisicion.MRPId;

                   // Estatus 9 de regresa
                        BRP.BitRPRPId = Id;
                        BRP.BitRPDetFlujoId = Requisicion.MRPFlujoId;
                        BRP.BitRPUserId = user.UserName;
                        BRP.BitRPFecha = FechaActual;

                        var FlujoActual = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == Requisicion.MRPFlujoId).FirstOrDefault();
                        var NuevoFlujo = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == FlujoActual.DetFlujoFlujoId && x.DetFlujoOrden == FlujoActual.DetFlujoOrden - 1).FirstOrDefault();

                        Requisicion.MRPEstatusId = 9;
                        Requisicion.MRPFlujoId = NuevoFlujo.DetFlujoId;

                        _context.CatBitacoraRequisicionPersonal.Add(BRP);
                        _context.Update(Requisicion);
                        _context.SaveChanges();

                        var BitacoraRequisicion = _context.CatBitacoraRequisicionPersonal.Where(x => x.BitRPRPId == Requisicion.MRPId).FirstOrDefault();
                        var usuarioRequisicion = _userManager.Users.Where(x => x.UserName == BitacoraRequisicion.BitRPUserId).FirstOrDefault();
                        var Empleados = _context.CatEmpleados.Where(x => x.EmpUserId == usuarioRequisicion.UserName).FirstOrDefault();
                        var FechaCorta = FechaActual.ToShortDateString();
                        var EmpleadoCierraIncidencia = _context.CatEmpleados.Where(x => x.EmpUserId == user.UserName).FirstOrDefault();
                        var puesto = _context.CatPuestos.Where(x => x.PuestoId == Requisicion.MRPPuestoId).FirstOrDefault();
                        var destinatario = usuarioRequisicion.Email;
                        var urlGlobal = "192.168.0.5/RequisicionPersonal?";
                        var Bandera = "7teugr"; // Reenvío para reenvío 
                        var FlujoRegreso = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == BRP.BitRPDetFlujoId).FirstOrDefault();
                        if (FlujoRegreso != null)
                        {
                            if (FlujoRegreso.DetFlujoCorreoDestino == "CapitalHumano")
                            {
                                Bandera = "3l2kjh50c98vx";// Validación para Modificar los primeros apartados de la requisición
                            }
                            else if (FlujoRegreso.DetFlujoCorreoDestino == "Dirección")
                            {
                                Bandera = "g8ychfuhv"; // Validación para modificar el apartado de salarios que registra capital humano
                            }
                            else
                            {
                                Bandera = ""; // Si hay una excepción, no se ejecutará nada y redirigirá a la ventana principal
                            }
                        }
                        var UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + Requisicion.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                        var bodyBuilderRemitente = new BodyBuilder();
                        bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} tu requisición de personal para el puesto de {5} ha sido regresada a revisión por {2}, por el siguiente motivo: {4}!</p>
                <p>Para consultarla da click en el siguiente link...</p> <br /><a href=""{3}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleados.EmpNombreCompleto, EmpleadoCierraIncidencia.EmpNombreCompleto, UrlDetallesRP, BRP.BitRPObservaciones, puesto.PuestoDescripcion);

                        MailMessage correo = new MailMessage();
                        correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                        correo.To.Add(destinatario);
                        correo.Subject = asuntoReenvio;
                        correo.Body = bodyBuilderRemitente.HtmlBody;
                        correo.IsBodyHtml = true;
                        correo.Priority = MailPriority.Normal;

                        smtp.Send(correo);
                    

                    _context.CatBitacoraRequisicionPersonal.Add(BRP);
                    _context.SaveChanges();

                    return Ok(BRP);
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

        [Microsoft.AspNetCore.Mvc.HttpGet("ValidacionRequisicionById/{Id}/{IdUsuario}/{Bandera}")]
        public async Task<IActionResult> ValidacionIncidenciaByIdAsync(int Id, string IdUsuario, string Bandera)
        {
            try
            {
                string Flag = "";
                bool RedFlag = false;
                var Requisicion = _context.MovRequisicionPersonal.Where(x => x.MRPId == Id).FirstOrDefault();
                if(Requisicion == null)
                {
                    return BadRequest();
                }
                var BitacoraRequisicion = _context.CatBitacoraRequisicionPersonal.Where(x => x.BitRPUserId == IdUsuario && x.BitRPRPId == Id).ToList();
                if(BitacoraRequisicion == null)
                {
                    return BadRequest();
                }

                var Flujo = _context.CatDetalleFlujo.Where(x=> x.DetFlujoId == Requisicion.MRPFlujoId).FirstOrDefault();
                var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                var userrole = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
                if (Flujo.DetFlujoOrden == 1)
                {
                    var ValidacionPerfil = userrole.Where(x => x.RoleId == "1" || x.RoleId == "2" || x.RoleId == "3" || x.RoleId == "8").FirstOrDefault();
                    if (ValidacionPerfil == null)
                    {
                        return BadRequest();
                    }
                }
                else if (Flujo.DetFlujoOrden == 2)
                {
                    var ValidacionPerfil = userrole.Where(x => x.RoleId == "1" || x.RoleId == "2").FirstOrDefault();
                    if (ValidacionPerfil == null)
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
                
                if(Bandera == "7teugr")// OpenModalDetails
                {
                    Flag = "Detalles";
                    RedFlag = true;
                }else if(Bandera == "B1qazfg4wsx3edc" && Flujo.DetFlujoOrden == 1) // OpenModalEdit
                {
                    Flag = Bandera;
                    RedFlag = true;
                }
                else if(Bandera == "3l2kjh50c98vx") // OpenModalEditDetails Not Sueldo
                {
                    Flag = Bandera;
                    RedFlag = true;
                } else if(Bandera == "3l2kjh50c98vx") // OpenModalEditDetails With Sueldo
                {
                    Flag = Bandera;
                    RedFlag = true;
                }else if (Bandera == "oudg42ljnrg355fushd" && Flujo.DetFlujoOrden == 2)
                {

                }else if (Bandera == "9734hyr3u4f30" && Flujo.DetFlujoOrden == 3)
                {
                    Flag = Bandera;
                    RedFlag = true;
                }
                else
                {
                    return BadRequest();
                }

                var obj = new
                {
                    Flag,
                    RedFlag
                };
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("RequisicionEditarDetallesById/{Id}")]
        public async Task<IActionResult> RequisicionEditarDetallesByIdAsync(int Id)
        {
            try
            {
                var requisicion = _context.MovRequisicionPersonal.Where(x => x.MRPId == Id).First();
                var DatosRequisicion = _context.MovRequisicionPersonal.Where(x => x.MRPId == Id).First();
                bool Bandera = false;
                var FlujoActual = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == DatosRequisicion.MRPFlujoId).First();
                var BanderaPaso1 = false;
                var BanderaPaso2= false;
                var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                var IsInRoleCH = _context.UserRoles.Where(x => x.UserId == user.Id && x.RoleId == "8").FirstOrDefault();
                
                if(IsInRoleCH != null &&  FlujoActual.DetFlujoOrden == 1 && requisicion.MRPFolio != null)
                {
                    BanderaPaso2 = true;
                }
                if (FlujoActual.DetFlujoOrden != 1)
                {
                    Bandera = true;
                }
                if(FlujoActual.DetFlujoOrden == 1)
                {
                    BanderaPaso1 = true;
                }
                var obj = new
                {
                    requisicion.MRPId,
                    requisicion.MRPNumeroVacantes,
                    requisicion.MRPSucursalId,
                    requisicion.MRPFechaElaboracion,
                    requisicion.MRPFechaRecepcion,
                    requisicion.MRPFolio,
                    requisicion.MRPPuestoId,
                    requisicion.MRPTipoVacante,
                    requisicion.MRPTurnoId,
                    requisicion.MRPRolarTurno,
                    requisicion.MRPTiempoAlimentos,
                    requisicion.MRPMotivoVacante,
                    requisicion.MRPMotivoDescripcion,
                    requisicion.MRPSexoId,
                    requisicion.MRPEdadMinima,
                    requisicion.MRPEdadMaxima,
                    requisicion.MRPEscolaridadId,
                    requisicion.MRPTituloIndispensable,
                    requisicion.MRPCedulaIndispensable,
                    requisicion.MRPExperienciaIndispensable,
                    requisicion.MRPFuncionesPuesto,
                    requisicion.MRPConocimientosPuesto,
                    requisicion.MRPSueldoMensualInicial,
                    requisicion.MRPSueldoMensualPlanta,
                    requisicion.MRPSueldoMensualMasCosto,
                    requisicion.MRPBonoVariable,
                    requisicion.MRPCandidato,
                    requisicion.MRPEsquemaContratacion,
                    requisicion.MRPFechaIngreso,
                    requisicion.MRPEstatusId,
                    Bandera,
                    BanderaPaso2,
                    BanderaPaso1,
                    FlujoActual.DetFlujoOrden
                };
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("EditDetallesRequisicion")]
        public async Task<IActionResult> EditDetallesRequisicion(MovRequisicionPersonal MRP, int Id, CatBitacoraRequisicionPersonal BRP)
        {
            try
            {
                var x = _context.MovRequisicionPersonal.Where(x => x.MRPId == Id).First();
                if (MRP != null)
                {
                    var FlujoActual = _context.CatDetalleFlujo.Where(y => y.DetFlujoId == x.MRPFlujoId).First();
                    string Observaciones = "";

                    if(FlujoActual.DetFlujoOrden == 1) // Solo detalles pertenecientes a la creación
                    {
                        if(x.MRPNumeroVacantes != MRP.MRPNumeroVacantes)
                        {
                            Observaciones = Observaciones + "Se modificó el número de vacantes, antes era " + x.MRPNumeroVacantes + " y ahora es: " + MRP.MRPNumeroVacantes + ".";
                        }
                        if(x.MRPSucursalId != MRP.MRPSucursalId)
                        {
                            Observaciones = Observaciones + " Se ha modificado la sucursal.";
                        }
                        if (x.MRPPuestoId != MRP.MRPPuestoId)
                        {
                            Observaciones = Observaciones + " Se ha modificado el puesto.";
                        }
                        if (x.MRPTipoVacante != MRP.MRPTipoVacante)
                        {
                            Observaciones = Observaciones + " Se ha modificado el tipo de vacante.";
                        }
                        if (x.MRPTurnoId != MRP.MRPTurnoId)
                        {
                            Observaciones = Observaciones + " Se ha modificado el tipo de turno.";
                        }
                        if (x.MRPRolarTurno != MRP.MRPRolarTurno)
                        {
                            Observaciones = Observaciones + " Se ha modificado el rolar turnos";
                        }
                        if (x.MRPTiempoAlimentos != MRP.MRPTiempoAlimentos)
                        {
                            Observaciones = Observaciones + " Se ha modificado el tiempo de alimentos. Antes era " + x.MRPTiempoAlimentos + " y ahora es " + MRP.MRPTiempoAlimentos + ".";
                        }
                        if (x.MRPMotivoVacante != MRP.MRPMotivoVacante)
                        {
                            Observaciones = Observaciones + " Se ha modificado el motivo de la vacante";
                        }
                        if (x.MRPMotivoDescripcion != MRP.MRPMotivoDescripcion) 
                        {
                            Observaciones = Observaciones + " Se ha modificado la descripción de la vacante. Antes era " + x.MRPMotivoDescripcion + " y ahora es " + MRP.MRPMotivoDescripcion + ".";
                        }
                        if (x.MRPSexoId != MRP.MRPSexoId)
                        {
                            Observaciones = Observaciones + " Se ha modificado el sexo requerido para el puesto";
                        }
                        if (x.MRPEdadMaxima != MRP.MRPEdadMaxima)
                        {
                            Observaciones = Observaciones + " Se ha modificado la edad máxima. Antes era " + x.MRPEdadMaxima + " y ahora es " + MRP.MRPEdadMaxima + ".";
                        }
                        if (x.MRPEdadMinima != MRP.MRPEdadMinima)
                        {
                            Observaciones = Observaciones + " Se ha modificado la edad máxima. Antes era " + x.MRPEdadMinima + " y ahora es " + MRP.MRPEdadMinima + ".";
                        }
                        if (x.MRPEscolaridadId != MRP.MRPEscolaridadId)
                        {
                            Observaciones = Observaciones + " Se ha modifcado el grado de escolaridad requerido.";
                        }
                        if (x.MRPTituloIndispensable != MRP.MRPTituloIndispensable)
                        {
                            Observaciones = Observaciones + " Se ha modificado el que el título sea indispensable.";
                        }
                        if (x.MRPCedulaIndispensable != MRP.MRPCedulaIndispensable)
                        {
                            Observaciones = Observaciones + " Se ha modificado el que la cédula profesional sea indispensable.";
                        }
                        if (x.MRPExperienciaIndispensable != MRP.MRPExperienciaIndispensable)
                        {
                            Observaciones = Observaciones + " Se ha modificado el que sea necesaria experiencia previa.";
                        }
                        if (x.MRPFuncionesPuesto != MRP.MRPFuncionesPuesto)
                        {
                            Observaciones = Observaciones + " Se han modificado las funciones del puesto, siendo antes " + x.MRPFuncionesPuesto + " y ahora son " + MRP.MRPFuncionesPuesto;
                        }
                        if (x.MRPConocimientosPuesto != MRP.MRPConocimientosPuesto)
                        {
                            Observaciones = Observaciones + " Se han modificado los conocimientos del puesto, siendo antes " + x.MRPConocimientosPuesto + " y ahora son " + MRP.MRPConocimientosPuesto;
                        }
                        x.MRPNumeroVacantes = MRP.MRPNumeroVacantes;
                        x.MRPSucursalId = MRP.MRPSucursalId; 
                        x.MRPPuestoId = MRP.MRPPuestoId;  
                        x.MRPTipoVacante = MRP.MRPTipoVacante;
                        x.MRPTurnoId = MRP.MRPTurnoId; 
                        x.MRPRolarTurno = MRP.MRPRolarTurno; 
                        x.MRPTiempoAlimentos = MRP.MRPTiempoAlimentos; 
                        x.MRPMotivoVacante = MRP.MRPMotivoVacante; 
                        x.MRPMotivoDescripcion = MRP.MRPMotivoDescripcion; 
                        x.MRPSexoId = MRP.MRPSexoId; 
                        x.MRPEdadMaxima = MRP.MRPEdadMaxima; 
                        x.MRPEdadMinima = MRP.MRPEdadMinima; 
                        x.MRPEscolaridadId = MRP.MRPEscolaridadId; 
                        x.MRPTituloIndispensable = MRP.MRPTituloIndispensable;
                        x.MRPCedulaIndispensable = MRP.MRPCedulaIndispensable;
                        x.MRPExperienciaIndispensable = MRP.MRPExperienciaIndispensable;
                        x.MRPFuncionesPuesto = MRP.MRPFuncionesPuesto;
                        x.MRPConocimientosPuesto = MRP.MRPConocimientosPuesto;
                        x.MRPEstatusId = 3;

                    }else if(FlujoActual.DetFlujoOrden == 2) // Solo detalles pertenecientes a los sueldos de CH
                    {
                        if (x.MRPSueldoMensualInicial != MRP.MRPSueldoMensualInicial)
                        {
                            Observaciones = Observaciones + " Se ha modificado el sueldo inicial, siendo antes " + x.MRPSueldoMensualInicial + " y ahora es " + MRP.MRPSueldoMensualInicial;
                        }
                        if (x.MRPSueldoMensualPlanta != MRP.MRPSueldoMensualPlanta)
                        {
                            Observaciones = Observaciones + " Se ha modificado los conocimientos del puesto, siendo antes " + x.MRPSueldoMensualPlanta + " y ahora es " + MRP.MRPSueldoMensualPlanta;
                        }
                        if (x.MRPSueldoMensualMasCosto != MRP.MRPSueldoMensualMasCosto)
                        {
                            Observaciones = Observaciones + " Se ha modificado los conocimientos del puesto, siendo antes " + x.MRPSueldoMensualMasCosto + " y ahora es " + MRP.MRPSueldoMensualMasCosto;
                        }
                        if (x.MRPBonoVariable != MRP.MRPBonoVariable)
                        {
                            Observaciones = Observaciones + " Se ha modificado los conocimientos del puesto, siendo antes " + x.MRPBonoVariable + " y ahora es " + MRP.MRPBonoVariable;
                        }
                        if (x.MRPEsquemaContratacion != MRP.MRPEsquemaContratacion)
                        {
                            Observaciones = Observaciones + " Se ha modificado los conocimientos del puesto, siendo antes " + x.MRPEsquemaContratacion + " y ahora es " + MRP.MRPEsquemaContratacion;
                        }
                        if (x.MRPCandidato != MRP.MRPCandidato)
                        {
                            Observaciones = Observaciones + " Se ha modificado los conocimientos del puesto, siendo antes " + x.MRPCandidato + " y ahora es " + MRP.MRPCandidato;
                        }
                        if (x.MRPFechaIngreso != MRP.MRPFechaIngreso)
                        {
                            Observaciones = Observaciones + " Se ha modificado los conocimientos del puesto, siendo antes " + x.MRPFechaIngreso + " y ahora es " + MRP.MRPFechaIngreso;
                        }

                        x.MRPSueldoMensualInicial = MRP.MRPSueldoMensualInicial;
                        x.MRPSueldoMensualPlanta = MRP.MRPSueldoMensualPlanta;
                        x.MRPSueldoMensualMasCosto = MRP.MRPSueldoMensualMasCosto;
                        x.MRPBonoVariable = MRP.MRPBonoVariable;
                        x.MRPEsquemaContratacion = MRP.MRPEsquemaContratacion;
                        x.MRPCandidato = MRP.MRPCandidato;
                        x.MRPFechaIngreso = MRP.MRPFechaIngreso;
                        x.MRPEstatusId = 3;
                    }
                    else // Por si alguien se pasa de listo y quiere editar algo que no va
                    {
                        return BadRequest();
                    }

                    var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                    DateTime Fecha = DateTime.Now;

                    BRP.BitRPUserId = user.UserName;
                    BRP.BitRPFecha = Fecha;
                    BRP.BitRPDetFlujoId = x.MRPFlujoId;
                    BRP.BitRPObservaciones = Observaciones;
                    BRP.BitRPRPId = x.MRPId;

                    _context.CatBitacoraRequisicionPersonal.Add(BRP);
                    _context.Update(x);
                    _context.SaveChanges();

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    var BitacoraRequisicion = _context.CatBitacoraRequisicionPersonal.Where(y => y.BitRPRPId == x.MRPId).FirstOrDefault();
                    var usuarioRequisicion = _userManager.Users.Where(y => y.UserName == x.MRPUserId).FirstOrDefault();
                    var Empleados = _context.CatEmpleados.Where(x => x.EmpUserId == usuarioRequisicion.UserName).FirstOrDefault();
                    var Puesto = _context.CatPuestos.Where(y => y.PuestoId == x.MRPPuestoId).First();
                    var FechaCorta = Fecha.ToShortDateString();
                    var destinatario = "";
                    var asunto = "";
                    var FlujoRequisicion = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == 18).ToList();
                    var CH = FlujoRequisicion.Where(x => x.DetFlujoCorreoDestino == "CapitalHumano").First();
                    var Dir = FlujoRequisicion.Where(x => x.DetFlujoCorreoDestino == "Dirección").First();
                    var CapitalHumano = CH.DetFlujoEmail;
                    var Direccion = Dir.DetFlujoEmail;
                    var FlujoPresente = _context.CatDetalleFlujo.Where(y => y.DetFlujoId == x.MRPFlujoId).First();
                    if(FlujoPresente.DetFlujoCorreoDestino == "CapitalHumano")
                    {
                        asunto = "La requisición de personal del puesto " + Puesto.PuestoDescripcion + " ha sido modificada para su nueva revisión";
                        destinatario = CapitalHumano;
                    }else if(FlujoPresente.DetFlujoCorreoDestino == "Dirección")
                    {
                        asunto = "La requisición de personal del puesto " + Puesto.PuestoDescripcion + " ha sido modificada por capital humano para su revisión";
                        destinatario=Direccion;
                    }
                    else
                    {
                        return BadRequest();
                    }

                    var urlGlobal = "https://localhost:44392/RequisicionPersonal?";
                    //var urlGlobal = "192.168.0.5/RequisicionPersonal?";
                    var Bandera = "";  // Reenvío para reenvío 
                    string Reenvio = "7teugr";
                    string EnvioACHODireccion = "B1qazfg4wsx3edc";
                    Bandera = EnvioACHODireccion;
                    var UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + x.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ La requisición de personal de {1} para el puesto de {4} ha sido modificada y enviada a revisión. Los cambios fueron los siguientes: {3}!</p>
                <p>Para consultarla da click en el siguiente link...</p> <br /><a href=""{2}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleados.EmpNombreCompleto, UrlDetallesRP, Observaciones, Puesto.PuestoDescripcion);

                    Bandera = Reenvio;
                    UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + x.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                    var bodyBuilderRemitente = new BodyBuilder();
                    bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ {1} tu requisición de personal para el puesto de {4} ha sido modificada y enviada de nuevo a su revisión. Las modificaciones que se hicieron fueron las siguientes: {3} !</p>
                <p>Para consultarla da click en el siguiente link...</p> <br /><a href=""{2}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleados.EmpNombreCompleto,  UrlDetallesRP, Observaciones, Puesto.PuestoDescripcion);

                    MailMessage correoFlujo = new MailMessage();
                    correoFlujo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                    correoFlujo.To.Add(destinatario);
                    correoFlujo.Subject = asunto;
                    correoFlujo.Body = bodyBuilder.HtmlBody;
                    correoFlujo.IsBodyHtml = true;
                    correoFlujo.Priority = MailPriority.Normal;

                    MailMessage correoRemitente = new MailMessage();
                    correoRemitente.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                    correoRemitente.To.Add(destinatario);
                    correoRemitente.Subject = asunto;
                    correoRemitente.Body = bodyBuilder.HtmlBody;
                    correoRemitente.IsBodyHtml = true;
                    correoRemitente.Priority = MailPriority.Normal;

                    smtp.Send(correoFlujo);
                    smtp.Send(correoRemitente);

                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("GenerarPdf/{Id}")]
        public IActionResult GenerarPdf(int Id)
        {
            try
            {
                bool Estatus = false;
                var Requisicion = _context.MovRequisicionPersonal.Where(x=> x.MRPId == Id).FirstOrDefault();
                var sucu = "";
                var pues = "";

                if(Requisicion != null)
                {
                    if (Directory.Exists("C:\\FormatosRequisicionPersonal")==false)
                    {
                        Directory.CreateDirectory("C:\\FormatosRequisicionPersonal");
                    }
                    DateTime Hoy = DateTime.Now;
                    string TipoVacante = "";
                    string MotivoVacante = "";
                    string Sexo = "";
                    string Puesto = "";
                    string Sucursal = "";
                    string Escolaridad = "";
                    string Titulo = "";
                    string Cedula = "";
                    string Experiencia = "";
                    string Esquema = "";
                    string Empleado = "";
                    string RolarTurno = "";
                    string MotivoDescripción = "";
                    string DescripcionMotivo = "";

                    var Sexos = _context.CatSexos.Where(x => x.SexId == Requisicion.MRPSexoId).FirstOrDefault();
                    var Puestos = _context.CatPuestos.Where(x=> x.PuestoId == Requisicion.MRPPuestoId).FirstOrDefault();
                    var Sucursales = _context.CatSucursales.Where(x => x.SucuId == Requisicion.MRPSucursalId).FirstOrDefault();
                    var Escolaridades = _context.CatEscolaridades.Where(x => x.EscoId == Requisicion.MRPEscolaridadId).FirstOrDefault();
                    var Empleados = _context.CatEmpleados.Where(x => x.EmpId == Requisicion.MRPEmpId).FirstOrDefault();
                    var Turnos = _context.CatTurnosLaborales.Where(x => x.TurId == Requisicion.MRPTurnoId).FirstOrDefault();
                    
                    if (Requisicion.MRPTipoVacante == 1)
                    {
                        TipoVacante = "Eventual";
                    }else if (Requisicion.MRPTipoVacante == 2)
                    {
                        TipoVacante = "Permanente";
                    }else if(Requisicion.MRPTipoVacante== 3)
                    {
                        TipoVacante = "Cubre incidencias";
                    }
                    else
                    {
                        TipoVacante = "";
                    }
                    if (Requisicion.MRPMotivoVacante == 1)
                    {
                        MotivoVacante = "Nueva creación";
                        MotivoDescripción = "Descripción: ";
                        DescripcionMotivo = Requisicion.MRPMotivoDescripcion;
                    }else if(Requisicion.MRPMotivoVacante== 2)
                    {
                        MotivoVacante = "Vacaciones";
                        MotivoDescripción = "Cubre a: ";
                        DescripcionMotivo = Empleados.EmpNombreCompleto;
                    }else if(Requisicion.MRPMotivoVacante == 3)
                    {
                        MotivoVacante = "Sustitución de personal";
                        MotivoDescripción = "Baja de: ";
                        DescripcionMotivo = Empleados.EmpNombreCompleto;
                    }else if(Requisicion.MRPMotivoVacante== 4)
                    {
                        MotivoVacante = "Cubrir incapacidad";
                        MotivoDescripción = "Cubre a :";
                        DescripcionMotivo = Empleados.EmpNombreCompleto;
                    }
                    else if(Requisicion.MRPMotivoVacante == 5)
                    {
                        MotivoVacante = "Promoción de personal";
                        MotivoDescripción = "De: ";
                        DescripcionMotivo = Empleados.EmpNombreCompleto;
                    }
                    else {
                        MotivoVacante = "";
                    }
                    if(Requisicion.MRPTituloIndispensable == true)
                    {
                        Titulo = "Sí";
                    }
                    else
                    {
                        Titulo = "No";
                    }
                    if(Requisicion.MRPCedulaIndispensable == true)
                    {
                        Cedula = "Sí";
                    }
                    else
                    {
                        Cedula = "No";
                    }
                    if(Requisicion.MRPExperienciaIndispensable == true)
                    {
                        Experiencia = "Sí";
                    }
                    else
                    {
                        Experiencia = "No";
                    }
                    
                    if(Requisicion.MRPEsquemaContratacion == 1)
                    {
                        Esquema = "Nómina";
                    }
                    else if(Requisicion.MRPEsquemaContratacion == 2)
                    {
                        Esquema = "Honorarios";
                    }
                    else
                    {
                    }
                    if(Requisicion.MRPRolarTurno == true)
                    {
                        RolarTurno = "Sí";
                    }
                    else
                    {
                        RolarTurno = "No";
                    }
                    Sexo = Sexos.SexDescripcion;
                    Puesto = Puestos.PuestoDescripcion;
                    Sucursal = Sucursales.SucuNombre;
                    Escolaridad = Escolaridades.EscoDescripcion;
                    Empleado = Empleados.EmpNombreCompleto;
                    using (FileStream fs = new FileStream("C:\\FormatosRequisicionPersonal\\RequisicionPersonal" + Requisicion.MRPFolio + "_" + Sucursal + "_" + Puesto + ".pdf", FileMode.Create))
                    {
                        Document doc = new Document(PageSize.LETTER, 18.3465f, 18.3465f, 25.3465f, 18.3465f);
                        PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                        doc.AddAuthor("Sistemas, Núcleo de Diagnóstico");
                        doc.AddCreator("Núcleo de diagnóstico");
                        doc.AddKeywords("Sistemas ND");
                        doc.AddTitle("Requisición de personal");

                        doc.Open();

                        PdfPTable Encabezado = new PdfPTable(3);
                        Encabezado.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        Encabezado.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Encabezado.WidthPercentage = 100;
                        float[] EncabezadoWidth = { 25f, 50f, 25f };
                        Encabezado.SetWidths(EncabezadoWidth);

                        var fontTitulo = FontFactory.GetFont("Arial", size: 17, iTextSharp.text.Font.BOLD);
                        fontTitulo.SetColor(1, 38, 57);
                        var fontRestoEncabezado = FontFactory.GetFont("Arial", size: 7);
                        fontRestoEncabezado.SetColor(1, 38, 57);

                        BaseFont Light = BaseFont.CreateFont(@"https://files.nucleodediagnostico.mx/fonts/WorkSans-Light.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                        BaseFont Labels = BaseFont.CreateFont(@"https://files.nucleodediagnostico.mx/fonts/WorkSans-SemiBold.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                        BaseFont Titulos = BaseFont.CreateFont(@"https://files.nucleodediagnostico.mx/fonts/WorkSans-Black.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                        BaseFont Datos = BaseFont.CreateFont(@"https://files.nucleodediagnostico.mx/fonts/WorkSans-Medium.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);

                        iTextSharp.text.Font FontLight = new iTextSharp.text.Font(Light, size: 9);
                        FontLight.SetColor(1, 38, 57);
                        iTextSharp.text.Font FontLabels = new iTextSharp.text.Font(Labels, size: 10);
                        FontLabels.SetColor(1, 38, 57);
                        iTextSharp.text.Font FontTitulos = new iTextSharp.text.Font(Titulos, size: 18);
                        FontTitulos.SetColor(1, 38, 57);
                        iTextSharp.text.Font FontDatos = new iTextSharp.text.Font(Datos, size: 10);
                        FontDatos.SetColor(1, 38, 57);
                        iTextSharp.text.Font FontLabelsWhite = new iTextSharp.text.Font(Labels, size: 10);
                        FontLabelsWhite.SetColor(255, 255, 255);

                        iTextSharp.text.Image Logo = iTextSharp.text.Image.GetInstance(new Uri("https://files.nucleodediagnostico.mx/img/ND_LOGO.png")); // Cargamos el logo desde una URL
                        float percentage = 0.0f;
                        percentage = 90 / Logo.Width;
                        Logo.ScalePercent(percentage * 100);

                        Encabezado.AddCell(new PdfPCell(Logo) { Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        Encabezado.AddCell(new PdfPCell(new Phrase("Código: CAP-NDG-REG-006" + "\n\n" + "Emisión: " + Hoy.ToShortDateString(), FontLight)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        Encabezado.AddCell(new PdfPCell(new Phrase("Requisición \n de Personal", FontTitulos)) { Rowspan = 2, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_BOTTOM, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        Encabezado.AddCell(new PdfPCell(new Phrase("Folio: " + Requisicion.MRPFolio.ToString(), FontLight)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM, Border = iTextSharp.text.Rectangle.NO_BORDER });

                        //Encabezado.AddCell(new PdfPCell(new Phrase("Rev.02", FontLight)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        //Encabezado.AddCell(new PdfPCell(new Phrase("Página 1 de 1", FontLight)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = iTextSharp.text.Rectangle.NO_BORDER });

                        doc.Add(Encabezado);

                        iTextSharp.text.Phrase SaltoLinea = new Phrase();
                        SaltoLinea.Add("\n");
                        doc.Add(SaltoLinea);

                        PdfPTable TablaFechasFolio = new PdfPTable(6);
                        TablaFechasFolio.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        TablaFechasFolio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        TablaFechasFolio.WidthPercentage = 100;
                        float[] FechasFolioWidth = { 20f, 20f, 16f, 16f, 12f, 12f };
                        TablaFechasFolio.SetWidths(FechasFolioWidth);

                        TablaFechasFolio.AddCell(new PdfPCell(new Phrase("", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        TablaFechasFolio.AddCell(new PdfPCell(new Phrase("", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        TablaFechasFolio.AddCell(new PdfPCell(new Phrase("Fecha de elaboración: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        TablaFechasFolio.AddCell(new PdfPCell(new Phrase(Requisicion.MRPFechaElaboracion.ToShortDateString(), FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        TablaFechasFolio.AddCell(new PdfPCell(new Phrase("Fecha de recepción: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        TablaFechasFolio.AddCell(new PdfPCell(new Phrase(Requisicion.MRPFechaRecepcion.Value.ToShortDateString(), FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BOTTOM, Border = iTextSharp.text.Rectangle.NO_BORDER });

                        doc.Add(TablaFechasFolio);

                        PdfPTable SaltoLínea = new PdfPTable(1);
                        SaltoLínea.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        SaltoLínea.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        SaltoLínea.WidthPercentage = 100;
                        float[] SaltoLíneaWidth = { 100 };
                        SaltoLínea.SetWidths(SaltoLíneaWidth);

                        SaltoLínea.AddCell(new PdfPCell(new Phrase("\n", FontLight)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER, PaddingTop = 0 });

                        doc.Add(SaltoLínea);

                        PdfPTable DatosGenerales = new PdfPTable(1);
                        DatosGenerales.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        DatosGenerales.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        DatosGenerales.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(0, 125, 196);
                        DatosGenerales.DefaultCell.PaddingBottom = 5f;
                        DatosGenerales.WidthPercentage = 100;
                        float[] DatosGeneralesWidth = { 100 };
                        DatosGenerales.SetWidths(DatosGeneralesWidth);

                        DatosGenerales.AddCell(new PdfPCell(new Phrase("DATOS GENERALES DEL PUESTO SOLICITADO", FontLabelsWhite)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER, BackgroundColor = new iTextSharp.text.BaseColor(0, 125, 196), PaddingBottom = 5f });

                        doc.Add(DatosGenerales);
                        doc.Add(SaltoLínea);

                        PdfPTable DatosPuesto = new PdfPTable(2);
                        DatosPuesto.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        DatosPuesto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        DatosPuesto.WidthPercentage = 100;
                        float[] DatosPuestoWidth = { 25, 75 };
                        DatosPuesto.SetWidths(DatosPuestoWidth);

                        DatosPuesto.AddCell(new PdfPCell(new Phrase("Nombre del puesto: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosPuesto.AddCell(new PdfPCell(new Phrase(Puestos.PuestoDescripcion, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });

                        doc.Add(DatosPuesto);
                        doc.Add(SaltoLínea);

                        PdfPTable DatosGeneralesValores = new PdfPTable(4);
                        DatosGeneralesValores.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        DatosGeneralesValores.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        DatosGeneralesValores.WidthPercentage = 100;
                        float[] DatosGeneralesValoresWidth = { 25, 15, 35, 25 };
                        DatosGeneralesValores.SetWidths(DatosGeneralesValoresWidth);

                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase("Tipo de vacante: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase(TipoVacante, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase("Sucursal: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase(Sucursales.SucuNombre, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase("Turno a cubrir: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase(Turnos.TurDescripcion, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase("Necesidad de rolar turnos: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase(RolarTurno, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase("Tiempo de alimentos: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase(Requisicion.MRPTiempoAlimentos + " horas", FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase("Motivo de la vacante: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase(MotivoVacante, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase("Número de vacantes: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase(Requisicion.MRPNumeroVacantes.ToString(), FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase("", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase("", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase(MotivoDescripción, FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase(DescripcionMotivo, FontDatos)) {Colspan=3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosGeneralesValores.AddCell(new PdfPCell(new Phrase("\n\n\n", FontDatos)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });

                        doc.Add(DatosGeneralesValores);
                        doc.Add(SaltoLínea);

                        PdfPTable PerfilVacante = new PdfPTable(1);
                        PerfilVacante.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        PerfilVacante.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        PerfilVacante.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(0, 125, 196);
                        PerfilVacante.DefaultCell.PaddingBottom = 5f;
                        PerfilVacante.WidthPercentage = 100;
                        float[] PerfilVacanteWidth = { 100 };
                        PerfilVacante.SetWidths(PerfilVacanteWidth);

                        PerfilVacante.AddCell(new PdfPCell(new Phrase("PERFIL DE LA VACANTE", FontLabelsWhite)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER, BackgroundColor = new iTextSharp.text.BaseColor(0, 125, 196), PaddingBottom = 5f });
                        
                        doc.Add(SaltoLínea);
                        doc.Add(PerfilVacante);
                        doc.Add(SaltoLínea);
                        doc.Add(SaltoLínea);

                        PdfPTable PerfilDatos = new PdfPTable(6);
                        PerfilDatos.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        PerfilDatos.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        PerfilDatos.WidthPercentage = 100;
                        float[] PerfilDatosWidth = { 16.6f, 16.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                        PerfilDatos.SetWidths(PerfilDatosWidth);

                        PerfilDatos.AddCell(new PdfPCell(new Phrase("Sexo: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase(Sexos.SexDescripcion, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase("Edad mínima: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase(Requisicion.MRPEdadMinima.ToString() + " años", FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase("Edad máxima: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase(Requisicion.MRPEdadMaxima.ToString() + " años", FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase("Escolaridad: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase(Escolaridades.EscoDescripcion, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase("Título indispensable: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase(Titulo, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase("Cédula profesional indispensable: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase(Cedula, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase("Experiencia en el puesto: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase(Experiencia, FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase("\n", FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase("\n", FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase("\n", FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        PerfilDatos.AddCell(new PdfPCell(new Phrase("\n", FontDatos)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });

                        doc.Add(PerfilDatos);
                        doc.Add(SaltoLínea);
                        doc.Add(SaltoLínea);

                        PdfPTable FuncionesPerfil = new PdfPTable(2);
                        FuncionesPerfil.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        FuncionesPerfil.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        FuncionesPerfil.WidthPercentage = 100;
                        float[] FuncionesPerfilWidth = { 30, 70 };
                        FuncionesPerfil.SetWidths(FuncionesPerfilWidth);

                        FuncionesPerfil.AddCell(new PdfPCell(new Phrase("Funciones del puesto: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        FuncionesPerfil.AddCell(new PdfPCell(new Phrase(Requisicion.MRPFuncionesPuesto, FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        FuncionesPerfil.AddCell(new PdfPCell(new Phrase("Conocimientos técnicos: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        FuncionesPerfil.AddCell(new PdfPCell(new Phrase(Requisicion.MRPConocimientosPuesto, FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });

                        doc.Add(FuncionesPerfil);
                        doc.Add(SaltoLínea);
                        doc.Add(SaltoLínea);

                        PdfPTable FirmasPerfil = new PdfPTable(3);
                        FirmasPerfil.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        FirmasPerfil.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        FirmasPerfil.WidthPercentage = 75;
                        float[] FirmasPerfilWidth = { 45, 10, 45 };
                        FirmasPerfil.SetWidths(FirmasPerfilWidth);

                        FirmasPerfil.AddCell(new PdfPCell(new Phrase("Solicitante: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        FirmasPerfil.AddCell(new PdfPCell(new Phrase("", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        FirmasPerfil.AddCell(new PdfPCell(new Phrase("Autorizó: ", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        FirmasPerfil.AddCell(new PdfPCell(new Phrase("\n\n ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        FirmasPerfil.AddCell(new PdfPCell(new Phrase("\n\n", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        FirmasPerfil.AddCell(new PdfPCell(new Phrase("\n\n", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        FirmasPerfil.AddCell(new PdfPCell(new Phrase("Jefe de departamento o área ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.TOP_BORDER });
                        FirmasPerfil.AddCell(new PdfPCell(new Phrase("", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        FirmasPerfil.AddCell(new PdfPCell(new Phrase("Gerente de departamento o área", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.TOP_BORDER });

                        doc.Add(FirmasPerfil);
                        doc.Add(SaltoLínea);
                        doc.Add(SaltoLínea); 

                        PdfPTable UsoCH = new PdfPTable(1);
                        UsoCH.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        UsoCH.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        UsoCH.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(1, 38, 57);
                        UsoCH.DefaultCell.PaddingBottom = 5f;
                        UsoCH.WidthPercentage = 100;
                        float[] UsoCHWidth = { 100 };
                        UsoCH.SetWidths(UsoCHWidth);

                        UsoCH.AddCell(new PdfPCell(new Phrase("PARA USO EXCLUSIVO DE CAPITAL HUMANO", FontLabelsWhite)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER, BackgroundColor = new BaseColor(1, 38, 57), PaddingBottom = 5f });

                        doc.Add(UsoCH);
                        doc.Add(SaltoLínea);
                        doc.Add(SaltoLínea);

                        PdfPTable DatosCH = new PdfPTable(4);
                        DatosCH.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        DatosCH.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        DatosCH.WidthPercentage = 100;
                        float[] DatosCHWidth = { 25, 25, 25, 25 };
                        DatosCH.SetWidths(DatosCHWidth);

                        DatosCH.AddCell(new PdfPCell(new Phrase("Sueldo mensual inicial: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER  });
                        DatosCH.AddCell(new PdfPCell(new Phrase("$" + Requisicion.MRPSueldoMensualInicial + " MXN", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase("Sueldo mensual planta: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase("$" + Requisicion.MRPSueldoMensualPlanta + " MXN", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase("Sueldo mensual + costo: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase("$" + Requisicion.MRPSueldoMensualMasCosto + " MXN", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase("Bono Variable: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase("$" + Requisicion.MRPBonoVariable + " MXN", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase("Esquema de contratación: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase(Esquema, FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase("Candidato a contratar: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase(Requisicion.MRPCandidato, FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase("Fecha de ingreso: ", FontLabels)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });
                        DatosCH.AddCell(new PdfPCell(new Phrase(Requisicion.MRPFechaIngreso.Value.ToShortDateString(), FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.NO_BORDER });

                        doc.Add(DatosCH);
                        doc.Add(SaltoLínea);
                        doc.Add(SaltoLínea);

                        PdfPTable FirmasAutorizacion = new PdfPTable(4);
                        FirmasAutorizacion.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        FirmasAutorizacion.DefaultCell.Border = iTextSharp.text.Rectangle.RECTANGLE;
                        FirmasAutorizacion.DefaultCell.UseBorderPadding = true;
                        FirmasAutorizacion.DefaultCell.PaddingBottom = 5f;
                        FirmasAutorizacion.WidthPercentage = 100;
                        float[] FirmasAutorizacionWidth = { 30, 25, 22, 23 };
                        FirmasAutorizacion.SetWidths(FirmasAutorizacionWidth);

                        FirmasAutorizacion.AddCell(new PdfPCell(new Phrase("PARA USO EXCLUSIVO DE CAPITAL HUMANO", FontLabelsWhite)) {Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.RECTANGLE, BorderColor= new BaseColor(0, 125, 196), BackgroundColor = new BaseColor(0, 125, 196), PaddingBottom = 5f });
                        FirmasAutorizacion.AddCell(new PdfPCell(new Phrase("Revisó", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.RECTANGLE, BorderColor = new BaseColor(1, 38, 57), PaddingBottom = 5f });
                        FirmasAutorizacion.AddCell(new PdfPCell(new Phrase("Visto Bueno", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.RECTANGLE, BorderColor = new BaseColor(1, 38, 57), PaddingBottom = 5f });
                        FirmasAutorizacion.AddCell(new PdfPCell(new Phrase("Autorización", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.RECTANGLE, BorderColor = new BaseColor(1, 38, 57), PaddingBottom = 5f });
                        FirmasAutorizacion.AddCell(new PdfPCell(new Phrase("Autorización puesto de nueva creación", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.RECTANGLE, BorderColor = new BaseColor(1, 38, 57), PaddingBottom = 5f });
                        FirmasAutorizacion.AddCell(new PdfPCell(new Phrase("\n\n\n\n Coordinador de Reclutamiento", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.RECTANGLE, BorderColor = new BaseColor(1, 38, 57), PaddingBottom = 5f });
                        FirmasAutorizacion.AddCell(new PdfPCell(new Phrase("\n\n\n\n Gerente de Capital Humano", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.RECTANGLE, BorderColor = new BaseColor(1, 38, 57), PaddingBottom = 5f });
                        FirmasAutorizacion.AddCell(new PdfPCell(new Phrase("\n\n\n\n Dirección de estrategia", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.RECTANGLE, BorderColor = new BaseColor(1, 38, 57), PaddingBottom = 5f });
                        FirmasAutorizacion.AddCell(new PdfPCell(new Phrase("\n\n\n\n Dirección General", FontDatos)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = iTextSharp.text.Rectangle.RECTANGLE, BorderColor = new BaseColor(1, 38, 57), PaddingBottom = 5f });

                        doc.Add(FirmasAutorizacion);

                        doc.Close();
                    }

                        Estatus = true;

                    pues = Puestos.PuestoDescripcion;
                    sucu = Sucursales.SucuNombre;
                }
                //return Ok(Estatus);

                var NombreArchivo = "RequisicionPersonal" + Requisicion.MRPFolio + "_" + sucu + "_" + pues + ".pdf";
                HttpContext.Response.Headers.Add("content-disposition", "inline;filename="+ NombreArchivo);
                byte[] myfile = System.IO.File.ReadAllBytes("C:\\FormatosRequisicionPersonal\\" + NombreArchivo);
                return new FileContentResult(myfile, "application/pdf");
            }
            catch (Exception e)
            {
                throw;
            }
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("PermisosRequisiciones")]
        public async Task<IActionResult> PermisosRequisiciones()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 1023 && x.URPUserId == user.Id).ToList();
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

        [Microsoft.AspNetCore.Mvc.HttpGet("CancelarRequisicionById/{Id}")]
        public IActionResult CancelarRequisicionById(int Id)
        {
            try
            {
                var requisicion = _context.MovRequisicionPersonal.Where(x => x.MRPId == Id).First();
                var Puesto = _context.CatPuestos.Where(x => x.PuestoId == requisicion.MRPPuestoId).FirstOrDefault();
                var Sucursal = _context.CatSucursales.Where(x => x.SucuId == requisicion.MRPSucursalId).FirstOrDefault();
                var Turno = _context.CatTurnosLaborales.Where(x => x.TurId == requisicion.MRPTurnoId).FirstOrDefault();
                var Escolaridades = _context.CatEscolaridades.Where(x => x.EscoId == requisicion.MRPEscolaridadId).FirstOrDefault();
                var Sexos = _context.CatSexos.Where(x => x.SexId == requisicion.MRPSexoId).FirstOrDefault();
                var TurnoRolado = "";
                var PasoDelFlujo = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == requisicion.MRPFlujoId).FirstOrDefault();
                string BanderaPaso = "";
                if (PasoDelFlujo.DetFlujoOrden == 1)
                {
                    BanderaPaso = "CH";
                }
                else if (PasoDelFlujo.DetFlujoOrden == 2)
                {
                    BanderaPaso = "Dirección";
                }
                else if (PasoDelFlujo.DetFlujoOrden == 3)
                {
                    BanderaPaso = "CapitalHumano";
                }
                else
                {
                    return BadRequest();
                }
                if (requisicion.MRPRolarTurno == true)
                {
                    TurnoRolado = "Sí";
                }
                else
                {
                    TurnoRolado = "No";
                }
                var TipoVacante = "";
                if (requisicion.MRPTipoVacante == 1)
                {
                    TipoVacante = "Eventual";
                }
                else if (requisicion.MRPTipoVacante == 2)
                {
                    TipoVacante = "Permanente";
                }
                else if (requisicion.MRPTipoVacante == 3)
                {
                    TipoVacante = "Cubre-incidencias";
                }
                else
                {
                    TipoVacante = "Tipo de vacante no válido";
                }
                var MotivoVacante = "";
                if (requisicion.MRPMotivoVacante == 1)
                {
                    MotivoVacante = "Nueva creación";
                }
                else if (requisicion.MRPMotivoVacante == 2)
                {
                    MotivoVacante = "Vacaciones";
                }
                else if (requisicion.MRPMotivoVacante == 3)
                {
                    MotivoVacante = "Sustitución de personal";
                }
                else if (requisicion.MRPMotivoVacante == 4)
                {
                    MotivoVacante = "Cubrir incapacidad";
                }
                else if (requisicion.MRPMotivoVacante == 5)
                {
                    MotivoVacante = "Promoción de personal";
                }
                else
                {
                    MotivoVacante = "Motivo no válido";
                }
                var Titulo = "";
                if (requisicion.MRPTituloIndispensable == true)
                {
                    Titulo = "Sí";
                }
                else
                {
                    Titulo = "No";
                }
                var Cedula = "";
                if (requisicion.MRPCedulaIndispensable == true)
                {
                    Cedula = "Si";
                }
                else
                {
                    Cedula = "No";
                }
                var Experiencia = "";
                if (requisicion.MRPExperienciaIndispensable == true)
                {
                    Experiencia = "Si";
                }
                else
                {
                    Experiencia = "No";
                }
                var fecharecepcioncorta = "";

                if (requisicion.MRPFechaRecepcion != null)
                {
                    fecharecepcioncorta = requisicion.MRPFechaRecepcion.Value.ToShortDateString();
                }
                string DiaIngreso = "";
                string MesIngreso = "";
                string AnioIngreso = "";
                if (requisicion.MRPFechaIngreso != null)
                {
                    DiaIngreso = requisicion.MRPFechaIngreso.Value.Day.ToString();
                    MesIngreso = requisicion.MRPFechaIngreso.Value.Month.ToString();
                    AnioIngreso = requisicion.MRPFechaIngreso.Value.Year.ToString();
                }

                if (requisicion.MRPFechaIngreso != null)
                {
                    if (requisicion.MRPFechaIngreso.Value.Day < 10)
                    {
                        DiaIngreso = "0" + DiaIngreso;
                    }
                    if (requisicion.MRPFechaIngreso.Value.Month < 10)
                    {
                        MesIngreso = "0" + MesIngreso;
                    }
                }
                string ingreso = AnioIngreso + "-" + MesIngreso + "-" + DiaIngreso;

                var obj = new
                {
                    Puesto.PuestoDescripcion,
                    Sucursal.SucuNombre,
                    Turno.TurDescripcion,
                    Escolaridades.EscoDescripcion,
                    Sexos.SexDescripcion,
                    TurnoRolado,
                    TipoVacante,
                    MotivoVacante,
                    Titulo,
                    Cedula,
                    Experiencia,
                    requisicion.MRPNumeroVacantes,
                    fe = requisicion.MRPFechaElaboracion.ToShortDateString(),
                    requisicion.MRPTiempoAlimentos,
                    requisicion.MRPMotivoDescripcion,
                    requisicion.MRPEdadMaxima,
                    requisicion.MRPEdadMinima,
                    requisicion.MRPFuncionesPuesto,
                    requisicion.MRPConocimientosPuesto,
                    requisicion.MRPEstatusId,
                    requisicion.MRPId,
                    requisicion.MRPFolio,
                    requisicion.MRPSueldoMensualInicial,
                    requisicion.MRPSueldoMensualPlanta,
                    requisicion.MRPSueldoMensualMasCosto,
                    requisicion.MRPBonoVariable,
                    requisicion.MRPEsquemaContratacion,
                    requisicion.MRPCandidato,
                    requisicion.MRPFechaIngreso,
                    fecharecepcioncorta,
                    ingreso,
                    BanderaPaso
                };
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [Microsoft.AspNetCore.Mvc.HttpPost("CancelarRequisicion")]
        public async Task<IActionResult> CancelarRequisicionAsync(CatBitacoraRequisicionPersonal BRP, int Id, string Motivo)
        {
            try
            {
                if (Motivo != null)
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.nucleodediagnostico.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    string sCuentaDeCorreo = "nucleorh@nucleodediagnostico.com";
                    string sPasswordCorreo = "431@0Xajf";
                    smtp.Credentials = new System.Net.NetworkCredential(sCuentaDeCorreo, sPasswordCorreo);

                    var Requisicion = _context.MovRequisicionPersonal.Where(x => x.MRPId == Id).FirstOrDefault();
                    var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
                    DateTime FechaActual = DateTime.Now;
                    var Sucursal = _context.CatSucursales.Where(x => x.SucuId == Requisicion.MRPSucursalId).First();
                    var Puesto = _context.CatPuestos.Where(x => x.PuestoId == Requisicion.MRPPuestoId).First();

                    BRP.BitRPRPId = Id;
                    BRP.BitRPDetFlujoId = Requisicion.MRPFlujoId;
                    BRP.BitRPUserId = user.UserName;
                    BRP.BitRPFecha = FechaActual;
                    BRP.BitRPObservaciones = Motivo;
                    Requisicion.MRPEstatusId = 4;

                    _context.CatBitacoraRequisicionPersonal.Add(BRP);
                    _context.Update(Requisicion);
                    _context.SaveChanges();

                    var x = _context.CatBitacoraRequisicionPersonal.Where(x => x.BitRPId == BRP.BitRPId).First();

                    var BitacoraRequisicion = _context.CatBitacoraRequisicionPersonal.Where(x => x.BitRPRPId == Requisicion.MRPId).FirstOrDefault();
                    var usuarioRequisicion = _userManager.Users.Where(x => x.UserName == BitacoraRequisicion.BitRPUserId).FirstOrDefault();
                    var Empleados = _context.CatEmpleados.Where(x => x.EmpUserId == usuarioRequisicion.UserName).FirstOrDefault();
                    var FechaCorta = FechaActual.ToShortDateString();
                    var EmpleadoCierraIncidencia = _context.CatEmpleados.Where(x => x.EmpUserId == user.UserName).FirstOrDefault();
                    var puesto = _context.CatPuestos.Where(x => x.PuestoId == Requisicion.MRPPuestoId).FirstOrDefault();
                    var FlujoRequisicion = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == Requisicion.MRPFlujoId).First();
                    var ListaFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoFlujoId == FlujoRequisicion.DetFlujoFlujoId).ToList();
                    var CapitalHumano = ListaFlujos.Where(x => x.DetFlujoCorreoDestino == "CapitalHumano").First();
                    var destinatario = CapitalHumano.DetFlujoEmail;
                    var asuntoReenvio = "La requisición de personal para el puesto de " + Puesto.PuestoDescripcion + " en la sucursal de  " + Sucursal.SucuNombre + " ha sido cancelada. ";
                    var urlGlobal = "192.168.0.5/RequisicionPersonal?";
                    var Bandera = "7teugr"; // Reenvío para reenvío 
                    var UrlDetallesRP = urlGlobal + "&Ubkdgmb687=" /*Id Usuario*/ + user.UserName + "&F54hfj=" /* Folio */ + Requisicion.MRPId + "&Bjgphij08=" /*Bandera*/ + Bandera;

                    var bodyBuilderRemitente = new BodyBuilder();
                    bodyBuilderRemitente.HtmlBody = string.Format(@"<table style='width:100%'> <tr>
                <th width='10%'><img src=""https://i.imgur.com/gns6ril.jpeg"" style='width:70%;'></th>
                <th><h4>NUCLEO DE DIAGNÓSTICO <br />  Av. Federalismo Sur No. 577 Col. Mexicaltzingo, Guadalajara, Jalisco, México. No. 1030 <br /> Teléfono: (33) 3942-1030 </h4></th>
                <th width='15%'>{0}</th> </tr> <tr>
                <td colspan='3'> <br /> <br /><br />
                <div style='display:block; text-align:center;'> 
                <p>¡ La requisición de personal del puesto de {4} a nombre de {1} ha sido cancelada. La misma ya no estará disponible para generar su archivo pdf!</p><br />
                <p>¡ El motivo de la cancelación es el siguiente: {3} !</p>
                <p>Para consultarla da click en el siguiente link...</p> <br /><a href=""{2}""><img src=""https://i.imgur.com/STwHSD4.png"" style='width:30%;'></a></div></td></tr></table>", FechaCorta, Empleados.EmpNombreCompleto,  UrlDetallesRP, Motivo, puesto.PuestoDescripcion);

                    MailMessage correo = new MailMessage();
                    correo.From = new MailAddress("nucleorh@nucleodediagnostico.com"); // <- Correo de donde se mandará todo.
                    correo.To.Add(destinatario);
                    correo.Subject = asuntoReenvio;
                    correo.Body = bodyBuilderRemitente.HtmlBody;
                    correo.IsBodyHtml = true;
                    correo.Priority = MailPriority.Normal;

                    smtp.Send(correo);

                    return Ok(BRP);
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

        [Microsoft.AspNetCore.Mvc.HttpGet("AreaByDepa")]
        public IActionResult AreaByDepa(int Id)
        {
            var AreasDep = _context.CatAreas.Where(x => x.AreaDepaId == Id).ToList();
            var obj = new
            {
                AreasDep

            };
            return Ok(obj);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("PuestoByArea")]
        public IActionResult PuestoByArea(int Id)
        {
            var Puestos = _context.CatPuestos.Where(x => x.PuestoAreaId == Id).ToList();
            var obj = new
            {
                Puestos

            };
            return Ok(obj);
        }
    }
}