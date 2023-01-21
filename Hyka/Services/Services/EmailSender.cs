using System.Text.Json;
using Hyka.Dtos;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Hyka.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public static string SUPPORT_EMAIL { get; } = "support@paf.com";

    public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
    }

    // public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var API_KEY = _configuration.GetSection("Keys")["SendGridKey"];
        if (string.IsNullOrEmpty(API_KEY))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(API_KEY, subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage();
        msg.SetFrom(new EmailAddress("feljpe000@gmail.com", "Notificador PAF"));
        msg.AddTo(new EmailAddress(toEmail));
        msg.SetTemplateId("d-bb229c44a3a94a779edc243b598cd98d");
        var finalMessage = JsonSerializer.Deserialize<MessageDto>(message);
        finalMessage.Subject = subject;
        msg.SetTemplateData(finalMessage);
        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error sending email: {response.StatusCode}");
        }
        _logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }

}