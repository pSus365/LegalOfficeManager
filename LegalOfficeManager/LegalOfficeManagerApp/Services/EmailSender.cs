using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using LegalOfficeManagerApp.Models;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var mail = new MailMessage()
        {
            From = new MailAddress(_emailSettings.Username),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };
        mail.To.Add(email);

        using var smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
        {
            Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
            EnableSsl = true
        };

        await smtp.SendMailAsync(mail);
    }
}
