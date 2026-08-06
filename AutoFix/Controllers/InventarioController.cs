using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    public class InventarioController : Controller
    {
        private readonly AutoFixContext _context;
        private readonly IRepuestoRepository _repuestoRepository;
        private readonly IMaterialUsadoRepository _materialUsadoRepository;

        public InventarioController()
        {
            _context = new AutoFixContext();
            _repuestoRepository = new RepuestoRepository(_context);
            _materialUsadoRepository = new MaterialUsadoRepository(_context);
        }

       
        // 1. INDEX - Listar repuestos
        
        public ActionResult Index()
        {
            try
            {
                var repuestos = _repuestoRepository.GetAll()
                    .Where(r => !r.Borrado)
                    .OrderBy(r => r.Nombre)
                    .ToList();

                return View(repuestos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar repuestos: " + ex.Message;
                return View(new List<Repuesto>());
            }
        }

        
        // 2. DETAILS - Ver detalles de un repuesto
       
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var repuesto = _repuestoRepository.GetById(id.Value);
            if (repuesto == null || repuesto.Borrado)
                return HttpNotFound();

            return View(repuesto);
        }

        
        // 3. CREATE - Crear nuevo repuesto
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Codigo,Descripcion,Stock,Precio,Categoria,Ubicacion")] Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repuesto.FechaRegistro = DateTime.Now;
                    repuesto.Borrado = false;
                    _repuestoRepository.Add(repuesto);
                    TempData["Success"] = "Repuesto creado exitosamente";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear repuesto: " + ex.Message);
                }
            }
            return View(repuesto);
        }

 
        // 4. EDIT - Editar repuesto
      
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var repuesto = _repuestoRepository.GetById(id.Value);
            if (repuesto == null || repuesto.Borrado)
                return HttpNotFound();

            return View(repuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Codigo,Descripcion,Stock,Precio,Categoria,Ubicacion,FechaRegistro,Borrado")] Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repuestoRepository.Update(repuesto);
                    TempData["Success"] = "Repuesto actualizado exitosamente";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar repuesto: " + ex.Message);
                }
            }
            return View(repuesto);
        }


        // 5. DELETE - Eliminar repuesto

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var repuesto = _repuestoRepository.GetById(id.Value);
            if (repuesto == null || repuesto.Borrado)
                return HttpNotFound();

            return View(repuesto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var repuesto = _repuestoRepository.GetById(id);
                if (repuesto != null)
                {
                    repuesto.Borrado = true;
                    _repuestoRepository.Update(repuesto);
                    TempData["Success"] = "Repuesto eliminado exitosamente";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar repuesto: " + ex.Message;
            }

            return RedirectToAction("Index");
        }


        // 6. SOLICITAR MATERIALES 

        public ActionResult SolicitarMateriales()
        {
            try
            {
                var repuestos = _repuestoRepository.GetAll()
                    .Where(r => !r.Borrado && r.Stock > 0)
                    .OrderBy(r => r.Nombre)
                    .ToList();

                // ✅ Obtener órdenes de trabajo activas
                var ordenes = _context.OrdenesTrabajo
                    .Where(o => !o.Borrado && o.Estado != "Completada")
                    .ToList();

                // ✅ Si no hay órdenes, mostrar mensaje
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
                        Text = $"OT-{o.Id} - {o.Cliente?.Nombre ?? "Cliente no disponible"} - {o.Estado}"
                    }).ToList();
                }

                return View(repuestos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar repuestos: " + ex.Message;
                return View(new List<Repuesto>());
            }
        }

        // POST: Inventario/SolicitarMaterial
        [HttpPost]
        public JsonResult SolicitarMaterial(int repuestoId, int cantidad, int ordenTrabajoId, string observaciones = "")
        {
            try
            {
                var repuesto = _repuestoRepository.GetById(repuestoId);
                if (repuesto == null || repuesto.Borrado)
                    return Json(new { success = false, message = "Repuesto no encontrado" });

                if (repuesto.Stock < cantidad)
                    return Json(new { success = false, message = $"Stock insuficiente. Disponible: {repuesto.Stock}" });

                var ordenTrabajo = _context.OrdenesTrabajo.Find(ordenTrabajoId);
                if (ordenTrabajo == null || ordenTrabajo.Borrado)
                    return Json(new { success = false, message = "Orden de trabajo no encontrada" });

                // Descontar stock
                repuesto.Stock -= cantidad;
                _repuestoRepository.Update(repuesto);

                // Registrar el uso
                var material = new MaterialUsado
                {
                    RepuestoId = repuestoId,
                    OrdenTrabajoId = ordenTrabajoId,
                    Cantidad = cantidad,
                    CostoUnitario = repuesto.Precio,
                    Observaciones = observaciones,
                    FechaUso = DateTime.Now,
                    Borrado = false
                };

                _materialUsadoRepository.Add(material);

                return Json(new
                {
                    success = true,
                    message = $"Material solicitado correctamente. Stock restante: {repuesto.Stock}"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        // 7. HISTORIAL DE USO

        public ActionResult HistorialUso()
        {
            try
            {
                var materiales = _materialUsadoRepository.GetAll()
                    .Where(m => !m.Borrado)
                    .OrderByDescending(m => m.FechaUso)
                    .ToList();

                return View(materiales);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar historial: " + ex.Message;
                return View(new List<MaterialUsado>());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }
    }
}