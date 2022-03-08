using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cps360.SyncWithCps.Presentation.CpsSyncSucceed
{
    public interface ICpsSyncSucceedMessageBus
    {
        void PublishCpsPortfolioMessage(CpsPortfolioMessage message);
        Task SubscribeForCpsSyncSucceedMessage(Func<CpsSyncSucceedMessage, Task> messageProcessor, CancellationToken cancellationToken = default);
    }
}