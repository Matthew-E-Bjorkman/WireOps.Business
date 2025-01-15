namespace WireOps.Business.Domain.Companies;

public partial class Company
{
    public interface Repository
    {
        Task<IReadOnlyList<Company>> GetAll();
        Task<Company> GetBy(CompanyId id);
        Task ValidateCanSave(Company company);
        Task Save();
        Task Delete(Company company);
    }
}
