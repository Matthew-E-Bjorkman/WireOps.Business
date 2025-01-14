namespace WireOps.Company.Common.Errors;

public class InfrastructureError : Exception
{
    public InfrastructureError() { }

    public InfrastructureError(string message) : base(message) { }

    public InfrastructureError(string message, Exception innerException) : base(message, innerException) { }
}