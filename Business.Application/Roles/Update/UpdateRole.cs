using WireOps.Business.Application.Common;
using WireOps.Business.Application.Roles.Permissions;

namespace WireOps.Business.Application.Roles.Update;

public readonly struct UpdateRole(Guid companyId, Guid id, string name, bool isAdmin, IEnumerable<PermissionModel>? permissions) : Command
{
    public Guid CompanyId { get; } = companyId;
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public bool IsAdmin { get; } = isAdmin;
    public IReadOnlyList<PermissionModel>? Permissions { get; } = permissions?.ToList();
}

