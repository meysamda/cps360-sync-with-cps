using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Logging;
using KafkaMessageBus.Abstractions;
using Cps360.SyncWithCps.Application.CpsPortfolios;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Cps360.SyncWithCps.Presentation.CpsSyncSucceed
{
    public class CpsSyncSucceedMessageProcessor : IMessageProcessor<CpsSyncSucceedMessage>
    {
        private static Action<IPublishOptions<string, CpsPortfolio>> PublishOptions = options => { };
        private readonly string _produceTopic;
        private readonly GetCpsPortfoliosHandler _getCpsPortfoliosHandler;
        private readonly ILogger<CpsSyncSucceedMessageProcessor> _logger;
        private readonly IMessageBus _messageBus;

        public CpsSyncSucceedMessageProcessor(GetCpsPortfoliosHandler getCpsPortfoliosHandler, IConfiguration configuration, ILogger<CpsSyncSucceedMessageProcessor> logger, IMessageBus messageBus)
        {
            _produceTopic = configuration.GetSection("Producers:CpsPortfoliosMessageProducer").GetValue<string>("Topic");
            _getCpsPortfoliosHandler = getCpsPortfoliosHandler;
            _logger = logger;
            _messageBus = messageBus;
        }

        public async Task Process(CpsSyncSucceedMessage message, CancellationToken cancellationToken = default)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogDebug("started to process message {Message}.", message);

            var portfolios = await _getCpsPortfoliosHandler.Handle(message.PortfoliosCount, cancellationToken);
            foreach (var portfolio in portfolios)
            {
                _messageBus.Publish(_produceTopic, portfolio, PublishOptions);
            }

            _logger.LogDebug("message {Message} processed successfully, elapsed-time: {TotalSeconds} seconds", message, watch.Elapsed.TotalSeconds);
        }
    }
}
