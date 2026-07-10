using AutoFix.Filters;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using System;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("Notificaciones")]
    [CustomAuthorize(Roles = "Cliente,Mecanico,Administrador")]
    public class NotificacionesController : Controller
    {
        private readonly INotificacionRepository _notificacionRepository;
        private readonly AutoFixContext _context;

        public NotificacionesController()
        {
            _context = new AutoFixContext();
            _notificacionRepository = new NotificacionRepository(_context);
        }

        private int UsuarioId
        {
            get { return Convert.ToInt32(Session["UsuarioId"]); }
        }

        [HttpGet]
        public ActionResult Index()
        {
            var notificaciones = _notificacionRepository.GetByCliente(UsuarioId);
            return View(notificaciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarcarLeida(int id)
        {
            var notificacion = _notificacionRepository.GetById(id);
            if (notificacion != null && notificacion.ClienteId == UsuarioId && !notificacion.Borrado)
            {
                notificacion.Leida = true;
                _notificacionRepository.Update(notificacion);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarcarTodasLeidas()
        {
            var noLeidas = _notificacionRepository.GetNoLeidasByCliente(UsuarioId);
            foreach (var notificacion in noLeidas)
            {
                notificacion.Leida = true;
                _notificacionRepository.Update(notificacion);
            }

            TempData["MensajeExito"] = "Todas las notificaciones fueron marcadas como leídas";
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
