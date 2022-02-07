namespace CPS360.Sync.CSD.Infrastructure.Data.Repositories
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}