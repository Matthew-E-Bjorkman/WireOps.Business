using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Company.Domain.Staffers.Events;

public class StafferUpdated(Guid stafferId) : StafferEvent
{
    public Guid StafferId { get; } = stafferId;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();
}