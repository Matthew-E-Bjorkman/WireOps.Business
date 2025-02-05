using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Roles.Delete;

public readonly struct DeleteRole(Guid companyId, Guid id) : Command
{
    public Guid CompanyId { get; } = companyId;
    public Guid Id { get; } = id;
}
