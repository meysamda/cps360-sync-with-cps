namespace Cps360.SyncWithCps.Infrastructure.Data.Repositories
{
    public interface IPagableFilter : IFilter
    {
        int? Skip { get; set; }
        int? Limit { get; set; }
        string Sort { get; set; }
    }
}
