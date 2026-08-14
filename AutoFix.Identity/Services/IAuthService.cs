using System.Threading.Tasks;

namespace AutoFix.Identity.Services
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string correo, string contraseña);
        Task<AuthResult> RegistrarAsync(string nombreCompleto, string correo, string telefono, string contraseña);
        void Logout();
    }
}
