using WireOps.Business.Domain.Companies;

namespace WireOps.Business.Domain.Roles;

public partial class Role
{
    public abstract class Factory
    {
        public Role New(Guid companyId, string name, bool isAdmin, bool isOwnerRole)
        {
            var data = CreateData(RoleId.New(), CompanyId.From(companyId), name, isAdmin, isOwnerRole);
            var role = new Role(data);
            role.DomainEvents.Add(Events.RoleCreated(role));
            return role;
        }

        protected abstract Data CreateData(RoleId id, CompanyId companyId, string name, bool isAdmin, bool isOwnerRole);
    }
}
