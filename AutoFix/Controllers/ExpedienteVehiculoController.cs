using AutoFix.Domain.Interfaces.Repositories;
using AutoFix.Domain.Entities;
using AutoFix.Filters;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("ExpedienteVehiculo")]
    [CustomAuthorize(Roles = "Administrador,Mecanico,Cliente")]
    public class ExpedienteVehiculoController : Controller
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IExpedienteVehiculoRepository _expedienteRepository;
        private readonly AutoFixContext _context;

        public ExpedienteVehiculoController()
        {
            _context = new AutoFixContext();
            _vehiculoRepository = new VehiculoRepository(_context);
            _expedienteRepository = new ExpedienteVehiculoRepository(_context);
        }

        private string Rol
        {
            get { return Session["UsuarioRol"]?.ToString() ?? ""; }
        }

        private int UsuarioId
        {
            get { return Convert.ToInt32(Session["UsuarioId"]); }
        }

        [HttpGet]
        public ActionResult Index(string placa)
        {

            if (Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.VehiculosCliente = _vehiculoRepository.GetVehiculosByCliente(UsuarioId).ToList();
            }

            if (string.IsNullOrWhiteSpace(placa))
            {
                return View();
            }

            placa = placa.Trim();
            var vehiculo = _vehiculoRepository.GetByPlaca(placa);

            if (vehiculo == null)
            {
                TempData["MensajeError"] = "No se encontrÃ³ ningÃºn vehÃ­culo con la placa \"" + placa + "\"";
                ViewBag.PlacaBuscada = placa;
                return View();
            }

            if (Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase) && vehiculo.ClienteId != UsuarioId)
            {
                TempData["MensajeError"] = "Ese vehÃ­culo no pertenece a su cuenta";
                ViewBag.PlacaBuscada = placa;
                return View();
            }

            var historial = _expedienteRepository.GetHistorialPorVehiculo(vehiculo.Id).ToList();

            ViewBag.PlacaBuscada = placa;
            ViewBag.Historial = historial;

            return View(vehiculo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}


