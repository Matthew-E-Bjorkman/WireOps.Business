using NodaTime;
using NodaTime.Extensions;
using WireOps.Business.Domain.Common.ValueObjects.Types;

namespace WireOps.Business.Domain.Companies.Events;

public class CompanyAddressChanged(Guid companyId, CompanyAddressChanged.EventData data) : CompanyEvent
{
    public Guid CompanyId { get; } = companyId;
    public EventData Data { get; } = data;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();

    public record EventData(Address Address);
}
