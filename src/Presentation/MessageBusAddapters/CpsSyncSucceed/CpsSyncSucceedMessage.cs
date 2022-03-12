using System;

namespace Cps360.SyncWithCps.Presentation.MessageBusAdapters.CpsSyncSucceed
{
    public class CpsSyncSucceedMessage
    {
        public DateTime Date { get; set; }
        public int PortfoliosCount { get; set; }
    }
}