using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Companies.Get;

public readonly struct GetCompany (Guid id) : Query
{
    public Guid Id { get; } = id;
}
