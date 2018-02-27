using System;
using System.Security.Claims;

namespace MasterDataManager.Utils
{
    public static class PrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null) return null;
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            int id;
            var parsed = int.TryParse(claim?.Value, out id);
            if (parsed) return id;
            return null;
        }
    }
}