using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Companies.Events;
using WireOps.Business.Domain.Staffers;

namespace WireOps.Business.Application.Companies.Update;

public class UpdateCompanyHandler (
    Company.Repository repository, 
    CompanyEventsOutbox eventsOutbox
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

        await repository.ValidateCanSave(company);
        await repository.Save();

        eventsOutbox.Add(UpdateEventFrom(company.Id));

        return CompanyModel.MapFromAggregate(company);
    }

    private static CompanyUpdated UpdateEventFrom(CompanyId companyId) =>
        new(companyId.Value); 
}
