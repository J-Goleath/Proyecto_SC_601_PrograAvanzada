using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Domain.Enums;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    public class OrdenTrabajoController : Controller
    {
        private readonly IOrdenTrabajoService _ordenTrabajoService;

        public OrdenTrabajoController(IOrdenTrabajoService ordenTrabajoService)
        {
            _ordenTrabajoService = ordenTrabajoService;
        }

        public ActionResult Index()
        {
            var resultado = _ordenTrabajoService.GetAll();
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return View(Enumerable.Empty<OrdenTrabajoDTO>());
            }
            return View(resultado.Value);
        }

        public ActionResult Detalle(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var resultado = _ordenTrabajoService.GetById(id.Value);
            if (!resultado.Success)
            {
                return HttpNotFound();
            }

            return View(resultado.Value);
        }

        public ActionResult ActualizarEstado(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var resultado = _ordenTrabajoService.GetById(id.Value);
            if (!resultado.Success)
            {
                return HttpNotFound();
            }

            var orden = resultado.Value;

            ViewBag.Estados = Enum.GetValues(typeof(EstadoOrden))
                .Cast<EstadoOrden>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = ObtenerNombreEstado(e),
                    Selected = orden.Estado == e.ToString()
                })
                .ToList();

            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarEstado(int id, string estado, string observaciones)
        {
            var actual = _ordenTrabajoService.GetById(id);
            if (!actual.Success)
            {
                return HttpNotFound();
            }

            var orden = actual.Value;

            DateTime? fechaInicio = orden.FechaInicio;
            if (estado == EstadoOrden.EnProceso.ToString() && fechaInicio == null)
            {
                fechaInicio = DateTime.Now;
            }

            DateTime? fechaFinalizacion = orden.FechaFinalizacion;
            if (estado == EstadoOrden.Finalizada.ToString())
            {
                fechaFinalizacion = DateTime.Now;
            }

            var dto = new UpdateOrdenTrabajoDTO
            {
                Id = id,
                Estado = estado,
                Diagnostico = orden.Diagnostico,
                Observaciones = observaciones,
                FechaInicio = fechaInicio,
                FechaFinalizacion = fechaFinalizacion
            };

            var resultado = _ordenTrabajoService.Update(dto);
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return RedirectToAction("Detalle", new { id });
            }

            TempData["Success"] = "Estado y observaciones de la orden actualizados correctamente.";

            return RedirectToAction("Detalle", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            var resultado = _ordenTrabajoService.Delete(id);
            if (!resultado.Success)
            {
                return HttpNotFound();
            }

            TempData["Success"] = "Orden de trabajo eliminada correctamente.";

            return RedirectToAction("Index");
        }

        private string ObtenerNombreEstado(EstadoOrden estado)
        {
            switch (estado)
            {
                case EstadoOrden.Pendiente:
                    return "Pendiente";

                case EstadoOrden.EnProceso:
                    return "En Proceso";

                case EstadoOrden.Finalizada:
                    return "Finalizada";

                case EstadoOrden.Cancelada:
                    return "Cancelada";

                default:
                    return estado.ToString();
            }
        }
    }
}
