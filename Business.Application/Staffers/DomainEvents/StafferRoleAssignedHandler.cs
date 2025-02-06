using WireOps.Business.Domain.Common.Definitions;
using WireOps.Business.Domain.Staffers.Events;
using WireOps.Business.Application.Auth;

namespace WireOps.Business.Application.Staffers.DomainEvents;

public class StafferRoleAssignedHandler(
    Auth0APIClient auth0APIClient
) : DomainEventHandler<StafferRoleAssigned>
{
    public async Task Handle(StafferRoleAssigned domainEvent)
    {
        var claims = domainEvent.Data.IsAdmin ? new List<string> { "admin" } : domainEvent.Data.Permissions.Select(p => $"{p.Action}:{p.Resource}");

        if (!string.IsNullOrEmpty(domainEvent.Data.UserId))
        {
            await auth0APIClient.UpdateUser(domainEvent.Data.UserId, claims: claims);
        }
    }
}