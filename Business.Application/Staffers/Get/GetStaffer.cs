using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.Get;

public readonly struct GetStaffer (Guid companyId, Guid id) : Query
{
    public Guid CompanyId { get; } = companyId;
    public Guid Id { get; } = id;
}
