using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Roles.Events;

namespace WireOps.Business.Application.Roles.Create;

public class CreateRoleHandler(
    Role.Repository repository,
    Role.Factory factory,
    RoleEventsOutbox eventsOutbox
) : CommandHandler<CreateRole, RoleModel>
{
    public async Task<RoleModel> Handle(CreateRole command)
    {
        var role = factory.New(command.CompanyId, command.Name, command.IsAdmin, false);

        role.AssignPermissions(command.Permissions?.Select(p => new Role.Permission(role.Id, p.Resource, p.Action)) ?? []);

        await repository.ValidateCanSave(role);
        await repository.Save();

        eventsOutbox.Add(CreateEventFrom(role.Id));

        var roleModel = RoleModel.MapFromAggregate(role);

        return roleModel;
    }

    private static RoleCreated CreateEventFrom(RoleId roleId) =>
        new(roleId.Value);
}