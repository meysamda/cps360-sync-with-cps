using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cps360.SyncWithCps.Infrastructure.Data.DbContexts;
using Microsoft.Extensions.Logging;

namespace Cps360.SyncWithCps.Application.Synchronization
{
    public class CpsPortfoliosSyncSucceedEventHandler
    {
        private const int PAGE_SIZE = 500;

        private readonly ICpsPortfoliosApiClient _cpsPortfolioApiClient;
        private readonly SyncWithCpsDbContext _dbContext;
        private readonly ILogger<CpsPortfoliosSyncSucceedEventHandler> _logger;

        public CpsPortfoliosSyncSucceedEventHandler(ICpsPortfoliosApiClient cpsPortfolioApiClient, SyncWithCpsDbContext dbContext, ILogger<CpsPortfoliosSyncSucceedEventHandler> logger)
        {
            _cpsPortfolioApiClient = cpsPortfolioApiClient;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<CpsPortfolio>> Handle(CpsPortfoliosSyncSucceedEvent evt, CancellationToken cancellationToken)
        {
            var pagesCount = ((evt.PortfoliosCount - 1) / PAGE_SIZE) + 1;

            var tasks = new Task<IEnumerable<CpsPortfolio>>[pagesCount];
            for (int page = 0; page < pagesCount; page++)
                tasks[page] = _cpsPortfolioApiClient.GetPortfolios(page, PAGE_SIZE, cancellationToken);

            try
            {
                var taskResults = await Task.WhenAll(tasks);
                var portfolios = tasks.SelectMany(o => o.Result);
                return portfolios;    
            }
            catch (System.Exception)
            {
                
                throw;
            }
            
        }
    }
}