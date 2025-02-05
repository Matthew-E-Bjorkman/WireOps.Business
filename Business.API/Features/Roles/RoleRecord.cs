using WireOps.Business.Application.Roles;

namespace WireOps.Business.API.Features.Roles;

public class RoleRecord
{
    public required Guid id { get; set; }
    public required Guid company_id { get; set; }
    public required string name { get; set; }
    public required bool is_admin { get; set; }
    public required bool is_owner_role { get; set; }
    public required IReadOnlyList<PermissionRecord> permissions { get; set; }

    public static RoleRecord FromModel(RoleModel model)
    {
        return new RoleRecord
        {
            id = model.Id,
            company_id = model.CompanyId,
            name = model.Name,
            is_admin = model.IsAdmin,
            is_owner_role = model.IsOwnerRole,
            permissions = model.Permissions.Select(PermissionRecord.FromModel).ToList()
        };
    }
}
