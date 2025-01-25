using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.GetList;

public readonly struct GetStafferList (Guid companyId) : Query
{
    public Guid CompanyId { get; } = companyId;
}
