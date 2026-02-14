using System.Windows.Controls;
using HayNotas.ViewModels;

namespace HayNotas.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        // PasswordBox can't be data-bound, so sync manually
        if (DataContext is SettingsViewModel vm)
        {
            ApiKeyBox.Password = vm.GeminiApiKey;
            SmtpPasswordBox.Password = vm.SmtpPassword;
        }
    }

    private void ApiKeyBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel vm)
            vm.GeminiApiKey = ApiKeyBox.Password;
    }

    private void SmtpPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel vm)
            vm.SmtpPassword = SmtpPasswordBox.Password;
    }
}
