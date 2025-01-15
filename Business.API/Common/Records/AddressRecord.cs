using Business.Application.Addresses;

namespace Business.API.Common.Records;

public record AddressRecord(string Address1, string? Address2, string City, string StateProvince, string Country, string PostalCode)
{
    public static AddressRecord? FromModel(AddressModel? model) =>
        model is null ? 
        null :
        new(
            model.Address1,
            model.Address2,
            model.City,
            model.StateProvince,
            model.Country,
            model.PostalCode
        );

    public static AddressModel? ToModel(AddressRecord? record) =>
        record is null ?
        null :
        new(
            record.Address1,
            record.Address2,
            record.City,
            record.StateProvince,
            record.Country,
            record.PostalCode
        );
}
