using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;

namespace WireOps.Business.Application.Companies.Get;

public class GetCompanyHandler (
    Company.Repository repository
) : QueryHandler<GetCompany, CompanyModel?>
{
    public async Task<CompanyModel?> Handle(GetCompany query)
    {
        var company = await repository.GetBy(CompanyId.From(query.Id));

        if (company == null)
        {
            return null;
        }

        return CompanyModel.MapFromAggregate(company);
    }
}
