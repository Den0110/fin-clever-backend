using System;
using System.Linq;
using System.Security.Claims;

namespace FinClever.Controllers
{
    public static class UserHelpers
    {
        public static string GetId(this ClaimsPrincipal principal)
        {
            return GetClaim(principal, ClaimTypes.NameIdentifier, "user_id");
        }

        public static string GetName(this ClaimsPrincipal principal)
        {
            return GetClaim(principal, "name");
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return GetClaim(principal, ClaimTypes.Email);
        }

        public static string GetImage(this ClaimsPrincipal principal)
        {
            return GetClaim(principal, "picture");
        }

        private static string GetClaim(this ClaimsPrincipal principal, params string[] type)
        {
            var id = principal?.FindFirst(c => type.Any(t => c?.Type == t))?.Value;
            if (string.IsNullOrEmpty(id))
                return null;
            else
                return id;
        }

    }
}
