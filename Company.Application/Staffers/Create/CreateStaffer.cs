using WireOps.Company.Application.Common;

namespace WireOps.Company.Application.Staffers.Create;

public readonly struct CreateStaffer(string email, string givenName, string familyName, string? userId) : Command
{
    public string Email { get; } = email;
    public string GivenName { get; } = givenName;
    public string FamilyName { get; } = familyName;
    public string? UserId { get; } = userId;
}
