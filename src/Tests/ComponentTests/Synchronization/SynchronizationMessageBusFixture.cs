using System;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Cps360.SyncWithCps.Tests.ComponentTests.Common;
using Microsoft.Extensions.Configuration;

namespace Cps360.SyncWithCps.Tests.ComponentTests.Synchronization
{
    public class SynchronizationMessageBusFixture : MessageBusFixture
    {
        private readonly string _cpsPortfoliosSyncSucceedTopic;
        private readonly string _cpsPortfoliosTopic;

        public SynchronizationMessageBusFixture()
        {
            var topicsStr = Configuration.GetSection("Consumers:CpsPortfoliosSyncSucceedMessageConsumer").GetValue<string>("TopicsStr");
            _cpsPortfoliosSyncSucceedTopic = topicsStr.Split(",").First();

            _cpsPortfoliosTopic = Configuration.GetSection("Producers:CpsPortfoliosMessageProducer").GetValue<string>("Topic");
        }

        public Task PrepareCleanTopics()
        {
            return PrepareCleanTopics(_cpsPortfoliosSyncSucceedTopic, _cpsPortfoliosTopic);
        }

        public Task PublishCpsPortfoliosSyncSucceedMessage(DateTime date, int portfoliosCount)
        {
            var message = new CpsPortfoliosSyncSucceedMessage {
                Date = date,
                PortfoliosCount = portfoliosCount
            };

            return _messageBus.PublishAsync(_cpsPortfoliosSyncSucceedTopic, message);
        }

        public IConsumer<string, CpsPortfoliosMessage> GetCpsPortfoliosMessageConsumer()
        {
            var options = _messageBus.GetDefaultSubscribeOptions<string, CpsPortfoliosMessage>();
            var consumer = _messageBus.GetConsumer(options);
            consumer.Subscribe(_cpsPortfoliosTopic);

            return consumer;
        }
    }
}