using Business.Application.Addresses;
using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Companies.Update;

public readonly struct UpdateCompany (Guid id, string name, AddressModel? address) : Command
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public AddressModel? Address { get; } = address;
}
