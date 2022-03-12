using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Cps360.SyncWithCps.Presentation.HttpAdapters.Common.Attributes
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public AuthAttribute(params string[] roles)
        {
            if (roles.Any())
            {
                Roles = $"{string.Join(", ", roles)}, root";
            }
        }
    }
}