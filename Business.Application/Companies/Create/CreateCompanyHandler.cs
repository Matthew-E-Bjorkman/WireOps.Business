using WireOps.Business.Application.Common;
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
    StafferEventsOutbox stafferEventsOutbox
) : CommandHandler<CreateCompany, CompanyModel>
{
    public async Task<CompanyModel> Handle(CreateCompany command)
    {
        var company = companyFactory.New(command.Name);

        var staffer = stafferFactory.New(company.Id.Value, command.OwnerEmail, command.OwnerGivenName, command.OwnerFamilyName, true);

        staffer.LinkUser(command.UserId);

        await companyRepository.ValidateCanSave(company);
        await stafferRepository.ValidateCanSave(staffer);

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
