using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HayNotas.Models;
using HayNotas.Services.Interfaces;

namespace HayNotas.ViewModels;

public partial class NotesViewModel : ObservableObject, IRecipient<NoteCreatedMessage>
{
    private readonly INotesService _notesService;
    private readonly ISharingService _sharingService;
    private readonly ISettingsService _settingsService;

    public ObservableCollection<Note> Notes { get; } = [];

    [ObservableProperty]
    private Note? _selectedNote;

    [ObservableProperty]
    private string _shareEmail = "";

    [ObservableProperty]
    private string _shareWhatsApp = "";

    [ObservableProperty]
    private string _statusText = "";

    [ObservableProperty]
    private bool _isSharePanelVisible;

    public NotesViewModel(INotesService notesService, ISharingService sharingService, ISettingsService settingsService)
    {
        _notesService = notesService;
        _sharingService = sharingService;
        _settingsService = settingsService;

        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(NoteCreatedMessage message)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(async () => await LoadNotes());
    }

    [RelayCommand]
    private async Task LoadNotes()
    {
        var notes = await _notesService.GetAllNotesAsync();
        Notes.Clear();
        foreach (var note in notes)
            Notes.Add(note);
    }

    [RelayCommand]
    private async Task DeleteNote()
    {
        if (SelectedNote is null) return;

        await _notesService.DeleteNoteAsync(SelectedNote.FileName);
        Notes.Remove(SelectedNote);
        SelectedNote = null;
    }

    [RelayCommand]
    private void ShowSharePanel()
    {
        if (SelectedNote is null) return;
        ShareWhatsApp = _settingsService.Current.DefaultWhatsAppNumber;
        IsSharePanelVisible = true;
    }

    [RelayCommand]
    private void HideSharePanel()
    {
        IsSharePanelVisible = false;
        StatusText = "";
    }

    [RelayCommand]
    private async Task SendEmail()
    {
        if (SelectedNote is null || string.IsNullOrWhiteSpace(ShareEmail)) return;

        try
        {
            await _sharingService.SendEmailAsync(ShareEmail, $"Note: {SelectedNote.Title}", SelectedNote.Content);
            StatusText = "Email sent successfully!";
        }
        catch (Exception ex)
        {
            StatusText = $"Email error: {ex.Message}";
        }
    }

    [RelayCommand]
    private void SendWhatsApp()
    {
        if (SelectedNote is null) return;

        var number = string.IsNullOrWhiteSpace(ShareWhatsApp)
            ? _settingsService.Current.DefaultWhatsAppNumber
            : ShareWhatsApp;

        if (string.IsNullOrWhiteSpace(number))
        {
            StatusText = "Please enter a WhatsApp number.";
            return;
        }

        _sharingService.ShareViaWhatsApp(number, $"*{SelectedNote.Title}*\n\n{SelectedNote.Content}");
        StatusText = "WhatsApp opened!";
    }
}
