using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Cps360.SyncWithCps.Presentation.Common;
using Cps360.SyncWithCps.Presentation.MessageBusAdapters.CpsSyncSucceed;
using Cps360.SyncWithCps.Tests.ComponentTests.Common;
using Microsoft.Extensions.Configuration;

namespace Cps360.SyncWithCps.Tests.ComponentTests.CpsSyncSucceed
{
    public class CpsSyncSucceedMessageBusFixture : MessageBusFixture
    {
        private IEnumerable<string> _cpsSyncSucceedMessageConsumeTopics;
        private readonly string _cpsPortfolioMessageProduceTopic;

        public CpsSyncSucceedMessageBusFixture()
        {
            _cpsSyncSucceedMessageConsumeTopics = Configuration.GetSection("Consumers:CpsSyncSucceedMessageConsumer").GetValue<string>("TopicsStr").Split(",");
            _cpsPortfolioMessageProduceTopic = Configuration.GetSection("Producers:CpsPortfolioMessageProducer").GetValue<string>("Topic");
        }

        public Task PrepareCleanTopics()
        {
            return PrepareCleanTopics(_cpsSyncSucceedMessageConsumeTopics.ToArray())
                .ContinueWith((t) => PrepareCleanTopics(_cpsPortfolioMessageProduceTopic));
        }

        public Task PublishCpsSyncSucceedMessage(DateTime date, int portfoliosCount)
        {
            var message = new CpsSyncSucceedMessage {
                Date = date,
                PortfoliosCount = portfoliosCount
            };

            return _messageBus.PublishAsync(_cpsSyncSucceedMessageConsumeTopics.First(), message);
        }

        public IConsumer<string, CpsPortfolioMessage> GetCpsPortfolioMessageConsumer()
        {
            var options = _messageBus.GetDefaultSubscribeOptions<string, CpsPortfolioMessage>();
            var consumer = _messageBus.GetConsumer(options);
            consumer.Subscribe(_cpsPortfolioMessageProduceTopic);

            return consumer;
        }
    }
}