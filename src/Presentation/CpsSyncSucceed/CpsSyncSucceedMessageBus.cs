using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaMessageBus.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Cps360.SyncWithCps.Presentation.CpsSyncSucceed
{
    public class CpsSyncSucceedMessageBus : ICpsSyncSucceedMessageBus
    {
        private readonly IEnumerable<string> _cpsSyncSucceedMessageConsumeTopics;
        private static Action<ISubscribeOptions<string, CpsSyncSucceedMessage>> CpsSyncSucceedMessageSubscriptionOptions = options =>
        {
            options.ConsumerConfig.GroupId = "Cps360";
            options.ConsumerConfig.EnableAutoCommit = false;
            options.ConsumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            options.ConsumerConfig.EnableAutoOffsetStore = true;
            options.ConsumerConfig.AllowAutoCreateTopics = true;
        };

        private readonly string _cpsPortfolioMessageProduceTopic;
        private static Action<IPublishOptions<string, CpsPortfolioMessage>> CpsPortfolioMessagePublishOptions = options => { };
        private readonly IMessageBus _messageBus;

        public CpsSyncSucceedMessageBus(
            IMessageBus messageBus,
            IConfiguration configuration)
        {
            _messageBus = messageBus;
            
            _cpsSyncSucceedMessageConsumeTopics = configuration.GetSection("Consumers:CpsSyncSucceedMessageConsumer").GetValue<string>("TopicsStr").Split(",").Select(o => o.Trim());
            _cpsPortfolioMessageProduceTopic = configuration.GetSection("Producers:CpsPortfolioMessageProducer").GetValue<string>("Topic");
        }

        public Task SubscribeForCpsSyncSucceedMessage(Func<CpsSyncSucceedMessage, Task> messageProcessor, CancellationToken cancellationToken = default)
        {
            return _messageBus.Subscribe<CpsSyncSucceedMessage>(
                _cpsSyncSucceedMessageConsumeTopics,
                messageProcessor,
                CpsSyncSucceedMessageSubscriptionOptions,
                cancellationToken);
        }

        public void PublishCpsPortfolioMessage(CpsPortfolioMessage portfolioMessage)
        {
            _messageBus.Publish(_cpsPortfolioMessageProduceTopic, portfolioMessage, CpsPortfolioMessagePublishOptions);
        }
    }
}