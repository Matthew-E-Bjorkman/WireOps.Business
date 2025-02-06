using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles.Events;
using WireOps.Business.Domain.Roles;

namespace WireOps.Business.Application.Roles.Update;

public class UpdateRoleHandler(
    Role.Repository repository
) : CommandHandler<UpdateRole, RoleModel?>
{
    public async Task<RoleModel?> Handle(UpdateRole command)
    {
        var role = await repository.GetBy(CompanyId.From(command.CompanyId), RoleId.From(command.Id));

        if (role == null)
        {
            return null;
        }

        role.AssignPermissions(command.IsAdmin, command.Permissions?.Select(p => new Role.Permission(role.Id, p.Resource, p.Action)) ?? []);
        role.ChangeName(command.Name);
        await repository.ValidateAndPublish(role);
        await repository.Save();

        return RoleModel.MapFromAggregate(role);
    }
}