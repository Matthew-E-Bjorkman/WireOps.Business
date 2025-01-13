using WireOps.Company.Application.Common;
using WireOps.Company.Domain.Staffers;
using WireOps.Company.Domain.Staffers.Events;

namespace WireOps.Company.Application.Staffers.Update;

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
        
        staffer.Update(command.Name, command.SKU, command.Description);

        await repository.Save(staffer);

        eventsOutbox.Add(UpdateEventFrom(staffer.Id));

        var stafferModel = StafferModel.MapFromAggregate(staffer);

        return stafferModel;
    }

    private static StafferUpdated UpdateEventFrom(StafferId stafferId) =>
        new(stafferId.Value); 
}
