using System.Speech.Recognition;
using HayNotas.Services.Interfaces;

namespace HayNotas.Services;

public class SpeechService : ISpeechService
{
    private SpeechRecognitionEngine? _engine;

    public event EventHandler<string>? SpeechRecognized;
    public event EventHandler<string>? SpeechHypothesized;
    public bool IsListening { get; private set; }

    public void StartListening()
    {
        if (IsListening) return;

        _engine = new SpeechRecognitionEngine();
        _engine.LoadGrammar(new DictationGrammar());
        _engine.SetInputToDefaultAudioDevice();

        _engine.SpeechRecognized += (_, e) =>
            SpeechRecognized?.Invoke(this, e.Result.Text);

        _engine.SpeechHypothesized += (_, e) =>
            SpeechHypothesized?.Invoke(this, e.Result.Text);

        _engine.RecognizeAsync(RecognizeMode.Multiple);
        IsListening = true;
    }

    public void StopListening()
    {
        if (!IsListening) return;

        _engine?.RecognizeAsyncStop();
        _engine?.Dispose();
        _engine = null;
        IsListening = false;
    }

    public void Dispose()
    {
        StopListening();
        GC.SuppressFinalize(this);
    }
}
