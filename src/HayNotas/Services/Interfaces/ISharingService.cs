namespace HayNotas.Services.Interfaces;

public interface ISharingService
{
    Task SendEmailAsync(string toAddress, string subject, string body);
    void ShareViaWhatsApp(string phoneNumber, string message);
}
