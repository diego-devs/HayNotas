using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HayNotas.Models;
using HayNotas.Services.Interfaces;

namespace HayNotas.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    [ObservableProperty] private string _geminiApiKey = "";
    [ObservableProperty] private string _geminiModel = "gemini-2.5-flash";
    [ObservableProperty] private string _notesFolder = "";
    [ObservableProperty] private string _smtpHost = "";
    [ObservableProperty] private int _smtpPort = 587;
    [ObservableProperty] private string _smtpUsername = "";
    [ObservableProperty] private string _smtpPassword = "";
    [ObservableProperty] private bool _smtpUseSsl = true;
    [ObservableProperty] private string _smtpFromAddress = "";
    [ObservableProperty] private string _defaultWhatsAppNumber = "";
    [ObservableProperty] private string _statusText = "";

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        LoadFromSettings();
    }

    public void LoadFromSettings()
    {
        var s = _settingsService.Current;
        GeminiApiKey = s.GeminiApiKey;
        GeminiModel = s.GeminiModel;
        NotesFolder = s.NotesFolder;
        SmtpHost = s.Smtp.Host;
        SmtpPort = s.Smtp.Port;
        SmtpUsername = s.Smtp.Username;
        SmtpPassword = s.Smtp.Password;
        SmtpUseSsl = s.Smtp.UseSsl;
        SmtpFromAddress = s.Smtp.FromAddress;
        DefaultWhatsAppNumber = s.DefaultWhatsAppNumber;
        StatusText = "";
    }

    [RelayCommand]
    private async Task Save()
    {
        var settings = new AppSettings
        {
            GeminiApiKey = GeminiApiKey,
            GeminiModel = string.IsNullOrWhiteSpace(GeminiModel) ? "gemini-2.5-flash" : GeminiModel,
            NotesFolder = NotesFolder,
            DefaultWhatsAppNumber = DefaultWhatsAppNumber,
            Smtp = new SmtpSettings
            {
                Host = SmtpHost,
                Port = SmtpPort,
                Username = SmtpUsername,
                Password = SmtpPassword,
                UseSsl = SmtpUseSsl,
                FromAddress = SmtpFromAddress
            }
        };

        await _settingsService.SaveAsync(settings);
        StatusText = "Settings saved!";
    }

    [RelayCommand]
    private void BrowseFolder()
    {
        var dialog = new Microsoft.Win32.OpenFolderDialog
        {
            Title = "Select Notes Folder"
        };

        if (dialog.ShowDialog() == true)
        {
            NotesFolder = dialog.FolderName;
        }
    }
}
