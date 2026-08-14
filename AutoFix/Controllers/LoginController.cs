using AutoFix.Application.DTOs;
using AutoFix.Application.Interfaces;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace AutoFix.Controllers
{
    public class LoginController : Controller
    {
        private readonly IClienteService _clienteService;

        public LoginController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string correo, string contraseña)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña))
            {
                ViewBag.Error = "Por favor, ingrese correo y contraseña.";
                return View();
            }

            var resultado = _clienteService.Login(new LoginDTO
            {
                Correo = correo,
                Contraseña = contraseña
            });

            if (resultado.Success)
            {
                var cliente = resultado.Value;

                Session["UsuarioId"] = cliente.Id;
                Session["UsuarioNombre"] = cliente.Nombre;
                Session["UsuarioRol"] = cliente.Rol;

                // ✅ CREAR TICKET DE AUTENTICACIÓN CON ROL
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    cliente.Correo,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(120),
                    false,
                    cliente.Rol
                );

                var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);

                System.Diagnostics.Debug.WriteLine($"=== LOGIN EXITOSO ===");
                System.Diagnostics.Debug.WriteLine($"Nombre: {cliente.Nombre}");
                System.Diagnostics.Debug.WriteLine($"Rol: {cliente.Rol}");

                TempData["MensajeExito"] = $"Bienvenido, {cliente.Nombre}!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Correo o contraseña incorrectos.";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            TempData["MensajeExito"] = "Sesión cerrada correctamente.";
            return RedirectToAction("Index", "Login");
        }
    }
}
