using WireOps.Company.Application.Common;
using WireOps.Company.Domain.Staffers;

namespace WireOps.Company.Application.Staffers.Create;

public readonly struct LinkUser (Guid id, string userId) : Command
{
    public Guid Id { get; } = id;
    public string UserId { get; } = userId;
}
