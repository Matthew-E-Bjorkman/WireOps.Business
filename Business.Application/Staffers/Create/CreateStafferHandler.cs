using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Staffers;

namespace WireOps.Business.Application.Staffers.Create;

public class CreateStafferHandler (
    Staffer.Repository repository, 
    Staffer.Factory factory
) : CommandHandler<CreateStaffer, StafferModel>
{
    public async Task<StafferModel> Handle(CreateStaffer command)
    {
        var staffer = factory.New(command.CompanyId, command.Email, command.GivenName, command.FamilyName, false, command.RoleId);

        await repository.ValidateAndPublish(staffer);
        await repository.Save();

        var stafferModel = StafferModel.MapFromAggregate(staffer);

        return stafferModel;
    }
}
