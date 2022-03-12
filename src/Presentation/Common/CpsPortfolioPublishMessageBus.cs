using System;
using KafkaMessageBus.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Cps360.SyncWithCps.Presentation.Common
{
    public class CpsPortfolioPublishMessageBus : ICpsPortfolioPublishMessageBus
    {
        private readonly string _topic;
        private static Action<IPublishOptions<string, CpsPortfolioMessage>> Options = options => { };
        private readonly IMessageBus _messageBus;

        public CpsPortfolioPublishMessageBus(IMessageBus messageBus, IConfiguration configuration)
        {
            _topic = configuration.GetSection("Producers:CpsPortfolioMessageProducer").GetValue<string>("Topic");
            _messageBus = messageBus;
        }

        public void Publish(CpsPortfolioMessage portfolioMessage)
        {
            _messageBus.Publish(_topic, portfolioMessage, Options);
        }
    }
}