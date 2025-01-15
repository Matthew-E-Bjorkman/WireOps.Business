using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.GetList;

public readonly struct GetStafferList (Guid id) : Query
{
    public Guid Id { get; } = id;
}
