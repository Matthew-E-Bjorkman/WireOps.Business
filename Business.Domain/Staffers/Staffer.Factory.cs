using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies;

namespace WireOps.Business.Domain.Staffers;

public partial class Staffer
{
    public abstract class Factory
    {
        public Staffer New(Guid companyId, string email, string givenName, string familyName, bool isOwner)
        {
            var data = CreateData(StafferId.New(), CompanyId.From(companyId), Email.From(email), givenName, familyName, isOwner);
            return new Staffer(data);
        }

        protected abstract Data CreateData(StafferId id, CompanyId companyId, Email email, string givenName, string familyName, bool isOwner);
    }
}
