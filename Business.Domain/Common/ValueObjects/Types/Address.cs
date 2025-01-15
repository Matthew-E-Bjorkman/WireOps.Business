namespace WireOps.Business.Domain.Common.ValueObjects.Types;

public class Address : ValueObject
{
    public string Address1 { get; private set; }
    public string? Address2 { get; private set; }
    public string City { get; private set; }
    public string StateProvince { get; private set; }
    public string Country { get; private set; }
    public string PostalCode { get; private set; }

    // EF Core Constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Address() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public Address(string address1, string? address2, string city, string stateProvince, string country, string postalCode)
    {
        Address1 = address1;
        Address2 = address2;
        City = city;
        StateProvince = stateProvince;
        Country = country;
        PostalCode = postalCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address1;
        yield return Address2;
        yield return City;
        yield return StateProvince;
        yield return Country;
        yield return PostalCode;
    }
}