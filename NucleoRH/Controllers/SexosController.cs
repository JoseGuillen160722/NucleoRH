using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NucleoRH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NucleoRH.Controllers
{
    [Authorize]
    public class SexosController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        public SexosController(Nucleo_RHContext context, IWebHostEnvironment env)
        {
            this._context = context;
            _env = env;
        }

        // LA VISTA MOSTRARÁ EL CONTENIDO DE LA TABLA

        public IActionResult Index()
        {            
            ViewBag.Sexos = _context.CatSexos.OrderBy(x => x.SexDescripcion).ToList();

            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddSexo")]
        public IActionResult AddSexo(CatSexos sex)
        {           
            try
            {
                if (sex != null)
                {
                    _context.CatSexos.Add(sex);
                    _context.SaveChanges();
                    var x = _context.CatSexos.Include(x => x.SexDescripcion).Where(x => x.SexId == sex.SexId).First();
                    var obj = new
                    {
                        x.SexId,
                        x.SexDescripcion
                    };
                    return Ok(sex);
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

        [HttpPost("DeleteSexo")]
        public IActionResult DeleteSexo(int Id)
        {
            try
            {
                var ins = _context.CatSexos.Where(x => x.SexId == Id).First();
                _context.CatSexos.Remove(ins);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ****************** MÉTODO DE MODIFICACIÓN - GUARDAR CAMBIOS ******************* 

        [HttpPost("EditSexo")]
        public async Task<IActionResult> EditSexo(CatSexos sexo)
        {
            try
            {
                var x = _context.CatSexos.Where(x => x.SexId == sexo.SexId).First();               
                if (sexo != null)
                {                                    
                        x.SexDescripcion = sexo.SexDescripcion;                     
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

        [HttpGet("SexoById/{Id}")]
        public IActionResult InsumoById(int Id)
        {
            try
            {
                var ins = _context.CatSexos.Where(x => x.SexId == Id).First();
                return Ok(ins);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}