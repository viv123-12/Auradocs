using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;

public class OpenAiClient : ILLMClients
{
    private readonly HttpClient _httpClient;
    private readonly LLMSettings _llmSettings;
    public OpenAiClient(HttpClient httpClient, IOptions<LLMSettings> llmSettings)
    {
        _httpClient = httpClient;
        _llmSettings = llmSettings.Value;

        _httpClient.BaseAddress = new Uri(_llmSettings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",_llmSettings.ApiKey);
    }
    public async Task<string> sendPromptAsync(string prompt)
    {
        LLMrequest llmRequest = new LLMrequest
        {
            Model = _llmSettings.Model,
            Messages = new List<LLMMessage>
            {
                new LLMMessage
                {
                    Role = "System",
                    Content = "You are an AI writing assistant."

                },
                new LLMMessage
                {
                    Role = "user",
                    Content = prompt
                }
            }
        };

        string json = JsonSerializer.Serialize(llmRequest);
        StringContent requestBody = new StringContent(json, Encoding.UTF8, "application/json");
        
        for (int attempt = 0; attempt < 3; attempt++)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(
                "chat/completions",
                requestBody
            );

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(1000);
                continue;
            }

            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseBody = JsonSerializer.Deserialize<LLMResponse>(responseJson);
            return responseBody?.Choices?.FirstOrDefault().llmMessage?.Content;
        }
        throw new Exception("AI service is rate limited. Try again later.");
    }
}