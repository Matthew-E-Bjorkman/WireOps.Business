using WireOps.Business.Domain.Roles;

namespace WireOps.Business.Application.Roles.Permissions;

public class PermissionModel
{    public required string Resource { get; set; }
    public required ResourceAction Action { get; set; }

    public static PermissionModel MapFromAggregate(Role.Permission aggregate)
    {
        return new PermissionModel
        {
            Resource = aggregate.Resource,
            Action = aggregate.Action
        };
    }
}
