using AutoFix.Application.Interfaces;
using AutoFix.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("ExpedienteVehiculo")]
    [CustomAuthorize(Roles = "Administrador,Mecanico,Cliente")]
    public class ExpedienteVehiculoController : Controller
    {
        private readonly IVehiculoService _vehiculoService;
        private readonly IExpedienteVehiculoService _expedienteService;

        public ExpedienteVehiculoController(
            IVehiculoService vehiculoService,
            IExpedienteVehiculoService expedienteService)
        {
            _vehiculoService = vehiculoService;
            _expedienteService = expedienteService;
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
                var resultadoVehiculos = _vehiculoService.GetByCliente(UsuarioId);
                ViewBag.VehiculosCliente = resultadoVehiculos.Success ? resultadoVehiculos.Value : new System.Collections.Generic.List<AutoFix.Application.DTOs.VehiculoDTO>();
            }

            if (string.IsNullOrWhiteSpace(placa))
            {
                return View();
            }

            placa = placa.Trim();

            var resultadoExpediente = _expedienteService.GetExpedienteByPlaca(placa);
            if (!resultadoExpediente.Success)
            {
                TempData["MensajeError"] = "No se encontró ningún vehículo con la placa \"" + placa + "\"";
                ViewBag.PlacaBuscada = placa;
                return View();
            }

            var expediente = resultadoExpediente.Value;

            if (Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase) && expediente.Vehiculo.ClienteId != UsuarioId)
            {
                TempData["MensajeError"] = "Ese vehículo no pertenece a su cuenta";
                ViewBag.PlacaBuscada = placa;
                return View();
            }

            ViewBag.PlacaBuscada = placa;
            ViewBag.Historial = expediente.HistorialReparaciones;

            return View(expediente.Vehiculo);
        }
    }
}
