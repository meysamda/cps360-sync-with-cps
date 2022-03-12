using Cps360.SyncWithCps.Application.CpsPortfolios;
using Cps360.SyncWithCps.Presentation.MessageBusAdapters.CpsSyncSucceed;
using Cps360.SyncWithCps.Presentation.Init;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cps360.SyncWithCps.Presentation.Common;
using Cps360.SyncWithCps.Presentation.HttpAdapters.ErrorHandling;

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
            services.AddSingleton<ICpsSyncSucceedSubscriptionMessageBus, CpsSyncSucceedSubscriptionMessageBus>();
            services.AddSingleton<ICpsPortfolioPublishMessageBus, CpsPortfolioPublishMessageBus>();
            
            services.AddHostedService<CpsSyncSucceedMessageSubscriber>();
            services.AddSingleton<CpsSyncSucceedMessageProcessor>();
            services.AddSingleton<GetCpsPortfoliosHandler>();
            services.AddSingleton<ICpsPortfoliosApiClient, CpsPortfoliosApiClient>();

            services.AddCustomizedSwagger(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.UseSwaggerAndSwaggerUI(Configuration);

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
