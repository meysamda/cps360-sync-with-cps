namespace Cps360.SyncWithCps.Infrastructure.Data.Repositories
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}