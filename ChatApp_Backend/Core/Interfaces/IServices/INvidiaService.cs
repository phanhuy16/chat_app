namespace Core.Interfaces.IServices
{
    public interface INvidiaService
    {
        Task<string> GetChatResponseAsync(string prompt);
    }
}
