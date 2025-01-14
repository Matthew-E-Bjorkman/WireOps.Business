using Company.Shared.Types;

namespace WireOps.Company.Domain.Staffers;

public partial class Staffer
{
    public abstract class Factory
    {
        public Staffer New(string email, string givenName, string familyName)
        {
            var data = CreateData(StafferId.New(), Email.From(email), givenName, familyName);
            return new Staffer(data);
        }

        protected abstract Data CreateData(StafferId id, Email email, string givenName, string familyName);
    }
}
