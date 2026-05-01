namespace B3PricingMonitor;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
public class EmailService
{
    private readonly SmtpConfig _smtpConfig;
    public EmailService(SmtpConfig _config)
    {
        _smtpConfig = _config;
    }

    public async Task SendAsync(List<string> emails, string subject, string body)
    {        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_smtpConfig.Username, _smtpConfig.Password);
        
        foreach(var email in emails)
        {
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Alerta de Preços", _smtpConfig.Username));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };
            
            await smtp.SendAsync(message);
        }
        await smtp.DisconnectAsync(true);
    }
}