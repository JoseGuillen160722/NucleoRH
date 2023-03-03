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
    public class IncidenciasController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IncidenciasController(Nucleo_RHContext context, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            ViewBag.Incidencias = _context.CatIncidencias.OrderBy(x => x.InciDescripcion).ToList();
            var Inci = _context.CatIncidencias.OrderBy(x => x.InciId).ToList(); // Relación a otra ID con la tabla. LLAVES FORÁNEAS RELACIONADAS A OTRA TABLA
            ViewData["Inci"] = new SelectList(Inci, "InciId", "InciDescripcion");

            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddIncidencia")]
        public IActionResult AddIncidencia(CatIncidencias Inci)
        {
            try
            {
                if (Inci != null)
                {
                    _context.CatIncidencias.Add(Inci);
                    _context.SaveChanges();
                    var x = _context.CatIncidencias.Include(x => x.InciDescripcion).Where(x => x.InciId == Inci.InciId).First();
                    var obj = new
                    {
                        x.InciId,
                        x.InciDescripcion
                    };
                    return Ok(Inci);
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

        [HttpPost("DeleteIncidencia")]
        public IActionResult DeleteIncidencia(int Id)
        {
            try
            {
                var Inci = _context.CatIncidencias.Where(x => x.InciId == Id).First();
                _context.CatIncidencias.Remove(Inci);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ****************** MÉTODO DE MODIFICACIÓN - GUARDAR CAMBIOS ******************* 

        [HttpPost("EditIncidencia")]
        public async Task<IActionResult> EditIncidencia(CatIncidencias Inci)
        {
            try
            {
                var x = _context.CatIncidencias.Where(x => x.InciId == Inci.InciId).First();
                if (Inci != null)
                {
                    x.InciDescripcion = Inci.InciDescripcion;
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

        [HttpGet("IncidenciaById/{Id}")]
        public IActionResult InsumoById(int Id)
        {
            try
            {
                var ins = _context.CatIncidencias.Where(x => x.InciId == Id).First();
                return Ok(ins);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("PermisosIncidencias")]
        public async Task<IActionResult> PermisosPermisos()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 15 && x.URPUserId == user.Id).ToList();
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
