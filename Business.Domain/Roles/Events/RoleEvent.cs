using WireOps.Business.Domain.Common.Definitions;

namespace WireOps.Business.Domain.Roles.Events;

public interface RoleEvent : DomainEvent
{
    public Guid CompanyId { get; }
    public Guid RoleId { get; }
}