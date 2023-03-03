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
    public class EmpTrazabilidadController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;

        public EmpTrazabilidadController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        public class EmpleadosTrazabilidad
        {
            public int EmpTrazaId { get; set; }
            public int EmpTrazaEmpId { get; set; }
            public string EmpTrazaPassword { get; set; }
            public string EmpTrazaNombreCompleto { get; set; }
            public int? EmpTrazaNomina { get; set; }
            public string EmpTrazaSucursal { get; set; }
        }

        public IActionResult Index()
        {
            var ConsultaJoinTrazabilidad = (from emptraza in _context.CatEmpTrazabilidad
                                            join empleado in _context.CatEmpleados on emptraza.EmpTrazaEmpId equals empleado.EmpId
                                            join sucursal in _context.CatSucursales on empleado.EmpSucuId equals sucursal.SucuId
                                            select new EmpleadosTrazabilidad
                                            {
                                                EmpTrazaId = emptraza.EmpTrazaId,
                                                EmpTrazaEmpId = empleado.EmpId,
                                                EmpTrazaPassword = emptraza.EmpTrazaPassword,
                                                EmpTrazaNombreCompleto = empleado.EmpNombreCompleto,
                                                EmpTrazaNomina = empleado.EmpNumero,
                                                EmpTrazaSucursal = sucursal.SucuNombre
                                            }).ToList();
            ViewBag.Trazabilidad = ConsultaJoinTrazabilidad.OrderBy(x => x.EmpTrazaNomina).ToList();
            return View();
        }

        [HttpPost("AddEmpTrazabilidad")]
        public IActionResult AddTrazabilidad(CatEmpTrazabilidad emptraza, int Nomina)
        {
            try
            {
                if (emptraza != null)
                {
                    var Empleado = _context.CatEmpleados.Where(x => x.EmpNumero == Nomina).FirstOrDefault();
                    if (Empleado == null)
                    {
                        return BadRequest();
                    }
                    emptraza.EmpTrazaEmpId = Empleado.EmpId;
                    _context.CatEmpTrazabilidad.Add(emptraza);
                    _context.SaveChanges();
                    var x = _context.CatEmpTrazabilidad.Where(x => x.EmpTrazaId == emptraza.EmpTrazaId).First();
                    var obj = new
                    {
                        x.EmpTrazaId,
                        x.EmpTrazaEmpId,
                        x.EmpTrazaPassword
                    };
                    return Ok(emptraza);
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

        [HttpGet("EmpTrazaById/{Id}")]
        public IActionResult EmpTrazaById(int Id)
        {
            try
            {
                var EmpTraza = _context.CatEmpTrazabilidad.Where(x => x.EmpTrazaId == Id).FirstOrDefault();
                if (EmpTraza == null){
                    return BadRequest();
                }
                var Empleado = _context.CatEmpleados.Where(x => x.EmpId == EmpTraza.EmpTrazaEmpId).FirstOrDefault();
                if(Empleado == null)
                {
                    return BadRequest();
                }
                var obj = new
                {
                    EmpTraza.EmpTrazaId,
                    EmpTraza.EmpTrazaPassword,
                    Empleado.EmpNumero
                };
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost("EditEmpTrazabilidad")]
        public async Task<IActionResult> EditEmpTrazabilidad(CatEmpTrazabilidad Traza)
        {
            try
            {
                var x = _context.CatEmpTrazabilidad.Where(x => x.EmpTrazaId == Traza.EmpTrazaId).First();
                if (Traza != null)
                {
                    x.EmpTrazaPassword = Traza.EmpTrazaPassword;
                    _context.Entry(x).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

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

        // *********** MÉTODO DE ELIMINACIÓN POR MEDIO DE BOTÓN ************

        [HttpPost("DeleteEmpTrazabilidad")]
        public IActionResult DeleteEmpTrazabilidad(int Id)
        {
            try
            {
                var Traza = _context.CatEmpTrazabilidad.Where(x => x.EmpTrazaId == Id).First();
                _context.CatEmpTrazabilidad.Remove(Traza);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("PermisosEmpTrazabilidad")]
        public async Task<IActionResult> PermisosEmpTrazabilidad()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 2023 && x.URPUserId == user.Id).ToList();
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
