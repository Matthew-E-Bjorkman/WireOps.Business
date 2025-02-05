namespace WireOps.Business.API.Features.Roles;

public record RoleRequest(string name, bool is_admin, List<PermissionRecord>? permissions = null);