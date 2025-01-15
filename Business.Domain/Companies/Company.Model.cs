using Business.Domain.Common.Definitions;
using WireOps.Business.Domain.Common.ValueObjects.Types;

namespace WireOps.Business.Domain.Companies;

public partial class Company
{
    public readonly Data _data;
    private Company(Data data) => _data = data;
    public static Company RestoreFrom(Data data) => new(data);
    public interface Data : IEquatable<Data>, AggregateData
    {
        public CompanyId Id { get; }
        public string Name { get; }
        public Address? Address { get; }
        bool IEquatable<Data>.Equals(Data? other) =>
            other is not null &&
            Id.Equals(other.Id);

        void SetName(string name);
        void SetAddress(Address address);
    }
}
