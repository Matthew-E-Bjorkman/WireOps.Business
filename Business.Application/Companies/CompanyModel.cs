using Business.Application.Addresses;
using WireOps.Business.Domain.Companies;

namespace WireOps.Business.Application.Companies;

public class CompanyModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public AddressModel? Address { get; set; }


    public static CompanyModel MapFromAggregate(Company aggregate)
    {
        return new CompanyModel
        {
            Id = aggregate._data.Id.Value,
            Name = aggregate._data.Name,
            Address = AddressModel.MapFromAggregate(aggregate._data.Address)
        };
    }
}
    