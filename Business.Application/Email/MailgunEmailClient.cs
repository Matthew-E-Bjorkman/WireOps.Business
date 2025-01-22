using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WireOps.Business.Common.Errors;

namespace Business.Application.Email;

public class MailgunEmailClient : IEmailClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _domain;

    public MailgunEmailClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Mailgun:ApiKey"]!;
        _domain = configuration["Mailgun:Domain"]!;

        if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_domain))
        {
            throw new DesignError("Mailgun API key and domain must be configured.");
        }
    }

    public async Task SendEmailAsync(IEmailTemplate email, string toAddress)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.mailgun.net/v3/{_domain}/messages");
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{_apiKey}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("from", $"WireOps <noreply@{_domain}>"),
            new KeyValuePair<string, string>("to", toAddress),
            new KeyValuePair<string, string>("subject", email.Subject),
            new KeyValuePair<string, string>("text", email.Body)
        });

        request.Content = content;

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}