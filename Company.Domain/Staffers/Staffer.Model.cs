using Company.Shared.Types;

namespace WireOps.Company.Domain.Staffers;

public partial class Staffer
{
    public readonly Data _data;
    private Staffer(Data data) => _data = data;
    public static Staffer RestoreFrom(Data data) => new(data);
    public interface Data : IEquatable<Data>
    {
        public StafferId Id { get; }
        public Email Email { get; }
        public string? UserId { get; }
        public string GivenName { get; }
        public string FamilyName { get; }
        bool IEquatable<Data>.Equals(Data? other) =>
            other is not null &&
            Id.Equals(other.Id);
        void SetUserId(string userId);
        void SetEmail(Email email);
        void SetFamilyName(string givenName);
        void SetGivenName(string givenName);
    }
}
