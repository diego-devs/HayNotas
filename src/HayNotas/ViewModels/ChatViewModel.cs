using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HayNotas.Models;
using HayNotas.Services.Interfaces;

namespace HayNotas.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    private readonly IChatService _chatService;
    private readonly ISpeechService _speechService;
    private readonly INotesService _notesService;
    private readonly ISettingsService _settingsService;

    public ObservableCollection<ChatMessage> Messages { get; } = [];

    [ObservableProperty]
    private string _inputText = "";

    [ObservableProperty]
    private bool _isListening;

    [ObservableProperty]
    private bool _isSending;

    [ObservableProperty]
    private string _hypothesisText = "";

    [ObservableProperty]
    private string _statusText = "";

    [ObservableProperty]
    private string _tokenUsageText = "";

    public ChatViewModel(IChatService chatService, ISpeechService speechService, INotesService notesService, ISettingsService settingsService)
    {
        _chatService = chatService;
        _speechService = speechService;
        _notesService = notesService;
        _settingsService = settingsService;

        _speechService.SpeechRecognized += OnSpeechRecognized;
        _speechService.SpeechHypothesized += OnSpeechHypothesized;
    }

    [RelayCommand(CanExecute = nameof(CanSend))]
    private async Task SendMessage()
    {
        var text = InputText.Trim();
        if (string.IsNullOrEmpty(text)) return;

        if (string.IsNullOrWhiteSpace(_settingsService.Current.GeminiApiKey))
        {
            StatusText = "Please configure your Gemini API key in Settings.";
            return;
        }

        // Add user message
        Messages.Add(new ChatMessage { Role = "user", Content = text });
        InputText = "";
        IsSending = true;
        StatusText = "";

        try
        {
            var history = Messages.ToList();
            var result = await _chatService.SendMessageAsync(history, text);

            // Add model response
            Messages.Add(new ChatMessage { Role = "model", Content = result.ResponseText });

            // Display token usage
            if (result.UsageMetadata != null)
            {
                TokenUsageText = $"Tokens: Input={result.UsageMetadata.PromptTokenCount}, Output={result.UsageMetadata.CandidatesTokenCount}, Total={result.UsageMetadata.TotalTokenCount}";
            }

            // Handle note creation
            if (result.NoteRequest is { } noteReq)
            {
                await _notesService.CreateNoteAsync(noteReq.Title, noteReq.FileName, noteReq.Content);
                WeakReferenceMessenger.Default.Send(new NoteCreatedMessage());
            }
        }
        catch (HttpRequestException ex)
        {
            StatusText = $"Network error: {ex.Message}";
            Messages.Add(new ChatMessage { Role = "model", Content = "Sorry, I couldn't connect to the API. Please check your internet connection and API key." });
        }
        catch (Exception ex)
        {
            StatusText = $"Error: {ex.Message}";
            Messages.Add(new ChatMessage { Role = "model", Content = "An unexpected error occurred. Please try again." });
        }
        finally
        {
            IsSending = false;
        }
    }

    private bool CanSend() => !IsSending && !string.IsNullOrWhiteSpace(InputText);

    partial void OnInputTextChanged(string value) => SendMessageCommand.NotifyCanExecuteChanged();
    partial void OnIsSendingChanged(bool value) => SendMessageCommand.NotifyCanExecuteChanged();

    [RelayCommand]
    private void ToggleListening()
    {
        try
        {
            if (IsListening)
            {
                _speechService.StopListening();
                IsListening = false;
                HypothesisText = "";
            }
            else
            {
                _speechService.StartListening();
                IsListening = true;
                HypothesisText = "Listening...";
            }
        }
        catch (Exception ex)
        {
            StatusText = $"Speech error: {ex.Message}";
            IsListening = false;
        }
    }

    private void OnSpeechRecognized(object? sender, string text)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            InputText = string.IsNullOrEmpty(InputText) ? text : $"{InputText} {text}";
            HypothesisText = "";
        });
    }

    private void OnSpeechHypothesized(object? sender, string text)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            HypothesisText = text;
        });
    }
}

public class NoteCreatedMessage;
