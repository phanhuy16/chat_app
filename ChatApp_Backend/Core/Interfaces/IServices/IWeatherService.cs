namespace Core.Interfaces.IServices
{
    public interface IWeatherService
    {
        Task<string> GetWeatherAsync(string location);
    }
}
