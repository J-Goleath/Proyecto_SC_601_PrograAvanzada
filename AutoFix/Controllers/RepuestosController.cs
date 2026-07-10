using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    public class RepuestosController : Controller
    {
        private AutoFixContext db = new AutoFixContext();

        public ActionResult Index()
        {
            var repuestos = db.Repuestos
                .Where(r => !r.Borrado)
                .ToList();

            return View(repuestos);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Repuesto repuesto = db.Repuestos.Find(id);

            if (repuesto == null || repuesto.Borrado)
            {
                return HttpNotFound();
            }

            return View(repuesto);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                db.Repuestos.Add(repuesto);
                db.SaveChanges();

                TempData["Success"] = "Repuesto registrado correctamente.";
                return RedirectToAction("Index");
            }

            return View(repuesto);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Repuesto repuesto = db.Repuestos.Find(id);

            if (repuesto == null || repuesto.Borrado)
            {
                return HttpNotFound();
            }

            return View(repuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(repuesto).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Repuesto actualizado correctamente.";
                return RedirectToAction("Index");
            }

            return View(repuesto);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Repuesto repuesto = db.Repuestos.Find(id);

            if (repuesto == null || repuesto.Borrado)
            {
                return HttpNotFound();
            }

            return View(repuesto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Repuesto repuesto = db.Repuestos.Find(id);

            if (repuesto != null)
            {
                repuesto.Borrado = true;
                db.Entry(repuesto).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Repuesto eliminado correctamente.";
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