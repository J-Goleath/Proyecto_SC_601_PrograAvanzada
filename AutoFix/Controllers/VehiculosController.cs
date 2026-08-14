using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Application.Validators;
using AutoFix.Filters;
using FluentValidation.Mvc;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("Vehiculos")]
    [CustomAuthorize(Roles = "Administrador")]
    public class VehiculosController : Controller
    {
        private readonly IVehiculoService _vehiculoService;
        private readonly IClienteService _clienteService;

        public VehiculosController(IVehiculoService vehiculoService, IClienteService clienteService)
        {
            _vehiculoService = vehiculoService;
            _clienteService = clienteService;
        }

        private void CargarClientes(int? clienteSeleccionado = null)
        {
            var clientes = _clienteService.GetAll();
            ViewBag.Clientes = new SelectList(clientes.Value, "Id", "Nombre", clienteSeleccionado);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var resultado = _vehiculoService.GetAll();
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return View(Enumerable.Empty<VehiculoDTO>());
            }
            return View(resultado.Value);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CargarClientes();
            return View(new CreateVehiculoDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateVehiculoDTO dto)
        {
            var validacion = new CreateVehiculoDTOValidator().Validate(dto);
            if (!validacion.IsValid)
            {
                validacion.AddToModelState(ModelState, null);
            }

            if (ModelState.IsValid)
            {
                var resultado = _vehiculoService.Create(dto);
                if (resultado.Success)
                {
                    TempData["MensajeExito"] = "Vehículo registrado correctamente";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", resultado.Error);
            }

            CargarClientes(dto.ClienteId);
            return View(dto);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var resultado = _vehiculoService.GetById(id);
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return RedirectToAction(nameof(Index));
            }
            return View(resultado.Value);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var resultado = _vehiculoService.GetById(id);
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return RedirectToAction(nameof(Index));
            }

            var vehiculo = resultado.Value;
            var dto = new UpdateVehiculoDTO
            {
                Id = vehiculo.Id,
                Placa = vehiculo.Placa,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Anio = vehiculo.Anio,
                Color = vehiculo.Color,
                ClienteId = vehiculo.ClienteId
            };

            CargarClientes(vehiculo.ClienteId);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateVehiculoDTO dto)
        {
            var validacion = new UpdateVehiculoDTOValidator().Validate(dto);
            if (!validacion.IsValid)
            {
                validacion.AddToModelState(ModelState, null);
            }

            if (ModelState.IsValid)
            {
                var resultado = _vehiculoService.Update(dto);
                if (resultado.Success)
                {
                    TempData["MensajeExito"] = "Vehículo actualizado correctamente";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", resultado.Error);
            }

            CargarClientes(dto.ClienteId);
            return View(dto);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var resultado = _vehiculoService.GetById(id);
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return RedirectToAction(nameof(Index));
            }
            return View(resultado.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var resultado = _vehiculoService.Delete(id);
            if (resultado.Success)
            {
                TempData["MensajeExito"] = "Vehículo eliminado correctamente";
            }
            else
            {
                TempData["MensajeError"] = resultado.Error;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
