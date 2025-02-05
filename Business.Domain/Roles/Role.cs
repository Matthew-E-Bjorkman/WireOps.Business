using System;
using System.Data;
using WireOps.Business.Common.Errors;

namespace WireOps.Business.Domain.Roles;

public partial class Role
{
    public RoleId Id => _data.Id;

    public void ChangeName(string name)
    {
        if (!string.Equals(name, _data.Name))
        {
            _data.SetName(name);
        }
    }

    public void SetIsAdmin(bool isAdmin)
    {
        if (isAdmin != _data.IsAdmin)
        {
            _data.SetIsAdmin(isAdmin);
        }
    }

    public void AssignPermissions(IEnumerable<Permission> permissions)
    {
        var newPermissions = permissions.Where(permissions => !_data.Permissions.Contains(permissions));
        var removedPermissions = _data.Permissions.Where(permission => !permissions.Contains(permission));

        if (newPermissions.Any())
        {
            foreach (var permission in newPermissions)
            {
                _data.Add(permission);
            }
        }
        
        if (removedPermissions.Any())
        {
            foreach (var permission in removedPermissions)
            {
                _data.Remove(permission);
            }
        }
    }
}
