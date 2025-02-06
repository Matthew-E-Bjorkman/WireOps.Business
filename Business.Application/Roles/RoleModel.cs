using WireOps.Business.Application.Roles.Permissions;
using WireOps.Business.Domain.Roles;

namespace WireOps.Business.Application.Roles;

public class RoleModel
{
    public required Guid Id { get; set; }
    public required Guid CompanyId { get; set; }
    public required string Name { get; set; }
    public required bool IsAdmin { get; set; }
    public required bool IsOwnerRole { get; set; }
    public required IReadOnlyList<PermissionModel> Permissions { get; set; }

    public static RoleModel MapFromAggregate(Role aggregate)
    {
        return new RoleModel
        {
            Id = aggregate.Id.Value,
            CompanyId = aggregate.CompanyId.Value,
            Name = aggregate._data.Name,
            IsAdmin = aggregate._data.IsAdmin,
            IsOwnerRole = aggregate._data.IsOwnerRole,
            Permissions = aggregate._data.Permissions.Select(PermissionModel.MapFromAggregate).ToList().AsReadOnly(),
        };
    }
}
