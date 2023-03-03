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
    public class UsuariosPermisosController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        



        public UsuariosPermisosController(Nucleo_RHContext context, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;


        }


        public IActionResult Index()
        {
            ViewBag.URP = _context.CatUsuariosPermisos.OrderBy(x => x.URPUserId).ToList();
            var Usuarios = _userManager.Users.ToList();
            ViewData["Usuarios"] = new SelectList(Usuarios, "Id", "UserName");
            var Roles = _roleManager.Roles.OrderBy(x => x.Name).ToList();
            ViewData["Roles"] = new SelectList(Roles, "Id", "Name");
            var Modulos = _context.CatModulos.OrderBy(x => x.ModuloId).ToList();
            ViewData["Modulos"] = new SelectList(Modulos, "ModuloId", "ModuloNombre");
            var SubModulos = _context.CatSubModulos.OrderBy(x => x.SubMId).ToList();
            ViewData["SubModulos"] = new SelectList(SubModulos, "SubMId", "SubMName");
            var ModCatalogos = _context.CatSubModulos.Where(x => x.SubMModuloId == 7).ToList();
            ViewBag.Cat = ModCatalogos;
            var ModEmpleados = _context.CatSubModulos.Where(x => x.SubMModuloId == 5).ToList();
            ViewBag.Emp = ModEmpleados;
            var ModCH = _context.CatSubModulos.Where(x => x.SubMModuloId == 4).ToList();
            ViewBag.CH = ModCH;
            var ModAdmin = _context.CatSubModulos.Where(x => x.SubMModuloId == 2).ToList();
            ViewBag.Admin = ModAdmin;
            var ModReportes = _context.CatSubModulos.Where(x => x.SubMModuloId == 3).ToList();
            ViewBag.Reportes = ModReportes;
            var Submodulos = _context.CatSubModulos.OrderBy(x => x.SubMId).ToList();
            ViewBag.SubM = Submodulos;

            ViewBag.UR = _context.UserRoles.OrderBy(x => x.UserId).ToList();

            ViewBag.User = _userManager.Users.ToList();







            return View();
        }



        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        //*-*-*-*-*-*-*-* MÉTODO DE INSERCIÓN PARA EL APARTADO DE SUBMÓDULOS CORRESPONDIENTES A CATÁLOGO *-*-*-*-*-*-*-*

        [HttpPost("AddURP")]
        public IActionResult AddURP(CatUsuariosPermisos URP, int[] URPSubIdCatalogos, bool[] URPCrearCatalogos, bool[] URPMostrarCatalogos, bool[] URPModificarCatalogos, bool[] URPEliminarCatalogos, /* Fin del módulo de Catálogos */ int[] URPSubModulosEmpleados, bool[] URPCrearEmpleados, bool[] URPMostrarEmpleados, bool[] URPModificarEmpleados, bool[] URPEliminarEmpleados, /* Fin del módulo de empleados */ int[] URPSubModulosCH, bool[] URPCrearCH, bool[] URPMostrarCH, bool[] URPModificarCH, bool[] URPEliminarCH /* Fin del módulo de Capital Humano*/, int[] URPSubModulosAdmin, bool[] URPCrearAdmin, bool[] URPMostrarAdmin, bool[] URPModificarAdmin, bool[] URPEliminarAdmin /* Fin del módulo de administración */ , int[] URPSubModulosReportes, bool[] URPCrearReportes, bool[] URPMostrarReportes, bool[] URPModificarReportes, bool[] URPEliminarReportes /* Fin del módulo de reportes */ )
        {
            try
            {
                if (URP != null)
                {
                    var TodosLosPermisosDelUsuario = _context.CatUsuariosPermisos.Where(x => x.URPUserId == URP.URPUserId).ToList();
                    if (TodosLosPermisosDelUsuario != null)
                    {
                        for (var i = 0; i < URPSubIdCatalogos.Length; i++) // Adición de elementos del submódulo de catálogos
                        {
                            
                                var ListaURPCatalogos = new CatUsuariosPermisos();
                                ListaURPCatalogos.URPUserId = URP.URPUserId;
                                ListaURPCatalogos.URPModuloId = 7; // Podría dejarse como URP.ModuloId = 7 porque ese número le corresponde en la tabla de Modulos.
                                ListaURPCatalogos.URPSubModuloId = URPSubIdCatalogos[i];
                                ListaURPCatalogos.URPCrear = URPCrearCatalogos[i];
                                ListaURPCatalogos.URPMostrar = URPMostrarCatalogos[i];
                                ListaURPCatalogos.URPModificar = URPModificarCatalogos[i];
                                ListaURPCatalogos.URPEliminar = URPEliminarCatalogos[i];
                                if (URPCrearCatalogos[i] != false || URPMostrarCatalogos[i] != false || URPModificarCatalogos[i] != false || URPEliminarCatalogos[i] != false)
                                {
                                    ListaURPCatalogos.URPAutorizado = true;
                                }
                                else
                                {
                                    ListaURPCatalogos.URPAutorizado = false;
                                }

                                var RegistroExistente = _context.CatUsuariosPermisos.Where(x => x.URPUserId == ListaURPCatalogos.URPUserId && x.URPModuloId == ListaURPCatalogos.URPModuloId && x.URPSubModuloId == ListaURPCatalogos.URPSubModuloId).FirstOrDefault();
                                if (RegistroExistente == null)
                                {
                                    _context.CatUsuariosPermisos.Add(ListaURPCatalogos);
                                    _context.SaveChanges();
                                }
                                
                                    
                                
                            

                        }

                        for (var i = 0; i < URPSubModulosEmpleados.Length; i++)
                        {
                            var ListaURPEmpleados = new CatUsuariosPermisos();
                            ListaURPEmpleados.URPUserId = URP.URPUserId;
                            ListaURPEmpleados.URPModuloId = 5; // Podría dejarse como URP.ModuloId = 5 porque es el número que le corresponde en la tabla de módulos
                            ListaURPEmpleados.URPSubModuloId = URPSubModulosEmpleados[i];
                            ListaURPEmpleados.URPCrear = URPCrearEmpleados[i];
                            ListaURPEmpleados.URPMostrar = URPMostrarEmpleados[i];
                            ListaURPEmpleados.URPModificar = URPModificarEmpleados[i];
                            ListaURPEmpleados.URPEliminar = URPEliminarEmpleados[i];
                            if (URPCrearEmpleados[i] != false || URPMostrarEmpleados[i] != false || URPModificarEmpleados[i] != false || URPEliminarEmpleados[i] != false)
                            {
                                ListaURPEmpleados.URPAutorizado = true;
                            }
                            else
                            {
                                ListaURPEmpleados.URPAutorizado = false;
                            }
                            var RegistroExistente = _context.CatUsuariosPermisos.Where(x => x.URPUserId == ListaURPEmpleados.URPUserId && x.URPModuloId == ListaURPEmpleados.URPModuloId && x.URPSubModuloId == ListaURPEmpleados.URPSubModuloId).FirstOrDefault();
                            if (RegistroExistente == null)
                            {
                                _context.CatUsuariosPermisos.Add(ListaURPEmpleados);
                                _context.SaveChanges();
                            }
                        }

                        for (var i = 0; i < URPSubModulosCH.Length; i++)
                        {
                            var ListaURPCH = new CatUsuariosPermisos();
                            ListaURPCH.URPUserId = URP.URPUserId;
                            ListaURPCH.URPModuloId = 4; // Podría dejarse como URP.ModuloId = 4 porque es el número que le corresponde en la tabla de módulos
                            ListaURPCH.URPSubModuloId = URPSubModulosCH[i];
                            ListaURPCH.URPCrear = URPCrearCH[i];
                            ListaURPCH.URPMostrar = URPMostrarCH[i];
                            ListaURPCH.URPModificar = URPModificarCH[i];
                            ListaURPCH.URPEliminar = URPEliminarCH[i];
                            if (URPCrearCH[i] != false || URPMostrarCH[i] != false || URPModificarCH[i] != false || URPEliminarCH[i] != false)
                            {
                                ListaURPCH.URPAutorizado = true;
                            }
                            else
                            {
                                ListaURPCH.URPAutorizado = false;
                            }
                            var RegistroExistente = _context.CatUsuariosPermisos.Where(x => x.URPUserId == ListaURPCH.URPUserId && x.URPModuloId == ListaURPCH.URPModuloId && x.URPSubModuloId == ListaURPCH.URPSubModuloId).FirstOrDefault();
                            if (RegistroExistente == null)
                            {
                                _context.CatUsuariosPermisos.Add(ListaURPCH);
                                _context.SaveChanges();
                            }

                            
                        }

                        for (var i = 0; i < URPSubModulosAdmin.Length; i++)
                        {
                            var ListaURPAdmin = new CatUsuariosPermisos();
                            ListaURPAdmin.URPUserId = URP.URPUserId;
                            ListaURPAdmin.URPModuloId = 2; // Podría dejarse como URP.ModuloId = 2 porque es el número que le corresponde en la tabla de módulos
                            ListaURPAdmin.URPSubModuloId = URPSubModulosAdmin[i];
                            ListaURPAdmin.URPCrear = URPCrearAdmin[i];
                            ListaURPAdmin.URPMostrar = URPMostrarAdmin[i];
                            ListaURPAdmin.URPModificar = URPModificarAdmin[i];
                            ListaURPAdmin.URPEliminar = URPEliminarAdmin[i];
                            if (URPCrearAdmin[i] != false || URPMostrarAdmin[i] != false || URPModificarAdmin[i] != false || URPEliminarAdmin[i] != false)
                            {
                                ListaURPAdmin.URPAutorizado = true;
                            }
                            else
                            {
                                ListaURPAdmin.URPAutorizado = false;
                            }
                            var RegistroExistente = _context.CatUsuariosPermisos.Where(x => x.URPUserId == ListaURPAdmin.URPUserId && x.URPModuloId == ListaURPAdmin.URPModuloId && x.URPSubModuloId == ListaURPAdmin.URPSubModuloId).FirstOrDefault();
                            if (RegistroExistente == null)
                            {
                                _context.CatUsuariosPermisos.Add(ListaURPAdmin);
                                _context.SaveChanges();
                            }

                        }

                        for (var i = 0; i < URPSubModulosReportes.Length; i++)
                        {
                            var ListaURPReportes = new CatUsuariosPermisos();
                            ListaURPReportes.URPUserId = URP.URPUserId;
                            ListaURPReportes.URPModuloId = 3; // Podría dejarse como URP.ModuloId = 3 porque es el número que le corresponde en la tabla de módulos
                            ListaURPReportes.URPSubModuloId = URPSubModulosReportes[i];
                            ListaURPReportes.URPCrear = URPCrearReportes[i];
                            ListaURPReportes.URPMostrar = URPMostrarReportes[i];
                            ListaURPReportes.URPModificar = URPModificarReportes[i];
                            ListaURPReportes.URPEliminar = URPEliminarReportes[i];
                            if (URPCrearReportes[i] != false || URPMostrarReportes[i] != false || URPModificarReportes[i] != false || URPEliminarReportes[i] != false)
                            {
                                ListaURPReportes.URPAutorizado = true;
                            }
                            else
                            {
                                ListaURPReportes.URPAutorizado = false;
                            }
                            var RegistroExistente = _context.CatUsuariosPermisos.Where(x => x.URPUserId == ListaURPReportes.URPUserId && x.URPModuloId == ListaURPReportes.URPModuloId && x.URPSubModuloId == ListaURPReportes.URPSubModuloId).FirstOrDefault();
                            if (RegistroExistente == null)
                            {
                                _context.CatUsuariosPermisos.Add(ListaURPReportes);
                                _context.SaveChanges();
                            }
                        }
                        _context.SaveChanges();
                        var x = _context.CatUsuariosPermisos.Where(x => x.URPUserId == URP.URPUserId).First();
                        var obj = new
                        {
                            x.URPId,
                            x.URPUserId,
                            x.URPModuloId,
                            x.URPCrear,
                            x.URPModificar,
                            x.URPMostrar,
                            x.URPEliminar

                        };
                        return Ok(URP);
                    }
                    else
                    {
                        for (var i = 0; i < URPSubIdCatalogos.Length; i++) // Adición de elementos del submódulo de catálogos
                        {
                            var ListaURPCatalogos = new CatUsuariosPermisos();
                            ListaURPCatalogos.URPUserId = URP.URPUserId;
                            ListaURPCatalogos.URPModuloId = 7; // Podría dejarse como URP.ModuloId = 7 porque ese número le corresponde en la tabla de Modulos.
                            ListaURPCatalogos.URPSubModuloId = URPSubIdCatalogos[i];
                            ListaURPCatalogos.URPCrear = URPCrearCatalogos[i];
                            ListaURPCatalogos.URPMostrar = URPMostrarCatalogos[i];
                            ListaURPCatalogos.URPModificar = URPModificarCatalogos[i];
                            ListaURPCatalogos.URPEliminar = URPEliminarCatalogos[i];
                            if (URPCrearCatalogos[i] != false || URPMostrarCatalogos[i] != false || URPModificarCatalogos[i] != false || URPEliminarCatalogos[i] != false)
                            {
                                ListaURPCatalogos.URPAutorizado = true;
                            }
                            else
                            {
                                ListaURPCatalogos.URPAutorizado = false;
                            }



                            _context.CatUsuariosPermisos.Add(ListaURPCatalogos);
                            _context.SaveChanges();

                        }

                        for (var i = 0; i < URPSubModulosEmpleados.Length; i++)
                        {
                            var ListaURPEmpleados = new CatUsuariosPermisos();
                            ListaURPEmpleados.URPUserId = URP.URPUserId;
                            ListaURPEmpleados.URPModuloId = 5; // Podría dejarse como URP.ModuloId = 5 porque es el número que le corresponde en la tabla de módulos
                            ListaURPEmpleados.URPSubModuloId = URPSubModulosEmpleados[i];
                            ListaURPEmpleados.URPCrear = URPCrearEmpleados[i];
                            ListaURPEmpleados.URPMostrar = URPMostrarEmpleados[i];
                            ListaURPEmpleados.URPModificar = URPModificarEmpleados[i];
                            ListaURPEmpleados.URPEliminar = URPEliminarEmpleados[i];
                            if (URPCrearEmpleados[i] != false || URPMostrarEmpleados[i] != false || URPModificarEmpleados[i] != false || URPEliminarEmpleados[i] != false)
                            {
                                ListaURPEmpleados.URPAutorizado = true;
                            }
                            else
                            {
                                ListaURPEmpleados.URPAutorizado = false;
                            }
                            _context.CatUsuariosPermisos.Add(ListaURPEmpleados);
                            _context.SaveChanges();
                        }

                        for (var i = 0; i < URPSubModulosCH.Length; i++)
                        {
                            var ListaURPCH = new CatUsuariosPermisos();
                            ListaURPCH.URPUserId = URP.URPUserId;
                            ListaURPCH.URPModuloId = 4; // Podría dejarse como URP.ModuloId = 4 porque es el número que le corresponde en la tabla de módulos
                            ListaURPCH.URPSubModuloId = URPSubModulosCH[i];
                            ListaURPCH.URPCrear = URPCrearCH[i];
                            ListaURPCH.URPMostrar = URPMostrarCH[i];
                            ListaURPCH.URPModificar = URPModificarCH[i];
                            ListaURPCH.URPEliminar = URPEliminarCH[i];
                            if (URPCrearCH[i] != false || URPMostrarCH[i] != false || URPModificarCH[i] != false || URPEliminarCH[i] != false)
                            {
                                ListaURPCH.URPAutorizado = true;
                            }
                            else
                            {
                                ListaURPCH.URPAutorizado = false;
                            }
                            _context.CatUsuariosPermisos.Add(ListaURPCH);
                            _context.SaveChanges();
                        }

                        for (var i = 0; i < URPSubModulosAdmin.Length; i++)
                        {
                            var ListaURPAdmin = new CatUsuariosPermisos();
                            ListaURPAdmin.URPUserId = URP.URPUserId;
                            ListaURPAdmin.URPModuloId = 2; // Podría dejarse como URP.ModuloId = 2 porque es el número que le corresponde en la tabla de módulos
                            ListaURPAdmin.URPSubModuloId = URPSubModulosAdmin[i];
                            ListaURPAdmin.URPCrear = URPCrearAdmin[i];
                            ListaURPAdmin.URPMostrar = URPMostrarAdmin[i];
                            ListaURPAdmin.URPModificar = URPModificarAdmin[i];
                            ListaURPAdmin.URPEliminar = URPEliminarAdmin[i];
                            if (URPCrearAdmin[i] != false || URPMostrarAdmin[i] != false || URPModificarAdmin[i] != false || URPEliminarAdmin[i] != false)
                            {
                                ListaURPAdmin.URPAutorizado = true;
                            }
                            else
                            {
                                ListaURPAdmin.URPAutorizado = false;
                            }
                            _context.CatUsuariosPermisos.Add(ListaURPAdmin);
                            _context.SaveChanges();
                        }

                        for (var i = 0; i < URPSubModulosReportes.Length; i++)
                        {
                            var ListaURPReportes = new CatUsuariosPermisos();
                            ListaURPReportes.URPUserId = URP.URPUserId;
                            ListaURPReportes.URPModuloId = 3; // Podría dejarse como URP.ModuloId = 3 porque es el número que le corresponde en la tabla de módulos
                            ListaURPReportes.URPSubModuloId = URPSubModulosReportes[i];
                            ListaURPReportes.URPCrear = URPCrearReportes[i];
                            ListaURPReportes.URPMostrar = URPMostrarReportes[i];
                            ListaURPReportes.URPModificar = URPModificarReportes[i];
                            ListaURPReportes.URPEliminar = URPEliminarReportes[i];
                            if (URPCrearReportes[i] != false || URPMostrarReportes[i] != false || URPModificarReportes[i] != false || URPEliminarReportes[i] != false)
                            {
                                ListaURPReportes.URPAutorizado = true;
                            }
                            else
                            {
                                ListaURPReportes.URPAutorizado = false;
                            }
                            _context.CatUsuariosPermisos.Add(ListaURPReportes);
                            _context.SaveChanges();
                        }
                        _context.SaveChanges();
                        var x = _context.CatUsuariosPermisos.Where(x => x.URPUserId == URP.URPUserId).First();
                        var obj = new
                        {
                            x.URPId,
                            x.URPUserId,
                            x.URPModuloId,
                            x.URPCrear,
                            x.URPModificar,
                            x.URPMostrar,
                            x.URPEliminar

                        };
                        return Ok(URP);
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


        public class ModeloParaVisualizacionDeTabla
        {
            public string IdUsuario { get; set; }
            public int IdPermiso { get; set; }
            public int IdSubModulo { get; set; }
            public string NombreSubModulo { get; set; }
            public bool TemporalUrpCrear { get; set; }
            public bool TemporalUrpMostrar { get; set; }
            public bool TemporalUrpEliminar { get; set; }
            public bool TemporalUrpModificar { get; set; }
            public int IdModulo { get; set; }

        }

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [HttpGet("URPById/{Id}")]
        public IActionResult URPById(string Id)
        {
            try
            {


                var ins = _context.CatUsuariosPermisos.Where(x => x.URPUserId == Id).ToList();
                var tabla = _context.CatUsuariosPermisos.Where(x => x.URPUserId == ins[0].URPUserId).ToList();
                var submodulos = _context.CatSubModulos.OrderBy(x => x.SubMId).ToList();
                if (ins.Count > 0)
                {

                    List<ModeloParaVisualizacionDeTabla> Tablita = new List<ModeloParaVisualizacionDeTabla>();


                    for (int i = 0; i < submodulos.Count; i++)
                    {
                        for (var j = 0; j < tabla.Count; j++)
                        {
                            if (tabla[j].URPSubModuloId == submodulos[i].SubMId)
                            {
                                Tablita.Add(new ModeloParaVisualizacionDeTabla
                                {
                                    IdUsuario = tabla[j].URPUserId,
                                    IdPermiso = tabla[j].URPId,
                                    IdSubModulo = submodulos[i].SubMId,
                                    IdModulo = tabla[j].URPModuloId,
                                    NombreSubModulo = submodulos[i].SubMName,
                                    TemporalUrpCrear = tabla[j].URPCrear,
                                    TemporalUrpMostrar = tabla[j].URPMostrar,
                                    TemporalUrpEliminar = tabla[j].URPEliminar,
                                    TemporalUrpModificar = tabla[j].URPModificar
                                });
                            }
                        }
                    }




                    return Ok(Tablita);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // ****************** MÉTODO DE MODIFICACIÓN - GUARDAR CAMBIOS ******************* 

        [HttpPost("EditURP")]
        public async Task<IActionResult> EditURP(CatUsuariosPermisos URP, int[] IdCat, int[] SubMCat, bool[] CatCrear, bool[] CatMostrar, bool[] CatModificar, bool[] CatEliminar, int[] IdEmp, int[] SubMEmp, bool[] EmpCrear, bool[] EmpMostrar, bool[] EmpModificar, bool[] EmpEliminar, int[] IdCapH, int[] SubMCapH, bool[] CapHCrear, bool[] CapHMostrar, bool[] CapHModificar, bool[] CapHEliminar, int[] AdminId, int[] AdminSubModulo, bool[] CrearAdmin, bool[] MostrarAdmin, bool[] ModificarAdmin, bool[] EliminarAdmin, int[] ReportesId, int[] ReportesSubModulos, bool[] RepCrear, bool[] RepMostrar, bool[] RepModificar, bool[] RepEliminar)
        {
            try
            {
                // Edición del método de catálogos
                for (int i = 0; i < IdCat.Length; i++)
                {
                    var x = _context.CatUsuariosPermisos.Where(x => x.URPId == IdCat[i]).First();
                    if (URP != null)
                    {
                        x.URPUserId = URP.URPUserId;
                        x.URPModuloId = 7; // Catálogos es el 7, se queda exactamente igual
                        x.URPSubModuloId = x.URPSubModuloId;
                        x.URPCrear = CatCrear[i];
                        x.URPMostrar = CatMostrar[i];
                        x.URPModificar = CatModificar[i];
                        x.URPEliminar = CatEliminar[i];
                        if (CatCrear[i] != false || CatMostrar[i] != false || CatModificar[i] != false || CatEliminar[i] != false)
                        {
                            x.URPAutorizado = true;
                        }
                        else
                        {
                            x.URPAutorizado = false;
                        }

                        _context.Entry(x).State = EntityState.Modified;
                        await _context.SaveChangesAsync();


                    }
                    else
                    {
                        return BadRequest();
                    }
                }

                // Edición del método de Empleados
                for (int i = 0; i < IdEmp.Length; i++)
                {
                    var x = _context.CatUsuariosPermisos.Where(x => x.URPId == IdEmp[i]).First();
                    if (URP != null)
                    {
                        x.URPUserId = URP.URPUserId;
                        x.URPModuloId = 5; // Catálogos es el 7, se queda exactamente igual
                        x.URPSubModuloId = x.URPSubModuloId;
                        x.URPCrear = EmpCrear[i];
                        x.URPMostrar = EmpMostrar[i];
                        x.URPModificar = EmpModificar[i];
                        x.URPEliminar = EmpEliminar[i];
                        if (EmpCrear[i] != false || EmpMostrar[i] != false || EmpModificar[i] != false || EmpEliminar[i] != false)
                        {
                            x.URPAutorizado = true;
                        }
                        else
                        {
                            x.URPAutorizado = false;
                        }

                        _context.Entry(x).State = EntityState.Modified;
                        await _context.SaveChangesAsync();


                    }
                    else
                    {
                        return BadRequest();
                    }
                }

                // Edición de datos para Capital Humano
                for (int i = 0; i < IdCapH.Length; i++)
                {
                    var x = _context.CatUsuariosPermisos.Where(x => x.URPId == IdCapH[i]).First();
                    if (URP != null)
                    {
                        x.URPUserId = URP.URPUserId;
                        x.URPModuloId = 4; // Catálogos es el 7, se queda exactamente igual
                        x.URPSubModuloId = x.URPSubModuloId;
                        x.URPCrear = CapHCrear[i];
                        x.URPMostrar = CapHMostrar[i];
                        x.URPModificar = CapHModificar[i];
                        x.URPEliminar = CapHEliminar[i];
                        if (CapHCrear[i] != false || CapHMostrar[i] != false || CapHModificar[i] != false || CapHEliminar[i] != false)
                        {
                            x.URPAutorizado = true;
                        }
                        else
                        {
                            x.URPAutorizado = false;
                        }

                        _context.Entry(x).State = EntityState.Modified;
                        await _context.SaveChangesAsync();


                    }
                    else
                    {
                        return BadRequest();
                    }
                }

                // Edición de datos para Administración

                for (int i = 0; i < AdminId.Length; i++)
                {
                    var x = _context.CatUsuariosPermisos.Where(x => x.URPId == AdminId[i]).First();
                    if (URP != null)
                    {
                        x.URPUserId = URP.URPUserId;
                        x.URPModuloId = 2; // Catálogos es el 7, se queda exactamente igual
                        x.URPSubModuloId = x.URPSubModuloId;
                        x.URPCrear = CrearAdmin[i];
                        x.URPMostrar = MostrarAdmin[i];
                        x.URPModificar = ModificarAdmin[i];
                        x.URPEliminar = EliminarAdmin[i];
                        if (CrearAdmin[i] != false || MostrarAdmin[i] != false || ModificarAdmin[i] != false || EliminarAdmin[i] != false)
                        {
                            x.URPAutorizado = true;
                        }
                        else
                        {
                            x.URPAutorizado = false;
                        }

                        _context.Entry(x).State = EntityState.Modified;
                        await _context.SaveChangesAsync();


                    }
                    else
                    {
                        return BadRequest();
                    }
                }

                // Método de edición para Reportes

                for (int i = 0; i < ReportesId.Length; i++)
                {
                    var x = _context.CatUsuariosPermisos.Where(x => x.URPId == ReportesId[i]).First();
                    if (URP != null)
                    {
                        x.URPUserId = URP.URPUserId;
                        x.URPModuloId = 3; // Catálogos es el 7, se queda exactamente igual
                        x.URPSubModuloId = x.URPSubModuloId;
                        x.URPCrear = RepCrear[i];
                        x.URPMostrar = RepMostrar[i];
                        x.URPModificar = RepModificar[i];
                        x.URPEliminar = RepEliminar[i];
                        if (RepCrear[i] != false || RepMostrar[i] != false || RepModificar[i] != false || RepEliminar[i] != false)
                        {
                            x.URPAutorizado = true;
                        }
                        else
                        {
                            x.URPAutorizado = false;
                        }

                        _context.Entry(x).State = EntityState.Modified;
                        await _context.SaveChangesAsync();


                    }
                    else
                    {
                        return BadRequest();
                    }
                }

                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("PermisosPermisos")]
        public async Task<IActionResult> PermisosPermisos()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 15 && x.URPUserId == user.Id).ToList();
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
