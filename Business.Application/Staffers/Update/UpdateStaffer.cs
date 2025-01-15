using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Staffers.Update;

public readonly struct UpdateStaffer (Guid id, string email, string givenName, string familyName) : Command
{
    public Guid Id { get; } = id;
    public string Email { get; } = email;
    public string GivenName { get; } = givenName;
    public string FamilyName { get; } = familyName;
}
