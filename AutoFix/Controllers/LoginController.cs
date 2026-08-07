using AutoFix.Entities;
using AutoFix.infraestructure.DBContext;
using AutoFix.infraestructure.Repositories;
using AutoFix.Utils;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace AutoFix.Controllers
{
    public class LoginController : Controller
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly AutoFixContext _context;

        public LoginController()
        {
            _context = new AutoFixContext();
            _clienteRepository = new ClienteRepository(_context);
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

            var cliente = _clienteRepository.Login(correo, contraseña);

            if (cliente != null)
            {

                Session["UsuarioId"] = cliente.Id;
                Session["UsuarioNombre"] = cliente.Nombre;
                Session["UsuarioRol"] = cliente.Rol.ToString();

                // ✅ CREAR TICKET DE AUTENTICACIÓN CON ROL
                var roles = cliente.Rol.ToString(); // "Administrador", "Mecanico", "Cliente"
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    cliente.Correo,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(120),
                    false,
                    roles
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


        // ==========================================================
        // RF-03: Recuperación de contraseña
        // ==========================================================

        // Paso 1: Solicitar correo
        [HttpGet]
        public ActionResult RecuperarContrasena()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarContrasena(string correo)
        {
            if (string.IsNullOrEmpty(correo))
            {
                ViewBag.Error = "Por favor, ingrese su correo electrónico.";
                return View();
            }

            var cliente = _clienteRepository.GetByCorreo(correo);

            if (cliente == null)
            {
                ViewBag.Error = "No existe una cuenta registrada con ese correo.";
                return View();
            }

            // Guardamos el correo en sesión temporalmente para el siguiente paso
            Session["RecuperacionCorreo"] = cliente.Correo;
            Session["RecuperacionVerificado"] = false;

            return RedirectToAction("VerificarIdentidad");
        }

        // Paso 2: Verificar identidad con el teléfono registrado
        [HttpGet]
        public ActionResult VerificarIdentidad()
        {
            if (Session["RecuperacionCorreo"] == null)
            {
                return RedirectToAction("RecuperarContrasena");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerificarIdentidad(string telefono)
        {
            if (Session["RecuperacionCorreo"] == null)
            {
                return RedirectToAction("RecuperarContrasena");
            }

            string correo = Session["RecuperacionCorreo"].ToString();
            var cliente = _clienteRepository.GetByCorreo(correo);

            if (cliente == null)
            {
                Session["RecuperacionCorreo"] = null;
                return RedirectToAction("RecuperarContrasena");
            }

            if (string.IsNullOrEmpty(telefono) || cliente.Telefono != telefono.Trim())
            {
                ViewBag.Error = "El teléfono no coincide con el registrado.";
                return View();
            }

            Session["RecuperacionVerificado"] = true;
            return RedirectToAction("NuevaContrasena");
        }

        // Paso 3: Definir nueva contraseña
        // Nota: los parámetros usan "Contrasena" (sin ñ) para evitar problemas
        // de encoding en el model binding del formulario.
        [HttpGet]
        public ActionResult NuevaContrasena()
        {
            if (Session["RecuperacionCorreo"] == null || Session["RecuperacionVerificado"] == null
                || (bool)Session["RecuperacionVerificado"] != true)
            {
                return RedirectToAction("RecuperarContrasena");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevaContrasena(string nuevaContrasena, string confirmarContrasena)
        {
            if (Session["RecuperacionCorreo"] == null || Session["RecuperacionVerificado"] == null
                || (bool)Session["RecuperacionVerificado"] != true)
            {
                return RedirectToAction("RecuperarContrasena");
            }

            if (string.IsNullOrEmpty(nuevaContrasena) || nuevaContrasena.Length < 6)
            {
                ViewBag.Error = "La contraseña debe tener al menos 6 caracteres.";
                return View();
            }

            if (nuevaContrasena != confirmarContrasena)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            string correo = Session["RecuperacionCorreo"].ToString();
            var cliente = _clienteRepository.GetByCorreo(correo);

            if (cliente == null)
            {
                Session["RecuperacionCorreo"] = null;
                Session["RecuperacionVerificado"] = null;
                return RedirectToAction("RecuperarContrasena");
            }

            cliente.Contraseña = PasswordHelper.Encriptar(nuevaContrasena);
            _clienteRepository.Update(cliente);

            // Limpiar sesión de recuperación
            Session["RecuperacionCorreo"] = null;
            Session["RecuperacionVerificado"] = null;

            TempData["MensajeExito"] = "Contraseña actualizada correctamente. Ya puede iniciar sesión.";
            return RedirectToAction("Index");
        }

    }
}