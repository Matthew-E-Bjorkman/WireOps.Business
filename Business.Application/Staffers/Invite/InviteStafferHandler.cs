using Auth0.ManagementApi;
using Business.Application.Auth;
using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Application.Staffers.Create;

public class InviteStafferHandler (
    Staffer.Repository repository, 
    Company.Repository companyRepository,
    StafferEventsOutbox eventsOutbox,
    Auth0APIClient auth0APIClient
) : CommandHandler<InviteStaffer, StafferModel?>
{
    public async Task<StafferModel?> Handle(InviteStaffer command)
    {
        var company = await companyRepository.GetBy(CompanyId.From(command.Id));

        if (company == null)
        {
            return null;
        }

        var staffer = await repository.GetBy(CompanyId.From(command.CompanyId), StafferId.From(command.Id));

        if (staffer == null)
        {
            return null;
        }

        // If and when Identity becomes an in-house bounded context,
        // this will be handled by publishing a message for consumption
        // by said service to generate and send the invite email.
        // For now, use Auth0 API
        var userId = await auth0APIClient.CreateUser(
            staffer._data.Email.Value, 
            staffer._data.CompanyId.Value.ToString(), 
            staffer._data.FamilyName,
            staffer._data.GivenName,
            company._data.Name
        );
        await auth0APIClient.SendInviteEmail(userId);

        staffer.LinkUser(userId);

        await repository.ValidateCanSave(staffer);
        await repository.Save();

        eventsOutbox.Add(StafferLinkedToUserEventFrom(staffer.Id, userId));

        return StafferModel.MapFromAggregate(staffer);
    }

    private static StafferLinkedToUser StafferLinkedToUserEventFrom(StafferId stafferId, string userId) =>
        new(stafferId.Value, userId);
}
