using NodaTime;
using NodaTime.Extensions;

namespace WireOps.Business.Domain.Companies.Events;

public class CompanyCreated(Guid companyId, Company.Data data) : CompanyEvent
{
    public Guid CompanyId { get; } = companyId;
    public Company.Data Data { get; } = data;
    public Instant EventCreatedAt { get; } = DateTime.UtcNow.ToInstant();
}