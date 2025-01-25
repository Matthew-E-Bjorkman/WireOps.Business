using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.Create;

public readonly struct InviteStaffer (Guid companyId, Guid id) : Command
{
    public Guid CompanyId { get; } = companyId;
    public Guid Id { get; } = id;
}
