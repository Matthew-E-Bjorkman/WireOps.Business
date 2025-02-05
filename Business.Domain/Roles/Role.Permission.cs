namespace WireOps.Business.Domain.Roles;

public partial class Role
{
    public class Permission : IEquatable<Permission>
    {
        public RoleId RoleId { get; }
        public string Resource { get; }
        public ResourceAction Action { get; }
        public Permission(RoleId roleId, string resource, ResourceAction action)
        {
            RoleId = roleId;
            Resource = resource;
            Action = action;
        }

        public bool Equals(Permission? other) 
            => other is not null 
            && Resource.Equals(other.Resource) 
            && Action.Equals(other.Action);
    }
}
