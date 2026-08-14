using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Application.Validators;
using AutoFix.Filters;
using FluentValidation.Mvc;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("Citas")]
    [CustomAuthorize(Roles = "Cliente")]
    public class CitasController : Controller
    {
        private readonly ICitaService _citaService;
        private readonly IVehiculoService _vehiculoService;
        private readonly INotificacionService _notificacionService;

        public CitasController(
            ICitaService citaService,
            IVehiculoService vehiculoService,
            INotificacionService notificacionService)
        {
            _citaService = citaService;
            _vehiculoService = vehiculoService;
            _notificacionService = notificacionService;
        }

        private int ClienteId
        {
            get { return Convert.ToInt32(Session["UsuarioId"]); }
        }

        [HttpGet]
        public ActionResult Index()
        {
            var resultado = _citaService.GetByCliente(ClienteId);
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return View(Enumerable.Empty<CitaDTO>());
            }
            return View(resultado.Value);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CargarVehiculosEnViewBag();
            return View(new CreateCitaDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCitaDTO dto)
        {
            var validacion = new CreateCitaDTOValidator().Validate(dto);
            if (!validacion.IsValid)
            {
                validacion.AddToModelState(ModelState, null);
            }

            if (ModelState.IsValid)
            {
                if (!VehiculoPerteneceAlCliente(dto.VehiculoId))
                {
                    ModelState.AddModelError("VehiculoId", "El vehículo seleccionado no es válido");
                    CargarVehiculosEnViewBag();
                    return View(dto);
                }

                var resultado = _citaService.Create(dto);
                if (resultado.Success)
                {
                    var cita = resultado.Value;
                    _notificacionService.Create(new CreateNotificacionDTO
                    {
                        ClienteId = ClienteId,
                        Mensaje = "Su cita para el " + cita.Fecha.ToString("dd/MM/yyyy") + " a las " + cita.Hora.ToString(@"hh\:mm") + " fue registrada correctamente."
                    });

                    TempData["MensajeExito"] = "Cita solicitada correctamente";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", resultado.Error);
            }

            CargarVehiculosEnViewBag();
            return View(dto);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var resultado = _citaService.GetById(id);
            if (!resultado.Success || !VehiculoPerteneceAlCliente(resultado.Value.VehiculoId))
            {
                TempData["MensajeError"] = "La cita no existe";
                return RedirectToAction(nameof(Index));
            }
            return View(resultado.Value);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var resultado = _citaService.GetById(id);
            if (!resultado.Success || !VehiculoPerteneceAlCliente(resultado.Value.VehiculoId))
            {
                TempData["MensajeError"] = "La cita no existe";
                return RedirectToAction(nameof(Index));
            }

            var cita = resultado.Value;
            if (cita.Procesada)
            {
                TempData["MensajeError"] = "No se puede editar una cita que ya fue procesada";
                return RedirectToAction(nameof(Index));
            }

            var dto = new UpdateCitaDTO
            {
                Id = cita.Id,
                VehiculoId = cita.VehiculoId,
                Fecha = cita.Fecha,
                Hora = cita.Hora,
                DescripcionFallos = cita.DescripcionFallos,
                Procesada = cita.Procesada,
                MecanicoId = cita.MecanicoId
            };

            CargarVehiculosEnViewBag();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateCitaDTO dto)
        {
            var validacion = new UpdateCitaDTOValidator().Validate(dto);
            if (!validacion.IsValid)
            {
                validacion.AddToModelState(ModelState, null);
            }

            if (ModelState.IsValid)
            {
                var citaExistente = _citaService.GetById(dto.Id);
                if (!citaExistente.Success || !VehiculoPerteneceAlCliente(citaExistente.Value.VehiculoId))
                {
                    TempData["MensajeError"] = "La cita no existe";
                    return RedirectToAction(nameof(Index));
                }

                if (citaExistente.Value.Procesada)
                {
                    TempData["MensajeError"] = "No se puede editar una cita que ya fue procesada";
                    return RedirectToAction(nameof(Index));
                }

                if (!VehiculoPerteneceAlCliente(dto.VehiculoId))
                {
                    ModelState.AddModelError("VehiculoId", "El vehículo seleccionado no es válido");
                    CargarVehiculosEnViewBag();
                    return View(dto);
                }

                // Se conserva el estado "Procesada" original (el cliente no debe poder marcarla como procesada)
                dto.Procesada = citaExistente.Value.Procesada;

                var resultado = _citaService.Update(dto);
                if (resultado.Success)
                {
                    TempData["MensajeExito"] = "Cita actualizada correctamente";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", resultado.Error);
            }

            CargarVehiculosEnViewBag();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var resultado = _citaService.GetById(id);
            if (!resultado.Success || !VehiculoPerteneceAlCliente(resultado.Value.VehiculoId))
            {
                TempData["MensajeError"] = "La cita no existe";
                return RedirectToAction(nameof(Index));
            }

            if (resultado.Value.Procesada)
            {
                TempData["MensajeError"] = "No se puede cancelar una cita que ya fue procesada";
                return RedirectToAction(nameof(Index));
            }

            var eliminado = _citaService.Delete(id);
            if (eliminado.Success)
            {
                TempData["MensajeExito"] = "Cita cancelada correctamente";
            }
            else
            {
                TempData["MensajeError"] = eliminado.Error;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VehiculoPerteneceAlCliente(int vehiculoId)
        {
            var resultado = _vehiculoService.GetById(vehiculoId);
            return resultado.Success && resultado.Value.ClienteId == ClienteId;
        }

        private void CargarVehiculosEnViewBag()
        {
            var resultado = _vehiculoService.GetByCliente(ClienteId);
            var vehiculos = resultado.Success ? resultado.Value : new System.Collections.Generic.List<VehiculoDTO>();

            ViewBag.Vehiculos = vehiculos.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Placa + " - " + v.Marca + " " + v.Modelo
            }).ToList();
        }
    }
}
