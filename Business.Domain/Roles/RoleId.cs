using WireOps.Business.Domain.Common.ValueObjects;

namespace WireOps.Business.Domain.Roles;

public readonly record struct RoleId(Guid Value) : ValueObject<Guid>
{
    public static RoleId New() => new(Guid.NewGuid());

    public static RoleId From(Guid value) => new(value);
}
