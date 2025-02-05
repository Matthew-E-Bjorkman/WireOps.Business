using Business.Infrastructure.Database.SQL.EntityFramework.Objects;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Infrastructure.Database.SQL.EntityFramework;

namespace WireOps.Business.Infrastructure.Repositories;

public class CompanyRepository
{
    public class EntityFramework(BusinessDbContext dbContext)
        : Company.Factory, Company.Repository
    {
        private readonly Dictionary<CompanyId, DbCompany> _companies = new();
        private bool saveValidated = false;

        protected override Company.Data CreateData(CompanyId id, string name)
        {
            var dbCompany = new DbCompany
            {
                Id = id,
                Name = name
            };
            _companies.Add(id, dbCompany);
            dbContext.Companies.Add(dbCompany);
            return dbCompany;
        }

        public async Task<Company> GetBy(CompanyId id)
        {
            if (_companies.ContainsKey(id))
                throw new DesignError(Error.SameAggregateRestoredMoreThanOnce);
            var dbCompany = await dbContext.Companies
                .SingleOrDefaultAsync(o => o.Id.Equals(id));
            if (dbCompany is null)
                throw new DomainError(Error.AggregateNotFound);
            var company = Company.RestoreFrom(dbCompany);
            _companies.Add(id, dbCompany);
            return company;
        }

        public async Task<IReadOnlyList<Company>> GetAll()
        {
            var dbCompanies = await dbContext.Companies.ToListAsync();
            return dbCompanies.Select(Company.RestoreFrom).ToList();
        }

        public Task ValidateCanSave(Company company)
        {
            if (saveValidated)
            {
                throw new DesignError(Error.SameAggregateValidatedMoreThanOnce);
            }

            if (!_companies.TryGetValue(company.Id, out var dbCompany))
                throw new DesignError(Error.SaveOfUnknownAggregate);

            dbCompany.Version++;

            saveValidated = true;

            return Task.CompletedTask;
        }

        public Task Save() => dbContext.SaveChangesAsync();

        public Task Delete(Company company)
        {
            if (!_companies.TryGetValue(company.Id, out var dbCompany))
                throw new DesignError(Error.DeleteOfUnknownAggregate);
            dbContext.Companies.Remove(dbCompany);
            return dbContext.SaveChangesAsync();
        }
    }
}
