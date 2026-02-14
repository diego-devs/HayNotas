using System.Text.Json;
using System.Text.RegularExpressions;
using HayNotas.Models;

namespace HayNotas.Helpers;

public static partial class GeminiResponseParser
{
    [GeneratedRegex(@"```json\s*\n(\{[\s\S]*?\})\s*\n```", RegexOptions.Compiled)]
    private static partial Regex JsonBlockRegex();

    public static NoteCreationRequest? TryParseNoteCreation(string responseText)
    {
        var match = JsonBlockRegex().Match(responseText);
        if (!match.Success) return null;

        try
        {
            var json = match.Groups[1].Value;
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("action", out var action)
                && action.GetString() == "create_note")
            {
                return new NoteCreationRequest
                {
                    Title = root.GetProperty("title").GetString() ?? "Untitled",
                    FileName = root.GetProperty("fileName").GetString() ?? "untitled.md",
                    Content = root.GetProperty("content").GetString() ?? ""
                };
            }
        }
        catch (JsonException)
        {
            // Malformed JSON from LLM, ignore
        }

        return null;
    }

    public static string ExtractDisplayText(string responseText)
    {
        return JsonBlockRegex().Replace(responseText, "").Trim();
    }
}
