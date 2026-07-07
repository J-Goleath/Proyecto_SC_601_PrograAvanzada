using AutoFix.Entities;
using AutoFix.Filters;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using AutoFix.Utils;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("Clientes")]
    [CustomAuthorize(Roles = "Administrador")]  
    public class ClientesController : Controller
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly AutoFixContext _context;

        public ClientesController()
        {
            _context = new AutoFixContext();
            _clienteRepository = new ClienteRepository(_context);
        }

        // ✅ PROTEGIDO - Solo Administradores
        [HttpGet]
        public ActionResult Index()
        {
            var clientes = _clienteRepository.GetAll();
            return View(clientes);
        }

        // ✅ PÚBLICO - Registro de clientes (SIN AUTENTICACIÓN)
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            CargarRoles();
            return View(new Cliente());
        }

        // ✅ PÚBLICO - Registro de clientes (SIN AUTENTICACIÓN)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(Cliente cliente)
        {
            System.Diagnostics.Debug.WriteLine("=== CREATE CLIENTE ===");
            System.Diagnostics.Debug.WriteLine($"Nombre: {cliente.Nombre}");
            System.Diagnostics.Debug.WriteLine($"Correo: {cliente.Correo}");
            System.Diagnostics.Debug.WriteLine($"Rol: {cliente.Rol}");
            System.Diagnostics.Debug.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                if (_clienteRepository.ExisteCorreo(cliente.Correo))
                {
                    ModelState.AddModelError("Correo", "Ya existe un cliente con este correo");
                    CargarRoles(cliente.Rol);
                    return View(cliente);
                }

                cliente.Contraseña = PasswordHelper.Encriptar(cliente.Contraseña);
                cliente.FechaRegistro = DateTime.Now;

                _clienteRepository.Add(cliente);
                TempData["MensajeExito"] = "¡Registro exitoso! Ahora puede iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            CargarRoles(cliente.Rol);
            return View(cliente);
        }

        // ✅ PROTEGIDO - Solo Administradores
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var cliente = _clienteRepository.GetById(id);
            if (cliente == null || cliente.Borrado)
            {
                TempData["MensajeError"] = "El cliente no existe";
                return RedirectToAction(nameof(Index));
            }

            CargarRoles(cliente.Rol);
            return View(cliente);
        }

        // ✅ PROTEGIDO - Solo Administradores
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.Contraseña))
            {
                var clienteExistente = _clienteRepository.GetById(cliente.Id);
                if (clienteExistente == null)
                {
                    TempData["MensajeError"] = "El cliente no existe";
                    return RedirectToAction(nameof(Index));
                }

                var correoDuplicado = _clienteRepository.ExisteCorreo(cliente.Correo);
                if (correoDuplicado && clienteExistente.Correo != cliente.Correo)
                {
                    ModelState.AddModelError("Correo", "Ya existe otro cliente con este correo");
                    CargarRoles(cliente.Rol);
                    return View(cliente);
                }

                if (!string.IsNullOrEmpty(cliente.Contraseña) && cliente.Contraseña != clienteExistente.Contraseña)
                {
                    cliente.Contraseña = PasswordHelper.Encriptar(cliente.Contraseña);
                }
                else
                {
                    cliente.Contraseña = clienteExistente.Contraseña;
                }

                cliente.FechaRegistro = clienteExistente.FechaRegistro;
                cliente.Borrado = clienteExistente.Borrado;

                _clienteRepository.Update(cliente);
                TempData["MensajeExito"] = "Cliente actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }

            CargarRoles(cliente.Rol);
            return View(cliente);
        }

        // ✅ PROTEGIDO - Solo Administradores
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var cliente = _clienteRepository.GetById(id);
            if (cliente != null && !cliente.Borrado)
            {
                var tieneVehiculos = _context.Vehiculos.Any(v => v.ClienteId == id && !v.Borrado);
                if (tieneVehiculos)
                {
                    TempData["MensajeError"] = "No se puede eliminar el cliente porque tiene vehículos asociados";
                    return RedirectToAction(nameof(Index));
                }

                cliente.Borrado = true;
                _clienteRepository.Update(cliente);
                TempData["MensajeExito"] = "Cliente eliminado correctamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["MensajeError"] = "El cliente no existe";
            return RedirectToAction(nameof(Index));
        }

        // ✅ PROTEGIDO - Solo Administradores
        [HttpGet]
        public ActionResult Details(int id)
        {
            var cliente = _clienteRepository.GetById(id);
            if (cliente == null || cliente.Borrado)
            {
                TempData["MensajeError"] = "El cliente no existe";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        private void CargarRoles(RolUsuario? rolSeleccionado = null)
        {
            ViewBag.Roles = new SelectList(
                Enum.GetValues(typeof(RolUsuario))
                    .Cast<RolUsuario>()
                    .Select(r => new { Id = (int)r, Nombre = r.ToString() }),
                "Id",
                "Nombre",
                rolSeleccionado
            );
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