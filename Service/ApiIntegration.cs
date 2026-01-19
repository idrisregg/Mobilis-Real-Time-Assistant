using System.Text.Json;

namespace Mobilis_Real_Time_Assistant.Service;

public class ApiIntegration
{
    // openrouter api configuration
    private readonly string _openRouterApiKey;
    private readonly HttpClient _httpClient;

    // context cache service for performance optimization
    private readonly IContextCacheService _contextCache;

    public ApiIntegration(IConfiguration configuration, HttpClient httpClient, IContextCacheService contextCache)
    {
        // get openrouter api key from appsettings
        _openRouterApiKey = configuration["OpenRouter:ApiKey"] ??
            throw new InvalidOperationException("OpenRouter API key not found in configuration.");

        _httpClient = httpClient;
        _contextCache = contextCache;

        // configure httpclient for openrouter
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openRouterApiKey}");
        _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", configuration["OpenRouter:SiteUrl"] ?? "http://localhost");
        _httpClient.DefaultRequestHeaders.Add("X-Title", configuration["OpenRouter:SiteName"] ?? "Mobilis Assistant");
    }

    // method for getting response from the API
    public async Task<string> GetResponseAsync(string userMessage)
    {
        try
        {
            // get context from cache (fast, no file I/O if already cached)
            var contextContent = await _contextCache.GetContextAsync();

            // prepare request payload for openrouter api
            var requestBody = new
            {
                model = "deepseek/deepseek-chat",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = @"You are a Mobilis customer service assistant.

        CRITICAL INSTRUCTIONS:
        1. A large reference document has been provided to you
        2. You MUST base ALL answers on that document
        3. You can Response in English Arabic and French and unlimited response in any language, and translate mobilis to موبليس in arabic
        4. If the answer is not in the document, say 'I don't have that information, Can you provide more context?, change you response dont just repeat this sentence'
        5. Be professional, helpful, and accurate
        6. NEVER ignore the reference document

        REFERENCE DOCUMENT:
        " + contextContent
                    },
                    new
                    {
                        role = "user",
                        content = userMessage
                    }
                }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            // send request to openrouter api
            var response = await _httpClient.PostAsync(
                "https://openrouter.ai/api/v1/chat/completions",
                jsonContent
            );

            // read response content before checking status
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // return detailed error information
                return $"OpenRouter API Error ({response.StatusCode}): {responseString}";
            }

            var responseData = JsonSerializer.Deserialize<JsonElement>(responseString);

            // extract assistant's message from response
            var assistantMessage = responseData
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return assistantMessage ?? "I apologize, I couldn't generate a response.";
        }
        catch (FileNotFoundException)
        {
            return "Error: Context file not found. Please ensure Data/ContextResponse.txt exists.";
        }
        catch (HttpRequestException ex)
        {
            return $"Network Error: {ex.Message}. Check your internet connection.";
        }
        catch (JsonException ex)
        {
            return $"Response Parse Error: {ex.Message}. The API returned invalid data.";
        }
        catch (Exception ex)
        {
            return $"Unexpected Error: {ex.Message}";
        }
    }

    // method to reload context without restarting the app
    public async Task ReloadContextAsync()
    {
        // reload context using cache service
        await _contextCache.ReloadContextAsync();
    }
}