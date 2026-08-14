using AutoFix.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AutoFix.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int ClienteId { get; set; }
        public string NombreCompleto { get; set; }
        public RolUsuario Rol { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim("ClienteId", ClienteId.ToString()));
            userIdentity.AddClaim(new Claim(ClaimTypes.Role, Rol.ToString()));
            return userIdentity;
        }
    }
}
