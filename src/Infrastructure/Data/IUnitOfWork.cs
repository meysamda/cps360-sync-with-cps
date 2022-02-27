using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cps360.SyncWithCps.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        int SaveChanges();
    }
}