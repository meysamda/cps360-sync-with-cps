using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System;
using Microsoft.Extensions.Logging;
using KafkaMessageBus.Abstractions;

namespace Cps360.SyncWithCps.Presentation.Syncronization
{
    public class CpsSyncSucceedMessageProcessor : IMessageProcessor<CpsSyncSucceedMessage>
    {
        public Task Process(CpsSyncSucceedMessage message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
