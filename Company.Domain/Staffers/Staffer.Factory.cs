namespace WireOps.Company.Domain.Staffers;

public partial class Staffer
{
    public abstract class Factory
    {
        public Staffer New(string name, string sku, string? description = null)
        {
            var data = CreateData(StafferId.New(), name, sku, description);
            return new Staffer(data);
        }

        protected abstract Data CreateData(StafferId id, string name, string sku, string? description);
    }
}
