namespace WireOps.Company.API.Features.Staffers;

public record StafferRequest(string Email, string GivenName, string FamilyName, string? UserId);
