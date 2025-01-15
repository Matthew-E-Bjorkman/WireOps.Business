using Business.Infrastructure.Database.SQL.EntityFramework.Common;
using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies;

namespace Business.Infrastructure.Database.SQL.EntityFramework.Objects;

public class DbCompany : DbObject, Company.Data
{
    public CompanyId Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public int Version { get; set; }

    public void SetName(string name) => Name = name;
    public void SetAddress(Address address) => Address = address;
}