using Moq;
using Moq.Protected;
using Mobilis_Real_Time_Assistant.Service;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Mobilis_Real_Time_Assistant.Test;

public class ApiIntegrationTests
{
    [Fact]
    public async Task GetResponseAsync_ReturnsAssistantMessage()
    {
        var mockContext = new Mock<IContextCacheService>();
        mockContext.Setup(x => x.GetContextAsync()).ReturnsAsync("Context");

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    JsonSerializer.Serialize(new { choices = new[] { new { message = new { content = "Hello" } } } }),
                    Encoding.UTF8,
                    "application/json")
            });

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> {
                {"OpenRouter:ApiKey", "test-key"},
                {"OpenRouter:SiteUrl", "http://test"},
                {"OpenRouter:SiteName", "Test"}
            }).Build();

        var api = new ApiIntegration(config, new HttpClient(mockHandler.Object), mockContext.Object);
        var result = await api.GetResponseAsync("Hi");

        Assert.Equal("Hello", result);
    }
}