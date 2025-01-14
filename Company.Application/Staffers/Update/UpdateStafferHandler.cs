﻿using WireOps.Company.Application.Common;
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

        staffer.ChangeInformation(command.Email, command.GivenName, command.FamilyName);

        await repository.Save(staffer);

        eventsOutbox.Add(UpdateEventFrom(staffer.Id));

        return StafferModel.MapFromAggregate(staffer);
    }

    private static StafferUpdated UpdateEventFrom(StafferId stafferId) =>
        new(stafferId.Value); 
}
