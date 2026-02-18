using API.Hubs;
using Core.DTOs.ChatHub;
using Core.Interfaces.IServices;
using Microsoft.AspNetCore.SignalR;

namespace API.Services
{
    public class MessageNotificationService : IMessageNotificationService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageNotificationService(IHubContext<ChatHub> hubContext) => _hubContext = hubContext;

        public async Task BroadcastMessageAsync(int conversationId, ChatMessage message)
        {
            string groupName = $"conversation_{conversationId}";
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        public async Task BroadcastTypingAsync(int conversationId, int userId, string username)
        {
            string groupName = $"conversation_{conversationId}";
            await _hubContext.Clients.Group(groupName).SendAsync("UserTyping", new { ConversationId = conversationId, UserId = userId, Username = username });
        }

        public async Task BroadcastStopTypingAsync(int conversationId, int userId)
        {
            string groupName = $"conversation_{conversationId}";
            await _hubContext.Clients.Group(groupName).SendAsync("UserStoppedTyping", userId);
        }
    }
}
