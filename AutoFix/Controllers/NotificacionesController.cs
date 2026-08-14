using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("Notificaciones")]
    [CustomAuthorize(Roles = "Cliente,Mecanico,Administrador")]
    public class NotificacionesController : Controller
    {
        private readonly INotificacionService _notificacionService;

        public NotificacionesController(INotificacionService notificacionService)
        {
            _notificacionService = notificacionService;
        }

        private int UsuarioId
        {
            get { return Convert.ToInt32(Session["UsuarioId"]); }
        }

        [HttpGet]
        public ActionResult Index()
        {
            var resultado = _notificacionService.GetByCliente(UsuarioId);
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return View(Enumerable.Empty<NotificacionDTO>());
            }
            return View(resultado.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarcarLeida(int id)
        {
            var resultado = _notificacionService.GetById(id);

            // Solo se puede marcar como leída una notificación propia
            if (resultado.Success && resultado.Value.ClienteId == UsuarioId)
            {
                _notificacionService.MarcarComoLeida(id);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarcarTodasLeidas()
        {
            var resultado = _notificacionService.MarcarTodasComoLeidas(UsuarioId);
            if (resultado.Success)
            {
                TempData["MensajeExito"] = "Todas las notificaciones fueron marcadas como leídas";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
