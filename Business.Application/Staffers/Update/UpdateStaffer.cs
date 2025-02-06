using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.Update;

public readonly struct UpdateStaffer (Guid companyId, Guid id, string email, string givenName, string familyName, Guid? roleId) : Command
{
    public Guid CompanyId { get; } = companyId;
    public Guid Id { get; } = id;
    public string Email { get; } = email;
    public string GivenName { get; } = givenName;
    public string FamilyName { get; } = familyName;
    public Guid? RoleId { get; } = roleId;
}
