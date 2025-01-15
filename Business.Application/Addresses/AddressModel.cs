using WireOps.Business.Domain.Common.ValueObjects.Types;

namespace Business.Application.Addresses;

public record AddressModel(string Address1, string? Address2, string City, string StateProvince, string Country, string PostalCode)
{
    public static AddressModel? MapFromAggregate(Address? aggregate) =>
        aggregate is null ?
        null :
        new(
            aggregate.Address1,
            aggregate.Address2,
            aggregate.City,
            aggregate.StateProvince,
            aggregate.Country,
            aggregate.PostalCode
        );
}

