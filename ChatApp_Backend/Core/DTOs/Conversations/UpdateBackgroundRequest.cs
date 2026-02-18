namespace Core.DTOs.Conversations
{
    public class UpdateBackgroundRequest
    {
        public string? BackgroundUrl { get; set; }
        public string BackgroundType { get; set; } = "default";
    }
}
