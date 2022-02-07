using System;
using System.Threading;
using System.Threading.Tasks;

namespace CPS360.Sync.CSD.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        int SaveChanges();
    }
}