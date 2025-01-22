using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Application.Staffers.Create;

public class LinkUserToStafferHandler (
    Staffer.Repository repository, 
    StafferEventsOutbox eventsOutbox
) : CommandHandler<LinkUserToStaffer, StafferModel?>
{
    public async Task<StafferModel?> Handle(LinkUserToStaffer command)
    {
        var staffer = await repository.GetBy(StafferId.From(command.Id));

        if (staffer == null)
        {
            return null;
        }

        staffer.LinkUser(command.UserId);

        await repository.ValidateCanSave(staffer);
        await repository.Save();

        eventsOutbox.Add(StafferLinkedToUserEventFrom(staffer.Id, command.UserId));

        return StafferModel.MapFromAggregate(staffer);
    }

    private static StafferLinkedToUser StafferLinkedToUserEventFrom(StafferId stafferId, string userId) =>
        new(stafferId.Value, userId);
}
