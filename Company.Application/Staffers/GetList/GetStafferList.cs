using WireOps.Company.Application.Common;

namespace WireOps.Company.Application.Staffers.GetList;

public readonly struct GetStafferList (Guid id) : Query
{
    public Guid Id { get; } = id;
}
