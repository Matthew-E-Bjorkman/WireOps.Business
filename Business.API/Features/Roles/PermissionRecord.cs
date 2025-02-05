using WireOps.Business.Application.Roles.Permissions;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Roles;

namespace WireOps.Business.API.Features.Roles;

public class PermissionRecord
{
    public required string resource { get; set; }
    public required string action { get; set; }

    public static PermissionRecord FromModel(PermissionModel model)
    {
        return new PermissionRecord
        {
            resource = model.Resource,
            action = model.Action.ToString()
        };
    }

    public static PermissionModel ToModel(PermissionRecord record)
    {
        if (!Enum.TryParse<ResourceAction>(record.action, true, out var resourceAction))
        {
            throw new DomainError(Error.InvalidResourceAction);
        }

        return new PermissionModel
        {
            Resource = record.resource,
            Action = resourceAction
        };
    }
}
