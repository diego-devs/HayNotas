using HayNotas.Models;

namespace HayNotas.Services.Interfaces;

public interface IChatService
{
    Task<ChatResult> SendMessageAsync(List<ChatMessage> conversationHistory, string userMessage, CancellationToken ct = default);
}
