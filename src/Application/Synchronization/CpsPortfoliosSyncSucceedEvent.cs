using System;

namespace Cps360.SyncWithCps.Application.Synchronization
{
    public class CpsPortfoliosSyncSucceedEvent
    {
        public DateTime Date { get; set; }
        public int PortfoliosCount { get; set; }
    }
}