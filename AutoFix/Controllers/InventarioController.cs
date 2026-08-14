using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Application.Validators;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    public class InventarioController : Controller
    {
        private readonly IRepuestoService _repuestoService;
        private readonly IMaterialUsadoService _materialUsadoService;
        private readonly IOrdenTrabajoService _ordenTrabajoService;

        public InventarioController(
            IRepuestoService repuestoService,
            IMaterialUsadoService materialUsadoService,
            IOrdenTrabajoService ordenTrabajoService)
        {
            _repuestoService = repuestoService;
            _materialUsadoService = materialUsadoService;
            _ordenTrabajoService = ordenTrabajoService;
        }

        // 1. INDEX - Listar repuestos
        public ActionResult Index()
        {
            var resultado = _repuestoService.GetAll();
            if (!resultado.Success)
            {
                ViewBag.Error = "Error al cargar repuestos: " + resultado.Error;
                return View(new List<RepuestoDTO>());
            }
            return View(resultado.Value);
        }

        // 2. DETAILS - Ver detalles de un repuesto
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var resultado = _repuestoService.GetById(id.Value);
            if (!resultado.Success)
                return HttpNotFound();

            return View(resultado.Value);
        }

        // 3. CREATE - Crear nuevo repuesto
        public ActionResult Create()
        {
            return View(new CreateRepuestoDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateRepuestoDTO dto)
        {
            var validacion = new CreateRepuestoDTOValidator().Validate(dto);
            if (!validacion.IsValid)
            {
                validacion.AddToModelState(ModelState, null);
            }

            if (ModelState.IsValid)
            {
                var resultado = _repuestoService.Create(dto);
                if (resultado.Success)
                {
                    TempData["Success"] = "Repuesto creado exitosamente";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al crear repuesto: " + resultado.Error);
            }
            return View(dto);
        }

        // 4. EDIT - Editar repuesto
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var resultado = _repuestoService.GetById(id.Value);
            if (!resultado.Success)
                return HttpNotFound();

            var repuesto = resultado.Value;
            var dto = new UpdateRepuestoDTO
            {
                Id = repuesto.Id,
                Nombre = repuesto.Nombre,
                Codigo = repuesto.Codigo,
                Descripcion = repuesto.Descripcion,
                Stock = repuesto.Stock,
                Precio = repuesto.Precio,
                Categoria = repuesto.Categoria,
                Ubicacion = repuesto.Ubicacion
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateRepuestoDTO dto)
        {
            var validacion = new UpdateRepuestoDTOValidator().Validate(dto);
            if (!validacion.IsValid)
            {
                validacion.AddToModelState(ModelState, null);
            }

            if (ModelState.IsValid)
            {
                var resultado = _repuestoService.Update(dto);
                if (resultado.Success)
                {
                    TempData["Success"] = "Repuesto actualizado exitosamente";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al actualizar repuesto: " + resultado.Error);
            }
            return View(dto);
        }

        // 5. DELETE - Eliminar repuesto
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var resultado = _repuestoService.GetById(id.Value);
            if (!resultado.Success)
                return HttpNotFound();

            return View(resultado.Value);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var resultado = _repuestoService.Delete(id);
            if (resultado.Success)
            {
                TempData["Success"] = "Repuesto eliminado exitosamente";
            }
            else
            {
                TempData["Error"] = "Error al eliminar repuesto: " + resultado.Error;
            }

            return RedirectToAction("Index");
        }

        // 6. SOLICITAR MATERIALES
        public ActionResult SolicitarMateriales()
        {
            var resultadoRepuestos = _repuestoService.GetDisponibles();
            var repuestos = resultadoRepuestos.Success ? resultadoRepuestos.Value : new List<RepuestoDTO>();

            var resultadoOrdenes = _ordenTrabajoService.GetAll();
            var ordenes = resultadoOrdenes.Success
                ? resultadoOrdenes.Value.Where(o => o.Estado != "Completada" && o.Estado != "Finalizada").ToList()
                : new List<OrdenTrabajoDTO>();

            if (!ordenes.Any())
            {
                ViewBag.Mensaje = "No hay órdenes de trabajo activas disponibles.";
                ViewBag.OrdenesTrabajo = new List<SelectListItem>();
            }
            else
            {
                ViewBag.OrdenesTrabajo = ordenes.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = $"OT-{o.Id} - {o.ClienteNombre} - {o.Estado}"
                }).ToList();
            }

            return View(repuestos);
        }

        // POST: Inventario/SolicitarMaterial
        [HttpPost]
        public JsonResult SolicitarMaterial(int repuestoId, int cantidad, int ordenTrabajoId, string observaciones = "")
        {
            var resultado = _materialUsadoService.SolicitarMaterial(new SolicitarMaterialDTO
            {
                RepuestoId = repuestoId,
                Cantidad = cantidad,
                OrdenTrabajoId = ordenTrabajoId,
                Observaciones = observaciones
            });

            if (!resultado.Success)
            {
                return Json(new { success = false, message = resultado.Error });
            }

            var repuestoActualizado = _repuestoService.GetById(repuestoId);
            var stockRestante = repuestoActualizado.Success ? repuestoActualizado.Value.Stock : 0;

            return Json(new
            {
                success = true,
                message = $"Material solicitado correctamente. Stock restante: {stockRestante}"
            });
        }

        // 7. HISTORIAL DE USO
        public ActionResult HistorialUso()
        {
            var resultado = _materialUsadoService.GetHistorial();
            if (!resultado.Success)
            {
                ViewBag.Error = "Error al cargar historial: " + resultado.Error;
                return View(new List<MaterialUsadoHistorialDTO>());
            }
            return View(resultado.Value);
        }
    }
}
