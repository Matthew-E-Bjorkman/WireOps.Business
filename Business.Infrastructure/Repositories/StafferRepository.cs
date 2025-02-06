using Business.Infrastructure.Database.SQL.EntityFramework.Objects;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;
using WireOps.Business.Infrastructure.Communication.Publisher;
using WireOps.Business.Infrastructure.Database.SQL.EntityFramework;

namespace WireOps.Business.Infrastructure.Repositories;

public class StafferRepository
{
    public class EntityFramework(
        BusinessDbContext dbContext,
        StafferEventsOutbox stafferEventsOutbox,
        DomainEventPublisher domainEventPublisher
    ) : Staffer.Factory, Staffer.Repository
    {
        private readonly Dictionary<StafferId, DbStaffer> _staffers = new();
        private bool saveValidated = false;

        protected override Staffer.Data CreateData(StafferId id, CompanyId companyId, Email email, string givenName, string familyName, bool isOwner, RoleId? roleId)
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

            if (roleId.HasValue)
            {
                dbStaffer.RoleId = roleId;
            }

            _staffers.Add(id, dbStaffer);
            dbContext.Staffers.Add(dbStaffer);
            return dbStaffer;
        }

        public async Task<Staffer> GetBy(CompanyId companyId, StafferId id)
        {
            if (_staffers.ContainsKey(id))
                throw new DesignError(Error.SameAggregateRestoredMoreThanOnce);
            var dbStaffer = await dbContext.Staffers.Where(s => s.CompanyId == companyId)
                .SingleOrDefaultAsync(o => o.Id.Equals(id));
            if (dbStaffer is null)
                throw new DomainError(Error.AggregateNotFound);
            var staffer = Staffer.RestoreFrom(dbStaffer);
            _staffers.Add(id, dbStaffer);
            return staffer;
        }

        public async Task<Staffer> GetByUserId(string userId)
        {
            var dbStaffer = await dbContext.Staffers
                .SingleOrDefaultAsync(o => o.UserId == userId);
            if (dbStaffer is null)
                throw new DomainError(Error.AggregateNotFound);
            return Staffer.RestoreFrom(dbStaffer);
        }

        public async Task<IReadOnlyList<Staffer>> GetAllForCompany(CompanyId companyId)
        {
            var dbStaffers = await dbContext.Staffers.Where(s => s.CompanyId == companyId).ToListAsync();
            return dbStaffers.Select(Staffer.RestoreFrom).ToList();
        }

        public async Task<IReadOnlyList<Staffer>> GetAllByRole(CompanyId companyId, RoleId roleId)
        {
            var dbStaffers = await dbContext.Staffers.Where(s => s.CompanyId == companyId && s.RoleId == roleId).ToListAsync();
            return dbStaffers.Select(Staffer.RestoreFrom).ToList();
        }

        public async Task ValidateAndPublish(Staffer staffer)
        {
            if (saveValidated)
            {
                throw new DesignError(Error.SameAggregateValidatedMoreThanOnce);
            }

            if (!_staffers.TryGetValue(staffer.Id, out var dbStaffer))
                throw new DesignError(Error.SaveOfUnknownAggregate);

            if (_staffers.Values.Any(o => o != dbStaffer && o.IsOwner && o.CompanyId == dbStaffer.CompanyId))
                throw new DomainError(Error.CompanyHasMultipleOwners);

            if (_staffers.Values.Any(o => o != dbStaffer && o.Email == dbStaffer.Email))
                throw new DomainError(Error.CompanyHasMultipleOwners);
            dbStaffer.Version++;

            foreach (var domainEvent in staffer.DomainEvents)
            {
                await domainEventPublisher.PublishAsync(domainEvent);
                stafferEventsOutbox.Add(domainEvent);
            }

            saveValidated = true;
        }

        public Task Save() => dbContext.SaveChangesAsync();

        public async Task Delete(Staffer staffer)
        {
            if (!_staffers.TryGetValue(staffer.Id, out var dbStaffer))
                throw new DesignError(Error.DeleteOfUnknownAggregate);
            dbContext.Staffers.Remove(dbStaffer);

            var stafferDeletedEvent = Staffer.Events.StafferDeleted(staffer);
            await domainEventPublisher.PublishAsync(stafferDeletedEvent);
            stafferEventsOutbox.Add(stafferDeletedEvent);

            await dbContext.SaveChangesAsync();
        }
    }
}
