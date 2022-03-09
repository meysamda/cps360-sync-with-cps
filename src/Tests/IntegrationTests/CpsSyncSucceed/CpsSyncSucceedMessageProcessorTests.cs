using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cps360.SyncWithCps.Application.CpsPortfolios;
using Cps360.SyncWithCps.Presentation.Adapters;
using Cps360.SyncWithCps.Presentation.Adapters.MessageBusAdapters.CpsSyncSucceed;
using Cps360.SyncWithCps.Tests.IntegrationTests.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Cps360.SyncWithCps.Tests.IntegrationTests.CpsSyncSucceed
{
    [Collection("Collection-1")]
    public class CpsSyncSucceedMessageProcessorTests : IClassFixture<AutoMapperFixture>
    {
        private readonly IMapper _mapper;

        public CpsSyncSucceedMessageProcessorTests(AutoMapperFixture mapperFixture)
        {
            _mapper = mapperFixture.GetMapper();
        }

        [Fact]
        public async Task Processing_cps_sync_succeed_message()
        {
            // arrange
            
            // prepare GetCpsPortfoliosHandler to being resolved form host
            var portfolios = GenerateCpsPortfolios(10);
            var getCpsPortfoliosHandlerLoggerMock = new Mock<ILogger<GetCpsPortfoliosHandler>>();
            var cpsPortfoliosApiClientStub = new Mock<ICpsPortfoliosApiClient>();
            cpsPortfoliosApiClientStub
                .Setup(x => x.GetCpsPortfolios(0, 500, default(CancellationToken)))
                .Returns(() => Task.FromResult(portfolios));
            
            var getCpsPortfoliosHandler = new GetCpsPortfoliosHandler(cpsPortfoliosApiClientStub.Object, getCpsPortfoliosHandlerLoggerMock.Object);
            var messageProcessorLoggerMock = new Mock<ILogger<CpsSyncSucceedMessageProcessor>>();
            var messageBusMock = new Mock<ICpsPortfolioPublishMessageBus>();
            var sut = new CpsSyncSucceedMessageProcessor(getCpsPortfoliosHandler, messageBusMock.Object, _mapper, messageProcessorLoggerMock.Object);
            
            // act
            var message = new CpsSyncSucceedMessage { PortfoliosCount = 10, Date = DateTime.Now };
            await sut.Process(message);

            // assert
            foreach (var portfolio in portfolios)
            {
                var portfolioMessage = _mapper.Map<CpsPortfolioMessage>(portfolio);
                messageBusMock.Verify(x => x.Publish(It.Is<CpsPortfolioMessage>(o => o.NationalCode == portfolioMessage.NationalCode)), Times.Once);
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