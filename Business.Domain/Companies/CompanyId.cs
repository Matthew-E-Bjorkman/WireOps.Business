using WireOps.Business.Domain.Common.ValueObjects;

namespace WireOps.Business.Domain.Companies;

public readonly record struct CompanyId(Guid Value) : ValueObject<Guid>
{
    public static CompanyId New() => new(Guid.NewGuid());

    public static CompanyId From(Guid value) => new(value);
}
