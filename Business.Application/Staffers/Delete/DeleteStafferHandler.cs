using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Application.Staffers.Delete;

public class DeleteStafferHandler (
    Staffer.Repository repository, 
    StafferEventsOutbox eventsOutbox
) : CommandHandler<DeleteStaffer, bool>
{
    public async Task<bool> Handle(DeleteStaffer command)
    {
        var staffer = await repository.GetBy(CompanyId.From(command.CompanyId), StafferId.From(command.Id));

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
