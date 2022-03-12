using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaMessageBus.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Cps360.SyncWithCps.Presentation.MessageBusAdapters.CpsSyncSucceed
{
    public class CpsSyncSucceedSubscriptionMessageBus : ICpsSyncSucceedSubscriptionMessageBus
    {
        private readonly IEnumerable<string> _topics;
        private static Action<ISubscribeOptions<string, CpsSyncSucceedMessage>> Options = options =>
        {
            options.ConsumerConfig.GroupId = "Cps360";
            options.ConsumerConfig.EnableAutoCommit = false;
            options.ConsumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            options.ConsumerConfig.EnableAutoOffsetStore = true;
            options.ConsumerConfig.AllowAutoCreateTopics = true;
        };

        
        private readonly IMessageBus _messageBus;

        public CpsSyncSucceedSubscriptionMessageBus(IMessageBus messageBus, IConfiguration configuration)
        {
            _messageBus = messageBus;
            _topics = configuration.GetSection("Consumers:CpsSyncSucceedMessageConsumer").GetValue<string>("TopicsStr").Split(",").Select(o => o.Trim());
        }

        public Task Subscribe(Func<CpsSyncSucceedMessage, Task> messageProcessor, CancellationToken cancellationToken = default)
        {
            return _messageBus.Subscribe<CpsSyncSucceedMessage>(
                _topics,
                messageProcessor,
                Options,
                cancellationToken);
        }
    }
}