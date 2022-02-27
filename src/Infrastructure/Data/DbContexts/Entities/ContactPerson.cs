namespace Cps360.SyncWithCps.Infrastructure.Data.DbContexts.Entities
{
    public class ContactPerson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}