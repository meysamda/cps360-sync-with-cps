namespace Cps360.SyncWithCps.Infrastructure.Data.Repositories.ContactPersons
{
    public class ContactPersonsFilter : IFilter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}