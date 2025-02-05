using Business.Domain.Common.Definitions;
using WireOps.Business.Domain.Common.ValueObjects.Types;
using WireOps.Business.Domain.Companies;

namespace WireOps.Business.Domain.Roles;

public partial class Role
{
    public readonly Data _data;
    private Role(Data data) => _data = data;
    public static Role RestoreFrom(Data data) => new(data);
    public interface Data : IEquatable<Data>, AggregateData
    {
        public RoleId Id { get; }
        public CompanyId CompanyId { get; }
        public string Name { get; }
        public bool IsAdmin { get; }
        public bool IsOwnerRole { get; }
        public IReadOnlyList<Permission> Permissions { get; }

        void Add(Permission permission);
        void Remove(Permission permission);

        bool IEquatable<Data>.Equals(Data? other) =>
            other is not null &&
            Id.Equals(other.Id);

        void SetName(string name);
        void SetIsAdmin(bool isAdmin);
    }
}
