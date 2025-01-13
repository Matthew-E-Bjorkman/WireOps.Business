using WireOps.Company.Application.Common;
using WireOps.Company.Domain.Staffers;
using WireOps.Company.Domain.Staffers.Events;

namespace WireOps.Company.Application.Staffers.Delete;

public class DeleteStafferHandler (
    Staffer.Repository repository, 
    StafferEventsOutbox eventsOutbox
) : CommandHandler<DeleteStaffer, bool>
{
    public async Task<bool> Handle(DeleteStaffer command)
    {
        var staffer = await repository.GetBy(StafferId.From(command.Id));

        if (staffer == null)
        {
            return false;
        }

        await repository.Delete(staffer);

        eventsOutbox.Add(DeleteEventFrom(staffer.Id)); 

        return true;
    }

    private static StafferDeleted DeleteEventFrom(StafferId stafferId) =>
        new(stafferId.Value);
}
