using System.IO;
using System.Text.Json;
using HayNotas.Models;
using HayNotas.Services.Interfaces;

namespace HayNotas.Services;

public class SettingsService : ISettingsService
{
    private static readonly string SettingsDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HayNotas");

    private static readonly string SettingsFile = Path.Combine(SettingsDir, "appsettings.json");

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public AppSettings Current { get; private set; } = new();

    public async Task LoadAsync()
    {
        if (!File.Exists(SettingsFile))
        {
            Current = CreateDefaults();
            await SaveAsync(Current);
            return;
        }

        var json = await File.ReadAllTextAsync(SettingsFile);
        Current = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions) ?? CreateDefaults();
    }

    public async Task SaveAsync(AppSettings settings)
    {
        Current = settings;
        Directory.CreateDirectory(SettingsDir);
        var json = JsonSerializer.Serialize(settings, JsonOptions);
        await File.WriteAllTextAsync(SettingsFile, json);
    }

    private static AppSettings CreateDefaults()
    {
        return new AppSettings
        {
            NotesFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HayNotas")
        };
    }
}
