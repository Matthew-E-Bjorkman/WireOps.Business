using WireOps.Business.Domain.Common.Definitions;

namespace WireOps.Business.Domain.Roles.Events;

public interface RoleEvent : DomainEvent
{
    public Guid RoleId { get; }
}