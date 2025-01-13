using WireOps.Company.Application.Common;

namespace WireOps.Company.Application.Staffers.Get;

public readonly struct GetStaffer (Guid id) : Query
{
    public Guid Id { get; } = id;
}
