using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;

namespace WireOps.Business.Application.Companies.Update;

public class UpdateCompanyHandler (
    Company.Repository repository
) : CommandHandler<UpdateCompany, CompanyModel?>
{
    public async Task<CompanyModel?> Handle(UpdateCompany command)
    {
        var company = await repository.GetBy(CompanyId.From(command.Id));

        if (company == null)
        {
            return null;
        }

        company.ChangeName(command.Name);
        if (command.Address != null)
        {
            company.ChangeAddress(
                command.Address.Address1, 
                command.Address.Address2, 
                command.Address.City, 
                command.Address.StateProvince, 
                command.Address.Country, 
                command.Address.PostalCode
            );
        }

        await repository.ValidateAndPublish(company);
        await repository.Save();

        return CompanyModel.MapFromAggregate(company);
    }
}
