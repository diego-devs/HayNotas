using System.IO;
using System.Text.RegularExpressions;
using HayNotas.Models;
using HayNotas.Services.Interfaces;

namespace HayNotas.Services;

public partial class NotesService : INotesService
{
    private readonly ISettingsService _settingsService;

    public NotesService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public Task<List<Note>> GetAllNotesAsync()
    {
        var folder = GetNotesFolder();
        Directory.CreateDirectory(folder);

        var notes = Directory.GetFiles(folder, "*.md")
            .Select(path =>
            {
                var info = new FileInfo(path);
                var content = File.ReadAllText(path);
                return new Note
                {
                    FileName = info.Name,
                    FullPath = path,
                    Content = content,
                    Title = ExtractTitle(content, info.Name),
                    CreatedAt = info.CreationTime,
                    ModifiedAt = info.LastWriteTime
                };
            })
            .OrderByDescending(n => n.ModifiedAt)
            .ToList();

        return Task.FromResult(notes);
    }

    public async Task<Note> CreateNoteAsync(string title, string fileName, string markdownContent)
    {
        var folder = GetNotesFolder();
        Directory.CreateDirectory(folder);

        // Sanitize filename
        fileName = SanitizeFileName(fileName);
        if (!fileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            fileName += ".md";

        var fullPath = Path.Combine(folder, fileName);

        // Avoid overwriting existing files
        var counter = 1;
        while (File.Exists(fullPath))
        {
            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            fullPath = Path.Combine(folder, $"{nameWithoutExt}-{counter}.md");
            counter++;
        }

        await File.WriteAllTextAsync(fullPath, markdownContent);

        var info = new FileInfo(fullPath);
        return new Note
        {
            FileName = info.Name,
            FullPath = fullPath,
            Content = markdownContent,
            Title = title,
            CreatedAt = info.CreationTime,
            ModifiedAt = info.LastWriteTime
        };
    }

    public Task DeleteNoteAsync(string fileName)
    {
        var fullPath = Path.Combine(GetNotesFolder(), fileName);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }

    private string GetNotesFolder()
    {
        var folder = _settingsService.Current.NotesFolder;
        if (string.IsNullOrWhiteSpace(folder))
            folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HayNotas");
        return folder;
    }

    private static string ExtractTitle(string content, string fallbackFileName)
    {
        var match = TitleRegex().Match(content);
        return match.Success ? match.Groups[1].Value.Trim() : Path.GetFileNameWithoutExtension(fallbackFileName);
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalid = Path.GetInvalidFileNameChars();
        return new string(fileName.Where(c => !invalid.Contains(c)).ToArray());
    }

    [GeneratedRegex(@"^#\s+(.+)$", RegexOptions.Multiline)]
    private static partial Regex TitleRegex();
}
