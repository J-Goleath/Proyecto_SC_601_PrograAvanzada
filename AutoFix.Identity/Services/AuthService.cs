using AutoFix.Identity.Managers;
using AutoFix.Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AutoFix.Identity.Services
{
    public class AuthService : IAuthService
    {
        private ApplicationUserManager UserManager =>
            HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private ApplicationSignInManager SignInManager =>
            HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();

        private IAuthenticationManager AuthenticationManager =>
            HttpContext.Current.GetOwinContext().Authentication;

        public async Task<AuthResult> LoginAsync(string correo, string contraseña)
        {
            var user = await UserManager.FindAsync(correo, contraseña);
            if (user == null)
                return AuthResult.Fail("Correo o contraseña incorrectos.");

            var result = await SignInManager.PasswordSignInAsync(correo, contraseña, isPersistent: false, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    return AuthResult.Ok(user.ClienteId, user.NombreCompleto, user.Rol.ToString());
                case SignInStatus.LockedOut:
                    return AuthResult.Fail("Cuenta bloqueada temporalmente por intentos fallidos. Intente de nuevo en unos minutos.");
                default:
                    return AuthResult.Fail("Correo o contraseña incorrectos.");
            }
        }

        public async Task<AuthResult> RegistrarAsync(string nombreCompleto, string correo, string telefono, string contraseña)
        {


            if (UserManager.Users.Any(u => u.Email == correo))
                return AuthResult.Fail("Ya existe una cuenta con este correo.");

            var user = new ApplicationUser
            {
                UserName = correo,
                Email = correo,
                PhoneNumber = telefono,
                NombreCompleto = nombreCompleto,
                Rol = AutoFix.Domain.Entities.RolUsuario.Cliente
            };

            var createResult = await UserManager.CreateAsync(user, contraseña);
            if (!createResult.Succeeded)
                return AuthResult.Fail(string.Join(" ", createResult.Errors));

            return AuthResult.Ok(user.ClienteId, user.NombreCompleto, user.Rol.ToString());
        }

        public void Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
