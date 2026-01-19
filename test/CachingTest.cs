using Xunit;
namespace Mobilis_Real_Time_Assistant.Test;

public class ContextCacheServiceTests
{
    [Fact]
    public async Task GetContextAsync_ReturnsContent()
    {
        var testFile = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.txt");
        await File.WriteAllTextAsync(testFile, "Hello World");
        
        var service = new TestableContextCacheService(testFile);
        var result = await service.GetContextAsync();
        
        Assert.Equal("Hello World", result);
        
        File.Delete(testFile);
    }
}