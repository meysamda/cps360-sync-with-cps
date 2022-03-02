using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cps360.SyncWithCps.Application.CpsPortfolios;
using Cps360.SyncWithCps.Presentation.CpsSyncSucceed;
using Cps360.SyncWithCps.Tests.IntegrationTests.Common;
using KafkaMessageBus.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Cps360.SyncWithCps.Tests.IntegrationTests.Synchronization
{
    [Collection("Collection-1")]
    public class CpsSyncSucceedTests
    {
        [Fact]
        public async Task Processing_cps_sync_succeed_message()
        {
            // arrange
            var cpsPortfolios = GenerateCpsPortfolios(10);
            var cpsPortfoliosApiClientMock = new Mock<ICpsPortfoliosApiClient>();
            cpsPortfoliosApiClientMock
                .Setup(x => x.GetCpsPortfolios(10, 500, default(CancellationToken)))
                .Returns(() => Task.FromResult(cpsPortfolios));

            var getCpsPortfoliosHandlerLoggerMock = new Mock<ILogger<GetCpsPortfoliosHandler>>();
            var getCpsPortfoliosHandler = new GetCpsPortfoliosHandler(cpsPortfoliosApiClientMock.Object, getCpsPortfoliosHandlerLoggerMock.Object);
            var messageProcessorLoggerMock = new Mock<ILogger<CpsSyncSucceedMessageProcessor>>();
            var messageBusMock = new Mock<IMessageBus>();
            var configuration = TestConfigurationUtility.GetConfiguration();
            var sut = new CpsSyncSucceedMessageProcessor(getCpsPortfoliosHandler, configuration, messageProcessorLoggerMock.Object, messageBusMock.Object);
            
            // act
            var message = new CpsSyncSucceedMessage { PortfoliosCount = 10, Date = DateTime.Now };
            await sut.Process(message);

            // assert
            foreach (var cpsPortfolio in cpsPortfolios)
            {
                messageBusMock.Verify(x => x.Publish("", cpsPortfolio, ), Times.Once);
            }
        }

        private IEnumerable<CpsPortfolio> GenerateCpsPortfolios(int count)
        {
            var result = new CpsPortfolio[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = new CpsPortfolio { NationalCode = i.ToString() };
            }
            
            return result;
        }
    }
}
