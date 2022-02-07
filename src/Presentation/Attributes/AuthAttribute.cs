using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CPS360.Sync.CSD.Presentation.Attributes
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