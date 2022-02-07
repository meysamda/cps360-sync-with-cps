using CPS360.Sync.CSD.Infrastructure.Data.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CPS360.Sync.CSD.Presentation.Init
{
    public static class DbContexts
    {
        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            var migrationsAssembly = typeof(CPS360.Sync.CSDDbContext).Assembly.FullName;

            services.AddDbContext<CPS360.Sync.CSDDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly))
            );
        }

        public static void MigrateCPS360.Sync.CSDDbContext(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CPS360.Sync.CSDDbContext>();
            dbContext.Database.Migrate();
        }
    }
}