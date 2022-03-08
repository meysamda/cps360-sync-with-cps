using Cps360.SyncWithCps.Application.CpsPortfolios;
using Cps360.SyncWithCps.Presentation.CpsSyncSucceed;
using Cps360.SyncWithCps.Presentation.ErrorHandling;
using Cps360.SyncWithCps.Presentation.Init;
using KafkaMessageBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cps360.SyncWithCps.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper();
            services.AddOptions(Configuration);
            services.AddHttpClient();

            services.AddMessageBus(Configuration);
            
            services.AddHostedService<CpsSyncSucceedMessageSubscriber>();
            services.AddSingleton<ICpsSyncSucceedMessageBus, CpsSyncSucceedMessageBus>();
            services.AddSingleton<CpsSyncSucceedMessageProcessor>();
            services.AddSingleton<GetCpsPortfoliosHandler>();
            services.AddSingleton<ICpsPortfoliosApiClient, CpsPortfoliosApiClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomizedExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
