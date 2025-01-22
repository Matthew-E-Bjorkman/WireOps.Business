using WireOps.Business.Common.Errors;

namespace Business.Application.Email.EmailTemplates;

public class UserInviteEmail(string inviteUrl, string companyName) : IEmailTemplate
{
    private string SubjectFormat = "You have been invited to join {0}";
    private string BodyFormat = 
        """
            You have been invited to join WireOps. 
            Please click the link below to accept the invitation.
            {0}
        """;

    public string Subject => string.Format(SubjectFormat, companyName);
    public string Body => string.Format(BodyFormat, inviteUrl);
}
