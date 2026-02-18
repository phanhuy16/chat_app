namespace Core.Interfaces.IServices
{
    public interface IOpenAIService
    {
        Task<string> GetChatResponseAsync(string prompt);
    }
}
