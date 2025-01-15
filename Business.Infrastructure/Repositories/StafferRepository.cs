using Business.Infrastructure.Database.SQL.EntityFramework.Objects;
using Microsoft.EntityFrameworkCore;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Infrastructure.Database.SQL.EntityFramework;

namespace WireOps.Business.Infrastructure.Repositories;

public class StafferRepository
{
    public class EntityFramework(BusinessDbContext dbContext)
        : Staffer.Factory, Staffer.Repository
    {
        private readonly Dictionary<StafferId, DbStaffer> _staffers = new();

        protected override Staffer.Data CreateData(StafferId id, CompanyId companyId, Email email, string givenName, string familyName, bool isOwner)
        {
            var dbStaffer = new DbStaffer
            {
                Id = id,
                CompanyId = companyId,
                Email = email,
                GivenName = givenName,
                FamilyName = familyName,
                IsOwner = isOwner
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
                throw new DomainError(Error.AggregateNotFound);
            var staffer = Staffer.RestoreFrom(dbStaffer);
            _staffers.Add(id, dbStaffer);
            return staffer;
        }

        public async Task<IReadOnlyList<Staffer>> GetAll()
        {
            var dbStaffers = await dbContext.Staffers.ToListAsync();
            return dbStaffers.Select(Staffer.RestoreFrom).ToList();
        }

        public Task ValidateCanSave(Staffer staffer)
        {
            if (!_staffers.TryGetValue(staffer.Id, out var dbStaffer))
                throw new DesignError(Error.SaveOfUnknownAggregate);

            if (_staffers.Values.Any(o => o != dbStaffer && o.IsOwner && o.CompanyId == dbStaffer.CompanyId))
                throw new DomainError(Error.CompanyHasMultipleOwners);

            if (_staffers.Values.Any(o => o != dbStaffer && o.Email == dbStaffer.Email))
                throw new DomainError(Error.CompanyHasMultipleOwners);

            dbStaffer.Version++;

            return Task.CompletedTask;
        }

        public Task Save() => dbContext.SaveChangesAsync();

        public Task Delete(Staffer staffer)
        {
            if (!_staffers.TryGetValue(staffer.Id, out var dbStaffer))
                throw new DesignError(Error.DeleteOfUnknownAggregate);
            dbContext.Staffers.Remove(dbStaffer);
            return dbContext.SaveChangesAsync();
        }
    }
}
