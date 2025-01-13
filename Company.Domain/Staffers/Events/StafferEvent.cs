using WireOps.Company.Domain.Common.Definitions;

namespace WireOps.Company.Domain.Staffers.Events;

public interface StafferEvent : DomainEvent
{
    public Guid StafferId { get; }
}