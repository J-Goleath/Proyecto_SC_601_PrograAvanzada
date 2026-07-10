using AutoFix.Entities;
using AutoFix.Filters;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("AsignacionCitas")]
    [CustomAuthorize(Roles = "Administrador")]
    public class AsignacionCitasController : Controller
    {
        private readonly ICitaSolicitudRepository _citaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly INotificacionRepository _notificacionRepository;
        private readonly AutoFixContext _context;

        public AsignacionCitasController()
        {
            _context = new AutoFixContext();
            _citaRepository = new CitaSolicitudRepository(_context);
            _clienteRepository = new ClienteRepository(_context);
            _notificacionRepository = new NotificacionRepository(_context);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var citas = _citaRepository.GetAll()
                .Where(c => !c.Borrado)
                .OrderBy(c => c.Procesada)
                .ThenBy(c => c.Fecha)
                .ToList();

            var mecanicos = _clienteRepository.GetClientesActivos()
                .Where(c => c.Rol == RolUsuario.Mecanico)
                .ToList();

            ViewBag.Mecanicos = mecanicos;

            return View(citas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarMecanico(int citaId, int mecanicoId)
        {
            var cita = _citaRepository.GetById(citaId);
            if (cita == null || cita.Borrado)
            {
                TempData["MensajeError"] = "La cita no existe";
                return RedirectToAction(nameof(Index));
            }

            var mecanico = _clienteRepository.GetById(mecanicoId);
            if (mecanico == null || mecanico.Rol != RolUsuario.Mecanico)
            {
                TempData["MensajeError"] = "El mecánico seleccionado no es válido";
                return RedirectToAction(nameof(Index));
            }

            cita.MecanicoId = mecanicoId;
            cita.Procesada = true;
            _citaRepository.Update(cita);

            if (cita.Vehiculo != null)
            {
                var notificacion = new Notificacion
                {
                    ClienteId = cita.Vehiculo.ClienteId,
                    Mensaje = "Se le asignó el mecánico " + mecanico.Nombre + " para su cita del " + cita.Fecha.ToString("dd/MM/yyyy") + ".",
                    FechaEnvio = DateTime.Now,
                    Leida = false
                };
                _notificacionRepository.Add(notificacion);
            }

            TempData["MensajeExito"] = "Mecánico asignado correctamente";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuitarAsignacion(int citaId)
        {
            var cita = _citaRepository.GetById(citaId);
            if (cita != null && !cita.Borrado)
            {
                cita.MecanicoId = null;
                cita.Procesada = false;
                _citaRepository.Update(cita);
                TempData["MensajeExito"] = "Asignación removida";
            }
            else
            {
                TempData["MensajeError"] = "La cita no existe";
            }

            return RedirectToAction(nameof(Index));
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