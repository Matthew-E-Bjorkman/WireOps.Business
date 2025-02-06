using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Business.Domain.Roles.Events;

public class RolePermissionsChanged(Guid companyId, Guid roleId, RolePermissionsChanged.EventData data) : RoleEvent
{
    public Guid CompanyId { get; } = companyId;
    public Guid RoleId { get; } = roleId;
    public EventData Data { get; } = data;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();

    public record EventData(bool IsAdmin, IEnumerable<Role.Permission> Permissions);
}