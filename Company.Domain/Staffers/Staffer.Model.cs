namespace WireOps.Company.Domain.Staffers;

public partial class Staffer
{
    public readonly Data _data;
    private Staffer(Data data) => _data = data;
    public static Staffer RestoreFrom(Data data) => new(data);

    public void Update(string name, string sku, string? description) => _data.Update(name, sku, description);

    public interface Data : IEquatable<Data>
    {
        public StafferId Id { get; }
        public string Name { get; }
        public string SKU { get; }
        public string Description { get; }
        bool IEquatable<Data>.Equals(Data? other) =>
            other is not null &&
            Id.Equals(other.Id);
        void Update(string name, string sku, string? description);
    }
}
