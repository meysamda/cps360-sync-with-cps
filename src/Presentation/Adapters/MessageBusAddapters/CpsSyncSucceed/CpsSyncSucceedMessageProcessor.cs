using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using Cps360.SyncWithCps.Application.CpsPortfolios;
using System.Diagnostics;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using Cps360.SyncWithCps.Application.Common.DomainExceptions;

namespace Cps360.SyncWithCps.Presentation.Adapters.MessageBusAdapters.CpsSyncSucceed
{
    public class CpsSyncSucceedMessageProcessor
    {
        private readonly GetCpsPortfoliosHandler _cpsPortfoliosHandler;
        private readonly ICpsPortfolioPublishMessageBus _messageBus;
        private readonly IMapper _mapper;
        private readonly ILogger<CpsSyncSucceedMessageProcessor> _logger;

        public CpsSyncSucceedMessageProcessor(
            GetCpsPortfoliosHandler cpsPortfoliosHandler,
            ICpsPortfolioPublishMessageBus messageBus,
            IMapper mapper,
            ILogger<CpsSyncSucceedMessageProcessor> logger)
        {
            _cpsPortfoliosHandler = cpsPortfoliosHandler;
            _messageBus = messageBus;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Process(CpsSyncSucceedMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                var watch = Stopwatch.StartNew();
                _logger.LogDebug("started to process message {Message}.", message);

                var portfolios = await _cpsPortfoliosHandler.Handle(message.PortfoliosCount, cancellationToken);
                foreach (var portfolio in portfolios)
                {
                    var portfolioMessage = _mapper.Map<CpsPortfolioMessage>(portfolio);
                    _messageBus.Publish(portfolioMessage);
                }

                _logger.LogDebug("message {Message} processed successfully, elapsed-time: {TotalSeconds} seconds", message, watch.Elapsed.TotalSeconds);
            }
            catch (Exception ex)
            {
                if (ex is DomainException domainException)
                    _logger.LogError(domainException, domainException.ErrorMessage.ToString());

                else throw;
            }
        }
    }
}
