using Microsoft.AspNetCore.Builder;

namespace CPS360.Sync.CSD.Presentation.ErrorHandling
{
    public static class Utility
    {
        public static IApplicationBuilder UseCustomizedExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
