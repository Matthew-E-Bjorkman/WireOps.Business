using NodaTime.Extensions;
using NodaTime;

namespace WireOps.Business.Domain.Roles.Events;

public class RoleCreated(Guid roleId) : RoleEvent
{
    public Guid RoleId { get; } = roleId;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();
}
