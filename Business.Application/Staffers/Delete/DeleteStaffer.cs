using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.Delete;

public readonly struct DeleteStaffer (Guid companyId, Guid id) : Command
{
    public Guid CompanyId { get; } = companyId;
    public Guid Id { get; } = id;
}
