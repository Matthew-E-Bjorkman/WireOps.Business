using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Application.Staffers.Create;

public class LinkUserToStafferHandler (
    Staffer.Repository repository
) : CommandHandler<LinkUserToStaffer, StafferModel?>
{
    public async Task<StafferModel?> Handle(LinkUserToStaffer command)
    {
        var staffer = await repository.GetBy(CompanyId.From(command.CompanyId), StafferId.From(command.Id));

        if (staffer == null)
        {
            return null;
        }

        staffer.LinkUser(command.UserId);

        await repository.ValidateAndPublish(staffer);
        await repository.Save();

        return StafferModel.MapFromAggregate(staffer);
    }
}
