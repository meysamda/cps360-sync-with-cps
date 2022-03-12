using AutoMapper;
using Cps360.SyncWithCps.Application.CpsPortfolios;
using Cps360.SyncWithCps.Presentation.Common;

namespace Cps360.SyncWithCps.Presentation.MessageBusAdapters.CpsSyncSucceed
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<CpsPortfolio, CpsPortfolioMessage>();
        }
    }
}