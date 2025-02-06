using WireOps.Business.Application.Auth;
using WireOps.Business.Application.Common;
using WireOps.Business.Application.Staffers.SetClaims;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Companies.Events;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Application.Companies.Create;

public class CreateCompanyHandler (
    Company.Repository companyRepository,
    Company.Factory companyFactory,
    Role.Repository roleRepository,
    Role.Factory roleFactory,
    Staffer.Repository stafferRepository,
    Staffer.Factory stafferFactory,
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

        var ownerRole = roleFactory.New(company.Id.Value, "System Admin", true, true);

        var staffer = stafferFactory.New(company.Id.Value, command.OwnerEmail, command.OwnerGivenName, command.OwnerFamilyName, true, ownerRole.Id.Value);

        staffer.LinkUser(command.UserId);

        await companyRepository.ValidateAndPublish(company);
        await roleRepository.ValidateAndPublish(ownerRole);
        await stafferRepository.ValidateAndPublish(staffer);

        var user = await auth0APIClient.UpdateUser(command.UserId, company._data.Name, company.Id.Value, ["admin"]);

        if (user == null)
        {
            throw new DesignError("Unable to link new company to Identity user.");
        }

        await companyRepository.Save();

        var companyModel = CompanyModel.MapFromAggregate(company);

        return companyModel;
    }
}
