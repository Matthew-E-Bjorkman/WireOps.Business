using WireOps.Company.Domain.Staffers;

namespace WireOps.Company.Infrastructure.Database.SQL.EntityFramework;

public class DbStaffer : Staffer.Data
{
    public StafferId Id { get; set; }
    public string Name { get; set; }
    public string SKU { get; set; }
    public string? Description { get; set; }
    public int Version { get; set; }

    public void Update(string name, string sku, string? description)
    {
        Name = name;
        SKU = sku;
        Description = description;
    }
}