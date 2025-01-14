using Company.Shared.Types;
using WireOps.Company.Domain.Staffers;

namespace WireOps.Company.Infrastructure.Database.SQL.EntityFramework;

public class DbStaffer : Staffer.Data
{
    public StafferId Id { get; set; }
    public Email Email { get; set; }
    public string? UserId { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public int Version { get; set; }

    public void SetUserId(string userId) => UserId = userId;
    public void SetEmail(Email email) => Email = email;
    public void SetFamilyName(string familyName) => FamilyName = familyName;
    public void SetGivenName(string givenName) => GivenName = givenName;
}