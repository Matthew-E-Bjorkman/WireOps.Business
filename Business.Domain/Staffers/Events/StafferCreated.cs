using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Business.Domain.Staffers.Events;

public class StafferCreated(Guid companyId, Guid stafferId, Staffer.Data data) : StafferEvent
{
    public Guid CompanyId { get; } = companyId;
    public Guid StafferId { get; } = stafferId;
    public Staffer.Data data { get; } = data;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();
}