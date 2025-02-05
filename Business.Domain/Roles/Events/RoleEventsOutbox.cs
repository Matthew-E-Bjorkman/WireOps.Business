namespace WireOps.Business.Domain.Roles.Events;

public interface RoleEventsOutbox
{
    public void Add(RoleEvent orderEvent);
}