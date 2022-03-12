using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Cps360.SyncWithCps.Presentation;
using Cps360.SyncWithCps.Presentation.Common;
using Cps360.SyncWithCps.Tests.ComponentTests.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Cps360.SyncWithCps.Tests.ComponentTests.CpsSyncSucceed
{
    [Collection("Collection-1")]
    public class CpsSyncSucceedMessageSubscriberTests : IClassFixture<CpsSyncSucceedMessageBusFixture>
    {
        private readonly CpsSyncSucceedMessageBusFixture _messageBusFixture;

        public CpsSyncSucceedMessageSubscriberTests(CpsSyncSucceedMessageBusFixture messageBusFixture)
        {
            _messageBusFixture = messageBusFixture;
        }

        [Fact]
        public async Task Recieves_and_publishes_cps_portfolios_by_recevinig_cps_sync_succeed_message()
        {
            // arrange
            var date = DateTime.UtcNow;
            var expectedPortfoliosCount = 10;
            var actualCount = 0; 
            await _messageBusFixture.PrepareCleanTopics();
            
            var host = CreateHost();
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, args) => host.Run();
            worker.RunWorkerAsync();
            // var sut = new TestServer(host.Services);

            // act
            await _messageBusFixture.PublishCpsSyncSucceedMessage(date, expectedPortfoliosCount);

            // assert
            var maxTime = TimeSpan.FromMinutes(2);
            var watch = Stopwatch.StartNew();
            var finished = false;
            var consumer = _messageBusFixture.GetCpsPortfolioMessageConsumer();
            var portfolios = new List<CpsPortfolioMessage>();

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

        private IHost CreateHost(){
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    var config = TestConfigurationUtility.GetConfiguration();
                    var startup = new Startup(config);
                    startup.ConfigureServices(services);
                });

            return hostBuilder.Build();
        }
    }
}
