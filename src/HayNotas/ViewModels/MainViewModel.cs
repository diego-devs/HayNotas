using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HayNotas.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ChatViewModel _chatViewModel;
    private readonly NotesViewModel _notesViewModel;
    private readonly SettingsViewModel _settingsViewModel;

    [ObservableProperty]
    private object _currentView;

    public MainViewModel(ChatViewModel chatViewModel, NotesViewModel notesViewModel, SettingsViewModel settingsViewModel)
    {
        _chatViewModel = chatViewModel;
        _notesViewModel = notesViewModel;
        _settingsViewModel = settingsViewModel;
        _currentView = _chatViewModel;
    }

    [RelayCommand]
    private void NavigateToChat() => CurrentView = _chatViewModel;

    [RelayCommand]
    private async Task NavigateToNotes()
    {
        CurrentView = _notesViewModel;
        await _notesViewModel.LoadNotesCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private void NavigateToSettings()
    {
        _settingsViewModel.LoadFromSettings();
        CurrentView = _settingsViewModel;
    }
}
