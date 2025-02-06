using NodaTime;
using NodaTime.Extensions;
using WireOps.Business.Domain.Roles;

namespace WireOps.Business.Domain.Staffers.Events;

public class StafferRoleAssigned(Guid companyId, Guid stafferId, StafferRoleAssigned.EventData data) : StafferEvent
{
    public Guid CompanyId { get; } = companyId;
    public Guid StafferId { get; } = stafferId;
    public EventData Data { get; } = data;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();

    public record EventData(string UserId, bool IsAdmin, IEnumerable<Role.Permission> Permissions);
}