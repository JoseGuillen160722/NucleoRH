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
    public class ModulosController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;

        public ModulosController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.Modulos = _context.CatModulos.OrderBy(x => x.ModuloNombre).ToList();
            var Modulo = _context.CatModulos.OrderBy(x => x.ModuloId).ToList();
            ViewData["Modulo"] = new SelectList(Modulo, "ModuloId", "ModuloNombre");
            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS
        [HttpPost("AddModulo")]
        public IActionResult AddModulo(CatModulos modulo)
        {
            try
            {
                if (modulo != null)
                {
                    _context.CatModulos.Add(modulo);
                    _context.SaveChanges();
                    var x = _context.CatModulos.Include(x => x.ModuloId).Where(x => x.ModuloNombre== modulo.ModuloNombre).First(); // Solo en consulta, puedes poner el primer campo != de ID
                    var obj = new
                    {
                        x.ModuloId,
                        x.ModuloNombre
                        
                    };
                    return Ok(modulo);
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

        [HttpPost("DeleteModulo")]
        public IActionResult DeleteModulo(int Id)
        {
            try
            {
                var modulo = _context.CatModulos.Where(x => x.ModuloId == Id).First();
                _context.CatModulos.Remove(modulo);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ****************** MÉTODO DE MODIFICACIÓN - GUARDAR CAMBIOS ******************* 

        [HttpPost("EditModulo")]
        public async Task<IActionResult> EditModulo(CatModulos modulo)
        {
            try
            {
                var x = _context.CatModulos.Where(x => x.ModuloId == modulo.ModuloId).First();
                if (modulo != null)
                {
                    x.ModuloNombre = modulo.ModuloNombre;
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

        [HttpGet("ModuloById/{Id}")]
        public IActionResult ModuloById(int Id)
        {
            try
            {
                var ins = _context.CatModulos.Where(x => x.ModuloId == Id).First();
                return Ok(ins);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS
        [HttpPost("AddModuloSubModulo")]
        public IActionResult AddSubModulo(CatSubModulos Submodulo, string NombreModulo)
        {
            try
            {
                if (Submodulo != null)
                {
                    var Modulo = _context.CatModulos.Where(x => x.ModuloNombre == NombreModulo).FirstOrDefault();
                    Submodulo.SubMModuloId = Modulo.ModuloId;
                    _context.CatSubModulos.Add(Submodulo);
                    _context.SaveChanges();
                    var x = _context.CatSubModulos.Include(x => x.SubMId).Where(x => x.SubMName == Submodulo.SubMName).First(); // Solo en consulta, puedes poner el primer campo != de ID
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

        [HttpGet("PermisosModulo")]
        public async Task<IActionResult> PermisosModulo()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 16 && x.URPUserId == user.Id).ToList();
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
