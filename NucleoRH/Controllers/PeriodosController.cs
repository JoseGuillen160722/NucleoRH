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
    public class PeriodosController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;

        public PeriodosController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.Periodos = _context.CatPeriodos.OrderBy(x => x.PerNum).ToList();

            return View();
        }

        [HttpPost("AddPeriodo")]
        public IActionResult AddPeriodo(CatPeriodos periodos)
        {
            try
            {
                if (periodos != null)
                {
                    _context.CatPeriodos.Add(periodos);
                    _context.SaveChanges();
                    var x = _context.CatPeriodos.Where(x => x.PerId == periodos.PerId).First();
                    var obj = new
                    {
                        x.PerId,
                        x.PerNum,
                        x.PerFechaDesde,
                        x.PerFechaHasta,
                        x.PerCerrado

                    };
                    return Ok(periodos);
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

        [HttpPost("DeletePeriodo")]
        public IActionResult DeletePeriodo(int Id)
        {
            try
            {
                var per = _context.CatPeriodos.Where(x => x.PerId == Id).First();
                _context.CatPeriodos.Remove(per);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [HttpGet("PeriodoById/{Id}")]
        public IActionResult PeriodoById(int Id)
        {
            try
            {
                var per = _context.CatPeriodos.Where(x => x.PerId == Id).First();
                var FechaCortaDesde = per.PerFechaDesde.Value.ToShortDateString();
                var FechaCortaHasta = per.PerFechaHasta.Value.ToShortDateString();
                var DiaDesde = per.PerFechaDesde.Value.Day;
                string DiaD;
                var MesDesde = per.PerFechaDesde.Value.Month;
                string MesD;
                var YearDesde = per.PerFechaDesde.Value.Year;
                var DiaHasta = per.PerFechaHasta.Value.Day;
                string DiaH;
                var MesHasta = per.PerFechaHasta.Value.Month;
                string MesH;
                var YearHasta = per.PerFechaHasta.Value.Year;
                
                if(DiaDesde < 10)
                {
                    DiaD = "0" + DiaDesde;
                }
                else
                {
                    DiaD = DiaDesde.ToString();
                }
                if(MesDesde < 10)
                {
                    MesD = "0" + MesDesde;
                }
                else
                {
                    MesD = MesDesde.ToString();
                }
                if(DiaHasta < 10)
                {
                    DiaH = "0" + DiaHasta;
                }
                else
                {
                    DiaH = DiaHasta.ToString();
                }
                if (MesHasta < 10)
                {
                    MesH = "0" + MesHasta;
                }
                else
                {
                    MesH = MesHasta.ToString();
                }

                var obj = new
                {
                    per.PerId,
                    per.PerNum,
                    per.PerFechaDesde,
                    per.PerFechaHasta,
                    FechaCortaDesde,
                    FechaCortaHasta,
                    DiaD,
                    MesDesde,
                    YearDesde,
                    DiaH,
                    MesH,
                    YearHasta
                };

                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // ****************** MÉTODO DE MODIFICACIÓN - GUARDAR CAMBIOS ******************* 

        [HttpPost("EditPeriodo")]
        public async Task<IActionResult> EditPeriodo(CatPeriodos periodos)
        {
            try
            {
                var x = _context.CatPeriodos.Where(x => x.PerId == periodos.PerId).First();
                if (periodos != null)
                {
                    x.PerNum = periodos.PerNum;
                    x.PerFechaDesde = periodos.PerFechaDesde;
                    x.PerFechaHasta = periodos.PerFechaHasta;
                    
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

        [HttpGet("PermisosPeriodos")]
        public async Task<IActionResult> PermisosPeriodos()
        {

            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 1019 && x.URPUserId == user.Id).ToList();
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
