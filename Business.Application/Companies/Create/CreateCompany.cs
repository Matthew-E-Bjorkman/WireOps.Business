using WireOps.Business.Application.Common;

namespace WireOps.Business.Application.Companies.Create;

public readonly struct CreateCompany(string name, string userId, string ownerEmail, string ownerGivenName, string ownerFamilyName) : Command
{
    public string Name { get; } = name;
    public string UserId { get; } = userId;
    public string OwnerEmail { get; } = ownerEmail;
    public string OwnerGivenName { get; } = ownerGivenName;
    public string OwnerFamilyName { get; } = ownerFamilyName;
}
