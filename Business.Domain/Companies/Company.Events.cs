using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies.Events;

namespace WireOps.Business.Domain.Companies;

public partial class Company
{
    public static class Events
    {
        public static CompanyCreated CompanyCreated(Company company) 
            => new(company.Id.Value, company._data);

        public static CompanyDeleted CompanyDeleted(Company company) 
            => new(company.Id.Value);

        public static CompanyAddressChanged CompanyAddressChanged(Company company) 
            => new(company.Id.Value, new(company._data.Address!));

        public static CompanyDetailsChanged CompanyDetailsChanged(Company company) 
            => new(company.Id.Value, new(company._data.Name));
    }
}
