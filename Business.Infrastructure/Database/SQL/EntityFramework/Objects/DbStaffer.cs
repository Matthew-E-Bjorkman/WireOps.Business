using Business.Infrastructure.Database.SQL.EntityFramework.Common;
using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Staffers;

namespace Business.Infrastructure.Database.SQL.EntityFramework.Objects;

public class DbStaffer : DbObject, Staffer.Data
{
    public StafferId Id { get; set; }
    public CompanyId CompanyId { get; set; }
    public RoleId? RoleId { get; set; }
    public Email Email { get; set; }
    public string? UserId { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public bool IsOwner { get; set; }
    public int Version { get; set; }

    public void SetUserId(string userId) => UserId = userId;
    public void SetEmail(Email email) => Email = email;
    public void SetFamilyName(string familyName) => FamilyName = familyName;
    public void SetGivenName(string givenName) => GivenName = givenName;
    public void SetRoleId(RoleId roleId) => RoleId = roleId;
}