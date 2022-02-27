using Cps360.SyncWithCps.Infrastructure.Data.DbContexts.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cps360.SyncWithCps.Infrastructure.Data.DbContexts
{
    public class SyncWithCpsDbContext : DbContext, IUnitOfWork
    {
        public DbSet<ContactPerson> ContactPersons { get; set; }

        public SyncWithCpsDbContext(DbContextOptions<SyncWithCpsDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(SyncWithCpsDbContext).Assembly);
        }
    }
}