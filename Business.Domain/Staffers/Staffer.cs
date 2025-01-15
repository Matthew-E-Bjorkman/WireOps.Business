using WireOps.Business.Domain.Common.ValueObjects.Types;

namespace WireOps.Business.Domain.Staffers;

public partial class Staffer
{
    public StafferId Id => _data.Id;

    public void LinkUser(string userId)
    {
        _data.SetUserId(userId);
    }

    public void ChangeInformation(string emailAddress, string givenName, string familyName)
    {
        var email = Email.From(emailAddress);
        if (email != _data.Email)
        {
            _data.SetEmail(email);
        }

        if (!string.Equals(givenName, _data.GivenName))
        {
            _data.SetGivenName(givenName);
        }

        if (!string.Equals(familyName, _data.FamilyName))
        {
            _data.SetFamilyName(givenName);
        };

    }
}
