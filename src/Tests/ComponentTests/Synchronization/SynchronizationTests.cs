using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Cps360.SyncWithCps.Tests.ComponentTests.Synchronization
{
    [Collection("Collection-1")]
    public class SynchronizationTests : IClassFixture<SynchronizationMessageBusFixture>
    {
        private readonly SynchronizationMessageBusFixture _messageBusFixture;

        public SynchronizationTests(SynchronizationMessageBusFixture messageBusFixture)
        {
            _messageBusFixture = messageBusFixture;
        }

        [Fact]
        public async Task Cps_portfolios_sync_succeed_message_triggers_csp_portfolios_messages_publishment()
        {
            // arrange
            var date = DateTime.UtcNow;
            var expectedPortfoliosCount = 10;
            await _messageBusFixture.PrepareCleanTopics();

            // act
            await _messageBusFixture.PublishCpsPortfoliosSyncSucceedMessage(date, expectedPortfoliosCount);

            // assert
            var maxTime = TimeSpan.FromMinutes(2);
            var watch = Stopwatch.StartNew();
            var finished = false;
            var consumer = _messageBusFixture.GetCpsPortfoliosMessageConsumer();
            CpsPortfoliosMessage message = null;

            while (!finished && watch.Elapsed < maxTime)
            {
                var consumeResult = consumer.Consume(default(CancellationToken));
                if (consumeResult != null && !consumeResult.IsPartitionEOF)
                {
                    message = consumeResult.Message.Value;
                    finished = true;
                }
            }

            message.Should().NotBeNull();
            message.Should().BeOfType<CpsPortfoliosMessage>();
            message.Portfolios.Should().NotBeNull();
            message.Portfolios.Count().Should().Be(expectedPortfoliosCount);
        }
    }
}
