using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Filters;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("AsignacionCitas")]
    [CustomAuthorize(Roles = "Administrador")]
    public class AsignacionCitasController : Controller
    {
        private readonly ICitaService _citaService;
        private readonly IClienteService _clienteService;
        private readonly IVehiculoService _vehiculoService;
        private readonly INotificacionService _notificacionService;

        public AsignacionCitasController(
            ICitaService citaService,
            IClienteService clienteService,
            IVehiculoService vehiculoService,
            INotificacionService notificacionService)
        {
            _citaService = citaService;
            _clienteService = clienteService;
            _vehiculoService = vehiculoService;
            _notificacionService = notificacionService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var resultadoCitas = _citaService.GetAll();
            var citas = resultadoCitas.Success
                ? resultadoCitas.Value.OrderBy(c => c.Procesada).ThenBy(c => c.Fecha).ToList()
                : new System.Collections.Generic.List<CitaDTO>();

            var resultadoClientes = _clienteService.GetAll();
            var mecanicos = resultadoClientes.Success
                ? resultadoClientes.Value.Where(c => c.Rol == "Mecanico").ToList()
                : new System.Collections.Generic.List<ClienteDTO>();

            ViewBag.Mecanicos = mecanicos;

            return View(citas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarMecanico(int citaId, int mecanicoId)
        {
            var resultadoCita = _citaService.GetById(citaId);
            if (!resultadoCita.Success)
            {
                TempData["MensajeError"] = "La cita no existe";
                return RedirectToAction(nameof(Index));
            }

            var resultadoMecanico = _clienteService.GetById(mecanicoId);
            if (!resultadoMecanico.Success || resultadoMecanico.Value.Rol != "Mecanico")
            {
                TempData["MensajeError"] = "El mecánico seleccionado no es válido";
                return RedirectToAction(nameof(Index));
            }

            var cita = resultadoCita.Value;
            var dto = new UpdateCitaDTO
            {
                Id = cita.Id,
                VehiculoId = cita.VehiculoId,
                Fecha = cita.Fecha,
                Hora = cita.Hora,
                DescripcionFallos = cita.DescripcionFallos,
                Procesada = true,
                MecanicoId = mecanicoId
            };

            var resultadoUpdate = _citaService.Update(dto);
            if (!resultadoUpdate.Success)
            {
                TempData["MensajeError"] = resultadoUpdate.Error;
                return RedirectToAction(nameof(Index));
            }

            if (cita.ClienteId != 0)
            {
                _notificacionService.Create(new CreateNotificacionDTO
                {
                    ClienteId = cita.ClienteId,
                    Mensaje = "Se le asignó el mecánico " + resultadoMecanico.Value.Nombre + " para su cita del " + cita.Fecha.ToString("dd/MM/yyyy") + "."
                });
            }

            TempData["MensajeExito"] = "Mecánico asignado correctamente";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuitarAsignacion(int citaId)
        {
            var resultadoCita = _citaService.GetById(citaId);
            if (!resultadoCita.Success)
            {
                TempData["MensajeError"] = "La cita no existe";
                return RedirectToAction(nameof(Index));
            }

            var cita = resultadoCita.Value;
            var dto = new UpdateCitaDTO
            {
                Id = cita.Id,
                VehiculoId = cita.VehiculoId,
                Fecha = cita.Fecha,
                Hora = cita.Hora,
                DescripcionFallos = cita.DescripcionFallos,
                Procesada = false,
                MecanicoId = null
            };

            var resultado = _citaService.Update(dto);
            if (resultado.Success)
            {
                TempData["MensajeExito"] = "Asignación removida";
            }
            else
            {
                TempData["MensajeError"] = resultado.Error;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
