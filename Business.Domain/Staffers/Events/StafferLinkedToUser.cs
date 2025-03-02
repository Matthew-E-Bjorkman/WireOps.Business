using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Business.Domain.Staffers.Events;

public class StafferLinkedToUser(Guid companyId, Guid stafferId, StafferLinkedToUser.EventData data) : StafferEvent
{
    public Guid CompanyId { get; } = companyId;
    public Guid StafferId { get; } = stafferId;
    public EventData Data { get; } = data;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();

    public record EventData(string UserId);
}