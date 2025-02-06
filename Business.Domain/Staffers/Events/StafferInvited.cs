using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Business.Domain.Staffers.Events;

public class StafferInvited(Guid companyId, Guid stafferId) : StafferEvent
{
    public Guid CompanyId { get; } = companyId;
    public Guid StafferId { get; } = stafferId;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();
}