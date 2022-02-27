using System;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaMessageBus;
using KafkaMessageBus.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Cps360.SyncWithCps.Tests.ComponentTests.Common
{
    public abstract class MessageBusFixture
    {
        protected readonly IConfiguration Configuration;
        public readonly IMessageBus _messageBus;
        private readonly AdminClientConfig _adminClientConfig;

        public MessageBusFixture()
        {
            Configuration = TestConfigurationUtility.GetConfiguration();

            var bootstrapServersStr = Configuration.GetSection("KafkaBootstrapServers").Value;
            _adminClientConfig = new AdminClientConfig {
                ClientId = "test",
                BootstrapServers = bootstrapServersStr,
                Acks = Acks.Leader
            };

            var bootstrapServers = bootstrapServersStr
                .Split(",")
                .Select(o => o.Trim())
                .ToArray();

            _messageBus = new MessageBus(bootstrapServers, bootstrapServers);
        }

        protected async Task PrepareCleanTopics(params string[] topics)
        {
            using var adminClient = new AdminClientBuilder(_adminClientConfig).Build();

            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
            var existingTopics = metadata.Topics.Select(o => o.Topic).Where(o => topics.Any(x => x == o)).ToList();
            if (existingTopics.Any())
            {
                var partitionOffsets = existingTopics.Select(o => new TopicPartitionOffset(new TopicPartition(o, new Partition(0)), new Offset(0)));
                var result = await adminClient.DeleteRecordsAsync(partitionOffsets);
            }

            var newTopics = topics.Except(existingTopics);
            if (newTopics.Any())
            {
                var topicSpecifications = newTopics.Select(o => new TopicSpecification
                { 
                    Name = o,
                    ReplicationFactor = 1,
                    NumPartitions = 1 
                }).ToList();
                
                await adminClient.CreateTopicsAsync(topicSpecifications);
            }
        }
    }
}