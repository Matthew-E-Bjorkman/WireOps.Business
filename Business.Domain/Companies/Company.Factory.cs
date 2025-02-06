using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies.Events;

namespace WireOps.Business.Domain.Companies;

public partial class Company
{
    public abstract class Factory
    {
        public Company New(string name)
        {
            var data = CreateData(CompanyId.New(), name);
            var company = new Company(data);
            company.DomainEvents.Add(Events.CompanyCreated(company));
            return company;
        }

        protected abstract Data CreateData(CompanyId id, string name);
    }
}
