using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.Create;

public readonly struct CreateStaffer(Guid companyId, string email, string givenName, string familyName, string? userId) : Command
{
    public Guid CompanyId { get; } = companyId;
    public string Email { get; } = email;
    public string GivenName { get; } = givenName;
    public string FamilyName { get; } = familyName;
    public string? UserId { get; } = userId;
}
