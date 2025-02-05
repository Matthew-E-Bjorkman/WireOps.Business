using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Roles.Get;

public readonly struct GetRole(Guid companyId, Guid id) : Query
{
    public Guid CompanyId { get; } = companyId;
    public Guid Id { get; } = id;
}

