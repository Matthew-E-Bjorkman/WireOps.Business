using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Application.Staffers.Update;

public class UpdateStafferHandler (
    Staffer.Repository repository, 
    StafferEventsOutbox eventsOutbox
) : CommandHandler<UpdateStaffer, StafferModel?>
{
    public async Task<StafferModel?> Handle(UpdateStaffer command)
    {
        var staffer = await repository.GetBy(StafferId.From(command.Id));

        if (staffer == null)
        {
            return null;
        }

        staffer.ChangeInformation(command.Email, command.GivenName, command.FamilyName);

        await repository.ValidateCanSave(staffer);
        await repository.Save();

        eventsOutbox.Add(UpdateEventFrom(staffer.Id));

        return StafferModel.MapFromAggregate(staffer);
    }

    private static StafferUpdated UpdateEventFrom(StafferId stafferId) =>
        new(stafferId.Value); 
}
