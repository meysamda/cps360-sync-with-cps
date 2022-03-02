using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Cps360.SyncWithCps.Application.CpsPortfolios;
using FluentAssertions;
using Xunit;

namespace Cps360.SyncWithCps.Tests.ComponentTests.Synchronization
{
    [Collection("Collection-1")]
    public class CpsSyncSucceedTests : IClassFixture<SynchronizationMessageBusFixture>
    {
        private readonly SynchronizationMessageBusFixture _messageBusFixture;

        public CpsSyncSucceedTests(SynchronizationMessageBusFixture messageBusFixture)
        {
            _messageBusFixture = messageBusFixture;
        }

        [Fact]
        public async Task Receiving_cps_sync_succeed_message()
        {
            // arrange
            var date = DateTime.UtcNow;
            var expectedPortfoliosCount = 10;
            var actualCount = 0; 
            await _messageBusFixture.PrepareCleanTopics();

            // act
            await _messageBusFixture.PublishCpsPortfoliosSyncSucceedMessage(date, expectedPortfoliosCount);

            // assert
            var maxTime = TimeSpan.FromMinutes(2);
            var watch = Stopwatch.StartNew();
            var finished = false;
            var consumer = _messageBusFixture.GetCpsPortfolioMessageConsumer();
            var portfolios = new List<CpsPortfolio>();

            while (!finished && watch.Elapsed < maxTime)
            {
                var consumeResult = consumer.Consume(default(CancellationToken));
                if (consumeResult != null && !consumeResult.IsPartitionEOF)
                {
                    actualCount ++;
                    finished = expectedPortfoliosCount == actualCount;

                    portfolios.Add(consumeResult.Message.Value);
                }
            }

            actualCount.Should().Be(expectedPortfoliosCount);
        }
    }
}
