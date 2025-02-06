using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles.Events;

namespace WireOps.Business.Domain.Roles;

public partial class Role
{
    public static class Events
    {
        public static RoleCreated RoleCreated(Role role) 
            => new(role.CompanyId.Value, role.Id.Value, role._data);

        public static RoleDeleted RoleDeleted(Role role) 
            => new(role.CompanyId.Value, role.Id.Value);

        public static RolePermissionsChanged RolePermissionsChanged(Role role) 
            => new(role.CompanyId.Value, role.Id.Value, new(role._data.IsAdmin, role._data.Permissions));
    }
}
