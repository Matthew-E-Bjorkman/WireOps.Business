using Business.Infrastructure.Database.SQL.EntityFramework.Objects;
using Microsoft.EntityFrameworkCore;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Infrastructure.Database.SQL.EntityFramework;

namespace WireOps.Business.Infrastructure.Repositories;

public class RoleRepository
{
    public class EntityFramework(BusinessDbContext dbContext)
        : Role.Factory, Role.Repository
    {
        private readonly Dictionary<RoleId, DbRole> _roles = new();
        private bool saveValidated = false;

        protected override Role.Data CreateData(RoleId id, CompanyId companyId, string name, bool isAdmin, bool isOwnerRole)
        {
            var dbRole = new DbRole
            {
                Id = id,
                CompanyId = companyId,
                Name = name, 
                IsAdmin = isAdmin,
                IsOwnerRole = isOwnerRole
            };

            _roles.Add(id, dbRole);
            dbContext.Roles.Add(dbRole);
            return dbRole;
        }

        public async Task<Role> GetBy(CompanyId companyId, RoleId id)
        {
            if (_roles.ContainsKey(id))
                throw new DesignError(Error.SameAggregateRestoredMoreThanOnce);
            var dbRole = await dbContext.Roles.Where(s => s.CompanyId == companyId)
                .SingleOrDefaultAsync(o => o.Id.Equals(id));
            if (dbRole is null)
                throw new DomainError(Error.AggregateNotFound);
            var role = Role.RestoreFrom(dbRole);
            _roles.Add(id, dbRole);
            return role;
        }

        public async Task<IReadOnlyList<Role>> GetAllForCompany(CompanyId companyId)
        {
            var dbRoles = await dbContext.Roles.Where(s => s.CompanyId == companyId).ToListAsync();
            return dbRoles.Select(Role.RestoreFrom).ToList();
        }

        public Task ValidateCanSave(Role role)
        {
            if (saveValidated)
            {
                throw new DesignError(Error.SameAggregateValidatedMoreThanOnce);
            }

            if (!_roles.TryGetValue(role.Id, out var dbRole))
                throw new DesignError(Error.SaveOfUnknownAggregate);

            if (_roles.Values.Any(o => o != dbRole && dbRole.IsOwnerRole && o.IsOwnerRole))
                throw new DomainError(Error.CompanyHasMultipleOwnerRoles);

            if (_roles.Values.Any(o => o != dbRole && o.Name == dbRole.Name))
                throw new DomainError(Error.NameAlreadyInUse);

            dbRole.Version++;
            saveValidated = true;

            return Task.CompletedTask;
        }

        public Task Save() => dbContext.SaveChangesAsync();

        public Task Delete(Role role)
        {
            if (!_roles.TryGetValue(role.Id, out var dbRole))
                throw new DesignError(Error.DeleteOfUnknownAggregate);
            dbContext.Roles.Remove(dbRole);
            return dbContext.SaveChangesAsync();
        }
    }
}
