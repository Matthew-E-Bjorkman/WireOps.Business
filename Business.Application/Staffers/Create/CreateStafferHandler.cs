using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Application.Staffers.Create;

public class CreateStafferHandler (
    Staffer.Repository repository, 
    Staffer.Factory factory,
    StafferEventsOutbox eventsOutbox
) : CommandHandler<CreateStaffer, StafferModel>
{
    public async Task<StafferModel> Handle(CreateStaffer command)
    {
        var staffer = factory.New(command.CompanyId, command.Email, command.GivenName, command.FamilyName, false, null);
        await repository.ValidateCanSave(staffer);
        await repository.Save();

        eventsOutbox.Add(CreateEventFrom(staffer.Id)); 

        var stafferModel = StafferModel.MapFromAggregate(staffer);

        return stafferModel;
    }

    private static StafferCreated CreateEventFrom(StafferId stafferId) =>
        new(stafferId.Value); 
}
