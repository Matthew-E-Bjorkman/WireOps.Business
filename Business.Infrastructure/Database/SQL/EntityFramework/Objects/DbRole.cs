using Business.Infrastructure.Database.SQL.EntityFramework.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;

namespace Business.Infrastructure.Database.SQL.EntityFramework.Objects;

public class DbRole : DbObject, Role.Data
{
    public RoleId Id { get; set; }
    public CompanyId CompanyId { get; set; }
    public string Name { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsOwnerRole { get; set; }
    public int Version { get; set; }

    public List<Role.Permission> Permissions { get; set; } = [];

    IReadOnlyList<Role.Permission> Role.Data.Permissions => Permissions;
    public void Add(Role.Permission permission) => Permissions.Add(permission);
    public void Remove(Role.Permission permission) => Permissions.Remove(permission);

    public void SetName(string name) => Name = name;
    public void SetIsAdmin(bool isAdmin) => IsAdmin = isAdmin;

}