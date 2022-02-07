using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExtension
    {
        public static string GetUserId(this HttpRequest request)
        {
            var claim = GetClaim(request, "sub");
            return claim?.Value;
        }
        
        private static Claim GetClaim(this HttpRequest request , string claimType)
        {
            var identityClaims = request.HttpContext.User.Identity as ClaimsIdentity;
            return identityClaims?.FindFirst(claimType);
        }
    }
}
