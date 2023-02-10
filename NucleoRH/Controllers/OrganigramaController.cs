using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
    public class OrganigramaController : Controller
    {
        private Nucleo_RHContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        public OrganigramaController(Nucleo_RHContext context, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            _env = env;
            this._userManager = userManager;
        }

        public class PuestoAreaDepartamento
        {
            public int PADId { get; set; }
            public string PADPuesto { get; set; }
            
        }

        public IActionResult Index()
        {
            var PAD = (from Puesto in _context.CatPuestos
                       join Area in _context.CatAreas on Puesto.PuestoAreaId equals Area.AreaId
                       join Departamento in _context.CatDepartamentos on Area.AreaDepaId equals Departamento.DepaId
                       select new PuestoAreaDepartamento
                       {
                           PADId = Puesto.PuestoId,
                           PADPuesto = Area.AreaDescripcion + ": " + Puesto.PuestoDescripcion + " - " + Departamento.DepaDescripcion,

                       }).ToList();
            
            ViewData["Puestos"] = new SelectList(PAD, "PADId", "PADPuesto");
            
            return View();
        }

        [HttpGet("SeleccionarJerarquia")]
        public IActionResult JerarquiaByPuestoId(int Id)
        {

            //var Puesto = _context.CatPuestos.Where(x => x.PuestoId == Id).First();
            //var JerarquiaPuestos = _context.CatPuestos.Where(x => x.PuestoId == Puesto.PuestoJerarquiaSuperiorPuestoId).FirstOrDefault();
            //var JerarquiaHaciaAbajo = _context.CatPuestos.Where(x => x.PuestoJerarquiaSuperiorPuestoId == Puesto.PuestoId).ToList();
            //var JerarquiasInferiores = _context.PuestosSP.FromSqlRaw<PuestosSP>("sp_ConsultaOrganigrama {0}", Puesto.PuestoId).ToList();
            //var PuestoSuperior = JerarquiaPuestos.PuestoDescripcion;


            //var obj = new
            //{
            //    Puesto.PuestoDescripcion,
            //    PuestoSuperior,
            //    JerarquiasInferiores

            //};
            //return Ok(obj);
            return Ok();
        }

        [HttpGet("RellenarPuestos")]
        public IActionResult RellenarPuestos(int Id)
        {
            
            var Puestos = _context.CatPuestos.Where(x => x.PuestoAreaId == Id).ToList();
            var obj = new
            {
                Puestos

            };
            return Ok(obj);
        }

        [HttpGet("PuestoByName")]
        public IActionResult PuestoByName(string Puesto)
        {
            var PuestoByName = _context.CatPuestos.Where(x => x.PuestoDescripcion == Puesto).FirstOrDefault();
            var obj = new
            {
                PuestoByName.PuestoId
            };
            return Ok(obj);
        }

        [HttpGet("PermisosOrganigrama")]
        public async Task<IActionResult> PermisosOrganigrama()
        {
            var user = await _userManager.FindByIdAsync((User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var ListaPermisos = _context.CatUsuariosPermisos.Where(x => x.URPSubModuloId == 1024 && x.URPUserId == user.Id).ToList();
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
