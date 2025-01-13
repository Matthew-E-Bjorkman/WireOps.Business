using WireOps.Company.Domain.Common.ValueObjects;

namespace WireOps.Company.Domain.Staffers;

public readonly record struct StafferId(Guid Value) : ValueObject<Guid>
{
    public static StafferId New() => new(Guid.NewGuid());

    public static StafferId From(Guid value) => new(value);
}
