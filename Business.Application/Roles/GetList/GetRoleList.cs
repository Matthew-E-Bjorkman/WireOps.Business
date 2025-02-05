using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Roles.GetList;

public readonly struct GetRoleList(Guid companyId) : Query
{
    public Guid CompanyId { get; } = companyId;
}

