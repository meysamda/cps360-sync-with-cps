using Cps360.SyncWithCps.Application.CpsPortfolios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cps360.SyncWithCps.Presentation.Init
{
    public static class OptionsRegistration
    {
        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var cpsPortfoliosApiOptions =  configuration.GetSection("CpsPortfoliosApi").Get<CpsPortfoliosApiOptions>();
            services.AddSingleton<CpsPortfoliosApiOptions>(cpsPortfoliosApiOptions);
        }
    }
}
