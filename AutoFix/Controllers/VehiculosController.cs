using AutoFix.Entities;
using AutoFix.Filters;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("Vehiculos")]
    [CustomAuthorize(Roles = "Administrador")]
    public class VehiculosController : Controller
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly AutoFixContext _context;

        public VehiculosController()
        {
            _context = new AutoFixContext();
            _vehiculoRepository = new VehiculoRepository(_context);
            _clienteRepository = new ClienteRepository(_context);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var vehiculos = _vehiculoRepository.GetAll();
            return View(vehiculos);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Clientes = new SelectList(_clienteRepository.GetAll(), "Id", "Nombre");
            return View(new Vehiculo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                if (_vehiculoRepository.ExistePlaca(vehiculo.Placa))
                {
                    ModelState.AddModelError("Placa", "Ya existe un vehículo con esta placa");
                    ViewBag.Clientes = new SelectList(_clienteRepository.GetAll(), "Id", "Nombre", vehiculo.ClienteId);
                    return View(vehiculo);
                }

                vehiculo.FechaRegistro = DateTime.Now;
                _vehiculoRepository.Add(vehiculo);
                TempData["MensajeExito"] = "Vehículo registrado correctamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = new SelectList(_clienteRepository.GetAll(), "Id", "Nombre", vehiculo.ClienteId);
            return View(vehiculo);
        }

        
        [HttpGet]
        public ActionResult Details(int id)
        {
            var vehiculo = _vehiculoRepository.GetById(id);
            if (vehiculo == null || vehiculo.Borrado)
            {
                TempData["MensajeError"] = "El vehículo no existe";
                return RedirectToAction(nameof(Index));
            }
            return View(vehiculo);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var vehiculo = _vehiculoRepository.GetById(id);
            if (vehiculo == null || vehiculo.Borrado)
            {
                TempData["MensajeError"] = "El vehículo no existe";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clientes = new SelectList(_clienteRepository.GetAll(), "Id", "Nombre", vehiculo.ClienteId);
            return View(vehiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                var vehiculoExistente = _vehiculoRepository.GetById(vehiculo.Id);
                if (vehiculoExistente == null)
                {
                    TempData["MensajeError"] = "El vehículo no existe";
                    return RedirectToAction(nameof(Index));
                }

                var placaDuplicada = _vehiculoRepository.ExistePlaca(vehiculo.Placa);
                if (placaDuplicada && vehiculoExistente.Placa != vehiculo.Placa)
                {
                    ModelState.AddModelError("Placa", "Ya existe otro vehículo con esta placa");
                    ViewBag.Clientes = new SelectList(_clienteRepository.GetAll(), "Id", "Nombre", vehiculo.ClienteId);
                    return View(vehiculo);
                }

                vehiculo.FechaRegistro = vehiculoExistente.FechaRegistro;
                vehiculo.Borrado = vehiculoExistente.Borrado;

                _vehiculoRepository.Update(vehiculo);
                TempData["MensajeExito"] = "Vehículo actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = new SelectList(_clienteRepository.GetAll(), "Id", "Nombre", vehiculo.ClienteId);
            return View(vehiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var vehiculo = _vehiculoRepository.GetById(id);
            if (vehiculo != null && !vehiculo.Borrado)
            {
                vehiculo.Borrado = true;
                _vehiculoRepository.Update(vehiculo);
                TempData["MensajeExito"] = "Vehículo eliminado correctamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["MensajeError"] = "El vehículo no existe";
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