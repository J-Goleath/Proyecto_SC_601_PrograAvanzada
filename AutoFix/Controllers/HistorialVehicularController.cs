using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    public class HistorialVehicularController : Controller
    {
        private AutoFixContext db = new AutoFixContext();

        public ActionResult Index()
        {
            var historial = db.HistorialVehicular
                .Include(h => h.Vehiculo)
                .Include(h => h.OrdenTrabajo)
                .Where(h => !h.Borrado)
                .ToList();

            return View(historial);
        }

        public ActionResult Create()
        {
            ViewBag.VehiculoId = new SelectList(
                db.Vehiculos.Where(v => !v.Borrado).ToList(),
                "Id",
                "Placa"
            );

            ViewBag.OrdenTrabajoId = new SelectList(
                db.OrdenesTrabajo.Where(o => !o.Borrado).ToList(),
                "Id",
                "DescripcionTrabajo"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HistorialVehicular historialVehicular)
        {
            if (ModelState.IsValid)
            {
                db.HistorialVehicular.Add(historialVehicular);
                db.SaveChanges();

                TempData["Success"] = "Historial vehicular registrado correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.VehiculoId = new SelectList(
                db.Vehiculos.Where(v => !v.Borrado).ToList(),
                "Id",
                "Placa",
                historialVehicular.VehiculoId
            );

            ViewBag.OrdenTrabajoId = new SelectList(
                db.OrdenesTrabajo.Where(o => !o.Borrado).ToList(),
                "Id",
                "DescripcionTrabajo",
                historialVehicular.OrdenTrabajoId
            );

            return View(historialVehicular);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var historial = db.HistorialVehicular
                .Include(h => h.Vehiculo)
                .Include(h => h.OrdenTrabajo)
                .FirstOrDefault(h => h.Id == id);

            if (historial == null || historial.Borrado)
            {
                return HttpNotFound();
            }

            return View(historial);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HistorialVehicular historialVehicular = db.HistorialVehicular.Find(id);

            if (historialVehicular == null || historialVehicular.Borrado)
            {
                return HttpNotFound();
            }

            ViewBag.VehiculoId = new SelectList(
                db.Vehiculos.Where(v => !v.Borrado).ToList(),
                "Id",
                "Placa",
                historialVehicular.VehiculoId
            );

            ViewBag.OrdenTrabajoId = new SelectList(
                db.OrdenesTrabajo.Where(o => !o.Borrado).ToList(),
                "Id",
                "DescripcionTrabajo",
                historialVehicular.OrdenTrabajoId
            );

            return View(historialVehicular);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HistorialVehicular historialVehicular)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historialVehicular).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Historial vehicular actualizado correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.VehiculoId = new SelectList(
                db.Vehiculos.Where(v => !v.Borrado).ToList(),
                "Id",
                "Placa",
                historialVehicular.VehiculoId
            );

            ViewBag.OrdenTrabajoId = new SelectList(
                db.OrdenesTrabajo.Where(o => !o.Borrado).ToList(),
                "Id",
                "DescripcionTrabajo",
                historialVehicular.OrdenTrabajoId
            );

            return View(historialVehicular);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var historial = db.HistorialVehicular
                .Include(h => h.Vehiculo)
                .Include(h => h.OrdenTrabajo)
                .FirstOrDefault(h => h.Id == id);

            if (historial == null || historial.Borrado)
            {
                return HttpNotFound();
            }

            return View(historial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HistorialVehicular historial = db.HistorialVehicular.Find(id);

            if (historial != null)
            {
                historial.Borrado = true;
                db.Entry(historial).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Historial vehicular eliminado correctamente.";
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