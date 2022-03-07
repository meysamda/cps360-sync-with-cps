using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System;
using KafkaMessageBus.Abstractions;
using System.Collections.Generic;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Cps360.SyncWithCps.Presentation.CpsSyncSucceed
{
    public class CpsSyncSucceedMessageSubscriber : BackgroundService
    {
        private readonly ICpsSyncSucceedMessageBus _messageBus;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<CpsSyncSucceedMessageSubscriber> _logger;

        public CpsSyncSucceedMessageSubscriber(
            ICpsSyncSucceedMessageBus messageBus,
            IHostApplicationLifetime hostApplicationLifetime,
            ILogger<CpsSyncSucceedMessageSubscriber> logger)
        {
            _messageBus = messageBus;
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _messageBus.SubscribeForCpsSyncSucceedMessage<CpsSyncSucceedMessageProcessor>(stoppingToken);
            }
            catch (System.Exception ex)
            {
                var hostedServiceTypeName = this.GetType().Name;

                _logger.LogError(ex, "Application stopped due to an unhandled exception in {HostedService}.", hostedServiceTypeName);

                // TODO: There is a bug: In dotnet core 3.1, exceptions thrown from tasks are lost.
                // Dotnet 6.0 fixed it so that such exceptions are caused to stop application host.
                // In earlier versions we can force expected behavior by stopping application host manually.

                // If everything worked properly, it was enough to replace the code blow with next line of code.
                // throw;

                _hostApplicationLifetime.StopApplication();
            }
            
        }
    }
}
