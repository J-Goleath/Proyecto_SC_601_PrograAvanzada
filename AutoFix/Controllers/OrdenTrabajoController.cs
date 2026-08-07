using AutoFix.Entities;
using AutoFix.infraestructure.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    public class OrdenTrabajoController : Controller
    {
        private readonly IOrdenTrabajoRepository ordenTrabajoRepository;

        public OrdenTrabajoController()
        {
            ordenTrabajoRepository = new OrdenTrabajoRepository();
        }

        public ActionResult Index()
        {
            var ordenes = ordenTrabajoRepository.ObtenerTodas();
            return View(ordenes);
        }

        public ActionResult Detalle(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var ordenTrabajo = ordenTrabajoRepository.ObtenerPorId(id.Value);

            if (ordenTrabajo == null)
            {
                return HttpNotFound();
            }

            return View(ordenTrabajo);
        }

        public ActionResult ActualizarEstado(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var ordenTrabajo = ordenTrabajoRepository.ObtenerPorId(id.Value);

            if (ordenTrabajo == null)
            {
                return HttpNotFound();
            }

            ViewBag.Estados = Enum.GetValues(typeof(EstadoOrden))
                .Cast<EstadoOrden>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = ObtenerNombreEstado(e),
                    Selected = ordenTrabajo.Estado == e.ToString()
                })
                .ToList();

            return View(ordenTrabajo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarEstado(int id, string estado, string observaciones)
        {
            var ordenTrabajo = ordenTrabajoRepository.ObtenerPorId(id);

            if (ordenTrabajo == null)
            {
                return HttpNotFound();
            }

            ordenTrabajo.Estado = estado;
            ordenTrabajo.Observaciones = observaciones;

            if (estado == EstadoOrden.EnProceso.ToString() && ordenTrabajo.FechaInicio == null)
            {
                ordenTrabajo.FechaInicio = DateTime.Now;
            }

            if (estado == EstadoOrden.Finalizada.ToString())
            {
                ordenTrabajo.FechaFinalizacion = DateTime.Now;
            }

            ordenTrabajoRepository.Actualizar(ordenTrabajo);
            ordenTrabajoRepository.Guardar();

            TempData["Success"] = "Estado y observaciones de la orden actualizados correctamente.";

            return RedirectToAction("Detalle", new { id = ordenTrabajo.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            var ordenTrabajo = ordenTrabajoRepository.ObtenerPorId(id);

            if (ordenTrabajo == null)
            {
                return HttpNotFound();
            }

            ordenTrabajoRepository.EliminarLogico(id);
            ordenTrabajoRepository.Guardar();

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