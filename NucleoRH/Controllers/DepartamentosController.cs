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
    public class DepartamentosController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;

        public DepartamentosController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        // LA VISTA MOSTRARÁ EL CONTENIDO DE LA TABLA

        public IActionResult Index()
        {
            ViewBag.Departamentos = _context.CatDepartamentos.OrderBy(x => x.DepaDescripcion).ToList();
            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddDepartamento")]
        public IActionResult AddDepartamento(CatDepartamentos depa)
        {
            try
            {
                if (depa != null)
                {
                    _context.CatDepartamentos.Add(depa);
                    _context.SaveChanges();
                    var x = _context.CatDepartamentos.Include(x => x.DepaDescripcion).Where(x => x.DepaId == depa.DepaId).First();
                    var obj = new
                    {
                        x.DepaId,
                        x.DepaDescripcion
                    };
                    return Ok(depa);
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

        [HttpPost("DeleteDepartamento")]
        public IActionResult DeleteDepartamento(int Id)
        {
            try
            {
                var depa = _context.CatDepartamentos.Where(x => x.DepaId == Id).First();
                _context.CatDepartamentos.Remove(depa);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ****************** MÉTODO DE MODIFICACIÓN - GUARDAR CAMBIOS ******************* 

        [HttpPost("EditDepartamento")]
        public async Task<IActionResult> EditDepartamento(CatDepartamentos depa)
        {
            try
            {
                var x = _context.CatDepartamentos.Where(x => x.DepaId == depa.DepaId).First();
                if (depa != null)
                {
                    x.DepaDescripcion = depa.DepaDescripcion;
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

        [HttpGet("DepartamentoById/{Id}")]
        public IActionResult InsumoById(int Id)
        {
            try
            {
                var ins = _context.CatDepartamentos.Where(x => x.DepaId == Id).First();
                return Ok(ins);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("PermisosDepartamentos")]
        public async Task<IActionResult> PermisosDepartamentos()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 6 && x.URPUserId == user.Id).ToList();
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

