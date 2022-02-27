using Cps360.SyncWithCps.Infrastructure.Data.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cps360.SyncWithCps.Presentation.Init
{
    public static class DbContexts
    {
        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            var migrationsAssembly = typeof(SyncWithCpsDbContext).Assembly.FullName;

            services.AddDbContext<SyncWithCpsDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly))
            );
        }

        public static void MigrateSyncDbContext(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SyncWithCpsDbContext>();
            dbContext.Database.Migrate();
        }
    }
}