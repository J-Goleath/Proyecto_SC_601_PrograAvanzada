using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    public class OrdenesTrabajoController : Controller
    {
        private AutoFixContext db = new AutoFixContext();

        public ActionResult Index()
        {
            var ordenes = db.OrdenesTrabajo
                .Include(o => o.Vehiculo)
                .Where(o => !o.Borrado)
                .ToList();

            return View(ordenes);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OrdenTrabajo ordenTrabajo = db.OrdenesTrabajo
                .Include(o => o.Vehiculo)
                .FirstOrDefault(o => o.Id == id);

            if (ordenTrabajo == null || ordenTrabajo.Borrado)
            {
                return HttpNotFound();
            }

            return View(ordenTrabajo);
        }

        public ActionResult Create()
        {
            ViewBag.VehiculoId = new SelectList(
                db.Vehiculos.Where(v => !v.Borrado).ToList(),
                "Id",
                "Placa"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrdenTrabajo ordenTrabajo)
        {
            if (ModelState.IsValid)
            {
                db.OrdenesTrabajo.Add(ordenTrabajo);
                db.SaveChanges();

                TempData["Success"] = "Orden de trabajo registrada correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.VehiculoId = new SelectList(
                db.Vehiculos.Where(v => !v.Borrado).ToList(),
                "Id",
                "Placa",
                ordenTrabajo.VehiculoId
            );

            return View(ordenTrabajo);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OrdenTrabajo ordenTrabajo = db.OrdenesTrabajo.Find(id);

            if (ordenTrabajo == null || ordenTrabajo.Borrado)
            {
                return HttpNotFound();
            }

            ViewBag.VehiculoId = new SelectList(
                db.Vehiculos.Where(v => !v.Borrado).ToList(),
                "Id",
                "Placa",
                ordenTrabajo.VehiculoId
            );

            return View(ordenTrabajo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrdenTrabajo ordenTrabajo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordenTrabajo).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Orden de trabajo actualizada correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.VehiculoId = new SelectList(
                db.Vehiculos.Where(v => !v.Borrado).ToList(),
                "Id",
                "Placa",
                ordenTrabajo.VehiculoId
            );

            return View(ordenTrabajo);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OrdenTrabajo ordenTrabajo = db.OrdenesTrabajo
                .Include(o => o.Vehiculo)
                .FirstOrDefault(o => o.Id == id);

            if (ordenTrabajo == null || ordenTrabajo.Borrado)
            {
                return HttpNotFound();
            }

            return View(ordenTrabajo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrdenTrabajo ordenTrabajo = db.OrdenesTrabajo.Find(id);

            if (ordenTrabajo != null)
            {
                ordenTrabajo.Borrado = true;
                db.Entry(ordenTrabajo).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Orden de trabajo eliminada correctamente.";
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}