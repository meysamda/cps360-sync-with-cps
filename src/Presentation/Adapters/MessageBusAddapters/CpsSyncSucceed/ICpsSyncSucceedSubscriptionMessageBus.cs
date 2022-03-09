using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cps360.SyncWithCps.Presentation.Adapters.MessageBusAdapters.CpsSyncSucceed
{
    public interface ICpsSyncSucceedSubscriptionMessageBus
    {
        Task Subscribe(Func<CpsSyncSucceedMessage, Task> messageProcessor, CancellationToken cancellationToken = default);
    }
}