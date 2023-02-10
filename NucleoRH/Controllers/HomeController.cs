using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NucleoRH.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;


namespace NucleoRH.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Nucleo_RHContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, Nucleo_RHContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this._context = context;
            this._userManager = userManager;
        }

        public class ValoresPermisos
        {
            public int IdMenu { get; set; }
            public int IdSubMenu { get; set; }
            public bool Authorizacion { get; set; }
        }

        [HttpGet("PermisosPorAutorizacion")]
        public async Task<IActionResult> Permisos()
        {
            bool BanderaRegistrosVacios = false;
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPUserId == user.Id).ToList();
            var EmpleadoConectado = _context.CatEmpleados.Where(x => x.EmpUserId == user.UserName).FirstOrDefault();
            List<ValoresPermisos> ListaValores = new List<ValoresPermisos>();

            if (ListaPermisos.Count == 0)
            {
                BanderaRegistrosVacios = true;


            }
            else
            {



                foreach (var item in ListaPermisos)
                {
                    ListaValores.Add(new ValoresPermisos()
                    { IdMenu = item.URPModuloId, IdSubMenu = item.URPSubModuloId, Authorizacion = item.URPAutorizado });
                }


            }

            var obj = new
            {
                ListaValores,
                BanderaRegistrosVacios,
                EmpleadoConectado.EmpNumero,
                EmpleadoConectado.EmpNombreCompleto
            };



            return Ok(obj);

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       

    }
}
