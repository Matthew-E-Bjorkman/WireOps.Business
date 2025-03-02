﻿using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies.Events;

namespace WireOps.Business.Domain.Companies;

public partial class Company
{
    public CompanyId Id => _data.Id;
    public List<CompanyEvent> DomainEvents { get; } = [];

    public void ChangeName(string name)
    {
        if (!string.Equals(name, _data.Name))
        {
            _data.SetName(name);
            DomainEvents.Add(Events.CompanyDetailsChanged(this));
        }
    }

    public void ChangeAddress(string address1, string? address2, string city, string stateProvince, string country, string postalCode)
    {
        var address = new Address(address1, address2, city, stateProvince, country, postalCode);
        if (address != _data.Address)
        {
            _data.SetAddress(address);
            DomainEvents.Add(Events.CompanyAddressChanged(this));
        }
    }
}
