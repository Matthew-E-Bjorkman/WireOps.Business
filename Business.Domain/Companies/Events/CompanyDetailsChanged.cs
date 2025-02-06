using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Business.Domain.Companies.Events;

public class CompanyDetailsChanged(Guid companyId, CompanyDetailsChanged.EventData data) : CompanyEvent
{
    public Guid CompanyId { get; } = companyId;
    public EventData Data { get; } = data;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();

    public record EventData(string Name);
}