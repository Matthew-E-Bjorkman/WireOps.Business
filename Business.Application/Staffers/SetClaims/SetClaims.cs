using WireOps.Business.Application.Common;
using WireOps.Business.Application.Roles.Permissions;

namespace WireOps.Business.Application.Staffers.SetClaims;

public readonly struct SetClaims(Guid companyId, Guid id, string userId, bool isAdmin, IEnumerable<PermissionModel> permissions) : Command
{
    public Guid CompanyId { get; } = companyId;
    public Guid Id { get; } = id;
    public string UserId { get; } = userId;
    public bool IsAdmin { get; } = isAdmin;
    public IEnumerable<PermissionModel> Permissions { get; } = permissions;
}

