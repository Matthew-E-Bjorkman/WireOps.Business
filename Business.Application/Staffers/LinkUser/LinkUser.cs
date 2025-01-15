using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Staffers;

namespace WireOps.Business.Application.Staffers.Create;

public readonly struct LinkUser (Guid id, string userId) : Command
{
    public Guid Id { get; } = id;
    public string UserId { get; } = userId;
}
