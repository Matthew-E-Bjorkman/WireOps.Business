namespace WireOps.Company.Domain.Staffers;

public partial class Staffer
{
    public interface Repository
    {
        Task<IReadOnlyList<Staffer>> GetAll();
        Task<Staffer> GetBy(StafferId id);
        Task Save(Staffer order);
        Task Delete(Staffer order);
    }
}
