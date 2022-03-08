using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using Cps360.SyncWithCps.Application.CpsPortfolios;
using System.Diagnostics;
using AutoMapper;

namespace Cps360.SyncWithCps.Presentation.CpsSyncSucceed
{
    public class CpsSyncSucceedMessageProcessor
    {
        private readonly GetCpsPortfoliosHandler _getCpsPortfoliosHandler;
        private readonly ICpsSyncSucceedMessageBus _messageBus;
        private readonly IMapper _mapper;
        private readonly ILogger<CpsSyncSucceedMessageProcessor> _logger;

        public CpsSyncSucceedMessageProcessor(
            GetCpsPortfoliosHandler getCpsPortfoliosHandler,
            ICpsSyncSucceedMessageBus messageBus,
            IMapper mapper,
            ILogger<CpsSyncSucceedMessageProcessor> logger)
        {
            _getCpsPortfoliosHandler = getCpsPortfoliosHandler;
            _messageBus = messageBus;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Process(CpsSyncSucceedMessage message, CancellationToken cancellationToken = default)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogDebug("started to process message {Message}.", message);

            var portfolios = await _getCpsPortfoliosHandler.Handle(message.PortfoliosCount, cancellationToken);
            foreach (var portfolio in portfolios)
            {
                var portfolioMessage = _mapper.Map<CpsPortfolioMessage>(portfolio);
                _messageBus.PublishCpsPortfolioMessage(portfolioMessage);
            }

            _logger.LogDebug("message {Message} processed successfully, elapsed-time: {TotalSeconds} seconds", message, watch.Elapsed.TotalSeconds);
        }
    }
}
