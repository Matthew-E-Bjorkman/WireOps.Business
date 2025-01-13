using Microsoft.EntityFrameworkCore;
using WireOps.Company.Common.Errors;
using WireOps.Company.Domain.Staffers;
using WireOps.Company.Infrastructure.Database.SQL.EntityFramework;

namespace WireOps.Company.Infrastructure.Repositories;

public class StafferRepository
{
    public class EntityFramework(CompanyDbContext dbContext)
        : Staffer.Factory, Staffer.Repository
    {
        private readonly Dictionary<StafferId, DbStaffer> _staffers = new();

        protected override Staffer.Data CreateData(StafferId id, string name, string sku, string? description)
        {
            var dbStaffer = new DbStaffer
            {
                Id = id,
                Name = name,
                SKU = sku,
                Description = description,
            };
            _staffers.Add(id, dbStaffer);
            dbContext.Staffers.Add(dbStaffer);
            return dbStaffer;
        }

        public async Task<Staffer> GetBy(StafferId id)
        {
            if (_staffers.ContainsKey(id))
                throw new DesignError(Error.SameAggregateRestoredMoreThanOnce);
            var dbStaffer = await dbContext.Staffers
                .SingleOrDefaultAsync(o => o.Id.Equals(id));
            if (dbStaffer is null)
                throw new DomainError();
            var staffer = Staffer.RestoreFrom(dbStaffer);
            _staffers.Add(id, dbStaffer);
            return staffer;
        }

        public async Task<IReadOnlyList<Staffer>> GetAll()
        {
            var dbStaffers = await dbContext.Staffers.ToListAsync();
            return dbStaffers.Select(Staffer.RestoreFrom).ToList();
        }

        public Task Save(Staffer staffer)
        {
            if (!_staffers.TryGetValue(staffer.Id, out var dbStaffer))
                throw new DesignError(Error.SaveOfUnknownAggregate);
            dbStaffer.Version++;
            return dbContext.SaveChangesAsync();
            // TODO: error when not all tracked staffers are explicitly saved
        }

        public Task Delete(Staffer staffer)
        {
            if (!_staffers.TryGetValue(staffer.Id, out var dbStaffer))
                throw new DesignError(Error.DeleteOfUnknownAggregate);
            dbContext.Staffers.Remove(dbStaffer);
            return dbContext.SaveChangesAsync();
        }
    }
}
