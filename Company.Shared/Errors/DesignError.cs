namespace WireOps.Company.Common.Errors;

public class DesignError : Exception
{
    public DesignError(string message) : base(message) { }
    public DesignError(string message, Exception innerException) : base(message, innerException) { }
}