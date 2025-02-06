

using WireOps.Business.Application.Auth;
using WireOps.Business.Domain.Common.Definitions;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Roles.Events;
using WireOps.Business.Domain.Staffers;

namespace WireOps.Business.Application.Staffers.DomainEvents;

public class RolePermissionsChangedHandler(
    Staffer.Repository stafferRepository,
    Auth0APIClient auth0APIClient
) : DomainEventHandler<RolePermissionsChanged>
{
    public async Task Handle(RolePermissionsChanged domainEvent)
    {
        var allStaffersWithChangedRole = await stafferRepository.GetAllByRole(CompanyId.From(domainEvent.CompanyId), RoleId.From(domainEvent.RoleId));

        if (allStaffersWithChangedRole.Any())
        {
            var claims = domainEvent.Data.IsAdmin ? new List<string> { "admin" }: domainEvent.Data.Permissions.Select(p => $"{p.Action}:{p.Resource}");

            foreach (var staffer in allStaffersWithChangedRole)
            {
                if (!string.IsNullOrEmpty(staffer._data.UserId))
                {
                    await auth0APIClient.UpdateUser(staffer._data.UserId, claims: claims);
                }
            }
        }
    }
}
