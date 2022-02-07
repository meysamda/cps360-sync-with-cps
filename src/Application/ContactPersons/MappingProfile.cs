using CPS360.Sync.CSD.Infrastructure.Data.DbContexts.Entities;
using AutoMapper;

namespace CPS360.Sync.CSD.Application.ContactPersons
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