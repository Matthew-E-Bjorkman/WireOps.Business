using WireOps.Business.Application.Auth;
using WireOps.Business.Application.Common;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Common.Definitions;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Staffers;

namespace WireOps.Business.Application.Staffers.Create;

public class InviteStafferHandler (
    Staffer.Repository stafferRepository, 
    Company.Repository companyRepository,
    Role.Repository roleRepository,
    Auth0APIClient auth0APIClient
) : CommandHandler<InviteStaffer, StafferModel?>
{
    public async Task<StafferModel?> Handle(InviteStaffer command)
    {
        var company = await companyRepository.GetBy(CompanyId.From(command.CompanyId));

        if (company == null)
        {
            return null;
        }

        var staffer = await stafferRepository.GetBy(CompanyId.From(command.CompanyId), StafferId.From(command.Id));

        if (staffer == null)
        {
            return null;
        }

        if (!staffer._data.RoleId.HasValue)
        {
            throw new DomainError(Error.InvitedStafferMustHaveRole);
        }

        var role = await roleRepository.GetBy(CompanyId.From(command.CompanyId), staffer._data.RoleId.Value);

        if (role == null)
        {
            return null;
        }

        var claims = role._data.IsAdmin ? new List<string> { "admin" } : role._data.Permissions.Select(p => $"{p.Action}:{p.Resource}");

        // If and when Identity becomes an in-house bounded context,
        // this will be handled by publishing a message for consumption
        // by said service to generate and send the invite email.
        // For now, use Auth0 API
        var userId = await auth0APIClient.CreateUser(
            staffer._data.Email.Value, 
            staffer.CompanyId.Value.ToString(), 
            staffer._data.FamilyName,
            staffer._data.GivenName,
            company._data.Name,
            claims
        );
        await auth0APIClient.SendInviteEmail(userId);

        staffer.LinkUser(userId);

        await stafferRepository.ValidateAndPublish(staffer);
        await stafferRepository.Save();

        return StafferModel.MapFromAggregate(staffer);
    }
}
