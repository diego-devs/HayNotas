namespace HayNotas.Services.Interfaces;

public interface ISpeechService : IDisposable
{
    event EventHandler<string>? SpeechRecognized;
    event EventHandler<string>? SpeechHypothesized;
    bool IsListening { get; }
    void StartListening();
    void StopListening();
}
