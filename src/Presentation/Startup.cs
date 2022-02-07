using CPS360.Sync.CSD.Application.ContactPersons;
using CPS360.Sync.CSD.Infrastructure.Data.Repositories.ContactPersons;
using CPS360.Sync.CSD.Presentation.ErrorHandling;
using CPS360.Sync.CSD.Presentation.Init;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CPS360.Sync.CSD.Presentation
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

            services.AddDbContexts(Configuration);

            services.AddCustomizedAuthentication(Configuration);

            services.AddCustomizedSwagger(Configuration);

            services.AddScoped<ContactPersonService>();
            services.AddScoped<ContactPersonRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomizedExceptionHandler();
            app.UseSwaggerAndSwaggerUI(Configuration);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MigrateCPS360.Sync.CSDDbContext();
        }
    }
}
