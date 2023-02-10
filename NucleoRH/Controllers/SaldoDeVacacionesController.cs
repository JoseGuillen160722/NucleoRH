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
    public class SaldoDeVacacionesController : Controller
    {

        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        public SaldoDeVacacionesController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        public class PeriodosConcatenados
        {
            public int PerId { get; set; }
            public string PeriodoConcatenado { get; set; }
        }

        public IActionResult Index()
        {
            var lista = _context.CatSaldoDeVacaciones.OrderBy(x=> x.SVId).ToList();
            
            ViewBag.SaldoDeVacaciones = _context.CatSaldoDeVacaciones.OrderBy(x => x.SVId).ToList();
            var Empleados = _context.CatEmpleados.OrderBy(x => x.EmpId).ToList();
            ViewData["Empleados"] = new SelectList(Empleados, "EmpId", "EmpNombreCompleto");
            var Periodos = _context.CatPeriodos.OrderBy(x => x.PerId).ToList();
            string Dia = "";
            string Mes = "";
            string DiaHasta = "";
            string MesHasta = "";
            
            List<PeriodosConcatenados> PerConc = new List<PeriodosConcatenados>();
            var PeriodosLista = new SelectList(PerConc);
            foreach (var item in Periodos)
            {
                if (item.PerFechaDesde.Value.Day < 10)
                {
                    Dia = "0" + item.PerFechaDesde.Value.Day;
                }
                else
                {
                    Dia = item.PerFechaDesde.Value.Day.ToString();
                }
                if(item.PerFechaDesde.Value.Month < 10)
                {
                    Mes = "0" + item.PerFechaDesde.Value.Month.ToString();
                }
                else
                {
                    Mes = item.PerFechaDesde.Value.Month.ToString();
                } // Empiezan las fechas HASTA
                if (item.PerFechaHasta.Value.Day < 10)
                {
                    DiaHasta = "0" + item.PerFechaHasta.Value.Day;
                }
                else
                {
                    DiaHasta = item.PerFechaHasta.Value.Day.ToString();
                }
                if (item.PerFechaHasta.Value.Month < 10)
                {
                    MesHasta = "0" + item.PerFechaHasta.Value.Month.ToString();
                }
                else
                {
                    MesHasta = item.PerFechaHasta.Value.Month.ToString();
                }

                PerConc.Add(new PeriodosConcatenados {  PerId = item.PerId, PeriodoConcatenado = "Desde: " + Dia + "/" + Mes + "/" + item.PerFechaDesde.Value.Year + " Hasta: " + DiaHasta + "/" + MesHasta + "/" + item.PerFechaHasta.Value.Year });
            }
            ViewData["PeriodosC"] = new SelectList(PerConc, "PerId", "PeriodoConcatenado");
            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddSaldoDeVacaciones")]
        public IActionResult AddSaldoDeVacaciones(CatSaldoDeVacaciones saldovaca, int NoNomina)
        {
            try
            {
                if (saldovaca != null)
                {
                    var Empleados = _context.CatEmpleados.Where(x => x.EmpNumero == NoNomina).FirstOrDefault();
                    if(Empleados != null)
                    {
                        saldovaca.SVEmpId = Empleados.EmpId;
                        var x = _context.CatSaldoDeVacaciones.Where(x => x.SVEmpId == saldovaca.SVEmpId).First(); // Solo en consulta, puedes poner el primer campo != de ID. Si la tabla está vacía, explota el universo
                        var y = _context.CatSaldoDeVacaciones.Where(y => y.SVEmpId == saldovaca.SVEmpId).ToList().OrderByDescending(x => x.SVId).First();
                        var DiasDisfrutadosPerEmpleado = _context.CatSaldoDeVacaciones.Where(z => z.SVEmpId == saldovaca.SVEmpId && z.SVEjercicio == saldovaca.SVEjercicio).Sum(suma => suma.SVDiasDisfrutados);

                        if (y.SVEmpId == saldovaca.SVEmpId && y.SVEjercicio == saldovaca.SVEjercicio)
                        {
                            var SacarElEmpleado = _context.CatEmpleados.Where(x => x.EmpId == saldovaca.SVEmpId).First();
                            var GetYearEmp = SacarElEmpleado.EmpFechaIngreso.Value.Year;
                            var GetYearRegistro = saldovaca.SVFechaRegistro.Year;
                            var SacarAnio = GetYearRegistro - GetYearEmp;
                            saldovaca.SVAniosAntiguedad = SacarAnio;
                            x.SVAniosAntiguedad = saldovaca.SVAniosAntiguedad;
                            // Validación del ID de Antiguedad dependiendo del resultado entre la fecha de registro y la fecha de ingreso del empleado

                            if (saldovaca.SVAniosAntiguedad == 0 || saldovaca.SVAniosAntiguedad == 1)
                            {
                                saldovaca.SVAntiId = 11;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 2)
                            {
                                saldovaca.SVAntiId = 12;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 3)
                            {
                                saldovaca.SVAntiId = 13;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 4)
                            {
                                saldovaca.SVAntiId = 14;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 5 || saldovaca.SVAniosAntiguedad == 6 || saldovaca.SVAniosAntiguedad == 7 || saldovaca.SVAniosAntiguedad == 8 || saldovaca.SVAniosAntiguedad == 9)
                            {
                                saldovaca.SVAntiId = 15;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 10 || saldovaca.SVAniosAntiguedad == 11 || saldovaca.SVAniosAntiguedad == 12 || saldovaca.SVAniosAntiguedad == 13 || saldovaca.SVAniosAntiguedad == 14)
                            {
                                saldovaca.SVAntiId = 16;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 15 || saldovaca.SVAniosAntiguedad == 16 || saldovaca.SVAniosAntiguedad == 17 || saldovaca.SVAniosAntiguedad == 18 || saldovaca.SVAniosAntiguedad == 19)
                            {
                                saldovaca.SVAntiId = 17;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 20 || saldovaca.SVAniosAntiguedad == 21 || saldovaca.SVAniosAntiguedad == 22 || saldovaca.SVAniosAntiguedad == 23 || saldovaca.SVAniosAntiguedad == 24)
                            {
                                saldovaca.SVAntiId = 18;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 25 || saldovaca.SVAniosAntiguedad == 26 || saldovaca.SVAniosAntiguedad == 27 || saldovaca.SVAniosAntiguedad == 28 || saldovaca.SVAniosAntiguedad == 29)
                            {
                                saldovaca.SVAntiId = 19;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 30 || saldovaca.SVAniosAntiguedad == 31 || saldovaca.SVAniosAntiguedad == 32 || saldovaca.SVAniosAntiguedad == 33 || saldovaca.SVAniosAntiguedad == 34)
                            {
                                saldovaca.SVAntiId = 20;
                            }
                            else
                            {
                                saldovaca.SVAntiId = 20;
                            }

                            // Calculamos los días con los de la tabla de antiguedad
                            var AntiguedadLista = _context.CatAntiguedad.Where(p => p.AntiId == saldovaca.SVAntiId).First();
                            var numeros = AntiguedadLista.AntiDias;
                            var DaysAva = numeros - DiasDisfrutadosPerEmpleado;

                            saldovaca.SVDiasRestantes = DaysAva;
                        }
                        // Validación si NO existe un registro anterior en la tabla
                        else
                        {
                            var SacarElEmpleado = _context.CatEmpleados.Where(x => x.EmpId == saldovaca.SVEmpId).First();
                            var GetYearEmp = SacarElEmpleado.EmpFechaIngreso.Value.Year;
                            var GetYearRegistro = saldovaca.SVFechaRegistro.Year;
                            var SacarAnio = GetYearRegistro - GetYearEmp;
                            saldovaca.SVAniosAntiguedad = SacarAnio;
                            x.SVAniosAntiguedad = saldovaca.SVAniosAntiguedad;
                            // Validación del ID de Antiguedad dependiendo del resultado entre la fecha de registro y la fecha de ingreso del empleado

                            if (saldovaca.SVAniosAntiguedad == 0 || saldovaca.SVAniosAntiguedad == 1)
                            {
                                saldovaca.SVAntiId = 11;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 2)
                            {
                                saldovaca.SVAntiId = 12;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 3)
                            {
                                saldovaca.SVAntiId = 13;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 4)
                            {
                                saldovaca.SVAntiId = 14;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 5 || saldovaca.SVAniosAntiguedad == 6 || saldovaca.SVAniosAntiguedad == 7 || saldovaca.SVAniosAntiguedad == 8 || saldovaca.SVAniosAntiguedad == 9)
                            {
                                saldovaca.SVAntiId = 15;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 10 || saldovaca.SVAniosAntiguedad == 11 || saldovaca.SVAniosAntiguedad == 12 || saldovaca.SVAniosAntiguedad == 13 || saldovaca.SVAniosAntiguedad == 14)
                            {
                                saldovaca.SVAntiId = 16;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 15 || saldovaca.SVAniosAntiguedad == 16 || saldovaca.SVAniosAntiguedad == 17 || saldovaca.SVAniosAntiguedad == 18 || saldovaca.SVAniosAntiguedad == 19)
                            {
                                saldovaca.SVAntiId = 17;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 20 || saldovaca.SVAniosAntiguedad == 21 || saldovaca.SVAniosAntiguedad == 22 || saldovaca.SVAniosAntiguedad == 23 || saldovaca.SVAniosAntiguedad == 24)
                            {
                                saldovaca.SVAntiId = 18;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 25 || saldovaca.SVAniosAntiguedad == 26 || saldovaca.SVAniosAntiguedad == 27 || saldovaca.SVAniosAntiguedad == 28 || saldovaca.SVAniosAntiguedad == 29)
                            {
                                saldovaca.SVAntiId = 19;
                            }
                            else if (saldovaca.SVAniosAntiguedad == 30 || saldovaca.SVAniosAntiguedad == 31 || saldovaca.SVAniosAntiguedad == 32 || saldovaca.SVAniosAntiguedad == 33 || saldovaca.SVAniosAntiguedad == 34)
                            {
                                saldovaca.SVAntiId = 20;
                            }
                            else
                            {
                                saldovaca.SVAntiId = 20;
                            }

                            // Calculamos los días con los de la tabla de antiguedad
                            var AntiguedadLista = _context.CatAntiguedad.Where(p => p.AntiId == saldovaca.SVAntiId).First();
                            var numeros = AntiguedadLista.AntiDias;
                            saldovaca.SVDiasRestantes = numeros - saldovaca.SVDiasDisfrutados;
                        }
                        _context.CatSaldoDeVacaciones.Add(saldovaca);
                        _context.SaveChanges();
                        var obj = new
                        {
                            x.SVId,
                            x.SVEmpId,
                            x.SVEjercicio,
                            x.SVPeriodoId,
                            x.SVFechaRegistro,
                            x.SVAniosAntiguedad,
                            x.SVAntiId,
                            x.SVDiasDisfrutados,
                            x.SVDiasRestantes
                        };

                        return Ok(saldovaca);
                    }
                    else
                    {
                        return BadRequest();
                    }
                    
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


        [HttpGet("PermisosSaldos")]
        public async Task<IActionResult> PermisosSaldos()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 14 && x.URPUserId == user.Id).ToList();
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
