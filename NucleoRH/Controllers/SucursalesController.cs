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
    public class SucursalesController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        public SucursalesController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        // LA VISTA MOSTRARÁ EL CONTENIDO DE LA TABLA

        public IActionResult Index()
        {
            ViewBag.Sucursales = _context.CatSucursales.OrderBy(x => x.SucuNombre).ToList();
            var PatId = _context.CatPatrones.OrderBy(x => x.PatId).ToList(); // Relación a otra ID con la tabla. LLAVES FORÁNEAS RELACIONADAS A OTRA TABLA
            ViewData["Patron"] = new SelectList(PatId, "PatId", "PatDescripcion"); // CREACIÓN DEL VIEWDATA DEL FORMULARIO DE INSERTAR.

            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddSucursales")]
        public IActionResult AddSucursales(CatSucursales sucu)
        {
            try
            {
                if (sucu != null)
                {
                    _context.CatSucursales.Add(sucu);
                    _context.SaveChanges();
                    var x = _context.CatSucursales.Include(x => x.SucuNcorto).Where(x => x.SucuId == sucu.SucuId).First(); // Solo en consulta, puedes poner el primer campo != de ID
                    var obj = new
                    {
                        x.SucuId,
                        x.SucuNcorto,
                        x.SucuNombre,
                        x.SucuPatId,
                        x.SucuEmail
                    };
                    return Ok(sucu);
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

        [HttpPost("DeleteSucursales")]
        public IActionResult DeleteSucursales(int Id)
        {
            try
            {
                var sucu = _context.CatSucursales.Where(x => x.SucuId == Id).First();
                _context.CatSucursales.Remove(sucu);
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

        [HttpPost("EditSucursales")]
        public async Task<IActionResult> EditSucursales(CatSucursales sucu)
        {
            try
            {
                var x = _context.CatSucursales.Where(x => x.SucuId == sucu.SucuId).First();
                if (sucu != null)
                {
                    x.SucuNcorto = sucu.SucuNcorto;
                    x.SucuNombre = sucu.SucuNombre;
                    x.SucuPatId = sucu.SucuPatId;
                    x.SucuEmail = sucu.SucuEmail;
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

        [HttpGet("SucursalesById/{Id}")]
        public IActionResult InsumoById(int Id)
        {
            try
            {
                var ins = _context.CatSucursales.Where(x => x.SucuId == Id).First();
                return Ok(ins);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("PermisosSucursales")]
        public async Task<IActionResult> PermisosSucursales()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 7 && x.URPUserId == user.Id).ToList();
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
