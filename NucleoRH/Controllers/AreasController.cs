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
    public class AreasController : Controller
    {

        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        public AreasController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        // LA VISTA MOSTRARÁ EL CONTENIDO DE LA TABLA

        public IActionResult Index()
        {
            ViewBag.Areas = _context.CatAreas.OrderBy(x => x.AreaDescripcion).ToList();
            var Depas = _context.CatDepartamentos.OrderBy(x => x.DepaId).ToList(); // Relación a otra ID con la tabla. LLAVES FORÁNEAS RELACIONADAS A OTRA TABLA
            ViewData["Depas"] = new SelectList(Depas, "DepaId", "DepaDescripcion");
                // CREACIÓN DEL VIEWDATA DEL FORMULARIO DE INSERTAR.
            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddAreas")]
        public IActionResult AddAreas(CatAreas areass)
        {
            try
            {
                if (areass != null)
                {
                    _context.CatAreas.Add(areass); // El nombre del catálogo de área
                    _context.SaveChanges();
                    var x = _context.CatAreas.Include(x => x.AreaDescripcion).Where(x => x.AreaId == areass.AreaId).First(); // Solo en consulta, puedes poner el primer campo != de ID
                    var obj = new
                    {
                        x.AreaId,
                        x.AreaDescripcion,
                        x.AreaDepaId
                      
                    };
                    return Ok(areass);
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

        [HttpPost("DeleteAreas")]
        public IActionResult DeleteAreas(int Id)
        {
            try
            {
                var areass = _context.CatAreas.Where(x => x.AreaId == Id).First();
                _context.CatAreas.Remove(areass);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        // ****************** MÉTODO DE MODIFICACIÓN - GUARDAR CAMBIOS ******************* 

        [HttpPost("EditAreas")]
        public async Task<IActionResult> EditAreas(CatAreas sucu)
        {
            try
            {
                var x = _context.CatAreas.Where(x => x.AreaId == sucu.AreaId).First();
                if (sucu != null)
                {
                    x.AreaDescripcion = sucu.AreaDescripcion;
                    x.AreaDepaId = sucu.AreaDepaId;
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

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [HttpGet("AreasById/{Id}")]
        public IActionResult InsumoById(int Id)
        {
            try
            {
                var ins = _context.CatAreas.Where(x => x.AreaId == Id).First();
                return Ok(ins);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("PermisosAreas")]
        public async Task<IActionResult> PermisosAreas()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 4 && x.URPUserId == user.Id).ToList();
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
