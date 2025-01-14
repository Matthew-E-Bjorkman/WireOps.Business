using WireOps.Company.Application.Common;
using WireOps.Company.Domain.Staffers;
using WireOps.Company.Domain.Staffers.Events;

namespace WireOps.Company.Application.Staffers.Create;

public class LinkUserHandler (
    Staffer.Repository repository, 
    StafferEventsOutbox eventsOutbox
) : CommandHandler<LinkUser, StafferModel?>
{
    public async Task<StafferModel?> Handle(LinkUser command)
    {
        var staffer = await repository.GetBy(StafferId.From(command.Id));

        if (staffer == null)
        {
            return null;
        }

        staffer.LinkUser(command.UserId);

        await repository.Save(staffer);

        eventsOutbox.Add(StafferLinkedToUserEventFrom(staffer.Id, command.UserId));

        return StafferModel.MapFromAggregate(staffer);
    }

    private static StafferLinkedToUser StafferLinkedToUserEventFrom(StafferId stafferId, string userId) =>
        new(stafferId.Value, userId);
}
