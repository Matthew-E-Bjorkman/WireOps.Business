using WireOps.Business.Domain.Companies;

namespace WireOps.Business.Domain.Roles;

public partial class Role
{
    public abstract class Factory
    {
        public Role New(Guid companyId, string name, bool isAdmin, bool isOwnerRole)
        {
            var data = CreateData(RoleId.New(), CompanyId.From(companyId), name, isAdmin, isOwnerRole);
            return new Role(data);
        }

        protected abstract Data CreateData(RoleId id, CompanyId companyId, string name, bool isAdmin, bool isOwnerRole);
    }
}
