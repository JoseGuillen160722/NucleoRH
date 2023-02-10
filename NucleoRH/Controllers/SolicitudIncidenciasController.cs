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
    public class SolicitudIncidenciasController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public SolicitudIncidenciasController(Nucleo_RHContext context, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.SolInci = _context.CatSolicitudIncidencias.OrderBy(x => x.SolInciId).ToList();
            var Empleados = _context.CatEmpleados.OrderBy(x => x.EmpId).ToList();
            ViewData["Empleados"] = new SelectList(Empleados, "EmpId", "EmpNombreCompleto");
            var Perfiles = _roleManager.Roles.ToList();
            ViewData["Perfiles"] = new SelectList(Perfiles, "Id", "Name");
            var Flujos = _context.CatFlujos.OrderBy(x => x.FlujoId).ToList();
            ViewData["Flujos"] = new SelectList(Flujos, "FlujoId", "FlujoDescripcion");

            return View();
        }

        // ------------------ Método de agregar ------------------
        [HttpPost("AddSolicitudIncidencias")]
        public IActionResult AddSolicitudIncidencias(CatSolicitudIncidencias Soli)
        {
            try
            {
                if (Soli != null)
                {
                    _context.CatSolicitudIncidencias.Add(Soli);
                    _context.SaveChanges();
                    var x = _context.CatSolicitudIncidencias.Where(x => x.SolInciId == Soli.SolInciId).First();
                    var obj = new
                    {
                        x.SolInciId,
                        x.SolInciEmpId,
                        x.SolInciReInciId,
                        x.SolInciFechaRegistro,
                        x.SolInciPuestoSuperior,
                        x.SolInciPerfiles,
                        x.SolInciFlujoId
                    };
                    return Ok(Soli);
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

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [HttpGet("SolicitudById/{Id}")]
        public IActionResult SolicitudById(int Id)
        {
            try
            {
                var Soli = _context.CatSolicitudIncidencias.Where(x => x.SolInciId == Id).First();
                var FechaCortada = Soli.SolInciFechaRegistro.ToShortDateString();

              

                var obj = new
                {
                    Soli,
                    FechaCortada
                };

                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost("EditSolicitudIncidencia")]
        public async Task<IActionResult> EditSolicitudIncidencia(CatSolicitudIncidencias Soli)
        {
            try
            {
                var x = _context.CatSolicitudIncidencias.Where(x => x.SolInciId == Soli.SolInciId).First();
                if (Soli != null)
                {
                    x.SolInciEmpId = Soli.SolInciEmpId;
                    x.SolInciReInciId = Soli.SolInciReInciId;
                    x.SolInciFlujoId = Soli.SolInciFlujoId;
                    x.SolInciFechaRegistro = Soli.SolInciFechaRegistro;
                    x.SolInciPerfiles = Soli.SolInciPerfiles;
                    x.SolInciPuestoSuperior = Soli.SolInciPuestoSuperior;
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

        [HttpGet("PermisosSolicitudIncidencias")]
        public async Task<IActionResult> PermisosSolicitudIncidencias()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 19 && x.URPUserId == user.Id).ToList();

            var obj = new
            {
                ListaPermisos
            };

            return Ok(obj);
        }

    }
}
