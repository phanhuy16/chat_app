using System.Net.Http.Json;
using System.Text.Json;
using Core.Interfaces.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(HttpClient httpClient, IConfiguration configuration, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetWeatherAsync(string location)
        {
            var apiKey = _configuration["OpenWeatherMap:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_OPENWEATHERMAP_API_KEY_HERE")
            {
                return "⚠️ **Weather Error:** API Key is missing. Please update `appsettings.json`.";
            }

            try
            {
                // Use Uri.EscapeDataString to handle spaces and unicode characters (e.g. "Cần Thơ")
                var encodedLocation = Uri.EscapeDataString(location);
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={encodedLocation}&appid={apiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                // Retry logic: If not found, try appending ",vn" (Vietnam) as a common fallback for this user context
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound && !location.ToLower().Contains("vn"))
                {
                    var retryUrl = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(location + ",vn")}&appid={apiKey}&units=metric";
                    var retryResponse = await _httpClient.GetAsync(retryUrl);
                    if (retryResponse.IsSuccessStatusCode)
                    {
                        response = retryResponse;
                    }
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Weather API returned {response.StatusCode} for location {location}");
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return $"Could not find weather data for **{location}**. Try adding the country code (e.g., 'Vung Tau, VN').";

                    return "⚠️ **Weather Error:** Unable to fetch data.";
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var cityName = root.GetProperty("name").GetString();
                var weatherDesc = root.GetProperty("weather")[0].GetProperty("description").GetString();
                var temp = root.GetProperty("main").GetProperty("temp").GetDouble();
                var humidity = root.GetProperty("main").GetProperty("humidity").GetInt32();
                var windSpeed = root.GetProperty("wind").GetProperty("speed").GetDouble();

                var cultureInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                if (!string.IsNullOrEmpty(weatherDesc))
                {
                    weatherDesc = cultureInfo.ToTitleCase(weatherDesc);
                }

                // Format:
                // ### 🌤️ Weather in Hanoi
                // **25°C** - Clear Sky
                // 💧 Humidity: 80%
                // 💨 Wind: 5 m/s
                var icon = GetWeatherIcon(weatherDesc);
                return $"### {icon} Weather in {cityName}\n\n**{Math.Round(temp, 1)}°C** - {weatherDesc}\n\n💧 Humidity: **{humidity}%**\n💨 Wind: **{windSpeed} m/s**";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data");
                return "⚠️ **Weather Error:** An unexpected error occurred.";
            }
        }

        private string GetWeatherIcon(string? description)
        {
            if (string.IsNullOrEmpty(description)) return "🌤️";
            description = description.ToLower();
            if (description.Contains("clear")) return "☀️";
            if (description.Contains("cloud")) return "☁️";
            if (description.Contains("rain")) return "🌧️";
            if (description.Contains("storm") || description.Contains("thunder")) return "⛈️";
            if (description.Contains("snow")) return "❄️";
            if (description.Contains("mist") || description.Contains("fog")) return "🌫️";
            return "🌤️";
        }
    }
}
