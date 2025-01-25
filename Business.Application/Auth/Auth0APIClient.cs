using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Business.Application.Email;
using Business.Application.Email.EmailTemplates;
using Microsoft.Extensions.Configuration;
using WireOps.Business.Common.Errors;

namespace Business.Application.Auth;

public class Auth0APIClient
{
    private readonly string _UiUrl;
    private readonly string _domain;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _audience;
    private readonly AuthenticationApiClient _authClient;
    private IEmailClient _emailClient;

    private ManagementApiClient? _managementApiClient;

    public Auth0APIClient(IConfiguration configuration, IEmailClient emailClient)
    {
        _emailClient = emailClient;

        _UiUrl = configuration["Routing:UiUrl"] ?? string.Empty;

        _domain = configuration["Auth0:Domain"] ?? string.Empty;
        _clientId = configuration["Auth0:ClientId"] ?? string.Empty;
        _clientSecret = configuration["Auth0:ClientSecret"] ?? string.Empty;
        _audience = configuration["Auth0:ManagementAPIAudience"] ?? string.Empty;

        if (string.IsNullOrEmpty(_domain))
        {
            throw new DesignError("Auth0 domain is not configured.");
        }

        _authClient = new AuthenticationApiClient(new Uri($"https://{_domain}"));
    }

    private async Task CheckAndGenerateManagementApiClient()
    {
        if (_managementApiClient != null)
            return;

        if (string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_clientSecret))
        {
            throw new DesignError("Auth0 client details are not configured.");
        }

        var token = await _authClient.GetTokenAsync(new ClientCredentialsTokenRequest
        {
            ClientId = _clientId,
            ClientSecret = _clientSecret,
            Audience = _audience
        });

        _managementApiClient = new ManagementApiClient(token.AccessToken, new Uri($"https://{_domain}/api/v2"));

        if (_managementApiClient == null)
        {
            throw new DesignError("Failed to create Management API client.");
        }
    }

    public async Task<string> CreateUser(string email, string tenantId, string givenName, string familyName, string companyName)
    {
        await CheckAndGenerateManagementApiClient();

        var request = new UserCreateRequest
        {
            Connection = "Username-Password-Authentication",
            Email = email,
            EmailVerified = false,
            NickName = companyName,
            FirstName = givenName,
            LastName = familyName,
            AppMetadata =
            new {
                company_id = tenantId,
                company_name = companyName,
                given_name = givenName,
                family_name = familyName
            },
            Password = Guid.NewGuid().ToString()
        };

        var user = await _managementApiClient!.Users.CreateAsync(request);

        return user.UserId;
    }

    public async Task<User> UpdateUser(string userId, string companyName, Guid companyId)
    {
        await CheckAndGenerateManagementApiClient();

        if (string.IsNullOrEmpty(userId))
        {
            throw new DesignError("Cannot update user without a proper userId");
        }

        var user = await _managementApiClient!.Users.GetAsync(userId);

        if (user == null)
        {
            throw new DomainError("Cannot find referenced user");
        }

        var request = new UserUpdateRequest
        {
            AppMetadata = new
            {
                company_id = companyId,
                company_name = companyName,
                given_name = user.AppMetadata.given_name,
                family_name = user.AppMetadata.family_name
            }
        };

        user = await _managementApiClient!.Users.UpdateAsync(userId, request);

        return user;
    }

    public async Task SendInviteEmail(string userId)
    {
        await CheckAndGenerateManagementApiClient();

        if (string.IsNullOrEmpty(userId))
        {
            throw new DomainError("Cannot invite a staffer with no userId");
        }

        var user = await _managementApiClient!.Users.GetAsync(userId);

        await SendInviteEmail(user);
    }

        

    internal async Task SendInviteEmail(User user)
    {
        if (string.IsNullOrEmpty(_UiUrl))
        {
            throw new DesignError("UiUrl is not configured");
        }

        var ticket = await _managementApiClient!.Tickets.CreatePasswordChangeTicketAsync(new PasswordChangeTicketRequest
        {
            UserId = user.UserId,
            ResultUrl = _UiUrl,
            MarkEmailAsVerified = true
        });

        ticket.Value += "type=invite"; //Switches from password forget to invite workflow

        var companyName = user.AppMetadata.company_name.ToString();

        //Send the ticket url in an email to the user
        var email = new UserInviteEmail(ticket.Value, companyName);
        await _emailClient.SendEmailAsync(email, user.Email);
    }

}
