using System.Net.Mail;
using System.Text.RegularExpressions;
using WireOps.Company.Common.Errors;
using WireOps.Company.Domain.Common.ValueObjects;

namespace Company.Shared.Types;

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
