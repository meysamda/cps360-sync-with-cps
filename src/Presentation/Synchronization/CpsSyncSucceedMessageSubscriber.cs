using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System;
using KafkaMessageBus.Abstractions;
using System.Collections.Generic;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Cps360.SyncWithCps.Presentation.Syncronization
{
    public class CpsSyncSucceedMessageSubscriber : BackgroundService
    {
        private const string CONSUMER_GROUP_ID = "cps-sync-succeed";
        private readonly IEnumerable<string> _topics;
        private readonly ISubscriptionMessageBus _messageBus;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<CpsSyncSucceedMessageSubscriber> _logger;

        public CpsSyncSucceedMessageSubscriber(
            IEnumerable<string> topics,
            ISubscriptionMessageBus messageBus,
            IHostApplicationLifetime hostApplicationLifetime,
            ILogger<CpsSyncSucceedMessageSubscriber> logger)
        {
            _topics = topics;
            _messageBus = messageBus;
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _messageBus.Subscribe<string, CpsSyncSucceedMessage, CpsSyncSucceedMessageProcessor>(_topics,
                    options =>
                    {
                        options.ConsumerConfig.GroupId = CONSUMER_GROUP_ID;
                        options.ConsumerConfig.EnableAutoCommit = false;
                        options.ConsumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
                        options.ConsumerConfig.EnableAutoOffsetStore = true;
                        options.ConsumerConfig.AllowAutoCreateTopics = true;
                    }, 
                    stoppingToken);
            }
            catch (System.Exception ex)
            {
                var hostedServiceTypeName = this.GetType().Name;

                _logger.LogError(ex, "Application stopped due to an unhandled exception in hosted service {HostedService}.", hostedServiceTypeName);

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
