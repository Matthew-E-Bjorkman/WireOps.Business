namespace Business.Application.Email;

public interface IEmailClient
{
    Task SendEmailAsync(IEmailTemplate email, string toAddress);
}
