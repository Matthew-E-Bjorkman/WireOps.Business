using WireOps.Business.Application.Auth;
using WireOps.Business.Application.Common;
using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Application.Staffers.Delete;

public class DeleteStafferHandler (
    Staffer.Repository repository,
    Auth0APIClient auth0APIClient
) : CommandHandler<DeleteStaffer, bool>
{
    public async Task<bool> Handle(DeleteStaffer command)
    {
        var staffer = await repository.GetBy(CompanyId.From(command.CompanyId), StafferId.From(command.Id));

        if (staffer == null)
        {
            return false;
        }

        if (staffer._data.IsOwner)
        {
            throw new DomainError("Cannot delete the owner's staffer record.");
        }

        //If the user exists in Auth0, delete it
        if (staffer._data.UserId != null && !await auth0APIClient.DeleteUser(staffer._data.UserId))
        {
            // Delete failed, abort
            throw new DesignError("Unable to delete Auth0 user");
        }

        await repository.Delete(staffer);

        return true;
    }
}
