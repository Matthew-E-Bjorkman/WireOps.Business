namespace Business.API.Features.Companies.Requests;

public record CompanyCreateRequest(string Name, string UserId, string OwnerEmail, string OwnerGivenName, string OwnerFamilyName);
