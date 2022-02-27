using Cps360.SyncWithCps.Infrastructure.Data.DbContexts.Entities;
using AutoMapper;

namespace Cps360.SyncWithCps.Application.ContactPersons
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ContactPerson, ContactPerson>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<ContactPerson, ContactPersonListItem>();
        }
    }
}