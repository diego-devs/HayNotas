using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using HayNotas.Services.Interfaces;

namespace HayNotas.Services;

public class SharingService : ISharingService
{
    private readonly ISettingsService _settingsService;

    public SharingService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public async Task SendEmailAsync(string toAddress, string subject, string body)
    {
        var smtp = _settingsService.Current.Smtp;

        using var client = new SmtpClient(smtp.Host, smtp.Port)
        {
            Credentials = new NetworkCredential(smtp.Username, smtp.Password),
            EnableSsl = smtp.UseSsl
        };

        var message = new MailMessage(smtp.FromAddress, toAddress, subject, body);
        await client.SendMailAsync(message);
    }

    public void ShareViaWhatsApp(string phoneNumber, string message)
    {
        var encoded = Uri.EscapeDataString(message);
        var url = $"https://wa.me/{phoneNumber}?text={encoded}";
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    }
}
