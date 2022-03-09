using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Cps360.SyncWithCps.Application.CpsPortfolios;
using Microsoft.Extensions.Logging;

namespace Cps360.SyncWithCps.Presentation.Adapters.HttpAdapters.CpsPortfolios
{
    public class CpsPortfoliosFetchAndPublishHandler
    {
        private readonly GetCpsPortfoliosHandler _getCpsPortfoliosHandler;
        private readonly GetCpsPortfoliosCountHandler _getCpsPortfoliosCountHandler;
        private readonly ICpsPortfolioPublishMessageBus _messageBus;
        private readonly ILogger<CpsPortfoliosFetchAndPublishHandler> _logger;
        private readonly IMapper _mapper;

        public CpsPortfoliosFetchAndPublishHandler(
            GetCpsPortfoliosHandler getCpsPortfoliosHandler,
            GetCpsPortfoliosCountHandler getCpsPortfoliosCountHandler,
            ICpsPortfolioPublishMessageBus messageBus,
            ILogger<CpsPortfoliosFetchAndPublishHandler> logger,
            IMapper mapper)
        {
            _getCpsPortfoliosHandler = getCpsPortfoliosHandler;
            _getCpsPortfoliosCountHandler = getCpsPortfoliosCountHandler;
            _messageBus = messageBus;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle()
        {
            var watch = Stopwatch.StartNew();
            _logger.LogDebug("started to fetch and publish cps portfolios.");
            var count = await _getCpsPortfoliosCountHandler.Handle();
            var portfolios = await _getCpsPortfoliosHandler.Handle(count);

            foreach (var portfolio in portfolios)
            {
                var portfolioMessage = _mapper.Map<CpsPortfolioMessage>(portfolio);
                _messageBus.Publish(portfolioMessage);
            }

            _logger.LogDebug($"fetched and published cps portfolios successfully, elapsed-time: {watch.Elapsed.TotalSeconds} seconds");
        }
    }
}