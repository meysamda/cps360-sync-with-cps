using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Cps360.SyncWithCps.Presentation.HttpAdapters.ErrorHandling
{
    public class ModelStateValidationActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                throw new BadRequestException(context);
            }
            else
            {
                await next();
            }
        }
    }
}
