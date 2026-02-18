using Core.DTOs.Messages;
using Core.DTOs.ChatHub;

namespace Core.Interfaces.IServices
{
    public interface IMessageNotificationService
    {
        Task BroadcastMessageAsync(int conversationId, ChatMessage message);
        Task BroadcastTypingAsync(int conversationId, int userId, string username);
        Task BroadcastStopTypingAsync(int conversationId, int userId);
    }
}
