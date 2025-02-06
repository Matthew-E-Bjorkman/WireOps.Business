using NodaTime.Extensions;
using NodaTime;

namespace WireOps.Business.Domain.Roles.Events;

public class RoleCreated(Guid companyId, Guid roleId, Role.Data data) : RoleEvent
{
    public Guid CompanyId { get; } = companyId;
    public Guid RoleId { get; } = roleId;
    public Role.Data Data { get; } = data;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();
}
