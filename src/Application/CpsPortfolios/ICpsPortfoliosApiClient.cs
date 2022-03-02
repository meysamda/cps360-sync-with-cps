using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cps360.SyncWithCps.Application.CpsPortfolios
{
    public interface ICpsPortfoliosApiClient
    {
        Task<IEnumerable<CpsPortfolio>> GetCpsPortfolios(int page, int pageSize, CancellationToken cancellationToken);
    }
}