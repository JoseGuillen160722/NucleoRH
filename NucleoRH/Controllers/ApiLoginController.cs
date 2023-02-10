using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NucleoRH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace NucleoRH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiLoginController : ControllerBase
    {
        private Nucleo_RHContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        
        public ApiLoginController(SignInManager<IdentityUser> signInManager,
            ILogger<ApiLoginController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            Nucleo_RHContext context,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._context = context;
            this._config = config;
        }
        [HttpGet("Login")]
        public async Task<IActionResult> Login(string email, string pass)
        {
            try
            {
                IdentityUser user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var Roles = await _userManager.GetRolesAsync(user);

                    var rol = Roles[0];

                    if (rol != "Admin" )
                        return BadRequest("No cuenta con permisos para el uso de la aplicación.");

                    var result = await _signInManager.PasswordSignInAsync(email, pass, false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        var userSession = _context.Users.Where(x => x.Email == email).Select(s => new UserSession
                        {
                            Id = s.Id,
                            UserName = s.UserName,
                            //Nombre = s.Nombre,
                            //Apellidos = s.Apellidos,
                            Email = email,
                            Pass = pass, 
                            Rol = rol
                        }).FirstOrDefault();

                       
                        return Ok(userSession);
                    }
                    else
                    {
                        return BadRequest("Usuario y/o contraseña incorrectos");
                    }
                }
                else
                    return BadRequest("Usuario y/o contraseña incorrectos");
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }


        //private string GenerateToken(IdentityUser user)
        //{
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.UserName),
        //        new Claim(ClaimTypes.Email, user.Email)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //    var securityToken = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(60),
        //        signingCredentials: creds);

        //    string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        //    return token;
            
        //}

        public class UserSession
        { 
            public string Id { get; set; } 
            public string UserName { get; set; } 
            public string Nombre { get; set; } 
            public string Apellidos { get; set; } 
            public string Email { get; set; }
            public string Pass { get; set; }
            public string Rol { get; set; } 
            public string Nombrecompleto { get { return Nombre + " " + Apellidos; } }
        }


        public async Task<IActionResult> LogOut(string returnUrl = null)
        {
            returnUrl = "/Identity/Account/Login?ReturnUrl=%2F";
            await _signInManager.SignOutAsync();

           
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage(returnUrl);
            }

        }


        //public async Task<IActionResult> ForgotPassword(IdentityUser Usuario)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(Usuario.Email);

        //        string code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //        await _userManager.SendEmailAsync(user.Id, "ResetPassword", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //        return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

            


        //    return Ok();
        //}

    }
}
