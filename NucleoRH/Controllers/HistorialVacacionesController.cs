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

namespace NucleoRH.Controllers
{
    [Authorize]
    public class HistorialVacacionesController : Controller
    {

        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        public HistorialVacacionesController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            
            ViewBag.EmpleadosPrincipal = _context.CatEmpleados.OrderBy(x => x.EmpNumero).ToList();
            
            ViewBag.HistorialVacaciones = _context.CatHistorialVacaciones.OrderBy(x => x.HVId).ToList();
            var Empleados = _context.CatEmpleados.OrderBy(x => x.EmpId).ToList();
            ViewData["Empleados"] = new SelectList(Empleados, "EmpId", "EmpNombreCompleto");
            var Antiguedad = _context.CatAntiguedad.OrderBy(x => x.AntiId).ToList();
            ViewData["Antiguedad"] = new SelectList(Antiguedad, "AntiId", "AntiAnios");
            var Puestos = _context.CatPuestos.OrderBy(x => x.PuestoId).ToList();
            ViewData["Puestos"] = new SelectList(Puestos, "PuestoId", "PuestoDescripcion");
            var Sucursales = _context.CatSucursales.OrderBy(x=> x.SucuId).ToList();
            ViewData["Sucursales"] = new SelectList(Sucursales, "SucuId", "SucuNombre");
            var ReInci = _context.CatRegistroIncidencias.OrderBy(x=> x.ReInciId).ToList();
            ViewData["ReInci"] = new SelectList(ReInci, "ReInciId", "ReInciId");
            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddHistorialVacaciones")]
        public IActionResult AddHistorialVacaciones(CatHistorialVacaciones vaca, int IdEmpleado)
        {
            try
            {
                if (vaca != null)
                {

                    var Empleado = _context.CatEmpleados.Where(x => x.EmpNumero == IdEmpleado).FirstOrDefault();
                    var Permiso = _context.CatRegistroIncidencias.Where(x => x.ReInciId == vaca.HVReInciId).FirstOrDefault();
                    var DetallesPermiso = _context.CatDetalleIncidencias.Where(x => x.DetInciReInciId == vaca.HVReInciId).FirstOrDefault();
                    DateTime FechaActual = DateTime.Now;
                    var AñoEmpleado = Empleado.EmpFechaIngreso.Value.Year;
                    var DiasPermiso = DetallesPermiso.FechaFinal.Day - DetallesPermiso.FechaInicio.Day;
                    var Resultado = FechaActual.Year - AñoEmpleado;
                    if (Resultado == 0 || Resultado == 1)
                    {
                        vaca.HVAntiguedadId = 11;
                    }
                    else if (Resultado == 2)
                    {
                        vaca.HVAntiguedadId = 12;
                    }
                    else if (Resultado == 3)
                    {
                        vaca.HVAntiguedadId = 13;
                    }
                    else if (Resultado == 4)
                    {
                        vaca.HVAntiguedadId = 14;
                    }
                    else if (Resultado == 5 || Resultado == 6 || Resultado == 7 || Resultado == 8 || Resultado == 9)
                    {
                        vaca.HVAntiguedadId = 15;
                    }
                    else if (Resultado == 10 || Resultado == 11 || Resultado == 12 || Resultado == 13 || Resultado == 14)
                    {
                        vaca.HVAntiguedadId = 16;
                    }
                    else if (Resultado == 15 || Resultado == 16 || Resultado == 17 || Resultado == 18 || Resultado == 19)
                    {
                        vaca.HVAntiguedadId = 17;
                    }
                    else if (Resultado == 20 || Resultado == 21 || Resultado == 22 || Resultado == 23 || Resultado == 24)
                    {
                        vaca.HVAntiguedadId = 18;
                    }
                    else if (Resultado == 25 || Resultado == 26 || Resultado == 27 || Resultado == 28 || Resultado == 29)
                    {
                        vaca.HVAntiguedadId = 19;
                    }
                    else if (Resultado == 30 || Resultado == 31 || Resultado == 32 || Resultado == 33 || Resultado == 34)
                    {
                        vaca.HVAntiguedadId = 20;
                    }
                    else
                    {
                        vaca.HVAntiguedadId = 20;
                    }

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


                    _context.CatHistorialVacaciones.Add(vaca);
                    _context.SaveChanges();
                    var x = _context.CatHistorialVacaciones.Include(x => x.HVEmpId).Where(x => x.HVEmpId == vaca.HVEmpId).First(); // Solo en consulta, puedes poner el primer campo != de ID
                    var obj = new
                    {
                        x.HVId,
                        x.HVEmpId,
                        x.HVAntiguedadId,
                        x.HVDiasSolicitados,
                        x.HVEjercicio,
                        x.HVFechaSolicitud,
                        x.HVFechaInicio,
                        x.HVFechaCulminacion,
                        x.HVFechaPresentacion,
                        x.HVVacacionesPendientesEjercicioActual,
                        x.HVVacacionesPendientesEjerciciosPasados,
                        x.HVReInciId,
                        x.HVPuestoId,
                        x.HVSucursalId
                    };
                    return Ok(vaca);
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

        [HttpGet("HistorialById/{Id}")]
        public IActionResult InsumoById(int Id)
        {
            try
            {
                var ins = _context.CatHistorialVacaciones.Where(x => x.HVId == Id).First();

                var FullName = _context.CatEmpleados.Where(x => x.EmpId == ins.HVEmpId).First();
                var Nombre = FullName.EmpNombre.ToString();
                var Paterno = FullName.EmpPaterno.ToString();
                var Materno = FullName.EmpMaterno.ToString();
                string NombreCompleto = Nombre + " " + Paterno + " " + Materno;

                var Puesto = _context.CatPuestos.Where(x => x.PuestoId == ins.HVPuestoId).First();
                string PuestoConversion = Puesto.PuestoDescripcion.ToString();

                var Sucursal = _context.CatSucursales.Where(x => x.SucuId == ins.HVSucursalId).First();
                string Sucu = Sucursal.SucuNombre.ToString();

                var AntiguedadLista = _context.CatAntiguedad.Where(x => x.AntiId == ins.HVAntiguedadId).First();
                //var AntiguedadAnios = AntiguedadLista.AntiAnios.ToString();
                var AntiguedadDias = AntiguedadLista.AntiDias.ToString();
                //string AntiguedadCorrespondiente = AntiguedadAnios + " Y le corresponden " + AntiguedadDias + " días.";



                var obj = new
                {
                    ins.HVId,
                    ins.HVEmpId,
                    ins.HVAntiguedadId,
                    ins.HVDiasSolicitados,
                    ins.HVEjercicio,
                    Fs = ins.HVFechaSolicitud.ToShortDateString(),
                    Fi = ins.HVFechaInicio.ToShortDateString(),
                    Fc = ins.HVFechaCulminacion.ToShortDateString(),
                    Fp = ins.HVFechaPresentacion.ToShortDateString(),
                    ins.HVVacacionesPendientesEjercicioActual,
                    ins.HVVacacionesPendientesEjerciciosPasados,
                    ins.HVReInciId,
                    ins.HVPuestoId,
                    ins.HVSucursalId,
                    NombreCompleto,
                    PuestoConversion,
                    Sucu,
                    //AntiguedadCorrespondiente
                };
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("PermisosHistorial")]
        public async Task<IActionResult> PermisosHistorial()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 13 && x.URPUserId == user.Id).ToList();
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

        [HttpGet("HistorialFiltradoById/{Id}")]
        public IActionResult HistorialFiltradoPorEmpleadoById(int Id)
        {
            try
            {


                var ins = _context.CatHistorialVacaciones.Where(x => x.HVEmpId == Id).ToList();

               
                return Ok(ins);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("VolverAHistorialById/{Id}")]
        public IActionResult VolverAHistorialById(int Id)
        {
            try
            {


                var ins = _context.CatHistorialVacaciones.Where(x => x.HVId == Id).FirstOrDefault();
                var empleado = _context.CatEmpleados.Where(x=> x.EmpId == ins.HVEmpId).FirstOrDefault();
                var IdEmpleado = empleado.EmpId;


                return Ok(IdEmpleado);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
