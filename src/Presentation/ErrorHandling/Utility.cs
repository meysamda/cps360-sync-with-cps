using Microsoft.AspNetCore.Builder;

namespace Cps360.SyncWithCps.Presentation.ErrorHandling
{
    public static class Utility
    {
        public static IApplicationBuilder UseCustomizedExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
