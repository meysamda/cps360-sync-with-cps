using AutoMapper;
using Cps360.SyncWithCps.Application.CpsPortfolios;

namespace Cps360.SyncWithCps.Presentation.CpsSyncSucceed
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<CpsPortfolio, CpsPortfolioMessage>();
        }
    }
}