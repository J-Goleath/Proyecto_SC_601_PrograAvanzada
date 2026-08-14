using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using AutoFix.Application.Validators;
using AutoFix.Domain.Entities;
using AutoFix.Filters;
using FluentValidation.Mvc;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AutoFix.Controllers
{
    [RoutePrefix("Clientes")]
    [CustomAuthorize(Roles = "Administrador")]
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IVehiculoService _vehiculoService;

        public ClientesController(IClienteService clienteService, IVehiculoService vehiculoService)
        {
            _clienteService = clienteService;
            _vehiculoService = vehiculoService;
        }

        // ✅ PROTEGIDO - Solo Administradores
        [HttpGet]
        public ActionResult Index()
        {
            var resultado = _clienteService.GetAll();
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return View(Enumerable.Empty<ClienteDTO>());
            }
            return View(resultado.Value);
        }

        // ✅ PÚBLICO - Registro de clientes (SIN AUTENTICACIÓN)
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            CargarRoles();
            return View(new CreateClienteDTO());
        }

        // ✅ PÚBLICO - Registro de clientes (SIN AUTENTICACIÓN)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(CreateClienteDTO dto)
        {
            var validacion = new CreateClienteDTOValidator().Validate(dto);
            if (!validacion.IsValid)
            {
                validacion.AddToModelState(ModelState, null);
            }

            if (ModelState.IsValid)
            {
                var resultado = _clienteService.Create(dto);
                if (resultado.Success)
                {
                    TempData["MensajeExito"] = "¡Registro exitoso! Ahora puede iniciar sesión.";
                    return RedirectToAction("Index", "Login");
                }

                ModelState.AddModelError("Correo", resultado.Error);
            }

            CargarRoles(dto.Rol);
            return View(dto);
        }

        // ✅ PROTEGIDO - Solo Administradores
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var resultado = _clienteService.GetById(id);
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return RedirectToAction(nameof(Index));
            }

            var cliente = resultado.Value;
            var dto = new UpdateClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                Rol = cliente.Rol
            };

            CargarRoles(cliente.Rol);
            return View(dto);
        }

        // ✅ PROTEGIDO - Solo Administradores
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateClienteDTO dto)
        {
            // La contraseña es opcional al editar, así que la quitamos de la
            // validación del ModelState si vino vacía.
            if (string.IsNullOrEmpty(dto.Contraseña))
            {
                ModelState.Remove(nameof(dto.Contraseña));
            }

            var validacion = new UpdateClienteDTOValidator().Validate(dto);
            if (!validacion.IsValid)
            {
                validacion.AddToModelState(ModelState, null);
            }

            if (ModelState.IsValid)
            {
                var resultado = _clienteService.Update(dto);
                if (resultado.Success)
                {
                    TempData["MensajeExito"] = "Cliente actualizado correctamente";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("Correo", resultado.Error);
            }

            CargarRoles(dto.Rol);
            return View(dto);
        }

        // ✅ PROTEGIDO - Solo Administradores
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var vehiculosResultado = _vehiculoService.GetByCliente(id);
            if (vehiculosResultado.Success && vehiculosResultado.Value.Any())
            {
                TempData["MensajeError"] = "No se puede eliminar el cliente porque tiene vehículos asociados";
                return RedirectToAction(nameof(Index));
            }

            var resultado = _clienteService.Delete(id);
            if (resultado.Success)
            {
                TempData["MensajeExito"] = "Cliente eliminado correctamente";
            }
            else
            {
                TempData["MensajeError"] = resultado.Error;
            }
            return RedirectToAction(nameof(Index));
        }

        // ✅ PROTEGIDO - Solo Administradores
        [HttpGet]
        public ActionResult Details(int id)
        {
            var resultado = _clienteService.GetById(id);
            if (!resultado.Success)
            {
                TempData["MensajeError"] = resultado.Error;
                return RedirectToAction(nameof(Index));
            }

            var vehiculosResultado = _vehiculoService.GetByCliente(id);
            ViewBag.Vehiculos = vehiculosResultado.Success ? vehiculosResultado.Value : new System.Collections.Generic.List<VehiculoDTO>();

            return View(resultado.Value);
        }

        private void CargarRoles(string rolSeleccionado = null)
        {
            ViewBag.Roles = new SelectList(
                Enum.GetValues(typeof(RolUsuario))
                    .Cast<RolUsuario>()
                    .Select(r => new { Id = r.ToString(), Nombre = r.ToString() }),
                "Id",
                "Nombre",
                rolSeleccionado
            );
        }
    }
}
