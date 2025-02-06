using WireOps.Business.Application.Common;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Staffers;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Application.Staffers.Update;

public class UpdateStafferHandler (
    Staffer.Repository stafferRepository,
    Role.Repository roleRepository
) : CommandHandler<UpdateStaffer, StafferModel?>
{
    public async Task<StafferModel?> Handle(UpdateStaffer command)
    {
        var staffer = await stafferRepository.GetBy(CompanyId.From(command.CompanyId), StafferId.From(command.Id));

        if (staffer == null)
        {
            return null;
        }

        staffer.ChangeInformation(command.Email, command.GivenName, command.FamilyName);

        if (command.RoleId.HasValue)
        {
            var role = await roleRepository.GetBy(CompanyId.From(command.CompanyId), RoleId.From(command.RoleId.Value));
            if (role == null)
            {
                return null;
            }
            staffer.AssignRole(role);
        }

        await stafferRepository.ValidateAndPublish(staffer);
        await stafferRepository.Save();

        return StafferModel.MapFromAggregate(staffer);
    }
}
