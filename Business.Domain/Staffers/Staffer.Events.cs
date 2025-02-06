using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Domain.Staffers;

public partial class Staffer
{
    public static class Events
    {
        public static StafferCreated StafferCreated(Staffer staffer) 
            => new(staffer.CompanyId.Value, staffer.Id.Value, staffer._data);

        public static StafferDeleted StafferDeleted(Staffer staffer) 
            => new(staffer.CompanyId.Value, staffer.Id.Value);

        public static StafferDetailsChanged StafferDetailsChanged(Staffer staffer) 
            => new(staffer.CompanyId.Value, staffer.Id.Value, new(staffer._data.GivenName, staffer._data.FamilyName));

        public static StafferRoleAssigned StafferRoleAssigned(Staffer staffer, Role role) 
            => new(staffer.CompanyId.Value, staffer.Id.Value, new(staffer._data.UserId!, role._data.IsAdmin, role._data.Permissions));

        public static StafferLinkedToUser StafferLinkedToUser(Staffer staffer) 
            => new(staffer.CompanyId.Value, staffer.Id.Value, new(staffer._data.UserId!));

        public static StafferInvited StafferInvited(Staffer staffer) 
            => new(staffer.CompanyId.Value, staffer.Id.Value);
    }
}
