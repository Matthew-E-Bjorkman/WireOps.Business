using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.Delete;

public readonly struct DeleteStaffer (Guid id) : Command
{
    public Guid Id { get; } = id;
}
