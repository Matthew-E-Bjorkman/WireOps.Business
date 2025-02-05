using WireOps.Business.Domain.Companies;

namespace WireOps.Business.Domain.Roles;

public partial class Role
{
    public interface Repository
    {
        Task<IReadOnlyList<Role>> GetAllForCompany(CompanyId companyId);
        Task<Role> GetBy(CompanyId companyId, RoleId id);
        Task ValidateCanSave(Role role);
        Task Save();
        Task Delete(Role role);
    }
}
