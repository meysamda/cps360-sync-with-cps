using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cps360.SyncWithCps.Application.Synchronization
{
    public interface ICpsPortfoliosApiClient
    {
        Task<IEnumerable<CpsPortfolio>> GetPortfolios(int page, int pageSize, CancellationToken cancellationToken);
    }
}