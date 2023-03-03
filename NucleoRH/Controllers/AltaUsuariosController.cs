using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NucleoRH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Security.Claims;

namespace NucleoRH.Controllers
{
    [Authorize]
    public class AltaUsuariosController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        




        public AltaUsuariosController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.Usuarios = _userManager.Users.OrderBy(x => x.Id).ToList();

            return View();
        }

        [HttpPost("AddUsuario")]
        public async Task<IActionResult> AddUsuarios(IdentityUser user)
        {
            try
            {
                if (user != null)
                {
                   
                    var usuario = new IdentityUser { UserName = user.UserName, Email = user.Email };
                    var result = await _userManager.CreateAsync(usuario, user.PasswordHash);



                    
                    return Ok();
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

        [HttpPost("DeleteUsuario")]
        public IActionResult DeleteUsuario(string Id)
        {
            try
            {
                var usuario = _context.Users.Where(x => x.Id == Id).First();
                _context.Users.Remove(usuario);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("PermisosAltaUsuarios")]
        public async Task<IActionResult> PermisosDepartamentos()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 1020 && x.URPUserId == user.Id).ToList();
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

        // ************************* MÉTODO DE MODIFICACIÓN - VALIDACIÓN DE LOS ID POR BÚSQUEDA *************************

        [HttpGet("UsuarioById/{Id}")]
        public IActionResult UsuarioById(string Id)
        {
            try
            {
                
                var usuario = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
                
                return Ok(usuario);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost("EditPassword")]
        public async Task<IActionResult> EditPassword(string NumNomina, string Password)
        {
            try
            {
                

                if (NumNomina != null)
                {

                    var user = await _userManager.FindByNameAsync(NumNomina);
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, code, Password);

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

    }
}
