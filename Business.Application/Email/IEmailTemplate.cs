namespace Business.Application.Email;

public interface IEmailTemplate
{
    string Subject { get; }
    string Body { get; }
}
