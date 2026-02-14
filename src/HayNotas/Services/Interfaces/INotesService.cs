using HayNotas.Models;

namespace HayNotas.Services.Interfaces;

public interface INotesService
{
    Task<List<Note>> GetAllNotesAsync();
    Task<Note> CreateNoteAsync(string title, string fileName, string markdownContent);
    Task DeleteNoteAsync(string fileName);
}
