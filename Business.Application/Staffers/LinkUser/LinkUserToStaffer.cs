using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.Create;

public readonly struct LinkUserToStaffer (Guid id, string userId) : Command
{
    public Guid Id { get; } = id;
    public string UserId { get; } = userId;
}
