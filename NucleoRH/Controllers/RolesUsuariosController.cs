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
    public class RolesUsuariosController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesUsuariosController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
            _roleManager = roleManager;
        }


        public class ListaRolesUsuarios
        {
            public string IdUsuario { get; set; }
            public string UserName { get; set; }
            public string RoleName { get; set; }
            public string NombreCompleto { get; set; }
           
        }
        public IActionResult Index()
        {
            var UsuarioRoles = (from UR in _context.UserRoles
                                join user in _userManager.Users on UR.UserId equals user.Id
                                join roles in _roleManager.Roles on UR.RoleId equals roles.Id
                                join Emp in _context.CatEmpleados on user.UserName equals Emp.EmpUserId
                                select new ListaRolesUsuarios
                                {
                                    IdUsuario = user.Id,
                                    UserName = user.UserName,
                                    RoleName = roles.Name,
                                    NombreCompleto = Emp.EmpNombreCompleto
                                }).ToList();

            ViewBag.UserRoles = UsuarioRoles;

            var Perfiles = _roleManager.Roles.OrderBy(x => x.Name);
            ViewData["Perfiles"] = new SelectList(Perfiles, "Id", "Name");
            return View();
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddRolUsuario")]
        public async Task<IActionResult> AddRolUsuario(string userid, string roleid )
        {
            try
            {
                
                var rol = _roleManager.Roles.Where(x => x.Id == roleid).FirstOrDefault();
                var user = await _userManager.FindByNameAsync(userid);

                await _userManager.AddToRoleAsync(user, rol.Name);
                
                return Ok();
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.StackTrace);
            }
        }

        // MÉTODO QUE RECIBE EL OBJETO PARA GUARDARLO EN LA BASE DE DATOS

        [HttpPost("AddRol")]
        public async Task<IActionResult> AddRol(string roleid)
        {
            try
            {

                if(!(await _roleManager.RoleExistsAsync(roleid)))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleid));
                }

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.StackTrace);
            }
        }

        // *********** MÉTODO DE ELIMINACIÓN POR MEDIO DE BOTÓN ************

        [HttpPost("DeletePerfilUsuario")]
        public async Task<IActionResult> DeletePerfilUsuario(string Id, string IdRol)
        {
            try
            {
                
                var user = await _userManager.FindByNameAsync(Id);
                await _userManager.RemoveFromRoleAsync(user, IdRol);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("PermisosRolesUsuarios")]
        public async Task<IActionResult> PermisosRolesUsuarios()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 1021 && x.URPUserId == user.Id).ToList();
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
