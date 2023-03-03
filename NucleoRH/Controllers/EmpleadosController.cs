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
    public class EmpleadosController : Controller
    {

        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;

        public EmpleadosController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewBag.Empleados = _context.CatEmpleados.OrderBy(x => x.EmpId).ToList();
            //Conversión de ID a Variables. Esto se hace cuando se tienen muchas Id y no quieres hacer el registro por ID, sino por Descripción.
            var Esco = _context.CatEscolaridades.OrderBy(x => x.EscoId);
            ViewData["Esco"] = new SelectList(Esco, "EscoId", "EscoDescripcion");
            var EstadoCivil = _context.CatEstadosCiviles.OrderBy(x => x.EdocId);
            ViewData["EstadoCivil"] = new SelectList(EstadoCivil, "EdocId", "EdocDescripcion");
            var Sex = _context.CatSexos.OrderBy(x => x.SexId).ToList();
            ViewData["Sex"] = new SelectList(Sex, "SexId", "SexDescripcion");
            var Estatus = _context.CatEstatus.OrderBy(x => x.EstId).Where(x=> x.EstDescripcion == "ACTIVO"|| x.EstDescripcion == "BAJA").ToList();
            ViewData["Estatus"] = new SelectList(Estatus, "EstId", "EstDescripcion");
            var Patron = _context.CatPatrones.OrderBy(x => x.PatId).ToList();
            ViewData["Patron"] = new SelectList(Patron, "PatId", "PatDescripcion");
            var Sucursal = _context.CatSucursales.OrderBy(x => x.SucuId).ToList();
            ViewData["Sucursal"] = new SelectList(Sucursal, "SucuId", "SucuNombre");
            var Puesto = _context.CatPuestos.OrderBy(x => x.PuestoId).ToList();
            ViewData["Puesto"] = new SelectList(Puesto, "PuestoId", "PuestoDescripcion");
            var Jorna = _context.CatJornadasLaborales.OrderBy(x => x.JornaId).ToList();
            ViewData["Jorna"] = new SelectList(Jorna, "JornaId", "JornaDescripcion");
            var TrabTipo = _context.CatTrabajadorTipos.OrderBy(x => x.TrabTipoId).ToList();
            ViewData["TrabTipo"] = new SelectList(TrabTipo, "TrabTipoId", "TrabTipoDescripcion");
            var Turno = _context.CatTurnosLaborales.OrderBy(x => x.TurId).ToList();
            ViewData["Turno"] = new SelectList(Turno, "TurId", "TurDescripcion");
            var Vacuna = _context.CatVacunacion.OrderBy(x => x.VacId).ToList();
            ViewData["Vacuna"] = new SelectList(Vacuna, "VacId", "VacDescripcion");
            var Municipio = _context.CatDomiciliosMunicipios.OrderBy(x => x.DomiMunicId).ToList();
            ViewData["Municipio"] = new SelectList(Municipio, "DomiMunicId", "DomiMunicDescripcion");
            var Estado = _context.CatDomiciliosEstados.OrderBy(x => x.DomiEstaId).ToList();
            ViewData["Estado"] = new SelectList(Estado, "DomiEstaId", "DomiEstaDescripcion");
            var Usuarios = _context.Users.OrderBy(x => x.Id).ToList();
            ViewData["Usuarios"] = new SelectList(Usuarios, "Id", "UserName");
            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddEmpleados")]
        public IActionResult AddEmpleados(CatEmpleados empleados)
        {
            try
            {
                if (empleados != null)
                {


                    _context.CatEmpleados.Add(empleados); // El nombre del catálogo
                    _context.SaveChanges();
                    var x = _context.CatEmpleados.Where(x => x.EmpId == empleados.EmpId).First(); // Solo en consulta, puedes poner el primer campo != de ID
                    var obj = new // El objeto almacena todo lo que haya en X y toma la propiedad del campo que se le está estableciendo
                    {
                        x.EmpId,
                        x.EmpNumero,
                        x.EmpNombre,
                        x.EmpPaterno,
                        x.EmpMaterno,
                        x.EmpTelefono,
                        x.EmpCelular,
                        x.EmpEmail,
                        x.EmpFechaIngreso,
                        x.EmpRfc,
                        x.EmpCurp,
                        x.EmpImss,
                        x.EmpEscoId,
                        x.EmpEdocId,
                        x.EmpSexId,
                        x.EmpEstId,
                        x.EmpComentarios,
                        x.EmpPatId,
                        x.EmpPuestoId,
                        x.EmpSucuId,
                        x.EmpJornaId,
                        x.EmpTrabTipoId,
                        x.EmpTurId,
                        x.EmpNacFecha,
                        x.EmpNacMunicId,
                        x.EmpVacId,
                        x.EmpUserId,
                        x.EmpAlias
                    };
                    return Ok(empleados);
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

        // *********** MÉTODO DE ELIMINACIÓN POR MEDIO DE BOTÓN ************

        [HttpPost("DeleteEmpleados")]
        public IActionResult DeleteEmpleados(int Id)
        {
            try
            {
                var empleado = _context.CatEmpleados.Where(x => x.EmpId == Id).First();
                empleado.EmpEstId = 2;
                _context.CatEmpleados.Update(empleado);
                //_context.CatEmpleados.Remove(empleado);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ****************** MÉTODO DE MODIFICACIÓN - GUARDAR CAMBIOS ******************* 

        [HttpPost("EditEmpleados")]
        public async Task<IActionResult> EditEmpleados(CatEmpleados empleados)
        {
            try
            {
                var x = _context.CatEmpleados.Where(x => x.EmpId == empleados.EmpId).First();
                if (empleados != null)
                {
                    x.EmpId = empleados.EmpId;
                    x.EmpNumero = empleados.EmpNumero;
                    x.EmpNombre = empleados.EmpNombre;
                    x.EmpPaterno = empleados.EmpPaterno;
                    x.EmpMaterno = empleados.EmpMaterno;
                    x.EmpTelefono = empleados.EmpTelefono;
                    x.EmpCelular = empleados.EmpCelular;
                    x.EmpEmail = empleados.EmpEmail;
                    x.EmpFechaIngreso = empleados.EmpFechaIngreso;
                    x.EmpRfc = empleados.EmpRfc;
                    x.EmpCurp = empleados.EmpCurp;
                    x.EmpImss = empleados.EmpImss;
                    x.EmpEscoId = empleados.EmpEscoId;
                    x.EmpEdocId = empleados.EmpEdocId;
                    x.EmpSexId = empleados.EmpSexId;
                    x.EmpEstId = empleados.EmpEstId;
                    x.EmpComentarios = empleados.EmpComentarios;
                    x.EmpPatId = empleados.EmpPatId;
                    x.EmpPuestoId = empleados.EmpPuestoId;
                    x.EmpSucuId = empleados.EmpSucuId;
                    x.EmpJornaId = empleados.EmpJornaId;
                    x.EmpTrabTipoId = empleados.EmpTrabTipoId;
                    x.EmpTurId = empleados.EmpTurId;
                    x.EmpNacFecha = empleados.EmpNacFecha;
                    x.EmpNacMunicId = empleados.EmpNacMunicId;
                    x.EmpVacId = empleados.EmpVacId;
                    x.EmpAlias = empleados.EmpAlias;

                    _context.Entry(x).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [HttpGet("EmpleadosById/{Id}")]
        public IActionResult EmpleadoById(int Id)
        {
            try
            {
                var ins = _context.CatEmpleados.Where(x => x.EmpId == Id).First();
                var MunicipioNacimiento = _context.CatDomiciliosMunicipios.Where(x => x.DomiMunicId == ins.EmpNacMunicId).First();

                var obj = new
                {
                    ins.EmpId,
                    ins.EmpNumero,
                    ins.EmpNombre,
                    ins.EmpPaterno, 
                    ins.EmpMaterno,
                    ins.EmpTelefono,
                    ins.EmpCelular,
                    ins.EmpEmail,
                    ins.EmpFechaIngreso,
                    ins.EmpRfc,
                    ins.EmpCurp,
                    ins.EmpImss,
                    ins.EmpEscoId,
                    ins.EmpEdocId,
                    ins.EmpSexId,
                    ins.EmpEstId,
                    ins.EmpComentarios,
                    ins.EmpPatId,
                    ins.EmpPuestoId,
                    ins.EmpSucuId,
                    ins.EmpJornaId,
                    ins.EmpTrabTipoId,
                    ins.EmpTurId,
                    ins.EmpNacFecha,
                    ins.EmpNacMunicId,
                    ins.EmpVacId,
                    ins.EmpUserId,
                    ins.EmpAlias,
                    MunicipioNacimiento.DomiMunicId,
                    MunicipioNacimiento.DomiMunicEstaId
                };

               
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("SeleccionarEstado")]
        public IActionResult MunicipioByEstadoId(int Id)
        {
            var Catalogo = _context.CatDomiciliosMunicipios.ToList();
            var MunicipioEstado = _context.CatDomiciliosMunicipios.Where(x => x.DomiMunicEstaId == Id).ToList();
            var obj = new
            {
                MunicipioEstado

            };
            return Ok(obj);
        }


        [HttpPost("AddUsuarios")]
        public async Task<IActionResult> AddUsuarios(IdentityUser user)
        {
            try
            {
                if (user != null)
                {

                    var usuario = new IdentityUser { UserName = user.UserName, Email = user.Email };
                    var result = await _userManager.CreateAsync(usuario, user.PasswordHash);




                    return Ok();
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

        [HttpGet("PermisosEmpleados")]
        public async Task<IActionResult> PermisosAreas()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 10 && x.URPUserId == user.Id).ToList();
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

    }
}