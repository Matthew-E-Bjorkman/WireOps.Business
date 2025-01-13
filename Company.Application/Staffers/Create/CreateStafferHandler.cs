using WireOps.Company.Application.Common;
using WireOps.Company.Domain.Staffers;
using WireOps.Company.Domain.Staffers.Events;

namespace WireOps.Company.Application.Staffers.Create;

public class CreateStafferHandler (
    Staffer.Repository repository, 
    Staffer.Factory factory,
    StafferEventsOutbox eventsOutbox
) : CommandHandler<CreateStaffer, StafferModel>
{
    public async Task<StafferModel> Handle(CreateStaffer command)
    {
        var staffer = factory.New(command.Name, command.SKU, command.Description);

        await repository.Save(staffer);

        eventsOutbox.Add(CreateEventFrom(staffer.Id)); 

        var stafferModel = StafferModel.MapFromAggregate(staffer);

        return stafferModel;
    }

    private static StafferCreated CreateEventFrom(StafferId stafferId) =>
        new(stafferId.Value); 
}
