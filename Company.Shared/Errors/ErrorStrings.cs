namespace WireOps.Company.Common.Errors;

public static class Error
{
    public const string SameAggregateRestoredMoreThanOnce =
        "Same aggregate is restored from the repository more than once in a single business transaction";

    public const string SaveOfUnknownAggregate =
        $"Attempt to save aggregate that wasn't created nor gotten with repository";

    public const string DeleteOfUnknownAggregate =
        $"Attempt to delete aggregate that wasn't created nor gotten with repository";
}
