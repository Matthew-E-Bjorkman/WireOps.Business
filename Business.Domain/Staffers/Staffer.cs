using WireOps.Business.Common.Errors;
using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;
using WireOps.Business.Domain.Staffers.Events;

namespace WireOps.Business.Domain.Staffers;

public partial class Staffer
{
    public StafferId Id => _data.Id;
    public CompanyId CompanyId => _data.CompanyId;
    public List<StafferEvent> DomainEvents { get; } = [];

    public void LinkUser(string userId)
    {
        if (!string.IsNullOrEmpty(_data.UserId))
        {
            throw new DomainError(Error.StafferUserIdAlreadyAssigned);
        }

        _data.SetUserId(userId);
        DomainEvents.Add(Events.StafferLinkedToUser(this));
    }

    public void ChangeInformation(string emailAddress, string givenName, string familyName)
    {
        bool isDirty = false;

        var email = Email.From(emailAddress);
        if (email != _data.Email)
        {
            _data.SetEmail(email);
            isDirty = true;
        }

        if (!string.Equals(givenName, _data.GivenName))
        {
            _data.SetGivenName(givenName);
            isDirty = true;
        }

        if (!string.Equals(familyName, _data.FamilyName))
        {
            _data.SetFamilyName(givenName);
            isDirty = true;
        };

        if (isDirty)
        {
            DomainEvents.Add(Events.StafferDetailsChanged(this));
        }
    }

    public void AssignRole(Role role)
    {
        if (role.Id != _data.RoleId)
        {
            _data.SetRoleId(role.Id);
            DomainEvents.Add(Events.StafferRoleAssigned(this, role));
        }
    }
}
