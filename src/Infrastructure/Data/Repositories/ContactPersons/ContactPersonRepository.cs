using System;
using System.Linq;
using Cps360.SyncWithCps.Infrastructure.Data.DbContexts;
using Cps360.SyncWithCps.Infrastructure.Data.DbContexts.Entities;

namespace Cps360.SyncWithCps.Infrastructure.Data.Repositories.ContactPersons
{
    public class ContactPersonRepository : IRepository
    {
        private readonly SyncWithCpsDbContext _dbContext;

        public ContactPersonRepository(SyncWithCpsDbContext dbContext)
        {
            _dbContext = dbContext;    
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public int GetContactPersonsCount(ContactPersonsFilter filter)
        {
            var query = GetContactPersonsQuery(filter);
            return query.Count();
        }

        public IOrderedQueryable<ContactPerson> GetContactPersons(ContactPersonsPagableFilter filter)
        {
            var query = GetContactPersonsQuery(filter);

            if (filter.Skip.HasValue)
                query = query.Skip(filter.Skip.Value);

            if (filter.Limit.HasValue)
                query = query.Take(filter.Limit.Value);

            var orderedQuery = query.OrderBy(o => o.Id);
            if (!string.IsNullOrEmpty(filter.Sort))
            {

            }

            return orderedQuery;
        }

        public void CreateContactPerson(ContactPerson contactPerson)
        {
            _dbContext.ContactPersons.Add(contactPerson);
        }

        public ContactPerson GetContactPersonById(int id)
        {
            return _dbContext.ContactPersons.FirstOrDefault(o => o.Id == id);
        }

        public void DeleteContactPerson(ContactPerson contactPerson)
        {
            _dbContext.ContactPersons.Remove(contactPerson);
        }

        public void UpdateContactPerson(ContactPerson entity)
        {
            _dbContext.ContactPersons.Update(entity);
        }

        private IQueryable<ContactPerson> GetContactPersonsQuery(ContactPersonsFilter filter)
        {
            var query = _dbContext.ContactPersons.AsQueryable();

            if (!string.IsNullOrEmpty(filter.FirstName))
                query = query.Where(o => o.FirstName.Contains(filter.FirstName));

            if (!string.IsNullOrEmpty(filter.LastName))
                query = query.Where(o => o.LastName.Contains(filter.LastName));

            return query;
        }
    }
}