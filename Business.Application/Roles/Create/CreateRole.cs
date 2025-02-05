using WireOps.Business.Application.Common;
using WireOps.Business.Application.Roles.Permissions;

namespace WireOps.Business.Application.Roles.Create;

public readonly struct CreateRole(Guid companyId, string name, bool isAdmin, IEnumerable<PermissionModel>? permissions) : Command
{
    public Guid CompanyId { get; } = companyId;
    public string Name { get; } = name;
    public bool IsAdmin { get; } = isAdmin;
    public IReadOnlyList<PermissionModel>? Permissions { get; } = permissions?.ToList();
}

