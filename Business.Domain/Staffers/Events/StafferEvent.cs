using WireOps.Business.Domain.Common.Definitions;

namespace WireOps.Business.Domain.Staffers.Events;

public interface StafferEvent : DomainEvent
{
    public Guid StafferId { get; }
}