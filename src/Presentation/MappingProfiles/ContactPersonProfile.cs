using CPS360.Sync.CSD.Infrastructure.Data.DbContexts.Entities;
using CPS360.Sync.CSD.Presentation.Commands;
using AutoMapper;

namespace CPS360.Sync.CSD.Presentation.MappingProfiles
{
    public class ContactPersonProfile : Profile
    {
        public ContactPersonProfile()
        {
            CreateMap<CreateContactPersonCommand, ContactPerson>();
            CreateMap<UpdateContactPersonCommand, ContactPerson>();
        }
    }
}