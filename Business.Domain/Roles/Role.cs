using System;
using System.Data;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Companies.Events;
using WireOps.Business.Domain.Roles.Events;

namespace WireOps.Business.Domain.Roles;

public partial class Role
{
    public RoleId Id => _data.Id;
    public CompanyId CompanyId => _data.CompanyId;
    public List<RoleEvent> DomainEvents { get; } = [];

    public void ChangeName(string name)
    {
        if (!string.Equals(name, _data.Name))
        {
            _data.SetName(name);
        }
    }

    public void AssignPermissions(bool isAdmin, IEnumerable<Permission>? permissions)
    {
        bool isDirty = false;

        if (isAdmin != _data.IsAdmin)
        {
            _data.SetIsAdmin(isAdmin);
            isDirty = true;
        }

        if (permissions is not null)
        {
            var newPermissions = permissions.Where(permissions => !_data.Permissions.Contains(permissions));
            var removedPermissions = _data.Permissions.Where(permission => !permissions.Contains(permission));

            if (newPermissions.Any())
            {
                foreach (var permission in newPermissions)
                {
                    _data.Add(permission);
                }
                isDirty = true;
            }

            if (removedPermissions.Any())
            {
                foreach (var permission in removedPermissions)
                {
                    _data.Remove(permission);
                }
                isDirty = true;
            }
        }
        
        if (isDirty)
        {
            DomainEvents.Add(Events.RolePermissionsChanged(this));
        }
    }       
}
