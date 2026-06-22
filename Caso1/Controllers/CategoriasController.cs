using Caso1.Entities;
using Caso1.infraestructure.DBContexts;
using Caso1.infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caso1.Controllers
{
    [RoutePrefix("Categorias")]
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepository _repository;

        public CategoriasController()
        {
            var context = new Caso1Context();
            _repository = new CategoriaRepository(context);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var categorias = _repository.GetAll();
            return View(categorias);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new Categoria();
            model.Id = new Random().Next(1000000);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                if (_repository.ExisteCategoriaConNombre(categoria.Nombre))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una categoría con este nombre");
                    return View(categoria);
                }

                categoria.FechaCreacion = DateTime.Now;
                _repository.Add(categoria);
                TempData["MensajeExito"] = "Categoría registrada correctamente";
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = _repository.GetById(id);
            if (model == null || model.Borrado)
            {
                TempData["MensajeError"] = "La categoría no existe";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _repository.GetById(id);
            if (model == null || model.Borrado)
            {
                TempData["MensajeError"] = "La categoría no existe";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                var existe = _repository.ExisteCategoriaConNombre(categoria.Nombre);
                var actual = _repository.GetById(categoria.Id);

                if (existe && actual != null && actual.Nombre != categoria.Nombre)
                {
                    ModelState.AddModelError("Nombre", "Ya existe otra categoría con este nombre");
                    return View(categoria);
                }

                var categoriaExistente = _repository.GetById(categoria.Id);
                if (categoriaExistente != null)
                {
                    categoria.FechaCreacion = categoriaExistente.FechaCreacion;
                    categoria.Borrado = categoriaExistente.Borrado;
                }

                _repository.Update(categoria);
                TempData["MensajeExito"] = "Categoría actualizada correctamente";
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var model = _repository.GetById(id);
            if (model != null && !model.Borrado)
            {
                model.Borrado = true;
                _repository.Update(model);
                TempData["MensajeExito"] = "Categoría eliminada correctamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["MensajeError"] = "La categoría no existe";
            return RedirectToAction(nameof(Index));
        }
    }
}