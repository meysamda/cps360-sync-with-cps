using System.Threading;
using System.Threading.Tasks;

namespace Cps360.SyncWithCps.Application.CpsPortfolios
{
    public class GetCpsPortfoliosCountHandler
    {
        private readonly ICpsPortfoliosApiClient _cpsPortfolioApiClient;

        public GetCpsPortfoliosCountHandler(ICpsPortfoliosApiClient cpsPortfolioApiClient)
        {
            _cpsPortfolioApiClient = cpsPortfolioApiClient;
        }

        public Task<int> Handle(CancellationToken cancellationToken = default) => 
            _cpsPortfolioApiClient.GetCpsPortfoliosCount(cancellationToken);
    }
}