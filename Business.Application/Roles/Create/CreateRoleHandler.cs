using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Roles.Events;

namespace WireOps.Business.Application.Roles.Create;

public class CreateRoleHandler(
    Role.Repository repository,
    Role.Factory factory
) : CommandHandler<CreateRole, RoleModel>
{
    public async Task<RoleModel> Handle(CreateRole command)
    {
        var role = factory.New(command.CompanyId, command.Name, command.IsAdmin, false);

        role.AssignPermissions(command.IsAdmin, command.Permissions?.Select(p => new Role.Permission(role.Id, p.Resource, p.Action)) ?? []);

        await repository.ValidateAndPublish(role);
        await repository.Save();

        var roleModel = RoleModel.MapFromAggregate(role);

        return roleModel;
    }
}