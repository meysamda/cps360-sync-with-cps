namespace CPS360.Sync.CSD.Infrastructure.Data.Repositories.ContactPersons
{
    public class ContactPersonsPagableFilter : ContactPersonsFilter, IPagableFilter
    {
        public int? Skip { get; set; }
        public int? Limit { get; set; }
        public string Sort { get; set; }
    }
}