using AutoFix.Entities;
using AutoFix.Filters;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("Citas")]
    [CustomAuthorize(Roles = "Cliente")]
    public class CitasController : Controller
    {
        private readonly ICitaSolicitudRepository _citaRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly INotificacionRepository _notificacionRepository;
        private readonly AutoFixContext _context;

        public CitasController()
        {
            _context = new AutoFixContext();
            _citaRepository = new CitaSolicitudRepository(_context);
            _vehiculoRepository = new VehiculoRepository(_context);
            _notificacionRepository = new NotificacionRepository(_context);
        }

        private int ClienteId
        {
            get { return Convert.ToInt32(Session["UsuarioId"]); }
        }

        [HttpGet]
        public ActionResult Index()
        {
            var citas = _citaRepository.GetCitasByCliente(ClienteId);
            return View(citas);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CargarVehiculosEnViewBag();
            return View(new CitaSolicitud());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CitaSolicitud cita)
        {
            ModelState.Remove("Procesada");
            ModelState.Remove("MecanicoId");
            ModelState.Remove("Mecanico");
            ModelState.Remove("Vehiculo");

            if (ModelState.IsValid)
            {
                if (!VehiculoPerteneceAlCliente(cita.VehiculoId))
                {
                    ModelState.AddModelError("VehiculoId", "El vehículo seleccionado no es válido");
                    CargarVehiculosEnViewBag();
                    return View(cita);
                }

                cita.FechaRegistro = DateTime.Now;
                cita.Procesada = false;
                cita.MecanicoId = null;

                _citaRepository.Add(cita);

                var notificacion = new Notificacion
                {
                    ClienteId = ClienteId,
                    Mensaje = "Su cita para el " + cita.Fecha.ToString("dd/MM/yyyy") + " a las " + cita.Hora.ToString(@"hh\:mm") + " fue registrada correctamente.",
                    FechaEnvio = DateTime.Now,
                    Leida = false
                };
                _notificacionRepository.Add(notificacion);

                TempData["MensajeExito"] = "Cita solicitada correctamente";
                return RedirectToAction(nameof(Index));
            }

            CargarVehiculosEnViewBag();
            return View(cita);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var cita = _citaRepository.GetById(id);
            if (cita == null || cita.Borrado || !VehiculoPerteneceAlCliente(cita.VehiculoId))
            {
                TempData["MensajeError"] = "La cita no existe";
                return RedirectToAction(nameof(Index));
            }
            return View(cita);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var cita = _citaRepository.GetById(id);
            if (cita == null || cita.Borrado || !VehiculoPerteneceAlCliente(cita.VehiculoId))
            {
                TempData["MensajeError"] = "La cita no existe";
                return RedirectToAction(nameof(Index));
            }

            if (cita.Procesada)
            {
                TempData["MensajeError"] = "No se puede editar una cita que ya fue procesada";
                return RedirectToAction(nameof(Index));
            }

            CargarVehiculosEnViewBag();
            return View(cita);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CitaSolicitud cita)
        {
            ModelState.Remove("Procesada");
            ModelState.Remove("MecanicoId");
            ModelState.Remove("Mecanico");
            ModelState.Remove("Vehiculo");

            if (ModelState.IsValid)
            {
                var citaExistente = _citaRepository.GetById(cita.Id);
                if (citaExistente == null || !VehiculoPerteneceAlCliente(citaExistente.VehiculoId))
                {
                    TempData["MensajeError"] = "La cita no existe";
                    return RedirectToAction(nameof(Index));
                }

                if (citaExistente.Procesada)
                {
                    TempData["MensajeError"] = "No se puede editar una cita que ya fue procesada";
                    return RedirectToAction(nameof(Index));
                }

                if (!VehiculoPerteneceAlCliente(cita.VehiculoId))
                {
                    ModelState.AddModelError("VehiculoId", "El vehículo seleccionado no es válido");
                    CargarVehiculosEnViewBag();
                    return View(cita);
                }

                citaExistente.Fecha = cita.Fecha;
                citaExistente.Hora = cita.Hora;
                citaExistente.DescripcionFallos = cita.DescripcionFallos;
                citaExistente.VehiculoId = cita.VehiculoId;

                _citaRepository.Update(citaExistente);
                TempData["MensajeExito"] = "Cita actualizada correctamente";
                return RedirectToAction(nameof(Index));
            }

            CargarVehiculosEnViewBag();
            return View(cita);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var cita = _citaRepository.GetById(id);
            if (cita != null && !cita.Borrado && VehiculoPerteneceAlCliente(cita.VehiculoId))
            {
                if (cita.Procesada)
                {
                    TempData["MensajeError"] = "No se puede cancelar una cita que ya fue procesada";
                    return RedirectToAction(nameof(Index));
                }

                cita.Borrado = true;
                _citaRepository.Update(cita);
                TempData["MensajeExito"] = "Cita cancelada correctamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["MensajeError"] = "La cita no existe";
            return RedirectToAction(nameof(Index));
        }

        private bool VehiculoPerteneceAlCliente(int vehiculoId)
        {
            var vehiculo = _vehiculoRepository.GetById(vehiculoId);
            return vehiculo != null && !vehiculo.Borrado && vehiculo.ClienteId == ClienteId;
        }

        private void CargarVehiculosEnViewBag()
        {
            var vehiculos = _vehiculoRepository.GetVehiculosByCliente(ClienteId);
            ViewBag.Vehiculos = vehiculos.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Placa + " - " + v.Marca + " " + v.Modelo
            }).ToList();
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
