using CPS360.Sync.CSD.Infrastructure.Data.DbContexts.Entities;
using Microsoft.EntityFrameworkCore;

namespace CPS360.Sync.CSD.Infrastructure.Data.DbContexts
{
    public class CPS360.Sync.CSDDbContext : DbContext, IUnitOfWork
    {
        public DbSet<ContactPerson> ContactPersons { get; set; }

        public CPS360.Sync.CSDDbContext(DbContextOptions<CPS360.Sync.CSDDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(CPS360.Sync.CSDDbContext).Assembly);
        }
    }
}