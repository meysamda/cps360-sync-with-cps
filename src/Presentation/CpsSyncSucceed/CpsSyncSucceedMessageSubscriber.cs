using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Cps360.SyncWithCps.Presentation.CpsSyncSucceed
{
    public class CpsSyncSucceedMessageSubscriber : BackgroundService
    {
        private readonly CpsSyncSucceedMessageProcessor _cpsSyncSucceedMessageProcessor;
        private readonly ICpsSyncSucceedMessageBus _messageBus;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<CpsSyncSucceedMessageSubscriber> _logger;

        public CpsSyncSucceedMessageSubscriber(
            CpsSyncSucceedMessageProcessor cpsSyncSucceedMessageProcessor,
            ICpsSyncSucceedMessageBus messageBus,
            IHostApplicationLifetime hostApplicationLifetime,
            ILogger<CpsSyncSucceedMessageSubscriber> logger)
        {
            _cpsSyncSucceedMessageProcessor = cpsSyncSucceedMessageProcessor;
            _messageBus = messageBus;
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _messageBus.SubscribeForCpsSyncSucceedMessage(
                    (message) => _cpsSyncSucceedMessageProcessor.Process(message, stoppingToken),
                    stoppingToken);
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
