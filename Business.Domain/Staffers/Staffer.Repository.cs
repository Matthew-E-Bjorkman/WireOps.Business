using WireOps.Business.Domain.Companies;

namespace WireOps.Business.Domain.Staffers;

public partial class Staffer
{
    public interface Repository
    {
        Task<IReadOnlyList<Staffer>> GetAll();
        Task<Staffer> GetBy(StafferId id);
        Task<Staffer> GetByUserId(string userId);
        Task ValidateCanSave(Staffer staffer);
        Task Save();
        Task Delete(Staffer staffer);
    }
}
