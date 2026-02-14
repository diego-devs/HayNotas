using System.Text.Json.Serialization;

namespace HayNotas.Models;

public class GeminiRequest
{
    [JsonPropertyName("contents")]
    public List<GeminiContent> Contents { get; set; } = [];

    [JsonPropertyName("systemInstruction")]
    public GeminiContent? SystemInstruction { get; set; }

    [JsonPropertyName("generationConfig")]
    public GeminiGenerationConfig? GenerationConfig { get; set; }
}

public class GeminiContent
{
    [JsonPropertyName("role")]
    public string? Role { get; set; }

    [JsonPropertyName("parts")]
    public List<GeminiPart> Parts { get; set; } = [];
}

public class GeminiPart
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = "";
}

public class GeminiGenerationConfig
{
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; } = 0.7;

    [JsonPropertyName("maxOutputTokens")]
    public int MaxOutputTokens { get; set; } = 2048;
}

public class GeminiResponse
{
    [JsonPropertyName("candidates")]
    public List<GeminiCandidate>? Candidates { get; set; }

    [JsonPropertyName("usageMetadata")]
    public GeminiUsageMetadata? UsageMetadata { get; set; }
}

public class GeminiUsageMetadata
{
    [JsonPropertyName("promptTokenCount")]
    public int PromptTokenCount { get; set; }

    [JsonPropertyName("candidatesTokenCount")]
    public int CandidatesTokenCount { get; set; }

    [JsonPropertyName("totalTokenCount")]
    public int TotalTokenCount { get; set; }
}

public class GeminiCandidate
{
    [JsonPropertyName("content")]
    public GeminiContent? Content { get; set; }

    [JsonPropertyName("finishReason")]
    public string? FinishReason { get; set; }
}

public class ChatResult
{
    public string ResponseText { get; set; } = "";
    public NoteCreationRequest? NoteRequest { get; set; }
    public GeminiUsageMetadata? UsageMetadata { get; set; }
}

public class NoteCreationRequest
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public string FileName { get; set; } = "";
}
