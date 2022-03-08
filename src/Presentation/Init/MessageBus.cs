using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cps360.SyncWithCps.Presentation.Init
{
    public static class MessageBus
    {
        public static void AddMessageBus(this IServiceCollection services, IConfiguration configuration)
        {
            var brokersString = configuration.GetValue<string>("KafkaBootstrapServers");
            var brokers = brokersString.Split(",").Select(o => o.Trim()).ToArray();

            services.AddMessageBus(brokers, brokers);
        }
    }
}
