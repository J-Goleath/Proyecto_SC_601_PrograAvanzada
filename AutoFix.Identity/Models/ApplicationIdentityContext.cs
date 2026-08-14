using Microsoft.AspNet.Identity.EntityFramework;

namespace AutoFix.Identity.Models
{
    public class ApplicationIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationIdentityContext() : base("name=AutoFixDB", throwIfV1Schema: false)
        {
        }

        public static ApplicationIdentityContext Create()
        {
            return new ApplicationIdentityContext();
        }
    }
}
