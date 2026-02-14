using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using HayNotas.Services;
using HayNotas.Services.Interfaces;
using HayNotas.ViewModels;
using HayNotas.Views;

namespace HayNotas;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        // Services
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<ISpeechService, SpeechService>();
        services.AddSingleton<INotesService, NotesService>();
        services.AddSingleton<ISharingService, SharingService>();
        services.AddHttpClient<IChatService, ChatService>();

        // ViewModels
        services.AddSingleton<ChatViewModel>();
        services.AddSingleton<NotesViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<MainViewModel>();

        _serviceProvider = services.BuildServiceProvider();

        // Load settings before showing the window
        var settingsService = _serviceProvider.GetRequiredService<ISettingsService>();
        await settingsService.LoadAsync();

        var mainWindow = new MainWindow
        {
            DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
        };
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (_serviceProvider is IDisposable disposable)
            disposable.Dispose();
        base.OnExit(e);
    }
}
