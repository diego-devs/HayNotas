using System.Net.Http;
using System.Text;
using System.Text.Json;
using HayNotas.Helpers;
using HayNotas.Models;
using HayNotas.Services.Interfaces;

namespace HayNotas.Services;

public class ChatService : IChatService
{
    private readonly HttpClient _httpClient;
    private readonly ISettingsService _settingsService;

    private const string SystemPrompt = """
        You are HayNotas, a helpful personal assistant. You help users manage their notes and answer questions.

        When the user asks you to create, save, or write a note, you MUST respond with BOTH:
        1. A natural language confirmation BEFORE the JSON block, telling the user you have created the note.
        2. A JSON block wrapped in ```json ... ``` fences at the END of your response, with this exact schema:
        ```json
        {
          "action": "create_note",
          "title": "Note Title Here",
          "fileName": "note-title-here.md",
          "content": "The full markdown content of the note..."
        }
        ```

        Rules for note creation:
        - The "fileName" should be a kebab-case version of the title with .md extension.
        - The "content" should be well-formatted Markdown. Use headings, bullet points, and other formatting as appropriate.
        - The "content" should start with a # heading matching the title.
        - If the user says something like "create a note about X" or "save a note with Y" or "write down Z", treat it as a note creation request.
        - For everything else, respond normally without any JSON block.

        Always be concise, friendly, and helpful. Respond in the same language the user writes in.
        """;

    public ChatService(HttpClient httpClient, ISettingsService settingsService)
    {
        _httpClient = httpClient;
        _settingsService = settingsService;
    }

    public async Task<ChatResult> SendMessageAsync(List<ChatMessage> conversationHistory, string userMessage, CancellationToken ct = default)
    {
        var settings = _settingsService.Current;
        var model = string.IsNullOrWhiteSpace(settings.GeminiModel) ? "gemini-2.5-flash" : settings.GeminiModel;

        var request = new GeminiRequest
        {
            SystemInstruction = new GeminiContent
            {
                Parts = [new GeminiPart { Text = SystemPrompt }]
            },
            Contents = [],
            GenerationConfig = new GeminiGenerationConfig
            {
                Temperature = 0.7,
                MaxOutputTokens = 8192 // Increased from 2048 to allow longer responses
            }
        };

        // Include all conversation history (no artificial limits)
        var recentHistory = conversationHistory;

        foreach (var msg in recentHistory)
        {
            request.Contents.Add(new GeminiContent
            {
                Role = msg.Role,
                Parts = [new GeminiPart { Text = msg.Content }]
            });
        }

        // Add current user message
        request.Contents.Add(new GeminiContent
        {
            Role = "user",
            Parts = [new GeminiPart { Text = userMessage }]
        });

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={settings.GeminiApiKey}";

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseJson);

        var responseText = geminiResponse?.Candidates?
            .FirstOrDefault()?.Content?.Parts?
            .FirstOrDefault()?.Text ?? "I couldn't generate a response. Please try again.";

        var noteRequest = GeminiResponseParser.TryParseNoteCreation(responseText);
        var displayText = noteRequest != null
            ? GeminiResponseParser.ExtractDisplayText(responseText)
            : responseText;

        return new ChatResult
        {
            ResponseText = displayText,
            NoteRequest = noteRequest,
            UsageMetadata = geminiResponse?.UsageMetadata
        };
    }
}
