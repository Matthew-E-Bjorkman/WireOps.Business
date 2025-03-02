using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Business.Domain.Companies.Events;

public class CompanyDeleted(Guid companyId) : CompanyEvent
{
    public Guid CompanyId { get; } = companyId;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();
}