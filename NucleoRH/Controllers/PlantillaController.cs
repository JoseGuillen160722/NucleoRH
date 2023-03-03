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
    public class PlantillaController : Controller
    {

        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        public PlantillaController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        // LA VISTA MOSTRARÁ EL CONTENIDO DE LA TABLA

        public IActionResult Index()
        {
            ViewBag.Plantillas = _context.CatPlantillas.OrderBy(x => x.PlantiId).ToList();
            var Depas = _context.CatDepartamentos.OrderBy(x => x.DepaId).ToList(); // Relación a otra ID con la tabla. LLAVES FORÁNEAS RELACIONADAS A OTRA TABLA
            ViewData["Depas"] = new SelectList(Depas, "DepaId", "DepaDescripcion");
            var Sucus = _context.CatSucursales.OrderBy(x => x.SucuId).ToList();
            ViewData["Sucus"] = new SelectList(Sucus, "SucuId", "SucuNombre");
            var Puestos = _context.CatPuestos.OrderBy(x => x.PuestoId).ToList();
            ViewData["Puestos"] = new SelectList(Puestos, "PuestoId", "PuestoDescripcion");
            // CREACIÓN DEL VIEWDATA DEL FORMULARIO DE INSERTAR.

            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddPlantilla")]
        public IActionResult AddPlantilla(CatPlantillas Plantillas)
        {
            try
            {
                if (Plantillas != null)
                {
                    _context.CatPlantillas.Add(Plantillas); // El nombre del catálogo de área
                    _context.SaveChanges();
                    var x = _context.CatPlantillas.Include(x => x.PlantiDepaId).Where(x => x.PlantiId == Plantillas.PlantiId).First(); // Solo en consulta, puedes poner el primer campo != de ID
                    var obj = new
                    {
                        x.PlantiId,
                        x.PlantiDepaId,
                        x.PlantiSucuId,
                        x.PlantiPuestoId

                    };
                    return Ok(Plantillas);
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

        [HttpPost("DeletePlantilla")]
        public IActionResult DeletePlantilla(int Id)
        {
            try
            {
                var plantis = _context.CatPlantillas.Where(x => x.PlantiId == Id).First();
                _context.CatPlantillas.Remove(plantis);
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

        [HttpPost("EditPlantilla")]
        public async Task<IActionResult> EditPlantilla(CatPlantillas planti)
        {
            try
            {
                var x = _context.CatPlantillas.Where(x => x.PlantiId == planti.PlantiId).First();
                if (planti != null)
                {
                    x.PlantiDepaId = planti.PlantiDepaId;
                    x.PlantiSucuId = planti.PlantiSucuId;
                    x.PlantiPuestoId = planti.PlantiPuestoId;
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
                throw;
            }
        }

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [HttpGet("PlantillaById/{Id}")]
        public IActionResult InsumoById(int Id)
        {
            try
            {
                var ins = _context.CatPlantillas.Where(x => x.PlantiId == Id).First();
                return Ok(ins);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("PermisosPlantilla")]
        public async Task<IActionResult> PermisosPlantilla()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 9 && x.URPUserId == user.Id).ToList();

            var obj = new
            {
                ListaPermisos
            };

            return Ok(obj);
        }

    }
}
