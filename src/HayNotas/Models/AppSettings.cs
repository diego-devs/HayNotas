namespace HayNotas.Models;

public class AppSettings
{
    public string GeminiApiKey { get; set; } = "";
    public string GeminiModel { get; set; } = "gemini-2.5-flash";
    public string NotesFolder { get; set; } = "";
    public SmtpSettings Smtp { get; set; } = new();
    public string DefaultWhatsAppNumber { get; set; } = "";
}

public class SmtpSettings
{
    public string Host { get; set; } = "";
    public int Port { get; set; } = 587;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool UseSsl { get; set; } = true;
    public string FromAddress { get; set; } = "";
}
