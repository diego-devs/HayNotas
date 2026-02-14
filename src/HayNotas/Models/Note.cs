namespace HayNotas.Models;

public class Note
{
    public string FileName { get; set; } = "";
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string FullPath { get; set; } = "";
}
