using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Utils;
using NucleoRH.Models;
using NucleoRH.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace NucleoRH.Controllers
{
    
    public class ValidacionURLController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        


        public ValidacionURLController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
            
        }

        public IActionResult Index()
        {
            ViewBag.RegistroIncidencias = _context.CatRegistroIncidencias.Include(r => r.Emp).Include(r => r.Inci).Include(r => r.Estatus);
            var Emp = _context.CatEmpleados.OrderBy(x => x.EmpId);
            ViewData["Emp"] = new SelectList(Emp, "EmpId", "EmpNombreCompleto");
            var Inci = _context.CatIncidencias.OrderBy(x => x.InciId);
            ViewData["Inci"] = new SelectList(Inci, "InciId", "InciDescripcion");
            var Horario = _context.CatHorarios.OrderBy(x => x.HoraId);
            ViewData["Horario"] = new SelectList(Horario, "HoraId", "HoraEntrada");
            var Estatus = _context.CatEstatus.OrderBy(x => x.EstId).Where(x => x.EstDescripcion != "ACTIVO" && x.EstDescripcion != "BAJA" && x.EstDescripcion != "PROCESADO" && x.EstDescripcion != "ACEPTADO");
            ViewData["Estatus"] = new SelectList(Estatus, "EstId", "EstDescripcion");

            return View();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("MostrarIncidenciaById/{Id}")]
        public IActionResult RegistroIncidenciaById(int Id)
        {
            try
            {
                var ReInci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Id).First();
                

                var GetFlujoActual = ReInci.ReInciEstatusFlujo;
                var DetallesDeFlujos = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == ReInci.ReInciEstatusFlujo).First();
                
                var IdEmpleado = ReInci.ReInciEmpId;
                var FechaDelRegistro = ReInci.Fecha;

                
                var EstadoIncidencia = ReInci.ReInciEstatusId;
                bool BanderaEstadoIncidencia = false;
                if (EstadoIncidencia == 6 || EstadoIncidencia == 5 || EstadoIncidencia == 4)
                {
                    BanderaEstadoIncidencia = false;
                }
                else
                {
                    BanderaEstadoIncidencia = true;
                }

                var FechaCortada = ReInci.Fecha.ToShortDateString();

                var obj = new
                {
                    // ReInci
                    FechaCortada,
                    ReInci.ReInciEmpId,
                    ReInci.Fecha,
                    ReInci.ReInciInciId,
                    ReInci.ReInciEstatusId,
                    ReInci.ReInciId,
                    IdEmpleado,
                    
                    BanderaEstadoIncidencia


                };

                return Ok(obj);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        
        //[Microsoft.AspNetCore.Mvc.HttpGet("ValidacionIncidenciaById/{Id}/{IdUsuario}/{Bandera}")]
        //public IActionResult ValidacionIncidenciaById(int Id, string IdUsuario, string Bandera)
        //{
        //    try
        //    {
        //        var PasoIncidencia = 0;
        //        string IdJefe = "0";
        //        string IdAprobadoEnBitacora = "0";
        //        bool redflag = false;

        //        var ReInci = _context.CatRegistroIncidencias.Where(x => x.ReInciId == Id).FirstOrDefault();

        //        if (ReInci == null)
        //        {
        //            return BadRequest();
        //        }
        //        else
        //        {
        //            var Flujo = _context.CatDetalleFlujo.Where(x => x.DetFlujoId == ReInci.ReInciEstatusFlujo).FirstOrDefault();
        //            if (Flujo == null)
        //            {
        //                return BadRequest();

        //            }
        //            else
        //            {
        //                PasoIncidencia = Flujo.DetFlujoOrden;
        //                var Bitacora = _context.CatBitacoraIncidencias.Where(x => x.BitInciReInciId == ReInci.ReInciId && x.BitInciDetFlujoId == Flujo.DetFlujoId).FirstOrDefault();

        //                if (Bandera == "3424hjlk234")
        //                {
                            
        //                     redflag = true;
                                                       
        //                }else if(Bandera == "4RT55cgd6FOR")
        //                {
        //                    redflag = true;
        //                }else if(Bandera == "jfnROs34")
        //                {
        //                    redflag = true;
        //                }
        //                else
        //                {
        //                    redflag = false;
        //                }

        //            }

        //        }

        //        var obj = new
        //        {
                    
        //            redflag
        //        };

        //        return Ok(obj);

        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}
    }
}
