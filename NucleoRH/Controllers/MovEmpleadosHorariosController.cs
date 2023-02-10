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
    public class MovEmpleadosHorariosController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;

        public MovEmpleadosHorariosController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        public class HorariosConcatenados
        {
            public int HoraId { get; set; }
            public string HorarioConcatenado { get; set; }

        }
        public IActionResult Index()
        {
            ViewBag.MovEmpleadosHorarios = _context.MovEmpleadosHorarios.OrderBy(x => x.EmpHoraId).ToList();
            var Emp = _context.CatEmpleados.OrderBy(x => x.EmpId).ToList();
            ViewData["Emp"] = new SelectList(Emp, "EmpId", "EmpNombreCompleto");
            var Horarios = _context.CatHorarios.OrderBy(x => x.HoraId).ToList();
            List<HorariosConcatenados> HorConc = new List<HorariosConcatenados>();
            var HorariosLista = new SelectList(HorConc);

            for(int i=0; i< Horarios.Count; i++)
            {
                if (Horarios[i].HoraSabadoEntrada == null)
                {
                    HorConc.Add(new HorariosConcatenados { HoraId = Horarios[i].HoraId, HorarioConcatenado = "L-V: " + Horarios[i].HoraEntrada + " - " + Horarios[i].HoraSalida  });
                }else if (Horarios[i].HoraEntrada == null)
                {
                    HorConc.Add(new HorariosConcatenados { HoraId = Horarios[i].HoraId, HorarioConcatenado = "S de: " + Horarios[i].HoraSabadoEntrada + " - " + Horarios[i].HoraSabadoSalida });
                }
                else
                {
                    HorConc.Add(new HorariosConcatenados { HoraId = Horarios[i].HoraId, HorarioConcatenado = "L-V: " + Horarios[i].HoraEntrada + " - " + Horarios[i].HoraSalida + " y S de: " + Horarios[i].HoraSabadoEntrada + " - " + Horarios[i].HoraSabadoSalida });
                }
            }

            ViewData["HorariosC"] = new SelectList(HorConc, "HoraId", "HorarioConcatenado");
            return View();
        }



        [HttpPost("AddMovimientoEmpleadoHorarios")]
        public IActionResult AddMovimientoEmpleadoHorarios(MovEmpleadosHorarios meh, TimeSpan? HELV, TimeSpan? HSLV, TimeSpan? HES, TimeSpan? HSS, int NoNomina, TimeSpan? HCELV, TimeSpan? HCES, TimeSpan? HCSLV, TimeSpan? HCSS, CatHorarios hora)
        {


            try
            {
                if (meh != null)
                {


                    var Empleado = _context.CatEmpleados.Where(x => x.EmpNumero == NoNomina).FirstOrDefault();
                    if(Empleado.EmpEstId == 2)
                    {
                        return BadRequest();

                    }

                    if (Empleado != null)
                    {
                        meh.EmpHoraEmpId = Empleado.EmpId;


                        var Horario = _context.CatHorarios.Where(x => x.HoraEntrada == HELV && x.HoraSalida == HSLV && x.HoraSabadoEntrada == HES && x.HoraSabadoSalida == HSS && x.HoraComidaEntrada == HCELV && x.HoraComidaSalida == HCSLV && x.HoraSabadoComidaEntrada == HCES && x.HoraSabadoComidaSalida == HCSS).FirstOrDefault();

                        if (Horario == null)
                        {
                            hora.HoraEntrada = HELV;
                            hora.HoraSalida = HSLV;
                            hora.HoraSabadoEntrada = HES;
                            hora.HoraSabadoSalida = HSS;
                            hora.HoraComidaEntrada = HCELV;
                            hora.HoraComidaSalida = HCSLV;
                            hora.HoraSabadoComidaEntrada = HCES;
                            hora.HoraSabadoComidaSalida = HCSS;

                            _context.CatHorarios.Add(hora);
                            _context.SaveChanges();

                        }

                        var NuevoHorario = _context.CatHorarios.Where(x => x.HoraEntrada == HELV && x.HoraSalida == HSLV && x.HoraSabadoEntrada == HES && x.HoraSabadoSalida == HSS && x.HoraComidaEntrada == HCELV && x.HoraComidaSalida == HCSLV && x.HoraSabadoComidaEntrada == HCES && x.HoraSabadoComidaSalida == HCSS).FirstOrDefault();

                        meh.EmpHoraHorId = NuevoHorario.HoraId;

                        var MovHorarioAnterior = _context.MovEmpleadosHorarios.Where(x => x.EmpHoraEmpId == meh.EmpHoraEmpId && x.EmpHoraFechaHasta == null).FirstOrDefault();
                        if(MovHorarioAnterior != null && meh.EmpHoraFechaHasta == null)
                        {
                            MovHorarioAnterior.EmpHoraFechaHasta = meh.EmpHoraFechaDesde;
                            MovHorarioAnterior.EmpHoraTipoHorario = "FIJO";
                            _context.MovEmpleadosHorarios.Update(MovHorarioAnterior);
                            _context.SaveChanges();
                        }

                        if(meh.EmpHoraFechaHasta == null)
                        {
                            meh.EmpHoraTipoHorario = "ACTUAL";
                        }
                        else
                        {
                            meh.EmpHoraTipoHorario = "TEMPORAL";
                        }

                        _context.MovEmpleadosHorarios.Add(meh);
                        _context.SaveChanges();
                        var x = _context.MovEmpleadosHorarios.Include(x => x.EmpHoraEmpId).Where(x => x.EmpHoraEmpId == meh.EmpHoraEmpId).First(); // Solo en consulta, puedes poner el primer campo != de ID

                        var obj = new
                        {

                            x.EmpHoraId,
                            x.EmpHoraFechaRegistro,
                            x.EmpHoraEmpId,
                            x.EmpHoraHorId,
                            x.EmpHoraFechaDesde,
                            x.EmpHoraFechaHasta
                        };

                        return Ok(meh);
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



        // *********** MÉTODO DE ELIMINACIÓN POR MEDIO DE BOTÓN ************

        [HttpPost("DeleteMovimientos")]
        public IActionResult DeleteMovimientos(int Id)
        {
            try
            {
                var meh = _context.MovEmpleadosHorarios.Where(x => x.EmpHoraId == Id).First();
                _context.MovEmpleadosHorarios.Remove(meh);
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

        [HttpPost("EditMovimientos")]
        public async Task<IActionResult> EditMovimientos(MovEmpleadosHorarios meh)
        {

            try
            {
                var x = _context.MovEmpleadosHorarios.Where(x => x.EmpHoraId == meh.EmpHoraId).First();
                if (meh != null)
                {

                    x.EmpHoraFechaRegistro = meh.EmpHoraFechaRegistro;
                    x.EmpHoraEmpId = meh.EmpHoraEmpId;
                    x.EmpHoraHorId = meh.EmpHoraHorId;
                    x.EmpHoraFechaDesde = meh.EmpHoraFechaDesde;
                    x.EmpHoraFechaHasta = meh.EmpHoraFechaHasta;

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

        [HttpGet("MovimientosById/{Id}")]
        public IActionResult MovimientoById(int Id)
        {
            try
            {
                var ins = _context.MovEmpleadosHorarios.Where(x => x.EmpHoraId == Id).First();
                var Empleado = _context.CatEmpleados.Where(x => x.EmpId == ins.EmpHoraEmpId).First();
                var Horario = _context.CatHorarios.Where(x => x.HoraId == ins.EmpHoraHorId).First();
                var FechaRegistroCorta = ins.EmpHoraFechaRegistro.ToShortDateString();
                var FechaInicioCorta = ins.EmpHoraFechaDesde.Value.ToShortDateString();
                var FechaCortaHasta = "";
                var nombreCompleto = Empleado.EmpNombreCompleto;
                if (ins.EmpHoraFechaHasta != null)
                {
                    FechaCortaHasta = ins.EmpHoraFechaHasta.Value.ToShortDateString();
                }

                var RegistroHorario = _context.CatHorarios.Where(x => x.HoraId == ins.EmpHoraHorId).First();
                TimeSpan? CalculoHorasComidaSemana;
                TimeSpan? CalculoHorasComidaSabado;
                int HorasSemana = 0;
                int HorasSabado = 0;
                int MinutosSemana = 0;
                int MinutosSabado = 0;

                string HorarioCadena = "Lunes - Viernes: ";

                if (RegistroHorario.HoraEntrada == null)
                {
                    HorarioCadena = HorarioCadena + " No viene entre semana. \n";
                }
                else
                {
                    HorarioCadena = HorarioCadena + RegistroHorario.HoraEntrada + " - " + RegistroHorario.HoraSalida + "\n";
                }
                if (RegistroHorario.HoraSabadoEntrada == null)
                {
                    HorarioCadena = HorarioCadena + " No viene los sábados";
                }
                else
                {
                    HorarioCadena = HorarioCadena + "Sábados: " + RegistroHorario.HoraSabadoEntrada + " - " + RegistroHorario.HoraSabadoSalida + ". \n";
                }
                if (RegistroHorario.HoraComidaSalida == null)
                {
                    HorarioCadena = HorarioCadena + "Entre semana no tiene hora de comida. \n";
                }
                else
                {
                    CalculoHorasComidaSemana = RegistroHorario.HoraComidaEntrada - RegistroHorario.HoraComidaSalida;
                    HorasSemana = CalculoHorasComidaSemana.Value.Hours * -1;

                    MinutosSemana = CalculoHorasComidaSemana.Value.Minutes * -1;
                    HorarioCadena = HorarioCadena + "Entre semana tiene: " + HorasSemana + " horas de comida. Con " + MinutosSemana + " minutos. \n";
                }
                if (RegistroHorario.HoraSabadoComidaSalida == null)
                {
                    HorarioCadena = HorarioCadena + "El sábado no tiene hora de comida. \n";
                }
                else
                {
                    CalculoHorasComidaSabado = RegistroHorario.HoraSabadoComidaEntrada - RegistroHorario.HoraSabadoComidaEntrada;
                    HorasSabado = CalculoHorasComidaSabado.Value.Hours * -1;
                    MinutosSabado = CalculoHorasComidaSabado.Value.Minutes * -1;
                    HorarioCadena = HorarioCadena + " El sábado tiene: " + CalculoHorasComidaSabado + "  horas de comida. Con " + MinutosSabado + " minutos. \n";
                }

                var obj = new
                {
                    nombreCompleto,
                    FechaRegistroCorta,
                    FechaInicioCorta,
                    FechaCortaHasta,
                    HorarioCadena

                };


                return Ok(obj);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("MovimientosDetalleById/{Id}")]
        public IActionResult MovimientoDetalleById(int Id)
        {
            try
            {
                var ins = _context.MovEmpleadosHorarios.Where(x => x.EmpHoraId == Id).First();
                var Empleado = _context.CatEmpleados.Where(x => x.EmpId == ins.EmpHoraEmpId).First();
                var Horario = _context.CatHorarios.Where(x => x.HoraId == ins.EmpHoraHorId).First();
                var FechaRegistroCorta = ins.EmpHoraFechaRegistro.ToShortDateString();
                var FechaInicioCorta = ins.EmpHoraFechaDesde.Value.ToShortDateString();
                var FechaCortaHasta = "";
                var nombreCompleto = Empleado.EmpNombreCompleto;
                if (ins.EmpHoraFechaHasta != null)
                {
                    FechaCortaHasta = ins.EmpHoraFechaHasta.Value.ToShortDateString();
                }

                var RegistroHorario = _context.CatHorarios.Where(x => x.HoraId == ins.EmpHoraHorId).First();
                TimeSpan? CalculoHorasComidaSemana;
                TimeSpan? CalculoHorasComidaSabado;
                int HorasSemana = 0;
                int HorasSabado = 0;
                int MinutosSemana = 0;
                int MinutosSabado = 0;

                string HorarioCadena = "Lunes - Viernes: ";

                if (RegistroHorario.HoraEntrada == null)
                {
                    HorarioCadena = HorarioCadena + " No viene entre semana. \n";
                }
                else
                {
                    HorarioCadena = HorarioCadena + RegistroHorario.HoraEntrada + " - " + RegistroHorario.HoraSalida + "\n";
                }
                if (RegistroHorario.HoraSabadoEntrada == null)
                {
                    HorarioCadena = HorarioCadena + " No viene los sábados";
                }
                else
                {
                    HorarioCadena = HorarioCadena + "Sábados: " + RegistroHorario.HoraSabadoEntrada + " - " + RegistroHorario.HoraSabadoSalida + ". \n";
                }
                if (RegistroHorario.HoraComidaSalida == null)
                {
                    HorarioCadena = HorarioCadena + "Entre semana no tiene hora de comida. \n";
                }
                else
                {
                    CalculoHorasComidaSemana = RegistroHorario.HoraComidaEntrada - RegistroHorario.HoraComidaSalida;
                    HorasSemana = CalculoHorasComidaSemana.Value.Hours * -1;

                    MinutosSemana = CalculoHorasComidaSemana.Value.Minutes * -1;
                    HorarioCadena = HorarioCadena + "Entre semana tiene: " + HorasSemana + " horas de comida. Con " + MinutosSemana + " minutos. \n";
                }
                if (RegistroHorario.HoraSabadoComidaSalida == null)
                {
                    HorarioCadena = HorarioCadena + "El sábado no tiene hora de comida. \n";
                }
                else
                {
                    CalculoHorasComidaSabado = RegistroHorario.HoraSabadoComidaEntrada - RegistroHorario.HoraSabadoComidaEntrada;
                    HorasSabado = CalculoHorasComidaSabado.Value.Hours * -1;
                    MinutosSabado = CalculoHorasComidaSabado.Value.Minutes * -1;
                    HorarioCadena = HorarioCadena + " El sábado tiene: " + CalculoHorasComidaSabado + "  horas de comida. Con " + MinutosSabado + " minutos. \n";
                }

                var obj = new
                {
                    nombreCompleto,
                    FechaRegistroCorta,
                    FechaInicioCorta,
                    FechaCortaHasta,
                    HorarioCadena

                };


                return Ok(obj);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("PermisosMovimientos")]
        public async Task<IActionResult> PermisosMovimientos()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 12 && x.URPUserId == user.Id).ToList();
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
