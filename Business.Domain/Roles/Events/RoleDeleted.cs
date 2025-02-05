using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Business.Domain.Roles.Events;

public class RoleDeleted(Guid roleId) : RoleEvent
{
    public Guid RoleId { get; } = roleId;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();
}