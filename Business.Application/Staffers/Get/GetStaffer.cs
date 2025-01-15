using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.Get;

public readonly struct GetStaffer (Guid id) : Query
{
    public Guid Id { get; } = id;
}
