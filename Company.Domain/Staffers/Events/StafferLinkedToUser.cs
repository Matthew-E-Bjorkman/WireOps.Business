using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Company.Domain.Staffers.Events;

public class StafferLinkedToUser(Guid stafferId, string userId) : StafferEvent
{
    public Guid StafferId { get; } = stafferId;
    public string UserId { get; } = userId;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();
}