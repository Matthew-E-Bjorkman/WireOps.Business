using WireOps.Business.Domain.Companies;
using WireOps.Business.Domain.Roles;

namespace WireOps.Business.Domain.Staffers;

public partial class Staffer
{
    public interface Repository
    {
        Task<IReadOnlyList<Staffer>> GetAllForCompany(CompanyId companyId);
        Task<IReadOnlyList<Staffer>> GetAllByRole(CompanyId companyId, RoleId roleId);  
        Task<Staffer> GetBy(CompanyId companyId, StafferId id);
        Task<Staffer> GetByUserId(string userId);
        Task ValidateAndPublish(Staffer staffer);
        Task Save();
        Task Delete(Staffer staffer);
    }
}
