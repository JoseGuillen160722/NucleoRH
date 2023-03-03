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
    public class SubModulosController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;

        public SubModulosController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.SubModulos = _context.CatSubModulos.OrderBy(x => x.SubMId).ToList();
            var Modulos = _context.CatModulos.OrderBy(x => x.ModuloId).ToList();
            ViewData["Modulos"] = new SelectList(Modulos, "ModuloId", "ModuloNombre");
            return View();
        }

       

        // ------------------ Método de agregar ------------------
        [HttpPost("AddSubModulo")]
        public IActionResult AddSubModulo(CatSubModulos Submodulo)
        {
            try
            {
                if (Submodulo != null)
                {
                    _context.CatSubModulos.Add(Submodulo);
                    _context.SaveChanges();
                    var x = _context.CatSubModulos.Where(x => x.SubMId== Submodulo.SubMId).First();
                    var obj = new
                    {
                        x.SubMId,
                        x.SubMModuloId,
                        x.SubMName
                    };
                    return Ok(Submodulo);
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

        [HttpPost("DeleteSubModulo")]
        public IActionResult DeleteSubModulo(int Id)
        {
            try
            {
                var submodulo = _context.CatSubModulos.Where(x => x.SubMId == Id).First();
                _context.CatSubModulos.Remove(submodulo);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [HttpGet("SubModuloById/{Id}")]
        public IActionResult SubModuloById(int Id)
        {
            try
            {
                var ins = _context.CatSubModulos.Where(x => x.SubMId == Id).First();
                return Ok(ins);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // ****************** MÉTODO DE MODIFICACIÓN - GUARDAR CAMBIOS ******************* 

        [HttpPost("EditSubModulo")]
        public async Task<IActionResult> EditSubModulo(CatSubModulos submodulo)
        {
            try
            {
                var x = _context.CatSubModulos.Where(x => x.SubMId == submodulo.SubMId).First();
                if (submodulo != null)
                {
                    x.SubMModuloId = submodulo.SubMModuloId;
                    x.SubMName = submodulo.SubMName;
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

        [HttpGet("PermisosSubModulo")]
        public async Task<IActionResult> PermisosSubModulo()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 17 && x.URPUserId == user.Id).ToList();
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
