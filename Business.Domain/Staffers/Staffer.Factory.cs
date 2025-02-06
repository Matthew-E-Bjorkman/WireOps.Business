using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Companies.Events;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Domain.Staffers;

public partial class Staffer
{
    public abstract class Factory
    {
        public Staffer New(Guid companyId, string email, string givenName, string familyName, bool isOwner, Guid? roleId = null)
        {
            var data = CreateData(StafferId.New(), CompanyId.From(companyId), Email.From(email), givenName, familyName, isOwner, roleId.HasValue ? RoleId.From(roleId.Value) : null);
            var staffer = new Staffer(data);
            staffer.DomainEvents.Add(Events.StafferCreated(staffer));
            return staffer;
        }

        protected abstract Data CreateData(StafferId id, CompanyId companyId, Email email, string givenName, string familyName, bool isOwner, RoleId? roleId);
    }
}
