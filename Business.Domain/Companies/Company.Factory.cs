using System.ComponentModel.Design;
using WireOps.Business.Domain.Staffers;

namespace WireOps.Business.Domain.Companies;

public partial class Company
{
    public abstract class Factory
    {
        public Company New(string name)
        {
            var data = CreateData(CompanyId.New(), name);
            return new Company(data);
        }

        protected abstract Data CreateData(CompanyId id, string name);
    }
}
