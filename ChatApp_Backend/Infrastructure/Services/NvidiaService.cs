using System.Text.Json;
using System.Text;
using Core.Interfaces.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class NvidiaService : INvidiaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<NvidiaService> _logger;

        public NvidiaService(HttpClient httpClient, IConfiguration configuration, ILogger<NvidiaService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetChatResponseAsync(string prompt)
        {
            var apiKey = _configuration["NvidiaDeepSeek:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return "Nvidia API Key is not configured.";
            }

            var requestBody = new
            {
                model = "z-ai/glm5",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 1,
                top_p = 1,
                max_tokens = 16384,
                seed = 42,
                stream = false,
                chat_template_kwargs = new { enable_thinking = false, clear_thinking = false }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await _httpClient.PostAsync("https://integrate.api.nvidia.com/v1/chat/completions", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetail = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Nvidia API error: {StatusCode} - {ErrorDetail}", response.StatusCode, errorDetail);

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        return "AI Service Error: Invalid API Key.";
                    if (response.StatusCode == (System.Net.HttpStatusCode)429)
                        return "AI Service Error: Quota exceeded or too many requests.";

                    return $"AI Service Error: {response.StatusCode}";
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                
                using var document = JsonDocument.Parse(jsonResponse);
                
                var chatResponse = document.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return chatResponse ?? "I'm sorry, I couldn't generate a response.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Nvidia API");
                return $"Error: {ex.Message}";
            }
        }
    }
}
