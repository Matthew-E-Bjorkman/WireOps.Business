using WireOps.Company.Application.Common;

namespace WireOps.Company.Application.Staffers.Delete;

public readonly struct DeleteStaffer (Guid id) : Command
{
    public Guid Id { get; } = id;
}
