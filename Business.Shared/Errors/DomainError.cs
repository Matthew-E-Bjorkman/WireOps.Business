namespace WireOps.Business.Common.Errors;

public class DomainError : Exception
{
    public DomainError(string message) : base(message) { }
}