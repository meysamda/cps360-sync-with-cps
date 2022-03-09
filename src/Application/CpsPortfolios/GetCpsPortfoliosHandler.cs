using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cps360.SyncWithCps.Application.Common.DomainExceptions;
using Microsoft.Extensions.Logging;

namespace Cps360.SyncWithCps.Application.CpsPortfolios
{
    public class GetCpsPortfoliosHandler
    {
        private const int PAGE_SIZE = 500;
        private readonly ICpsPortfoliosApiClient _cpsPortfolioApiClient;
        private readonly ILogger<GetCpsPortfoliosHandler> _logger;
        private static readonly object _lock = new object();
        private bool _inProgress;

        public GetCpsPortfoliosHandler(ICpsPortfoliosApiClient cpsPortfolioApiClient, ILogger<GetCpsPortfoliosHandler> logger)
        {
            _cpsPortfolioApiClient = cpsPortfolioApiClient;
            _logger = logger;
        }

        public async Task<IEnumerable<CpsPortfolio>> Handle(int cpsPortfoliosTotalCount, CancellationToken cancellationToken = default)
        {
            lock (_lock)
            {
                if (_inProgress)
                    throw new DomainException(ErrorMessage.alreadyInProgress);

                _inProgress = true;
            }

            var watch = Stopwatch.StartNew();
            var pagesCount = ((cpsPortfoliosTotalCount - 1) / PAGE_SIZE) + 1;

            _logger.LogDebug($"started to retrieve cps-portfolios, total count: {cpsPortfoliosTotalCount}, pages size: {PAGE_SIZE}, pages count: {pagesCount}");

            var tasks = new Task<IEnumerable<CpsPortfolio>>[pagesCount];
            for (int page = 0; page < pagesCount; page++)
            {
                var taskStart = watch.Elapsed;

                tasks[page] = _cpsPortfolioApiClient
                    .GetCpsPortfolios(page, PAGE_SIZE, cancellationToken)
                    .ContinueWith<IEnumerable<CpsPortfolio>>((t) =>
                    {
                        var taskEnd = watch.Elapsed;
                        var taskTotalSeconds = taskEnd.Subtract(taskStart).TotalSeconds;

                        _logger.LogDebug($"cps-portfolios retrieved succcessfully, page: {page}, count: {t.Result.Count()}, elapsed-time: {taskTotalSeconds} seconds");
                        return t.Result;
                    },
                    cancellationToken);
            }

            var taskResults = await Task.WhenAll(tasks);

            _logger.LogDebug($"cps-portfolios retrieved succcessfully, elapsed-time: {watch.Elapsed.TotalSeconds} seconds");

            _inProgress = false;

            return tasks.SelectMany(o => o.Result);
        }
    }
}