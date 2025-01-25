using Business.Application.Auth;
using WireOps.Business.Application.Common;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Companies.Events;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Application.Companies.Create;

public class CreateCompanyHandler (
    Company.Repository companyRepository,
    Company.Factory companyFactory,
    Staffer.Repository stafferRepository,
    Staffer.Factory stafferFactory,
    CompanyEventsOutbox companyEventsOutbox,
    StafferEventsOutbox stafferEventsOutbox,
    Auth0APIClient auth0APIClient
) : CommandHandler<CreateCompany, CompanyModel>
{
    public async Task<CompanyModel> Handle(CreateCompany command)
    {
        try
        {
            var existingStaffer = await stafferRepository.GetByUserId(command.UserId);
            throw new DomainError("This user already belongs to a company. Can only create a new company for a fresh registration.");

        }
        catch (DomainError) { } // No staffer was found

        var company = companyFactory.New(command.Name);

        var staffer = stafferFactory.New(company.Id.Value, command.OwnerEmail, command.OwnerGivenName, command.OwnerFamilyName, true);

        staffer.LinkUser(command.UserId);

        await companyRepository.ValidateCanSave(company);
        await stafferRepository.ValidateCanSave(staffer);

        var user = await auth0APIClient.UpdateUser(command.UserId, company._data.Name, company.Id.Value);

        if (user == null)
        {
            throw new DesignError("Unable to link new company to Identity user.");
        }

        await companyRepository.Save();

        companyEventsOutbox.Add(CreateEventFrom(company.Id));
        stafferEventsOutbox.Add(CreateEventFrom(staffer.Id));

        var companyModel = CompanyModel.MapFromAggregate(company);

        return companyModel;
    }

    private static CompanyCreated CreateEventFrom(CompanyId companyId) =>
        new(companyId.Value); 

    private static StafferCreated CreateEventFrom(StafferId stafferId) =>
        new(stafferId.Value);
}
