using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace AutoFix.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            // ✅ OBTENER ROL DEL TICKET
            var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {
                    var userRoles = ticket.UserData.Split(',');
                    var allowedRoles = Roles.Split(',');

                    foreach (var role in allowedRoles)
                    {
                        if (userRoles.Contains(role.Trim()))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}