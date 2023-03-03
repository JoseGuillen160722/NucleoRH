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
    public class PuestosController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;

        public PuestosController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            _userManager = userManager;
        }

        // LA VISTA MOSTRARÁ EL CONTENIDO DE LA TABLA

        public IActionResult Index()
        {

            ViewBag.Puestos = _context.CatPuestos.OrderBy(x => x.PuestoId).ToList();
            var Departamentos = _context.CatDepartamentos.OrderBy(x => x.DepaDescripcion);
            ViewData["Departamentos"] = new SelectList(Departamentos, "DepaId", "DepaDescripcion");
            var Jerarquia = _context.CatPuestos.OrderBy(x => x.PuestoId).ToList();
            ViewData["Jerarquia"] = new SelectList(Jerarquia, "PuestoId", "PuestoDescripcion");
            var Areas = _context.CatAreas.OrderBy(x => x.AreaId).ToList();
            ViewData["Areas"] = new SelectList(Areas, "AreaId", "AreaDescripcion");
            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddPuestos")]
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

        // *********** MÉTODO DE ELIMINACIÓN POR MEDIO DE BOTÓN ************

        [HttpPost("DeletePuestos")]
        public IActionResult DeletePuestos(int Id)
        {
            try
            {
                var puesto = _context.CatPuestos.Where(x => x.PuestoId == Id).First();
                _context.CatPuestos.Remove(puesto);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ****************** MÉTODO DE MODIFICACIÓN - GUARDAR CAMBIOS ******************* 

        [HttpPost("EditPuestos")]
        public async Task<IActionResult> EditPuesto(CatPuestos puestos)
        {
            try
            {
                var x = _context.CatPuestos.Where(x => x.PuestoId == puestos.PuestoId).First();
                if (puestos != null)
                {
                    x.PuestoDescripcion = puestos.PuestoDescripcion;
                    x.PuestoAreaId = puestos.PuestoAreaId;
                    x.PuestoJerarquiaSuperiorPuestoId = puestos.PuestoJerarquiaSuperiorPuestoId;
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

        [HttpGet("PuestosById/{Id}")]
        public IActionResult InsumoById(int Id)
        {
            try
            {
                var ins = _context.CatPuestos.Where(x => x.PuestoId == Id).First();
                return Ok(ins);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("SeleccionarDepartamento")]
        public IActionResult AreaByDepaId(int Id)
        {
            var Catalogo = _context.CatAreas.ToList();
                var AreasDep = _context.CatAreas.Where(x => x.AreaDepaId == Id).ToList();
            var obj = new
            {
                AreasDep

            };
            return Ok(obj); 
         }

        [HttpGet("PermisosPuesto")]
        public async Task<IActionResult> PermisosPuesto()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 8 && x.URPUserId == user.Id).ToList();
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