using System.Net.Mail;
using WireOps.Business.Common.Errors;

namespace WireOps.Business.Domain.Common.ValueObjects.Types;

public readonly record struct Email(string Value) : ValueObject<string>
{
    public static Email From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainError("Email cannot be empty or whitespace");
        }

        try
        {
            var email = new MailAddress(value);
        }
        catch 
        {
            throw new InfrastructureError("Email validation timed out");
        }

        return new(value);
    }
}
