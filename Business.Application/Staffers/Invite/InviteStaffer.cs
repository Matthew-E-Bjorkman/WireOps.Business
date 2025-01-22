using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.Create;

public readonly struct InviteStaffer (Guid id) : Command
{
    public Guid Id { get; } = id;
}
