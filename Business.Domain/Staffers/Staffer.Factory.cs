using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;

namespace WireOps.Business.Domain.Staffers;

public partial class Staffer
{
    public abstract class Factory
    {
        public Staffer New(Guid companyId, string email, string givenName, string familyName, bool isOwner, Guid? roleId)
        {
            var data = CreateData(StafferId.New(), CompanyId.From(companyId), Email.From(email), givenName, familyName, isOwner, roleId.HasValue ? RoleId.From(roleId.Value) : null);
            return new Staffer(data);
        }

        protected abstract Data CreateData(StafferId id, CompanyId companyId, Email email, string givenName, string familyName, bool isOwner, RoleId? roleId);
    }
}
