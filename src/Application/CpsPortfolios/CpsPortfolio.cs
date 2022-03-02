using System;
using System.Collections.Generic;

namespace Cps360.SyncWithCps.Application.CpsPortfolios
{
    public class CpsPortfolio
    {
        public string NationalCode { get; set; }
        public string Account { get; set; }
        public string NewAccount { get; set; }
        public long TradingCode { get; set; }
        public string PersonTypeName { get; set; }
        public DateTime PortfolioDate { get; set; }
        public int PortfolioPersainDate { get; set; }
        public IEnumerable<CpsStock> Stocks { get; set; }

        public class CpsStock
        {
            public string Isin { get; set; }
            public string Symbol { get; set; }
            public long Asset { get; set; }
            public bool IsFreezed { get; set; }
            public DateTime LastAssetChangeDate { get; set; }
            public int LastAssetChangePersianDate { get; set; }
        }
    }
}