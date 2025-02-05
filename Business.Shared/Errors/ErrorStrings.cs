namespace WireOps.Business.Common.Errors;

public static class Error
{
    public const string SameAggregateRestoredMoreThanOnce =
        "Same aggregate is restored from the repository more than once in a single business transaction";

    public const string SameAggregateValidatedMoreThanOnce =
        "Same aggregate is validated for saving from the repository more than once in a single business transaction";

    public const string SaveOfUnknownAggregate =
        $"Attempt to save aggregate that wasn't created nor gotten with repository";

    public const string DeleteOfUnknownAggregate =
        $"Attempt to delete aggregate that wasn't created nor gotten with repository";

    public const string AggregateNotFound =
        "Aggregate not found";

    public const string NameAlreadyInUse =
        "Operation would result in the same name value being used twice for the same type of object";

    public const string CompanyHasMultipleOwners =
        "Operation would result in company having multiple owning staffers";

    public const string CompanyHasMultipleOwnerRoles =
        "Operation would result in company having multiple owner roles";

    public const string RoleAlreadyHasPermission =
        "Cannot add permission to role that already possesses it";

    public const string RoleDoesNotHavePermission =
        "Cannot remove permission from role that does not already possess it";

    public const string InvalidResourceAction =
        "Resource action must be either 'read' or 'write'";
}
