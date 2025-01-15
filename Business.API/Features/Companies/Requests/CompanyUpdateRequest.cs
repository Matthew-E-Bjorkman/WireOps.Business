using Business.API.Common.Records;

namespace WireOps.Business.API.Features.Companies;

public record CompanyUpdateRequest(string Name, AddressRecord? Address);
