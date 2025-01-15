using WireOps.Business.Domain.Common.Definitions;

namespace WireOps.Business.Domain.Companies.Events;

public interface CompanyEvent : DomainEvent
{
    public Guid CompanyId { get; }
}