using System.Threading;
using System.Threading.Tasks;
using KafkaMessageBus.Abstractions;

namespace Cps360.SyncWithCps.Presentation.CpsSyncSucceed
{
    public interface ICpsSyncSucceedMessageBus
    {
        void PublishCpsPortfolioMessage(CpsPortfolioMessage message);
        Task SubscribeForCpsSyncSucceedMessage<TMessageProcessor>(CancellationToken cancellationToken = default) where TMessageProcessor : IMessageProcessor<CpsSyncSucceedMessage>;
    }
}