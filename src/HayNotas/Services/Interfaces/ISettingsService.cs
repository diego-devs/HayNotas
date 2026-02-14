using HayNotas.Models;

namespace HayNotas.Services.Interfaces;

public interface ISettingsService
{
    AppSettings Current { get; }
    Task LoadAsync();
    Task SaveAsync(AppSettings settings);
}
